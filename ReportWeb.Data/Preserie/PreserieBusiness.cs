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
        public void FillPRE_DETTAGLIOByLancio(string IDLANCIOD, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillPRE_DETTAGLIOByLancio(IDLANCIOD, ds);
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
        public void FillPRE_DETTAGLIO(string Barcode, PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.FillPRE_DETTAGLIO(Barcode, ds);
        }

        [DataContext(true)]
        public void UpdatePRE_DETTAGLIO(PreserieDS ds)
        {
            PreserieAdapter a = new PreserieAdapter(DbConnection, DbTransaction);
            a.UpdatePREDTable(ds.PRE_DETTAGLIO.TableName, ds);
        }
    }
}
