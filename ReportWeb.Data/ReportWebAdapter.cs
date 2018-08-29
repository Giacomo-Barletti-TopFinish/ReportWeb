using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data
{
    public class ReportWebAdapter : ReportWebAdapterBase
    {
        public ReportWebAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillTV_SCADENZE_CLIFO(ReportDS ds)
        {
            string select = @" SELECT * FROM $T{TV_SCADENZE_CLIFO}";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.TV_SCADENZE_CLIFO);
            }
        }

        public void FillODL_APERTI(ReportDS ds)
        {
            string select = @" SELECT * FROM $T{ODL_APERTI}";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.ODL_APERTI);
            }
        }

        public void FillODL_APERTI(string codiceTipoMovimentoFase, ReportDS ds)
        {
            string select = @" SELECT * FROM $T{ODL_APERTI} WHERE CODTIPOMOVFASE = $P{CODTIPOMOVFASE}";

            ParamSet ps = new ParamSet();
            ps.AddParam("CODTIPOMOVFASE", DbType.String, codiceTipoMovimentoFase);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.ODL_APERTI);
            }
        }

        public void FillODL_APERTI(string reparto, string codiceTipoMovimentoFase, ReportDS ds)
        {
            string select = @" SELECT * FROM $T{ODL_APERTI} WHERE CODTIPOMOVFASE = $P{CODTIPOMOVFASE} AND CODICECLIFO = $P{CODICECLIFO}";

            ParamSet ps = new ParamSet();
            ps.AddParam("CODTIPOMOVFASE", DbType.String, codiceTipoMovimentoFase);
            ps.AddParam("CODICECLIFO", DbType.String, reparto);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.ODL_APERTI);
            }
        }


    }
}
