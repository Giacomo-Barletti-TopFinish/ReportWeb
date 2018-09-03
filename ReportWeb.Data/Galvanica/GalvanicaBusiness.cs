using MetalPlus.Kernel.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Galvanica
{
    public class GalvanicaBusiness : ReportWebBusinessBase
    {
        public GalvanicaBusiness() : base() { }

        [DataContext]
        public void FillRW_GALV_CONSUNTIVO(GalvanicaDS ds)
        {
            GalvanicaAdapter a = new GalvanicaAdapter(DbConnection, DbTransaction);
            a.FillRW_GALV_CONSUNTIVO(ds);
        }

        [DataContext]
        public void FillRW_GALV_FERMI(GalvanicaDS ds)
        {
            GalvanicaAdapter a = new GalvanicaAdapter(DbConnection, DbTransaction);
            a.FillRW_GALV_FERMI(ds);
        }

        [DataContext(true)]
        public void SalvaConsuntivo(long IdConsuntivo, string Inizio_turno, string Fine_turno, int Barre, string UIDUSER)
        {
            GalvanicaAdapter a = new GalvanicaAdapter(DbConnection, DbTransaction);
            a.SalvaConsuntivo(IdConsuntivo, Inizio_turno, Fine_turno, Barre, UIDUSER);
        }

        [DataContext(true)]
        public void SalvaFermo(long IdConsuntivo, string Tipo, string Ora, string Durata, string Motivo, string UIDUSER)

        {
            GalvanicaAdapter a = new GalvanicaAdapter(DbConnection, DbTransaction);
            a.SalvaFermo(IdConsuntivo, Tipo, Ora, Durata, Motivo, UIDUSER);
        }

        [DataContext(true)]
        public void CancellaConsuntivo(long IdConsuntivo)
        {
            GalvanicaAdapter a = new GalvanicaAdapter(DbConnection, DbTransaction);
            a.CancellaConsuntivo(IdConsuntivo);
        }

        [DataContext(true)]
        public void CancellaFermi(long IdConsuntivo)
        {
            GalvanicaAdapter a = new GalvanicaAdapter(DbConnection, DbTransaction);
            a.CancellaFermi(IdConsuntivo);
        }
    }
}
