using ReportWeb.Data.Verniciatura;
using ReportWeb.Entities;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class VerniciaturaBLL
    {
        public List<VerniciaturaConsuntivoModel> EstraiConsutivoUltimoPeriodo(int NumeroGiorni)
        {
            List<VerniciaturaConsuntivoModel> consuntivo = new List<VerniciaturaConsuntivoModel>();

            VerniciaturaDS ds = new VerniciaturaDS();
            using (VerniciaturaBusiness bPDV = new VerniciaturaBusiness())
            {
                bPDV.FillRW_VERNICIATURA_CONSUNTIVO(ds);
            }
            DateTime fine = DateTime.Now;
            DateTime inizio = fine.AddDays(NumeroGiorni * -1);

            foreach (VerniciaturaDS.RW_VERNICIATURA_CONSUNTIVORow m in ds.RW_VERNICIATURA_CONSUNTIVO.Where(X => X.GIORNO >= inizio && X.GIORNO <= fine))
            {
                VerniciaturaConsuntivoModel model = new VerniciaturaConsuntivoModel();

                model.Giorno = m.GIORNO;
                model.QuantitaManuale = (int)m.QUANTITA_MANUALE;
                model.Barre = (int)m.BARRE;
                model.IdConsuntivo = (int)m.IDCONSUNTIVO;
                consuntivo.Add(model);
            }
            return consuntivo;
        }

        public void CancellaRigaConsuntivo(int IdConsuntivo)
        {
            using (VerniciaturaBusiness bPDV = new VerniciaturaBusiness())
            {
                bPDV.CancellaRigaConsuntivo(IdConsuntivo);
            }
        }

        public void SalvaConsuntivo(string Giorno, int QuantitaManuale, int Barre, string UIDUSER)
        {
            DateTime giorno = DateTime.Parse(Giorno);
            using (VerniciaturaBusiness bPDV = new VerniciaturaBusiness())
            {
                bPDV.SalvaConsuntivo(Giorno, QuantitaManuale, Barre, UIDUSER);
            }

        }


        public VerniciaturaReportModel EstraiConsutivo(DateTime dataInizio, DateTime dataFine)
        {
            List<VerniciaturaConsuntivoModel> consuntivo = new List<VerniciaturaConsuntivoModel>();

            VerniciaturaDS ds = new VerniciaturaDS();
            using (VerniciaturaBusiness bPDV = new VerniciaturaBusiness())
            {
                bPDV.FillRW_VERNICIATURA_CONSUNTIVO(ds);
            }

            List<VerniciaturaDS.RW_VERNICIATURA_CONSUNTIVORow> elementiTrovati = ds.RW_VERNICIATURA_CONSUNTIVO.Where(X => X.GIORNO >= dataInizio && X.GIORNO < dataFine).ToList();


            int quantitaTotale = 0;
            int barreTotali = 0;

            foreach (VerniciaturaDS.RW_VERNICIATURA_CONSUNTIVORow m in elementiTrovati)
            {
                VerniciaturaConsuntivoModel model = new VerniciaturaConsuntivoModel();
                model.Giorno = m.GIORNO;
                model.QuantitaManuale = (int)m.QUANTITA_MANUALE;
                model.Barre = (int)m.BARRE;
                model.IdConsuntivo = (int)m.IDCONSUNTIVO;
                consuntivo.Add(model);

                quantitaTotale += model.QuantitaManuale;
                barreTotali += model.Barre;
            }
            VerniciaturaReportModel report = new VerniciaturaReportModel();
            report.Cosuntivo = consuntivo;
            report.BarreTotali = barreTotali.ToString();
            report.QuantitaManualeTotale = quantitaTotale.ToString();

            return report;
        }

    }
}
