using ReportWeb.Common.Helpers;
using ReportWeb.Entities;
using ReportWeb.Models.ALE;
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
            string select = @"SELECT CT.*, 'METAL-PLUS' AS AZIENDA FROM DITTA1.USR_CHECKQ_T CT WHERE BARCODE = $P{BARCODE1}
                                UNION ALL
                              SELECT CT.*, 'TOP FINISH' AS AZIENDA FROM DITTA2.USR_CHECKQ_T CT WHERE BARCODE = $P{BARCODE2}";

            ParamSet ps = new ParamSet();
            ps.AddParam("BARCODE1", DbType.String, Barcode);
            ps.AddParam("BARCODE2", DbType.String, Barcode);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_CHECKQ_T);
            }
        }

        public void FillUSR_CHECKQ_T(ALEDS ds, List<string> IDCHECKQT)
        {
            if (IDCHECKQT.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDCHECKQT);
            string select = @"SELECT CT.*,'METAL-PLUS' AS AZIENDA FROM DITTA1.USR_CHECKQ_T CT WHERE IDCHECKQT IN ({0})
                                UNION ALL
                              SELECT CT.*,'TOP FINISH' AS AZIENDA FROM DITTA2.USR_CHECKQ_T CT WHERE IDCHECKQT IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
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

        public void FillUSR_CHECKQ_C(ALEDS ds, List<string> IDCHECKQT)
        {

            if (IDCHECKQT.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDCHECKQT);
            string select = @"SELECT * FROM DITTA1.USR_CHECKQ_C WHERE IDCHECKQT IN ({0})
                                UNION ALL
                              SELECT * FROM DITTA2.USR_CHECKQ_C WHERE IDCHECKQT IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
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

        public void FillRW_ALE_DETTAGLIO(ALEDS ds, string STATO)
        {
            string select = @"SELECT * FROM RW_ALE_DETTAGLIO WHERE STATO = $P{STATO} AND MANCANTE = '0'";
            ParamSet ps = new ParamSet();
            ps.AddParam("STATO", DbType.String, STATO);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_ALE_DETTAGLIO);
            }
        }

        public void FillRW_ALE_DETTAGLIO(ALEDS ds, string dataInizio, string dataFine, bool SoloMancante)
        {
            DateTime inizio = DateTime.Parse(dataInizio);
            DateTime fine = DateTime.Parse(dataFine);
            string select = @"SELECT * FROM RW_ALE_DETTAGLIO WHERE DATA_INSERIMENTO >= to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND DATA_INSERIMENTO <= to_date('{1}','DD/MM/YYYY HH24:MI:SS') ";

            string dtInizio = inizio.ToString("dd/MM/yyyy");
            dtInizio += " 00:00:01";
            string dtFine = fine.ToString("dd/MM/yyyy");
            dtFine += " 23:59:59";
            select = string.Format(select, dtInizio, dtFine);

            if (SoloMancante)
                select = select + " AND MANCANTE = '1'";
            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_ALE_DETTAGLIO);
            }
        }

        public void FillRW_ALE_DETTAGLIO(ALEDS ds, decimal IDALEGRUPPO)
        {
            string select = @"SELECT * FROM RW_ALE_DETTAGLIO WHERE IDALEGRUPPO = $P{IDALEGRUPPO} AND MANCANTE = '0'";
            ParamSet ps = new ParamSet();
            ps.AddParam("IDALEGRUPPO", DbType.Decimal, IDALEGRUPPO);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_ALE_DETTAGLIO);
            }
        }

        public void FillRW_ALE_DETTAGLIO(ALEDS ds, List<decimal> IDALEGRUPPO)
        {

            if (IDALEGRUPPO.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDALEGRUPPO);
            string select = @"SELECT * FROM RW_ALE_DETTAGLIO WHERE MANCANTE = '0' AND IDALEGRUPPO IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_ALE_DETTAGLIO);
            }
        }

        public void FillRW_ALE_DETTAGLIOByPK(ALEDS ds, decimal IdAleDettaglio)
        {
            string select = @"SELECT * FROM RW_ALE_DETTAGLIO WHERE IDALEDETTAGLIO = $P{IDALEDETTAGLIO} AND MANCANTE = '0'";
            ParamSet ps = new ParamSet();
            ps.AddParam("IDALEDETTAGLIO", DbType.Decimal, IdAleDettaglio);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_ALE_DETTAGLIO);
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

        public void FillCLIFO(ALEDS ds, string codice)
        {
            string select = @"SELECT * FROM GRUPPO.CLIFO WHERE CODICE = $P{CODICE}";

            ParamSet ps = new ParamSet();
            ps.AddParam("CODICE", DbType.String, codice);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.CLIFO);
            }
        }

        public void FillUSR_PRD_MOVFASI(ALEDS ds, string IDCHECKQT)
        {
            string select = @"SELECT CQ.IDCHECKQT,MF.* FROM DITTA1.USR_PRD_MOVFASI MF
                                INNER JOIN DITTA1.USR_PRD_FLUSSO_MOVFASI FMF ON   FMF.IDPRDMOVFASE= MF.IDPRDMOVFASE
                                INNER JOIN DITTA1.USR_CHECKQ_T CQ ON CQ.IDORIGINE_RIL = FMF.IDFLUSSOMOVFASE 
                                WHERE   CQ.ORIGINE_RIL  = 2 AND CQ.IDCHECKQT = $P{IDCHECKQT1} 
                                UNION ALL
                                SELECT CQ.IDCHECKQT,MF.* FROM DITTA2.USR_PRD_MOVFASI MF
                                INNER JOIN DITTA2.USR_PRD_FLUSSO_MOVFASI FMF ON   FMF.IDPRDMOVFASE= MF.IDPRDMOVFASE
                                INNER JOIN DITTA2.USR_CHECKQ_T CQ ON CQ.IDORIGINE_RIL = FMF.IDFLUSSOMOVFASE 
                                WHERE   CQ.ORIGINE_RIL  = 2 AND CQ.IDCHECKQT = $P{IDCHECKQT2}  ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDCHECKQT1", DbType.String, IDCHECKQT);
            ps.AddParam("IDCHECKQT2", DbType.String, IDCHECKQT);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }
        }

        public void FillUSR_PRD_MOVFASI(ALEDS ds, List<string> IDCHECKQT)
        {

            if (IDCHECKQT.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDCHECKQT);
            string select = @"SELECT CQ.IDCHECKQT,MF.* FROM DITTA1.USR_PRD_MOVFASI MF
                                INNER JOIN DITTA1.USR_PRD_FLUSSO_MOVFASI FMF ON   FMF.IDPRDMOVFASE= MF.IDPRDMOVFASE
                                INNER JOIN DITTA1.USR_CHECKQ_T CQ ON CQ.IDORIGINE_RIL = FMF.IDFLUSSOMOVFASE 
                                WHERE   CQ.ORIGINE_RIL  = 2 AND CQ.IDCHECKQT IN ({0}) 
                                UNION ALL
                                SELECT CQ.IDCHECKQT,MF.* FROM DITTA2.USR_PRD_MOVFASI MF
                                INNER JOIN DITTA2.USR_PRD_FLUSSO_MOVFASI FMF ON   FMF.IDPRDMOVFASE= MF.IDPRDMOVFASE
                                INNER JOIN DITTA2.USR_CHECKQ_T CQ ON CQ.IDORIGINE_RIL = FMF.IDFLUSSOMOVFASE 
                                WHERE   CQ.ORIGINE_RIL  = 2 AND CQ.IDCHECKQT IN ({0})  ";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_MOVFASI);
            }

        }

        public void FillUSR_PRD_FLUSSO_MOVFASI(ALEDS ds, string IDCHECKQT)
        {
            string select = @"  SELECT CQ.IDCHECKQT,FMF.* FROM ditta1.USR_PRD_FLUSSO_MOVFASI FMF
                                INNER JOIN ditta1.usr_checkq_t CQ ON CQ.IDORIGINE_RIL = FMF.IDFLUSSOMOVFASE 
                                WHERE   CQ.ORIGINE_RIL  = 2 AND CQ.IDDOCCHECKQ = '1' AND CQ.IDCHECKQT = $P{IDCHECKQT1} 
                                UNION ALL
                                SELECT CQ.IDCHECKQT,FMF.* FROM ditta2.USR_PRD_FLUSSO_MOVFASI FMF
                                INNER JOIN ditta2.usr_checkq_t CQ ON CQ.IDORIGINE_RIL = FMF.IDFLUSSOMOVFASE 
                                WHERE   CQ.ORIGINE_RIL  = 2 AND CQ.IDDOCCHECKQ = '1' AND CQ.IDCHECKQT = $P{IDCHECKQT1} ";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDCHECKQT1", DbType.String, IDCHECKQT);
            ps.AddParam("IDCHECKQT2", DbType.String, IDCHECKQT);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PRD_FLUSSO_MOVFASI);
            }
        }

        public void FillMAGAZZ(ALEDS ds, string IDMAGAZZ)
        {
            string select = @"  SELECT * FROM GRUPPO.MAGAZZ WHERE IDMAGAZZ = $P{IDMAGAZZ}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MAGAZZ);
            }
        }

        public void FillMAGAZZ(ALEDS ds, List<string> IDMAGAZZ)
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

        public void FillRW_ALE_DETT_COSTO(ALEDS ds, List<decimal> IDALEDETTAGLIO)
        {
            if (IDALEDETTAGLIO.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDALEDETTAGLIO);
            string select = @"  SELECT * FROM RW_ALE_DETT_COSTO WHERE IDALEDETTAGLIO IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_ALE_DETT_COSTO);
            }
        }

        public void FillUSR_PDM_FILES(ALEDS ds, string IDMAGAZZ)
        {
            string select = @"  select FI.*, IM.IDMAGAZZ from gruppo.USR_PDM_FILES FI
            INNER JOIN GRUPPO.USR_PDM_IMG_MAGAZZ IM ON IM.IDPDMFILE = FI.IDPDMFILE
            where IM.idmagazz = $P{IDMAGAZZ}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDMAGAZZ", DbType.String, IDMAGAZZ);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.USR_PDM_FILES);
            }
        }

        public void FillUSR_PDM_FILES(ALEDS ds, List<string> IDMAGAZZ)
        {
            if (IDMAGAZZ.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDMAGAZZ);
            string select = @"  select FI.*, IM.IDMAGAZZ from gruppo.USR_PDM_FILES FI
                                INNER JOIN GRUPPO.USR_PDM_IMG_MAGAZZ IM ON IM.IDPDMFILE = FI.IDPDMFILE
                                where IM.idmagazz IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PDM_FILES);
            }

        }

        public void InsertRW_ALE_DETTAGLIO(string Azienda, string Barcode, string IDCHECKQT, int Difettosi, int Inseriti, string Lavorante, bool Mancante, string Nota, string UIDUSER)
        {

            string insert = @"INSERT INTO RW_ALE_DETTAGLIO (AZIENDA,BARCODE,IDCHECKQT,QUANTITADIFETTOSI,QUANTITAINSERITA,NOTAINSERIMENTO,LAVORANTE, STATO, DATA_INSERIMENTO,UIDUSER_INSERIMENTO,MANCANTE) VALUES
                                            ($P<AZIENDA>,$P<BARCODE>,$P<IDCHECKQT>,$P<QUANTITADIFETTOSI>,$P<QUANTITAINSERITA>,$P<NOTA>,$P<LAVORANTE>, $P<STATO>,$P<NOW>,$P<UIDUSER>,$P{MANCANTE})";
            ParamSet ps = new ParamSet();
            ps.AddParam("AZIENDA", DbType.String, Azienda);
            ps.AddParam("BARCODE", DbType.String, Barcode);
            ps.AddParam("IDCHECKQT", DbType.String, IDCHECKQT);
            ps.AddParam("QUANTITADIFETTOSI", DbType.Int32, Difettosi);
            ps.AddParam("QUANTITAINSERITA", DbType.Int32, Inseriti);
            ps.AddParam("NOTA", DbType.String, Nota);
            ps.AddParam("LAVORANTE", DbType.String, Lavorante);
            ps.AddParam("STATO", DbType.String, ALEStatoDettaglio.INSERITO);
            ps.AddParam("NOW", DbType.DateTime, DateTime.Now);
            ps.AddParam("UIDUSER", DbType.String, UIDUSER);
            ps.AddParam("MANCANTE", DbType.String, Mancante ? "1" : "0");

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void FillRW_ALE_DETTAGLIOByBarcode(ALEDS ds, string Barcode)
        {
            string select = @"SELECT * FROM RW_ALE_DETTAGLIO WHERE BARCODE = $P{BARCODE}";
            ParamSet ps = new ParamSet();
            ps.AddParam("BARCODE", DbType.String, Barcode);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_ALE_DETTAGLIO);
            }
        }

        public void InsertRW_ALE_GRUPPO(int IDALEGRUPPO, string NotaAddebito, string lavorante, string UIDUSER)
        {

            string insert = @"INSERT INTO RW_ALE_GRUPPO (IDALEGRUPPO,NOTA_ADDEBITO,APERTO, DATA_ADDEBITO,LAVORANTE,UIDUSER,UIDUSER_ADDEBITO,DATA_CREAZIONE) VALUES
                                            ($P<IDALEGRUPPO>,$P<NOTA_ADDEBITO>,$P<APERTO>,$P<NOW>,$P<LAVORANTE>,$P<UIDUSER>,$P<UIDUSER_ADDEBITO>,$P<DATA_CREAZIONE>)";
            ParamSet ps = new ParamSet();
            ps.AddParam("IDALEGRUPPO", DbType.Int32, IDALEGRUPPO);
            ps.AddParam("NOTA_ADDEBITO", DbType.String, NotaAddebito);
            ps.AddParam("APERTO", DbType.String, 0);
            ps.AddParam("NOW", DbType.DateTime, DateTime.Now);
            ps.AddParam("UIDUSER", DbType.String, UIDUSER);
            ps.AddParam("UIDUSER_ADDEBITO", DbType.String, UIDUSER);
            ps.AddParam("DATA_CREAZIONE", DbType.DateTime, DateTime.Now);
            ps.AddParam("LAVORANTE", DbType.String, lavorante);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }


        public void UpdateALEDSTable(string tablename, ALEDS ds)
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

        public void FillRW_ALE_GRUPPO(ALEDS ds, List<decimal> IDALEGRUPPO)
        {
            if (IDALEGRUPPO.Count > 0)
            {
                string result = ConvertToStringForInCondition(IDALEGRUPPO);
                string select = @"SELECT * FROM RW_ALE_GRUPPO WHERE IDALEGRUPPO in ({0})";
                select = string.Format(CultureInfo.InvariantCulture, select, result);

                using (DbDataAdapter da = BuildDataAdapter(select))
                {
                    da.Fill(ds.RW_ALE_GRUPPO);
                }
            }
        }

        public void FillRW_ALE_GRUPPO(ALEDS ds, decimal IDALEGRUPPO)
        {
            string select = @"SELECT * FROM RW_ALE_GRUPPO WHERE IDALEGRUPPO = $P{IDALEGRUPPO}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDALEGRUPPO", DbType.Decimal, IDALEGRUPPO);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_ALE_GRUPPO);
            }
        }

        public void FillRW_ALE_GRUPPO(ALEDS ds, bool Aperto)
        {
            string select = @"SELECT * FROM RW_ALE_GRUPPO WHERE APERTO= $P{APERTO}";

            ParamSet ps = new ParamSet();
            ps.AddParam("APERTO", DbType.String, Aperto ? "1" : "0");

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.RW_ALE_GRUPPO);
            }

        }

        public void FillRW_ALE_GRUPPOFatturato(ALEDS ds, DateTime DataInizio, DateTime DataFine)
        {
            string select = @"SELECT * FROM RW_ALE_GRUPPO WHERE DATA_FATTURAZIONE >= to_date('{0}','DD/MM/YYYY') AND DATA_FATTURAZIONE <= to_date('{1}','DD/MM/YYYY') ";

            string dtInizio = DataInizio.ToString("dd/MM/yyyy");
            string dtFine = DataFine.ToString("dd/MM/yyyy");
            select = string.Format(select, dtInizio, dtFine);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_ALE_GRUPPO);
            }

        }

        public void FillTABFAS(ALEDS ds)
        {
            string select = @"SELECT * FROM GRUPPO.TABFAS ";


            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.TABFAS);
            }
        }

        public void FillUSR_PRD_FASI(ALEDS ds, List<string> IDPRDMOVFASE)
        {
            if (IDPRDMOVFASE.Count > 0)
            {
                string result = ConvertToStringForInCondition(IDPRDMOVFASE);
                string select = @"SELECT FFS.*  
                                    FROM DITTA1.USR_PRD_FASI FFS 
                                    WHERE FFS.IDLANCIOD IN (
                                        SELECT DISTINCT IDLANCIOD 
                                            FROM DITTA1.USR_PRD_FASI F
                                            INNER JOIN DITTA1.USR_PRD_MOVFASI MF ON MF.IDPRDFASE = F.IDPRDFASE
                                            WHERE MF.IDPRDMOVFASE IN ({0}))
                                UNION ALL

                                 SELECT FFS.*  
                                    FROM DITTA2.USR_PRD_FASI FFS 
                                    WHERE FFS.IDLANCIOD IN (
                                        SELECT DISTINCT IDLANCIOD 
                                            FROM DITTA2.USR_PRD_FASI F
                                            INNER JOIN DITTA2.USR_PRD_MOVFASI MF ON MF.IDPRDFASE = F.IDPRDFASE
                                            WHERE MF.IDPRDMOVFASE IN ({0})) ";

                select = string.Format(CultureInfo.InvariantCulture, select, result);

                using (DbDataAdapter da = BuildDataAdapter(select))
                {
                    da.Fill(ds.USR_PRD_FASI);
                }
            }
        }


    }
}
