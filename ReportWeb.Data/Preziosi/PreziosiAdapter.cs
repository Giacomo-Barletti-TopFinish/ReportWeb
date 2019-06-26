using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Preziosi
{
    public class PreziosiAdapter : ReportWebAdapterBase
    {
        public PreziosiAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
         base(connection, transaction)
        { }

        public void FillRW_PREZIOSI(PreziosiDS ds)
        {
            string select = @"SELECT * FROM RW_PREZIOSI";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_PREZIOSI);
            }
        }

        public void FillRW_MOVIMENTI_PREZIOSI(PreziosiDS ds)
        {
            string select = @"SELECT * FROM RW_MOVIMENTI_PREZIOSI ORDER BY IDMOVIMENTI";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_MOVIMENTI_PREZIOSI);
            }
        }

        public void UpdatePreziosiDS(string tablename, PreziosiDS ds)
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
