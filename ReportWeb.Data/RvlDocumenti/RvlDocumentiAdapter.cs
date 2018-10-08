using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.RvlDocumenti
{
    public class RvlDocumentiAdapter : ReportWebAdapterBase
    {
        public RvlDocumentiAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillUSR_VENDITET(RvlDocumentiDS ds, string NumeroDocumento)
        {

            string f = string.Format("{0}", NumeroDocumento.Trim().ToUpper());
            string select = @"
    SELECT DITTA1.USR_VENDITET.*,'METAL-PLUS' AS AZIENDA FROM DITTA1.USR_VENDITET WHERE NUMDOC LIKE $P{NUMDOC}
    UNION ALL 
    SELECT DITTA2.USR_VENDITET.*, 'TOPFINISH' AS AZIENDA FROM DITTA2.USR_VENDITET WHERE NUMDOC LIKE $P{NUMDOC}";

            ParamSet ps = new ParamSet();
            ps.AddParam("NUMDOC", DbType.String, f);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_VENDITET);
            }
        }

        public void FillUSR_VENDITED(RvlDocumentiDS ds, List<string> IDVENDITET)
        {
            if (IDVENDITET.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDVENDITET);
            string select = @"SELECT * FROM DITTA1.USR_VENDITED WHERE IDVENDITET IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_VENDITED WHERE IDVENDITET IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_VENDITED);
            }
        }

        public void FillUSR_VENDITET(RvlDocumentiDS ds, List<string> IDVENDITET)
        {
            if (IDVENDITET.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDVENDITET);
            string select = @"SELECT * FROM DITTA1.USR_VENDITET WHERE IDVENDITET IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_VENDITET WHERE IDVENDITET IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_VENDITET);
            }
        }

        public void FillUSR_PRD_FLUSSO_MOVMATE(RvlDocumentiDS ds, List<string> IDVENDITED)
        {
            if (IDVENDITED.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDVENDITED);
            string select = @"SELECT * FROM DITTA1.USR_PRD_FLUSSO_MOVMATE WHERE IDVENDITED IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_PRD_FLUSSO_MOVMATE WHERE IDVENDITED IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FLUSSO_MOVMATE);
            }
        }

        public void FillUSR_PRD_FLUSSO_MOVMATEByIDPRDMOVMATE(RvlDocumentiDS ds, List<string> IDPRDMOVMATE)
        {
            if (IDPRDMOVMATE.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDPRDMOVMATE);
            string select = @"SELECT * FROM DITTA1.USR_PRD_FLUSSO_MOVMATE WHERE IDPRDMOVMATE IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_PRD_FLUSSO_MOVMATE WHERE IDPRDMOVMATE IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FLUSSO_MOVMATE);
            }
        }
        public void FillUSR_PRD_MOVMATE(RvlDocumentiDS ds, List<string> IDPRDMOVMATE)
        {
            if (IDPRDMOVMATE.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDPRDMOVMATE);
            string select = @"SELECT * FROM DITTA1.USR_PRD_MOVMATE WHERE IDPRDMOVMATE IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_PRD_MOVMATE WHERE IDPRDMOVMATE IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVMATE);
            }
        }

        public void FillUSR_PRD_MOVMATEByIDPRDMOVFASE(RvlDocumentiDS ds, List<string> IDPRDMOVFASE)
        {
            if (IDPRDMOVFASE.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDPRDMOVFASE);
            string select = @"SELECT * FROM DITTA1.USR_PRD_MOVMATE WHERE IDPRDMOVFASE IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_PRD_MOVMATE WHERE IDPRDMOVFASE IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVMATE);
            }
        }

        public void FillUSR_PRD_FLUSSO_MOVFASI(RvlDocumentiDS ds, List<string> IDPRDMOVFASE)
        {
            if (IDPRDMOVFASE.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDPRDMOVFASE);
            string select = @"SELECT * FROM DITTA1.USR_PRD_FLUSSO_MOVFASI WHERE IDPRDMOVFASE IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_PRD_FLUSSO_MOVFASI WHERE IDPRDMOVFASE IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FLUSSO_MOVFASI);
            }
        }

        public void FillUSR_PRD_FLUSSO_MOVFASIByIDACQUISTID(RvlDocumentiDS ds, List<string> IDACQUISTID)
        {
            if (IDACQUISTID.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDACQUISTID);
            string select = @"SELECT * FROM DITTA1.USR_PRD_FLUSSO_MOVFASI WHERE IDACQUISTID IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_PRD_FLUSSO_MOVFASI WHERE IDACQUISTID IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_FLUSSO_MOVFASI);
            }
        }

        public void FillUSR_PRD_MOVFASI(RvlDocumentiDS ds, List<string> IDPRDMOVFASE)
        {
            if (IDPRDMOVFASE.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDPRDMOVFASE);
            string select = @"SELECT * FROM DITTA1.USR_PRD_MOVFASI WHERE IDPRDMOVFASE IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_PRD_MOVFASI WHERE IDPRDMOVFASE IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }
        }
        public void FillUSR_ACQUISTID(RvlDocumentiDS ds, List<string> IDUSRACQUISTID)
        {
            if (IDUSRACQUISTID.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDUSRACQUISTID);
            string select = @"SELECT * FROM DITTA1.USR_ACQUISTID WHERE IDACQUISTID IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_ACQUISTID WHERE IDACQUISTID IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_ACQUISTID);
            }
        }

        public void FillUSR_ACQUISTIDByIDUSRACQUISTIT(RvlDocumentiDS ds, List<string> IDUSRACQUISTIT)
        {
            if (IDUSRACQUISTIT.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDUSRACQUISTIT);
            string select = @"SELECT * FROM DITTA1.USR_ACQUISTID WHERE IDACQUISTIT IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_ACQUISTID WHERE IDACQUISTIT IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_ACQUISTID);
            }
        }

        public void FillUSR_ACQUISTIT(RvlDocumentiDS ds, List<string> IDUSRACQUISTIT)
        {
            if (IDUSRACQUISTIT.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDUSRACQUISTIT);
            string select = @"SELECT * FROM DITTA1.USR_ACQUISTIT WHERE IDACQUISTIT IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_ACQUISTIT WHERE IDACQUISTIT IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_ACQUISTIT);
            }
        }


        public void FillUSR_ACQUISTIT(RvlDocumentiDS ds, string NumeroDocumento, string TipoDocumento, string Data, string Riferimento, string Fornitore)
        {
            string whereCondition = "WHERE 1=1 ";
            ParamSet ps = new ParamSet();

            if (!string.IsNullOrEmpty(NumeroDocumento))
            {
                whereCondition += "AND NUMDOC=$P{NUMDOC} ";
                ps.AddParam("NUMDOC", DbType.String, NumeroDocumento);
            }

            if (!string.IsNullOrEmpty(TipoDocumento) && TipoDocumento != "-1")
            {
                whereCondition += "AND IDTABTIPDOC=$P{IDTABTIPDOC} ";
                ps.AddParam("IDTABTIPDOC", DbType.String, TipoDocumento);
            }

            DateTime data;
            if (!string.IsNullOrEmpty(Data) && DateTime.TryParse(Data, out data))
            {
                whereCondition += "AND DATDOC=$P{DATDOC} ";
                ps.AddParam("DATDOC", DbType.DateTime, data);
            }

            if (!string.IsNullOrEmpty(Riferimento))
            {
                whereCondition += "AND RIFERIMENTO LIKE $P{RIFERIMENTO} ";
                Riferimento = Riferimento.Trim() + "%";
                ps.AddParam("RIFERIMENTO", DbType.String, Riferimento);
            }

            if (!string.IsNullOrEmpty(Fornitore) && Fornitore != "-1")
            {
                whereCondition += "AND CODICECLIFO=$P{CODICECLIFO} ";
                ps.AddParam("CODICECLIFO", DbType.String, Fornitore);
            }

            string select = @"SELECT * FROM DITTA1.USR_ACQUISTIT " + whereCondition;


            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_ACQUISTIT);
            }

            select = @"SELECT * FROM DITTA2.USR_ACQUISTIT " + whereCondition;
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_ACQUISTIT);
            }
        }

        public void FillCLIFO(RvlDocumentiDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.CLIFO WHERE RAGIONESOC IS NOT NULL";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.CLIFO);
            }
        }

        public void FillTABTIPDOC(RvlDocumentiDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.TABTIPDOC ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.TABTIPDOC);
            }
        }

        public void FillMAGAZZ(RvlDocumentiDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.MAGAZZ ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZ);
            }
        }

        public void FillMAGAZZ(RvlDocumentiDS ds, List<string> IDMAGAZZ)
        {
            if (IDMAGAZZ.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDMAGAZZ);
            string select = @"SELECT * FROM GRUPPO.MAGAZZ WHERE IDMAGAZZ IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZ);
            }
        }
    }
}

