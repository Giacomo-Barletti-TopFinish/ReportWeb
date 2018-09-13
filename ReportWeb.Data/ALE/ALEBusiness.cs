using ReportWeb.Common.Helpers;
using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.ALE
{
    public class ALEBusiness : ReportWebBusinessBase
    {
        public ALEBusiness() : base() { }

        [DataContext]
        public void FillUSR_CHECKQ_T(ALEDS ds, string Barcode)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_CHECKQ_T(ds, Barcode);
        }

        [DataContext]
        public void FillUSR_CHECKQ_T(ALEDS ds, List<string> IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_CHECKQ_T(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillUSR_CHECKQ_D(ALEDS ds, string IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_CHECKQ_D(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillUSR_CHECKQ_C(ALEDS ds, List<string> IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_CHECKQ_C(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillUSR_CHECKQ_C(ALEDS ds, string IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_CHECKQ_C(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillUSR_TAB_TIPODIFETTI(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_TAB_TIPODIFETTI(ds);
        }

        [DataContext]
        public void FillUSR_ANA_DIFETTI(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_ANA_DIFETTI(ds);
        }

        [DataContext]
        public void FillTABCAUMGT(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillTABCAUMGT(ds);
        }

        [DataContext]
        public void FillCLIFO(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillCLIFO(ds);
        }

        [DataContext]
        public void FillCLIFO(ALEDS ds, string Codice)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillCLIFO(ds, Codice);
        }

        [DataContext]
        public void FillUSR_PRD_FLUSSO_MOVFASI(ALEDS ds, string IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FLUSSO_MOVFASI(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillRW_ALE_DETT_COSTO(ALEDS ds, List<decimal> IDALEDETTAGLIO)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillRW_ALE_DETT_COSTO(ds, IDALEDETTAGLIO);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASI(ALEDS ds, string IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASI(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASI(ALEDS ds, List<string> IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASI(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillMAGAZZ(ALEDS ds, string IDMAGAZZ)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, IDMAGAZZ);
        }

        [DataContext]
        public void FillMAGAZZ(ALEDS ds, List<string> IDMAGAZZ)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, IDMAGAZZ);
        }

        [DataContext]
        public void FillUSR_PDM_FILES(ALEDS ds, string IDMAGAZZ)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_PDM_FILES(ds, IDMAGAZZ);
        }

        [DataContext]
        public void FillUSR_PDM_FILES(ALEDS ds, List<string> IDMAGAZZ)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_PDM_FILES(ds, IDMAGAZZ);
        }

        [DataContext(true)]
        public void SalvaInserimento(string Azienda, string Barcode, string IDCHECKQT, int Difettosi, int Inseriti, string Lavorante, string Nota, string UIDUSER)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.InsertRW_ALE_DETTAGLIO(Azienda, Barcode, IDCHECKQT, Difettosi, Inseriti, Lavorante, Nota, UIDUSER);
        }

        [DataContext]
        public void FillRW_ALE_DETTAGLIO(ALEDS ds, string STATO)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);

            a.FillRW_ALE_DETTAGLIO(ds, STATO);
        }

        [DataContext]
        public void FillRW_ALE_DETTAGLIOByBarcode(ALEDS ds, string Barcode)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillRW_ALE_DETTAGLIOByBarcode(ds, Barcode);
        }
        [DataContext(true)]
        public int CreaGruppo(string NotaAddebito, string Lavorante, string UIDUSER)
        {
            Int32 id = (int)GetID();
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.InsertRW_ALE_GRUPPO(id, NotaAddebito, Lavorante, UIDUSER);
            return id;
        }

        [DataContext(true)]
        public void UpdateRW_ALE_DETTAGLIO(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.UpdateALEDSTable(ds.RW_ALE_DETTAGLIO.TableName, ds);
        }

        [DataContext]
        public void FillRW_ALE_GRUPPO(ALEDS ds, List<decimal> IDALEGRUPPO)
        {
            ALEAdapter a = new Data.ALEAdapter(DbConnection, DbTransaction);
            a.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
        }

        [DataContext]
        public void FillRW_ALE_GRUPPO(ALEDS ds, decimal IDALEGRUPPO)
        {
            ALEAdapter a = new Data.ALEAdapter(DbConnection, DbTransaction);
            a.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
        }

        [DataContext(true)]
        public void UpdateRW_ALE_GRUPPO(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.UpdateALEDSTable(ds.RW_ALE_GRUPPO.TableName, ds);
        }
        [DataContext]
        public void FillRW_ALE_GRUPPO(ALEDS ds, bool Aperto)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillRW_ALE_GRUPPO(ds, Aperto);
        }

        [DataContext]
        public void FillRW_ALE_DETTAGLIO(ALEDS ds, decimal IDALEGRUPPO)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillRW_ALE_DETTAGLIO(ds, IDALEGRUPPO);
        }

        [DataContext]
        public void FillRW_ALE_DETTAGLIO(ALEDS ds, List<decimal> IDALEGRUPPO)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillRW_ALE_DETTAGLIO(ds, IDALEGRUPPO);
        }

        [DataContext]
        public void FillRW_ALE_DETTAGLIOByPK(ALEDS ds, decimal IdAleDettaglio)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillRW_ALE_DETTAGLIOByPK(ds, IdAleDettaglio);
        }
    }
}
