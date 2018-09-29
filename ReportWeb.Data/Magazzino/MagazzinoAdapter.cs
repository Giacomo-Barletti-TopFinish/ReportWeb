using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Magazzino
{
    public class MagazzinoAdapter : ReportWebAdapterBase
    {
        public MagazzinoAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }


        public void FillMONITOR_GIACENZA(MagazzinoDS ds)
        {
            string select = @"SELECT * FROM MONITOR_GIACENZA ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MONITOR_GIACENZA);
            }
        }

        public void FillMAGAZZ(MagazzinoDS ds, string filtro)
        {
            string f = string.Format("%{0}%", filtro);
            string select = @"SELECT * FROM GRUPPO.MAGAZZ WHERE MODELLO LIKE $P{FILTRO}";

            ParamSet ps = new ParamSet();
            ps.AddParam("FILTRO", DbType.String, f);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MAGAZZ);
            }
        }
    }
}
