using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Preziosi
{
    public class PreziosiBusiness : ReportWebBusinessBase
    {
        public PreziosiBusiness() : base() { }

        [DataContext]
        public void FillRW_PREZIOSI(PreziosiDS ds)
        {
            PreziosiAdapter a = new PreziosiAdapter(DbConnection, DbTransaction);
            a.FillRW_PREZIOSI(ds);
        }

        [DataContext]
        public void FillRW_MOVIMENTI_PREZIOSI(PreziosiDS ds)
        {
            PreziosiAdapter a = new PreziosiAdapter(DbConnection, DbTransaction);
            a.FillRW_MOVIMENTI_PREZIOSI(ds);
        }

        [DataContext(true)]
        public void UpdatePreziosiDS(string tablename, PreziosiDS ds)
        {
            PreziosiAdapter a = new PreziosiAdapter(DbConnection, DbTransaction);
            a.UpdatePreziosiDS(tablename, ds);
        }
    }
}
