using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.MailDispatcher
{

    public class MailDispatcherAdapter : ReportWebAdapterBase
    {
        public MailDispatcherAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillMD_GRUPPI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI);
            }
        }

        public void FillMD_GRUPPI_DESTINATARI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI_DESTINATARI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI_DESTINATARI);
            }
        }

        public void FillMD_GRUPPI_APP(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI_APP ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI_APP);
            }
        }

        public void UpdateMailDispatcherDSTable(string tablename, MailDispatcherDS ds)
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
