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

namespace ReportWeb.Data
{
    public class ALEAdapter : ReportWebAdapterBase
    {
        public ALEAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillUSR_CHECKQ_T(ALEDS ds, string Barcode)
        {
            string select = @"SELECT * FROM DITTA1.USR_CHECKQ_T WHERE BARCODE = $P{BARCODE1}
                                UNION ALL
                              SELECT * FROM DITTA2.USR_CHECKQ_T WHERE BARCODE = $P{BARCODE2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("BARCODE1", DbType.String, Barcode);
            ps.AddParam("BARCODE2", DbType.String, Barcode);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_CHECKQ_T);
            }
        }

        public void FillUSR_CHECKQ_D(ALEDS ds, string IDCHECKQT)
        {
            string select = @"SELECT * FROM DITTA1.USR_CHECKQ_D WHERE IDCHECKQT = $P{IDCHECKQT1}
                                UNION ALL
                              SELECT * FROM DITTA2.USR_CHECKQ_D WHERE IDCHECKQT = $P{IDCHECKQT2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDCHECKQT1", DbType.String, IDCHECKQT);
            ps.AddParam("IDCHECKQT2", DbType.String, IDCHECKQT);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_CHECKQ_D);
            }
        }

        public void FillUSR_CHECKQ_C(ALEDS ds, string IDCHECKQT)
        {
            string select = @"SELECT * FROM DITTA1.USR_CHECKQ_C WHERE IDCHECKQT = $P{IDCHECKQT1}
                                UNION ALL
                              SELECT * FROM DITTA2.USR_CHECKQ_C WHERE IDCHECKQT = $P{IDCHECKQT2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDCHECKQT1", DbType.String, IDCHECKQT);
            ps.AddParam("IDCHECKQT2", DbType.String, IDCHECKQT);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_CHECKQ_C);
            }
        }

        public void FillUSR_TAB_TIPODIFETTI(ALEDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.USR_TAB_TIPODIFETTI";


            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_TAB_TIPODIFETTI);
            }
        }

        public void FillUSR_ANA_DIFETTI(ALEDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.USR_ANA_DIFETTI";


            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_ANA_DIFETTI);
            }
        }

        public void FillTABCAUMGT(ALEDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.TABCAUMGT";


            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.TABCAUMGT);
            }
        }

        public void FillCLIFO(ALEDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.CLIFO";


            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.CLIFO);
            }
        }
    }
}
