using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data
{
    public class SecurityAdapter : ReportWebAdapterBase
    {
        public SecurityAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillUsers(SecurityDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.USR_USER";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_USER);
            }
        }

        public void SaveToken(string UIDUSER, string Token, int Durata, string IpAddress)
        {
            string insert = @"insert into $T{RW_TOKEN} (UIDUSER,DATA_CREAZIONE,TOKEN,DURATA,IPADDRESS)
                                values ($P{UIDUSER},$P{DATA_CREAZIONE},$P{TOKEN},$P{DURATA},$P{IPADDRESS})";

            ParamSet ps = new ParamSet();
            ps.AddParam("UIDUSER", DbType.String, UIDUSER);
            ps.AddParam("DATA_CREAZIONE", DbType.DateTime, DateTime.Now);
            ps.AddParam("TOKEN", DbType.String, Token);
            ps.AddParam("DURATA", DbType.Int32, Durata);
            ps.AddParam("IPADDRESS", DbType.String, IpAddress);
            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                object o = cmd.ExecuteNonQuery();
            }
        }

        public void GetToken(SecurityDS ds, string token)
        {
            string select = @"SELECT * FROM RW_TOKEN where TOKEN = $P{token}";
            ParamSet ps = new ParamSet();
            ps.AddParam("token", DbType.String, token);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_TOKEN);
            }
        }

        public void FillMenu(SecurityDS ds)
        {
            string select = @"SELECT * FROM RW_MENU ORDER BY SEQUENZA";
            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_MENU);
            }
        }

        public void FillUserMenu(string UIDUSER, SecurityDS ds)
        {
            string select = @"SELECT * FROM RW_USER_MENU WHERE UIDUSER = $P{UIDUSER}";

            ParamSet ps = new ParamSet();
            ps.AddParam("UIDUSER", DbType.String, UIDUSER);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_USER_MENU);
            }
        }

        public void UpdateSecurityDSTable(string tablename, SecurityDS ds)
        {
            string query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}", tablename);

            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                a.ContinueUpdateOnError = false;
                DataTable dt = ds.Tables[tablename];
                DbCommandBuilder cmd = BuildCommandBuilder(a);
                a.UpdateCommand = cmd.GetUpdateCommand();
                a.DeleteCommand = cmd.GetDeleteCommand();
                a.InsertCommand = cmd.GetInsertCommand();
                a.Update(dt);
            }
        }
    }
}
