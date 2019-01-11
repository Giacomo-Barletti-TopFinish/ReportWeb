
using ReportWeb.Common.Helpers;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Registrazione
{
    public class RegistrazioneAdapter : ReportWebAdapterBase
    {
        public RegistrazioneAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillRW_REGISTRAZIONE( RegistrazioneDS ds)
        {

            string query = @"SELECT * FROM RW_REGISTRAZIONE WHERE USCITA IS NULL";

            using (DbDataAdapter da = BuildDataAdapter(query))
            {
                da.Fill(ds.RW_REGISTRAZIONE);
            }
        }      

        public void UpdateRegistrazioneDS(string tablename, RegistrazioneDS ds)
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
