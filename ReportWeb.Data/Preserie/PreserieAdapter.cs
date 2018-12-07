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
    public class PreserieAdapter : ReportWebAdapterBase
    {
        public PreserieAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void TrovaCommessaPerNome(string nomeCommessa, PreserieDS ds)
        {
            if (string.IsNullOrEmpty(nomeCommessa)) return;

            string query = @"SELECT * FROM DITTA1.USR_PRD_LANCIOD WHERE NOMECOMMESSA LIKE '%{0}%'
                             UNION ALL
                            SELECT * FROM DITTA2.USR_PRD_LANCIOD WHERE NOMECOMMESSA LIKE '%{1}%'";
            query = string.Format(query, nomeCommessa, nomeCommessa);

            using (DbDataAdapter da = BuildDataAdapter(query))
            {
                da.Fill(ds.USR_PRD_LANCIOD);
            }
        }

        public void FillUSR_PRD_LANCIOD(string IDLANCIOD, PreserieDS ds)
        {
            string query = @"SELECT * FROM DITTA1.USR_PRD_LANCIOD WHERE IDLANCIOD = $P{IDLANCIOD1}
                             UNION ALL
                            SELECT * FROM DITTA2.USR_PRD_LANCIOD WHERE IDLANCIOD = $P{IDLANCIOD2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD1", DbType.String, IDLANCIOD);
            ps.AddParam("IDLANCIOD2", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(query, ps))
            {
                da.Fill(ds.USR_PRD_LANCIOD);
            }
        }

        public void TrovaCommessaPerModello(string Modello, PreserieDS ds)
        {
            if (string.IsNullOrEmpty(Modello)) return;

            string query = @"SELECT LA.* FROM DITTA1.USR_PRD_LANCIOD LA
                                    INNER JOIN GRUPPO.MAGAZZ MA ON MA.IDMAGAZZ = LA.IDMAGAZZ
                                    WHERE MA.MODELLO LIKE '{0}%'
                             UNION ALL
                            SELECT LA.* FROM DITTA2.USR_PRD_LANCIOD LA
                            INNER JOIN GRUPPO.MAGAZZ MA ON MA.IDMAGAZZ = LA.IDMAGAZZ
                            WHERE MA.MODELLO LIKE '{1}%'";
            query = string.Format(query, Modello, Modello);

            using (DbDataAdapter da = BuildDataAdapter(query))
            {
                da.Fill(ds.USR_PRD_LANCIOD);
            }
        }

        public void FillMAGAZZ(PreserieDS ds, List<string> IDMAGAZZ)
        {
            if (IDMAGAZZ.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDMAGAZZ);
            string select = @"  SELECT * FROM GRUPPO.MAGAZZ WHERE IDMAGAZZ IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZ);
            }
        }

        public void FillUSR_PRD_FASI(string IDLANCIOD, PreserieDS ds)
        {
            string query = @"SELECT * FROM DITTA1.USR_PRD_FASI WHERE IDLANCIOD = $P{IDLANCIOD1}
                             UNION ALL
                            SELECT * FROM DITTA2.USR_PRD_FASI WHERE IDLANCIOD = $P{IDLANCIOD2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD1", DbType.String, IDLANCIOD);
            ps.AddParam("IDLANCIOD2", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(query, ps))
            {
                da.Fill(ds.USR_PRD_FASI);
            }
        }

        public void FillUSR_PRD_MOVFASI(string IDLANCIOD, PreserieDS ds)
        {
            string query = @"SELECT MF.* FROM DITTA1.USR_PRD_MOVFASI MF INNER JOIN DITTA1.USR_PRD_FASI FA ON FA.IDPRDFASE = MF.IDPRDFASE WHERE FA.IDLANCIOD = $P{IDLANCIOD1}
                             UNION ALL
                            SELECT MF.* FROM DITTA2.USR_PRD_MOVFASI MF INNER JOIN DITTA2.USR_PRD_FASI FA ON FA.IDPRDFASE = MF.IDPRDFASE WHERE FA.IDLANCIOD = $P{IDLANCIOD2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDLANCIOD1", DbType.String, IDLANCIOD);
            ps.AddParam("IDLANCIOD2", DbType.String, IDLANCIOD);

            using (DbDataAdapter da = BuildDataAdapter(query, ps))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }
        }

    }
}
