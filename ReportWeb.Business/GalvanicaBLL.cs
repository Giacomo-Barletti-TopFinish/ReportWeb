using ReportWeb.Common.Helpers;
using ReportWeb.Data.Galvanica;
using ReportWeb.Entities;
using ReportWeb.Models;
using ReportWeb.Models.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class GalvanicaBLL
    {
        public void SalvaConsuntivo(string Inizio, string Fine, int Barre, string Fermi, string UIDUSER)
        {

            FermiJsonModel[] fermiJson = JSonSerializer.Deserialize<FermiJsonModel[]>(Fermi);
            using (GalvanicaBusiness bGalvanica = new GalvanicaBusiness())
            {
                try
                {
                    long idConsuntivo = bGalvanica.GetID();

                    bGalvanica.SalvaConsuntivo(idConsuntivo, Inizio, Fine, Barre, UIDUSER);

                    foreach (FermiJsonModel f in fermiJson)
                        bGalvanica.SalvaFermo(idConsuntivo, f.Tipo, f.Ora, f.Durata, f.Motivo, UIDUSER);
                }
                catch
                {
                    bGalvanica.Rollback();
                    throw;
                }
            }
        }

        public List<GalvanicaConsuntivoModel> EstraiConsutivoUltimoPeriodo(int NumeroGiorni)
        {
            List<GalvanicaConsuntivoModel> consuntivo = new List<GalvanicaConsuntivoModel>();

            GalvanicaDS ds = new GalvanicaDS();
            using (GalvanicaBusiness bGalvanica = new GalvanicaBusiness())
            {
                bGalvanica.FillRW_GALV_CONSUNTIVO(ds);
                bGalvanica.FillRW_GALV_FERMI(ds);
            }
            DateTime fine = DateTime.Now;
            DateTime inizio = fine.AddDays(NumeroGiorni * -1);

            foreach (GalvanicaDS.RW_GALV_CONSUNTIVORow m in ds.RW_GALV_CONSUNTIVO.Where(X => X.INIZIO_TURNO >= inizio && X.INIZIO_TURNO <= fine))
            {
                GalvanicaConsuntivoModel model = CreaGalvanicaConsuntivo(m, ds);
                consuntivo.Add(model);
            }
            return consuntivo;
        }

        public void CancellaRigaConsuntivo(int IdConsuntivo)
        {
            using (GalvanicaBusiness bGalvanica = new GalvanicaBusiness())
            {
                try
                {
                    bGalvanica.CancellaFermi(IdConsuntivo);
                    bGalvanica.CancellaConsuntivo(IdConsuntivo);
                }
                catch
                {
                    bGalvanica.Rollback();
                    throw;
                }
            }
        }

        private GalvanicaConsuntivoModel CreaGalvanicaConsuntivo(GalvanicaDS.RW_GALV_CONSUNTIVORow m, GalvanicaDS ds)
        {
            GalvanicaConsuntivoModel model = new GalvanicaConsuntivoModel();
            model.Barre = (int)m.BARRE;
            model.FineTurno = m.FINE_TURNO;
            model.InizioTurno = m.INIZIO_TURNO;
            model.IdConsuntivo = (int)m.IDCONSUNTIVO;

            model.Durata = DateTimeHelper.CalcolaDurata(m.INIZIO_TURNO, m.FINE_TURNO);

            model.Fermi = new List<FermoModel>();
            model.FermoTotale = new TimeSpan();
            foreach (GalvanicaDS.RW_GALV_FERMIRow f in ds.RW_GALV_FERMI.Where(x => x.IDCONSUNTIVO == m.IDCONSUNTIVO))
            {
                FermoModel fm = new FermoModel()
                {
                    Durata = f.DURATA,
                    IdConsuntivo = (int)f.IDCONSUNTIVO,
                    IdFermo = (int)f.IDFERMO,
                    Motivo = f.MOTIVO,
                    Ora = f.ORA,
                    Tipo = f.TIPO
                };
                TimeSpan ts = DateTimeHelper.ConvertiTimespan(f.DURATA);
                model.FermoTotale = model.FermoTotale.Add(ts);
                model.Fermi.Add(fm);
            }
            model.DurataEffettiva = model.Durata.Subtract(model.FermoTotale);
            model.BarreHH = Math.Round((decimal)(model.Barre / model.DurataEffettiva.TotalHours), 1);
            model.MinBarre = Math.Round((decimal)(model.DurataEffettiva.TotalMinutes / model.Barre), 1);

            return model;
        }

        public GalvanicaReportModel EstraiConsutivo(DateTime dataInizio, DateTime dataFine)
        {
            List<GalvanicaConsuntivoModel> consuntivo = new List<GalvanicaConsuntivoModel>();

            GalvanicaDS ds = new GalvanicaDS();
            using (GalvanicaBusiness bGalvanica = new GalvanicaBusiness())
            {
                bGalvanica.FillRW_GALV_CONSUNTIVO(ds);
                bGalvanica.FillRW_GALV_FERMI(ds);
            }

            List<GalvanicaDS.RW_GALV_CONSUNTIVORow> elementiTrovati = ds.RW_GALV_CONSUNTIVO.Where(X => X.INIZIO_TURNO >= dataInizio && X.INIZIO_TURNO < dataFine).ToList();

            int barreTotali = 0;
            TimeSpan durataTotale = new TimeSpan();
            TimeSpan fermoTotale = new TimeSpan();
            foreach (GalvanicaDS.RW_GALV_CONSUNTIVORow m in elementiTrovati)
            {
                GalvanicaConsuntivoModel model = CreaGalvanicaConsuntivo(m, ds);
                consuntivo.Add(model);
                barreTotali += model.Barre;
                durataTotale = durataTotale.Add(model.Durata);
                fermoTotale = fermoTotale.Add(model.FermoTotale);
            }
            GalvanicaReportModel report = new GalvanicaReportModel();
            report.Consuntivo = consuntivo;
            report.BarreTotali = barreTotali;
            report.FermoTotale = fermoTotale;
            report.TempoTotale = durataTotale;
            report.DurataEffettiva = report.TempoTotale.Subtract(report.FermoTotale);
            report.BarreHH = report.DurataEffettiva.TotalHours == 0 ? 0 : Math.Round((decimal)(report.BarreTotali / report.DurataEffettiva.TotalHours), 1);
            report.MinBarre = report.BarreTotali == 0 ? 0 : Math.Round((decimal)(report.DurataEffettiva.TotalMinutes / report.BarreTotali), 1);
            return report;
        }
    }
}
