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

namespace ReportWeb.Data.Preserie
{
    public class PreserieBusiness : ReportWebBusinessBase
    {
        public PreserieBusiness() : base() { }

        [DataContext]
        public void TrovaCommessaPerNome(string nomeCommessa, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.TrovaCommessaPerNome(nomeCommessa, ds);
        }

        [DataContext]
        public void TrovaCommessaPerModello(string Modello, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.TrovaCommessaPerModello(Modello, ds);
        }

        [DataContext]
        public void FillMAGAZZ(PreserieDS ds, List<string> IDMAGAZZ)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, IDMAGAZZ);
        }

        [DataContext]
        public void FillUSR_PRD_LANCIOD(string IDLANCIOD, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_LANCIOD(IDLANCIOD, ds);
        }

        [DataContext]
        public void FillUSR_PRD_MOVFASI(string IDLANCIOD, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASI(IDLANCIOD, ds);
        }

        [DataContext]
        public void FillRW_PR_DETTAGLIOByLancio(string IDLANCIOD, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillRW_PR_DETTAGLIOByLancio(IDLANCIOD, ds);
        }


        [DataContext]
        public void FillUSR_PRD_FASI(string IDLANCIOD, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FASI(IDLANCIOD, ds);
        }
        [DataContext]
        public void FillUSR_PRD_MOVFASIByBarcode(string Barcode, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASIByBarcode(Barcode, ds);
            a.FillUSR_PRD_FASIByBarcode(Barcode, ds);
            a.FillUSR_PRD_LANCIODByBarcode(Barcode, ds);
        }

        [DataContext]
        public void FillCLIFO(PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillCLIFO(ds);
        }

        [DataContext]
        public void FillUSR_PDM_FILES(PreserieDS ds, string IDMAGAZZ)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PDM_FILES(ds, IDMAGAZZ);
        }

        [DataContext]
        public void FillTABFAS(PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillTABFAS(ds);
        }

        [DataContext]
        public void FillRW_PR_MATERIALE(PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillRW_PR_MATERIALE(ds);
        }

        [DataContext]
        public void FillRW_PR_DETTAGLIO(string Barcode, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillRW_PR_DETTAGLIO(Barcode, ds);
        }

        [DataContext]
        public void FillRW_PR_PACKAGING(PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillRW_PR_PACKAGING(ds);
        }

        [DataContext]
        public void FillRW_PR_LAVORAZIONE(PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillRW_PR_LAVORAZIONE(ds);
        }

        [DataContext]
        public void FillUSR_PRD_RESOURCESF(PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_RESOURCESF(ds);
        }

        [DataContext]
        public void FillMetalliBase(PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillMetalliBase(ds);
        }

        [DataContext(true)]
        public void UpdateRW_PR(string tablename, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.UpdatePREDTable(tablename, ds);
        }

        [DataContext]
        public void FillDettaglioReparto(PreserieDS ds, DataTable dt, string barcode)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillDettaglioReparto(ds, dt, barcode);
        }
    }
}
