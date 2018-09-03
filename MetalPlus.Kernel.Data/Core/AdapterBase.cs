using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MetalPlus.Kernel.Data.Core
{
    public class AdapterBase
    {
        protected enum DBEngines
        {
            Unknown,
            MSSqlServer,
            Oracle
        }

        protected class ParamSet
        {
            internal class ParamInfo
            {
                public string Name { get; set; }
                public DbType DbType { get; set; }
                public OracleDbType? OracleDbType { get; set; }
                public object Value { get; set; }
                public ParameterDirection Direction { get; set; } = ParameterDirection.Input;
            }

            private Dictionary<string, ParamInfo> _params = new Dictionary<string, ParamInfo>();

            public void AddParam(string name, DbType dbType, object value)
            {
                ParamInfo pi = new ParamInfo()
                {
                    Name = name,
                    DbType = dbType,
                    Value = value,
                    Direction = ParameterDirection.Input
                };
                _params.Add(name, pi);
            }

            public void AddOutputParam(string name, DbType dbType)
            {
                ParamInfo pi = new ParamInfo()
                {
                    Name = name,
                    DbType = dbType,
                    Direction = ParameterDirection.Output
                };
                _params.Add(name, pi);
            }

            public void AddRefCursorParameter(string name)
            {
                ParamInfo pi = new ParamInfo()
                {
                    Name = name,
                    OracleDbType = OracleDbType.RefCursor,
                    Direction = ParameterDirection.Output
                };
                _params.Add(name, pi);
            }

            internal ParamInfo GetInfo(string name)
            {
                return _params[name];
            }

            internal IEnumerable<ParamInfo> GetInfos()
            {
                return _params.Values;
            }
        }

        private const string OracleParamIdentityPrefix = "last_";
        protected DBEngines DBEngine { get; private set; }

        protected DbConnection DbConnection
        {
            get;
            private set;
        }

        protected DbTransaction DbTransaction
        {
            get;
            private set;
        }

        protected string Schema
        {
            get;
            private set;
        }

        public AdapterBase(IDbConnection connection, IDbTransaction transaction)
            : this(string.Empty, connection, transaction)
        {
        }

        public AdapterBase(string schema, IDbConnection connection, IDbTransaction transaction)
        {
            if (connection is SqlConnection)
            {
                DBEngine = DBEngines.MSSqlServer;
                FieldPrefix = TablePrefix = "[";
                FieldSuffix = TableSuffix = "]";
                ParamPrefix = "@";
                ParamSuffix = string.Empty;
            }
            else if (connection is OracleConnection)
            {
                DBEngine = DBEngines.Oracle;
                FieldPrefix = TablePrefix = FieldSuffix = TableSuffix = "\"";
                ParamPrefix = ":";
                ParamSuffix = string.Empty;
            }
            else
                throw new UnsupportedDatabaseEngineException(connection.ConnectionString);

            DbConnection = connection as DbConnection;
            DbTransaction = transaction as DbTransaction;
            Schema = schema;
        }

        private string TablePrefix { get; set; }
        private string TableSuffix { get; set; }
        private string FieldPrefix { get; set; }
        private string FieldSuffix { get; set; }
        private string ParamPrefix { get; set; }
        private string ParamSuffix { get; set; }

        protected string EscapeColumnName(string columnName)
        {
            if (DBEngine == DBEngines.Oracle)
                columnName = columnName.ToUpper();
            return FieldPrefix + columnName + FieldSuffix;
        }

        private string StringConcatOperatorName
        {
            get
            {
                switch (DBEngine)
                {
                    case DBEngines.MSSqlServer:
                        return "+";
                    case DBEngines.Oracle:
                        return "||";
                    default:
                        return "+";
                }
            }
        }

        private string HashJoinOperatorName
        {
            get
            {
                switch (DBEngine)
                {
                    case DBEngines.MSSqlServer:
                        return "HASH JOIN";
                    case DBEngines.Oracle:
                        return "JOIN";
                    default:
                        return "JOIN";
                }
            }
        }

        protected DbCommand BuildCommand(string query)
        {
            return BuildCommand(query, null);
        }

        protected DbCommand BuildCommand(string query, ParamSet paramSet)
        {
            DbCommand cmd = null;
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    cmd = new SqlCommand();
                    break;
                case DBEngines.Oracle:
                    cmd = new OracleCommand();
                    break;
            }

            cmd.Connection = DbConnection;
            cmd.Transaction = DbTransaction;

            query = ResolveTablePlaceholders(query, TablePrefix, TableSuffix);
            query = ResolveFieldPlaceholders(query, FieldPrefix, FieldSuffix);
            query = ResolveStoredProcedurePlaceholders(query);

            if (paramSet != null)
                query = ResolveParamPlaceholders(query, ParamPrefix, ParamSuffix, paramSet, cmd);

            query = ResolveOperatorsPlaceholders(query);
            query = ResolveFunctionsPlaceholders(query);

            cmd.CommandText = query;
            return cmd;
        }

        protected DbDataAdapter BuildDataAdapter(string query)
        {
            return BuildDataAdapter(query, null);
        }

        protected DbDataAdapter BuildDataAdapter(string query, ParamSet paramSet)
        {
            DbDataAdapter da = null;
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    da = new SqlDataAdapter();
                    break;
                case DBEngines.Oracle:
                    da = new OracleDataAdapter();
                    break;
            }

            da.SelectCommand = BuildCommand(query, paramSet);
            return da;
        }

        protected DbDataAdapter BuildStoredProcedureDataAdapter(string query, ParamSet paramSet)
        {
            DbDataAdapter da = null;
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    da = new SqlDataAdapter();
                    break;
                case DBEngines.Oracle:
                    da = new OracleDataAdapter();
                    break;
            }

            da.SelectCommand = BuildStoredProcedureCommand(query, paramSet);
            return da;
        }

        protected DbCommand BuildStoredProcedureCommand(string query, ParamSet paramSet)
        {
            DbCommand cmd = null;
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    cmd = new SqlCommand();
                    break;
                case DBEngines.Oracle:
                    cmd = new OracleCommand();
                    break;
            }

            cmd.Connection = DbConnection;
            cmd.Transaction = DbTransaction;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = ResolveStoredProcedurePlaceholders(query);

            if (paramSet != null)
            {
                foreach (ParamSet.ParamInfo pi in paramSet.GetInfos())
                {
                    IDbDataParameter param = cmd.CreateParameter();
                    param.ParameterName = pi.Name;
                    if (pi.OracleDbType.HasValue && DBEngine == DBEngines.Oracle)
                    {
                        OracleParameter oracleParam = param as OracleParameter;
                        if (oracleParam != null)
                            oracleParam.OracleDbType = pi.OracleDbType.Value;
                    }
                    else
                    {
                        param.DbType = pi.DbType;
                    }
                    param.Value = pi.Value;
                    param.Direction = pi.Direction;
                    cmd.Parameters.Add(param);
                }
            }

            return cmd;
        }

        protected DbCommandBuilder BuildCommandBuilder(DbDataAdapter da)
        {
            DbCommandBuilder cb = null;
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    cb = new SqlCommandBuilder(da as SqlDataAdapter);
                    break;
                case DBEngines.Oracle:
                    {
                        OracleDataAdapter daOracle = da as OracleDataAdapter;
                        cb = new OracleCommandBuilder(daOracle);
                        cb.DataAdapter = daOracle;
                        break;
                    }
            }

            return cb;
        }

        protected void ConfigureAdapterCommands(DataTable dt, DbDataAdapter da, DbCommandBuilder cb)
        {
            bool isIdentity;
            string identityField;
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    break;
                case DBEngines.Oracle:
                    {
                        da.InsertCommand = cb.GetInsertCommand();
                        da.UpdateCommand = cb.GetUpdateCommand();
                        da.DeleteCommand = cb.GetDeleteCommand();

                        OracleDbType identityType;
                        MapOracleIdentityField(dt, out isIdentity, out identityField, out identityType);
                        if (isIdentity)
                        {
                            OracleDataAdapter oDa = (OracleDataAdapter)da;
                            da.InsertCommand.CommandText += string.Format(" RETURNING {0}{1}{2} INTO :{3}{4}",
                                TablePrefix, identityField.ToUpper(), TableSuffix,
                                OracleParamIdentityPrefix, identityField);
                            oDa.InsertCommand.Parameters.Add(OracleParamIdentityPrefix + identityField, identityType).Direction = ParameterDirection.Output;
                        }
                        else
                            da.InsertCommand = cb.GetInsertCommand();
                        break;
                    }
            }
        }

        protected T RetrievePostUpdateID<T>(IDbCommand cmd, DataRow row)
        {
            DataTable dt = row.Table;
            T returnValue = default(T);
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    using (DbCommand cmdNewID = BuildCommand("SELECT @@IDENTITY"))
                    {
                        returnValue = (T)cmdNewID.ExecuteScalar();
                    }
                    break;
                case DBEngines.Oracle:
                    bool isIdentity;
                    string identityField;
                    OracleDbType identityType;
                    MapOracleIdentityField(dt, out isIdentity, out identityField, out identityType);
                    if (isIdentity)
                        returnValue = (T)Convert.ChangeType(row[identityField], typeof(T));
                    break;
            }
            return returnValue;
        }

        private void MapOracleIdentityField(DataTable dt, out bool isIdentity, out string identityField, out OracleDbType identityType)
        {
            DataColumn identityColumn = dt.Columns.Cast<DataColumn>().FirstOrDefault(x => x.AutoIncrement);
            isIdentity = identityColumn != null;
            identityField = identityColumn != null ? identityColumn.ColumnName : string.Empty;
            identityType = OracleDbType.Int32;
            if (identityColumn != null)
            {
                if (identityColumn.DataType == typeof(int))
                {
                    identityType = OracleDbType.Int32;
                }
                else if (identityColumn.DataType == typeof(long))
                {
                    identityType = OracleDbType.Int64;
                }
                else if (identityColumn.DataType == typeof(decimal))
                {
                    identityType = OracleDbType.Decimal;
                }
            }
        }

        private OracleDbType MapOracleDbType(Type type)
        {
            OracleDbType dbType;
            if (type == typeof(int))
                dbType = OracleDbType.Int32;
            else if (type == typeof(long))
                dbType = OracleDbType.Int64;
            else if (type == typeof(decimal))
                dbType = OracleDbType.Decimal;
            else if (type == typeof(double))
                dbType = OracleDbType.Double;
            else if (type == typeof(string))
                dbType = OracleDbType.Varchar2;
            else if (type == typeof(DateTime))
                dbType = OracleDbType.Date;
            else if (type == typeof(short))
                dbType = OracleDbType.Int16;
            else
                throw new DataException("Unsupported type " + type.ToString() + " in oracle");
            return dbType;

        }

        public delegate void RowUpdatedEventHandler(object sender, RowUpdatedEventArgs e);

        protected void InstallRowUpdatedHandler(DbDataAdapter da, RowUpdatedEventHandler rowUpdatedHandler)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    (da as SqlDataAdapter).RowUpdated += new SqlRowUpdatedEventHandler(rowUpdatedHandler);
                    break;
                case DBEngines.Oracle:
                    (da as OracleDataAdapter).RowUpdated += new OracleRowUpdatedEventHandler(rowUpdatedHandler);
                    break;
            }
        }

        protected void UninstallRowUpdatedHandler(DbDataAdapter da, RowUpdatedEventHandler rowUpdatedHandler)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    (da as SqlDataAdapter).RowUpdated -= new SqlRowUpdatedEventHandler(rowUpdatedHandler);
                    break;
                case DBEngines.Oracle:
                    (da as OracleDataAdapter).RowUpdated -= new OracleRowUpdatedEventHandler(rowUpdatedHandler);
                    break;
            }
        }

        protected List<T> Select<T>(string query) where T : class, new()
        {
            return Select<T>(query, null);
        }

        protected List<T> Select<T>(string query, ParamSet paramSet) where T : class, new()
        {
            using (IDbCommand cmd = BuildCommand(query, paramSet))
            {
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    List<T> entities = reader.ToEntityList<T>();
                    return entities;
                }
            }
        }

        protected string ResolveTablePlaceholders(string input, string prefix, string suffix)
        {
            string output = string.Empty;
            Regex reg = new Regex(@"\$T(\{|\<)[0-9A-Z_a-z]+(\}|\>)");
            MatchCollection matches = reg.Matches(input);
            int cursor = 0;
            foreach (Match m in matches)
            {
                string tableName = m.Value.Substring(3, m.Value.Length - 4).ToUpper(); //cutting $T{ and }
                if (DBEngine == DBEngines.Oracle)
                {
                    if (tableName.Length > 30)
                        tableName = tableName.Substring(0, 30);
                    tableName = tableName.ToUpper();
                }
                output += input.Substring(cursor, m.Index - cursor);
                output += prefix + tableName + suffix;
                cursor = m.Index + m.Length;
            }
            if (cursor < input.Length)
                output += input.Substring(cursor);
            return output;
        }

        protected string ResolveFieldPlaceholders(string input, string prefix, string suffix)
        {
            string output = string.Empty;
            Regex reg = new Regex(@"\$C\{[0-9A-Z_.\sa-z]+\}");
            MatchCollection matches = reg.Matches(input);
            int cursor = 0;
            foreach (Match m in matches)
            {
                string fieldName = m.Value.Substring(3, m.Value.Length - 4).ToUpper(); //cutting $C{ and }
                if (DBEngine == DBEngines.Oracle)
                {
                    if (fieldName.Length > 30)
                        fieldName = fieldName.Substring(0, 30);
                    fieldName = fieldName.ToUpper();
                }
                output += input.Substring(cursor, m.Index - cursor);
                output += prefix + fieldName + suffix;
                cursor = m.Index + m.Length;
            }
            if (cursor < input.Length)
                output += input.Substring(cursor);
            return output;
        }

        protected string ResolveStoredProcedurePlaceholders(string input)
        {
            string output = string.Empty;
            Regex reg = new Regex(@"\$S\{[0-9A-Z_\sa-z]+\}");
            MatchCollection matches = reg.Matches(input);
            int cursor = 0;
            foreach (Match m in matches)
            {
                string spName = m.Value.Substring(3, m.Value.Length - 4).ToUpper(); //cutting $S{ and }
                if (DBEngine == DBEngines.Oracle)
                {
                    if (spName.Length > 30)
                        spName = spName.Substring(0, 30);
                    spName = spName.ToUpper();
                }
                output += input.Substring(cursor, m.Index - cursor);
                output += spName;
                cursor = m.Index + m.Length;
            }
            if (cursor < input.Length)
                output += input.Substring(cursor);
            return output;
        }

        protected string ResolveOperatorsPlaceholders(string input)
        {
            string output = string.Empty;
            Regex reg = new Regex(@"\$O\{[^\}]*\}");
            MatchCollection matches = reg.Matches(input);
            int cursor = 0;
            foreach (Match m in matches)
            {
                string operatorPlaceholder = m.Value.Substring(3, m.Value.Length - 4).ToUpper(); //cutting $O{ and }
                output += input.Substring(cursor, m.Index - cursor);
                switch (operatorPlaceholder)
                {
                    case "++": //string concatenation
                        output += StringConcatOperatorName;
                        break;

                    case "HASH_JOIN":
                        output += HashJoinOperatorName;
                        break;

                    default:
                        throw new DataException("Unsupported map of sql operator '" + operatorPlaceholder + "'");

                }
                cursor = m.Index + m.Length;
            }
            if (cursor < input.Length)
                output += input.Substring(cursor);
            return output;
        }

        protected string ResolveParamPlaceholders(string input, string prefix, string suffix, ParamSet paramSet, IDbCommand cmd)
        {
            string output = string.Empty;
            Regex reg = new Regex(@"\$P(\{|\<)[0-9A-Z_a-z]+(\}|\>)");
            MatchCollection matches = reg.Matches(input);
            int cursor = 0;
            foreach (Match m in matches)
            {
                string paramName = m.Value.Substring(3, m.Value.Length - 4); //cutting $P{ and }
                string physicParamName = prefix + paramName + suffix;
                output += input.Substring(cursor, m.Index - cursor);
                output += physicParamName;
                cursor = m.Index + m.Length;
                IDbDataParameter param = cmd.CreateParameter();
                ParamSet.ParamInfo pi = paramSet.GetInfo(paramName);
                param.ParameterName = physicParamName;
                param.DbType = pi.DbType;
                param.Value = pi.Value;
                param.Direction = pi.Direction;
                cmd.Parameters.Add(param);
            }
            if (cursor < input.Length)
                output += input.Substring(cursor);
            return output;
        }

        public T RetrieveParamValue<T>(DbCommand cmd, string paramName)
        {
            string physicParamName = ParamPrefix + paramName + ParamSuffix;
            return (T)cmd.Parameters[physicParamName].Value;
        }

        protected string ResolveFunctionsPlaceholders(string input)
        {
            string output = string.Empty;
            Regex reg = new Regex(@"\$F\{[^\}]*\}");
            MatchCollection matches = reg.Matches(input);
            int cursor = 0;
            foreach (Match m in matches)
            {
                output += input.Substring(cursor, m.Index - cursor);

                string call = m.Value.Substring(3, m.Value.Length - 4); //cutting $F{ and }
                call = call.Trim();
                if (call.Length == 0)
                    throw new DataException("Empty sql function call detected");
                if (call[call.Length - 1] != ')')
                    throw new DataException("Minning close braket of sql function call detected in call " + call);
                int posOfBracket = call.IndexOf('(');
                if (posOfBracket == -1)
                    throw new DataException("Missing open bracket of sql function detected in call" + call);
                string functionPlaceholder = call.Substring(0, posOfBracket).Trim().ToUpper();

                output += " ";
                switch (functionPlaceholder)
                {
                    case "ISNULL":
                        output += ResolveFunctionISNULL(call);
                        break;

                    case "NVL":
                        output += ResolveFunctionNVL(call);
                        break;

                    case "LEN":
                        output += ResolveFunctionLEN(call);
                        break;

                    case "LENGTH":
                        output += ResolveFunctionLENGTH(call);
                        break;

                    case "SUBSTRING":
                        output += ResolveFunctionSUBSTRING(call);
                        break;

                    case "SUBSTR":
                        output += ResolveFunctionSUBSTR(call);
                        break;

                    case "GETDATE":
                        output += ResolveFunctionGETDATE(call);
                        break;

                    case "STR":
                        output += ResolveFunctionSTR(call);
                        break;
                    default:
                        throw new DataException("Unsupported map of sql function '" + functionPlaceholder + "' detected in call " + call);
                }

                output += " ";
                cursor = m.Index + m.Length;
            }
            if (cursor < input.Length)
                output += input.Substring(cursor);
            return output;
        }

        private string ResolveFunctionISNULL(string call)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return call;

                case DBEngines.Oracle:
                    return ReplaceFirstOccurrenceCaseInsensitive(call, "ISNULL", "NVL");

                default:
                    return call;
            }
        }

        private string ResolveFunctionNVL(string call)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return ReplaceFirstOccurrenceCaseInsensitive(call, "NVL", "ISNULL");

                case DBEngines.Oracle:
                    return call;

                default:
                    return call;
            }
        }

        private string ResolveFunctionLEN(string call)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return call;

                case DBEngines.Oracle:
                    return ReplaceFirstOccurrenceCaseInsensitive(call, "LEN", "LENGTH");

                default:
                    return call;
            }
        }

        private string ResolveFunctionLENGTH(string call)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return ReplaceFirstOccurrenceCaseInsensitive(call, "LENGTH", "LEN");

                case DBEngines.Oracle:
                    return call;

                default:
                    return call;
            }
        }

        private string ResolveFunctionSUBSTRING(string call)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return call;

                case DBEngines.Oracle:
                    return ReplaceFirstOccurrenceCaseInsensitive(call, "SUBSTRING", "SUBSTR");

                default:
                    return call;
            }
        }

        private string ResolveFunctionSUBSTR(string call)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return ReplaceFirstOccurrenceCaseInsensitive(call, "SUBSTR", "SUBSTRING");

                case DBEngines.Oracle:
                    return call;

                default:
                    return call;
            }
        }

        private string ResolveFunctionGETDATE(string call)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return call;

                case DBEngines.Oracle:
                    return "SYSDATE";

                default:
                    return call;
            }
        }

        private string ResolveFunctionSTR(string call)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return call;

                case DBEngines.Oracle:
                    return ReplaceFirstOccurrenceCaseInsensitive(call, "STR", "TO_CHAR");

                default:
                    return call;
            }
        }

        private static string ReplaceFirstOccurrenceCaseInsensitive(string str, string oldValue, string newValue)
        {
            int prevPos = 0;
            string retval = str;
            int pos = retval.IndexOf(oldValue, StringComparison.InvariantCultureIgnoreCase);

            if (pos > -1)
            {
                retval = retval.Remove(pos, oldValue.Length);
                retval = retval.Insert(pos, newValue);
                prevPos = pos + newValue.Length;
                pos = retval.IndexOf(oldValue, prevPos, StringComparison.InvariantCultureIgnoreCase);
            }

            return retval;
        }

        protected string BuildSqlDateDiffInDay(string date1, string date2)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return "DATEDIFF(day, " + date1 + ", " + date2 + ")";

                case DBEngines.Oracle:
                    return "CAST(" + date2 + " - " + date1 + " AS NUMBER(10, 0))";

                default:
                    return "DATEDIFF(day, " + date1 + ", " + date2 + ")";
            }
        }

        protected string BuildSqlRightFunction(string expression, int length)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return "RIGHT(" + expression + ", " + length + ")";

                case DBEngines.Oracle:
                    return "SUBSTR(" + expression + ", " + (-length) + ")";

                default:
                    return "RIGHT(" + expression + ", " + length + ")";
            }
        }

        protected string BuildSqlWithNoLock()
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    return "WITH (nolock)";

                case DBEngines.Oracle:
                    return string.Empty;

                default:
                    return "WITH (nolock)";
            }
        }

        protected void PrepareAutonumbers(DataTable dt)
        {
            switch (DBEngine)
            {
                case DBEngines.MSSqlServer:
                    //nothing to do
                    break;

                case DBEngines.Oracle:
                    PrepareAutonumbersForOracle(dt);
                    break;

                default:
                    break;
            }
        }

        private void PrepareAutonumbersForOracle(DataTable dt)
        {
            bool hasIdentity;
            string identityField;
            OracleDbType identityType;
            MapOracleIdentityField(dt, out hasIdentity, out identityField, out identityType);

            if (hasIdentity)
            {
                bool isIdentityReadOnly = dt.Columns[identityField].ReadOnly;

                try
                {
                    if (isIdentityReadOnly)
                        dt.Columns[identityField].ReadOnly = false;

                    foreach (DataRow row in dt.Select(string.Empty, string.Empty, DataViewRowState.Added))
                    {
                        if (Convert.ToInt64(row[identityField]) < 0)
                        {
                            using (IDbCommand cmd = BuildCommand("SELECT SEQFORALL.NEXTVAL NextValue FROM DUAL", null))
                            {
                                object nextSeqVal = Convert.ChangeType(cmd.ExecuteScalar(), dt.Columns[identityField].DataType);
                                row[identityField] = nextSeqVal;
                            }
                        }
                    }
                }
                finally
                {
                    if (isIdentityReadOnly)
                        dt.Columns[identityField].ReadOnly = true;
                }
            }
        }
    }
}
