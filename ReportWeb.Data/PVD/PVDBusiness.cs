using MetalPlus.Kernel.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.PVD
{
    public class PVDBusiness : ReportWebBusinessBase
    {
        public PVDBusiness() : base() { }

        [DataContext]
        public void FillRW_PVD_CONSUNTIVO(PVDDS ds)
        {
            PVDAdapter a = new PVDAdapter(DbConnection, DbTransaction);
            a.FillRW_PVD_CONSUNTIVO(ds);
        }

        [DataContext]
        public void FillUSR_PRD_RESOURCESF(PVDDS ds)
        {
            PVDAdapter a = new PVDAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_RESOURCESF(ds);
        }

        [DataContext(true)]
        public void CancellaRigaConsuntivo(int IdConsuntivo)
        {
            PVDAdapter a = new PVDAdapter(DbConnection, DbTransaction);
            a.CancellaRigaConsuntivo(IdConsuntivo);
        }

        [DataContext(true)]
        public void SalvaConsuntivo(string IDRESOURCEF, string FinituraCodice, string FinituraDescrizione, string Tipo, string Giorno, string Inizio, string Fine, int Quantita, string Clienti, string Articolo, int Impegno, string UIDUSER)
        {
            PVDAdapter a = new PVDAdapter(DbConnection, DbTransaction);
            a.SalvaConsuntivo(IDRESOURCEF, FinituraCodice, FinituraDescrizione, Tipo, Giorno, Inizio, Fine, Quantita, Clienti, Articolo, Impegno, UIDUSER);
        }

    }
}
