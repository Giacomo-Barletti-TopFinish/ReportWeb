using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.RvlDocumenti
{
    public class RvlDocumentiBusiness : ReportWebBusinessBase
    {
        public RvlDocumentiBusiness() : base() { }
        [DataContext]
        public void FillTABCAUMGT(RvlDocumentiDS ds)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillTABCAUMGT(ds);

        }

        [DataContext]
        public void FillUSR_VENDITET(RvlDocumentiDS ds, string NumeroDocumento, string TipoDocumento, string Data, string Cliente)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_VENDITET(ds, NumeroDocumento, TipoDocumento, Data, Cliente);
        }

        [DataContext]
        public void FillUSR_VENDITED(RvlDocumentiDS ds, List<string> IDVENDITET)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_VENDITED(ds, IDVENDITET);
        }

        [DataContext]
        public void FillUSR_VENDITET(RvlDocumentiDS ds, List<string> IDVENDITET)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_VENDITET(ds, IDVENDITET);
        }

        [DataContext]
        public void FillUSR_PRD_FLUSSO_MOVMATE(RvlDocumentiDS ds, List<string> IDVENDITED)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FLUSSO_MOVMATE(ds, IDVENDITED);
        }

        [DataContext]
        public void FillUSR_PRD_FLUSSO_MOVMATEByIDPRDMOVMATE(RvlDocumentiDS ds, List<string> IDPRDMOVMATE)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FLUSSO_MOVMATEByIDPRDMOVMATE(ds, IDPRDMOVMATE);
        }

        [DataContext]
        public void FillUSR_PRD_MOVMATE(RvlDocumentiDS ds, List<string> IDPRDMOVMATE)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVMATE(ds, IDPRDMOVMATE);
        }

        [DataContext]
        public void FillUSR_PRD_MOVMATEByIDPRDMOVFASE(RvlDocumentiDS ds, List<string> IDPRDMOVFASE)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVMATEByIDPRDMOVFASE(ds, IDPRDMOVFASE);
        }

        [DataContext]
        public void FillUSR_PRD_FLUSSO_MOVFASI(RvlDocumentiDS ds, List<string> IDPRDMOVFASE)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FLUSSO_MOVFASI(ds, IDPRDMOVFASE);
        }

        [DataContext]
        public void FillUSR_PRD_FLUSSO_MOVFASIByIDACQUISTID(RvlDocumentiDS ds, List<string> IDACQUISTID)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_FLUSSO_MOVFASIByIDACQUISTID(ds, IDACQUISTID);
        }
        [DataContext]
        public void FillUSR_PRD_MOVFASI(RvlDocumentiDS ds, List<string> IDPRDMOVFASE)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_MOVFASI(ds, IDPRDMOVFASE);
        }

        [DataContext]
        public void FillUSR_ACQUISTID(RvlDocumentiDS ds, List<string> IDUSRACQUISTID)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_ACQUISTID(ds, IDUSRACQUISTID);
        }

        [DataContext]
        public void FillUSR_ACQUISTIDByIDUSRACQUISTIT(RvlDocumentiDS ds, List<string> IDUSRACQUISTIT)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_ACQUISTIDByIDUSRACQUISTIT(ds, IDUSRACQUISTIT);
        }

        [DataContext]
        public void FillUSR_ACQUISTIT(RvlDocumentiDS ds, List<string> IDUSRACQUISTIT)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_ACQUISTIT(ds, IDUSRACQUISTIT);
        }

        [DataContext]
        public void FillUSR_ACQUISTIT(RvlDocumentiDS ds, string NumeroDocumento, string TipoDocumento, string Data, string Riferimento, string Fornitore)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_ACQUISTIT(ds, NumeroDocumento, TipoDocumento, Data, Riferimento.ToUpper(), Fornitore);
        }

        [DataContext]
        public void FillCLIFO(RvlDocumentiDS ds)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillCLIFO(ds);
        }

        [DataContext]
        public void FillTABTIPDOC(RvlDocumentiDS ds)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillTABTIPDOC(ds);
        }

        [DataContext]
        public void FillMAGAZZ(RvlDocumentiDS ds)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds);
        }

        [DataContext]
        public void FillMAGAZZ(RvlDocumentiDS ds, List<string> IDMAGAZZ)
        {
            RvlDocumentiAdapter a = new RvlDocumentiAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, IDMAGAZZ);
        }
    }
}
