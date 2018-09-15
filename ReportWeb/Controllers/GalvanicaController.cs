using ReportWeb.Business;
using ReportWeb.Common.Helpers;
using ReportWeb.Models;
using ReportWeb.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class GalvanicaController : ControllerBase
    {
        private const int _numeroGiorni = 15;
        // GET: Galvanica
        public ActionResult Consuntivo()
        {
            List<RWListItem> tipoFermo = new List<RWListItem>();
            tipoFermo.Add(new RWListItem(string.Empty, string.Empty));
            tipoFermo.Add(new RWListItem("Pianificato", "Pianificato"));
            tipoFermo.Add(new RWListItem("Guasto", "Guasto"));

            ViewData.Add("tipoFermo", tipoFermo);
            VerificaAbilitazioneUtenteConUscita(10);
            return View();
        }

        public ActionResult SalvaConsuntivo(string Inizio, string Fine, int Barre, string Fermi)
        {
            GalvanicaBLL bll = new GalvanicaBLL();
            bll.SalvaConsuntivo(Inizio, Fine, Barre, Fermi, ConnectedUser);
            List<GalvanicaConsuntivoModel> consuntivo = bll.EstraiConsutivoUltimoPeriodo(_numeroGiorni);
            return PartialView("GetGrigliaPartial", consuntivo);
        }

        public ActionResult Report()
        {
            VerificaAbilitazioneUtenteConUscita(11);

            List<RWListItem> settimane = new List<RWListItem>();
            settimane.Add(new RWListItem(string.Empty, string.Empty));
            for (int i = 1; i <= 52; i++)
                settimane.Add(new RWListItem(i.ToString(), i.ToString()));

            List<RWListItem> Anni = new List<RWListItem>();
            Anni.Add(new RWListItem(string.Empty, string.Empty));
            Anni.Add(new RWListItem("2018", "2018"));
            Anni.Add(new RWListItem("2019", "2019"));
            Anni.Add(new RWListItem("2020", "2020"));
            Anni.Add(new RWListItem("2020", "2020"));
            VerniciaturaBLL bll = new VerniciaturaBLL();
            ViewData.Add("settimane", settimane);
            ViewData.Add("anni", Anni);
            return View();
        }

        public ActionResult GetGriglia()
        {
            GalvanicaBLL bll = new GalvanicaBLL();
            List<GalvanicaConsuntivoModel> consuntivo = bll.EstraiConsutivoUltimoPeriodo(_numeroGiorni);
            return PartialView("GetGrigliaPartial", consuntivo);
        }

        public ActionResult CancellaConsuntivo(int IdConsuntivo)
        {
            GalvanicaBLL bll = new GalvanicaBLL();
            bll.CancellaRigaConsuntivo(IdConsuntivo);
            List<GalvanicaConsuntivoModel> consuntivo = bll.EstraiConsutivoUltimoPeriodo(_numeroGiorni);
            return PartialView("GetGrigliaPartial", consuntivo);
        }

        public ActionResult TrovaConsuntivo(int Anno, int Settimana)
        {
            GalvanicaBLL bll = new GalvanicaBLL();
            DateTime dataInizioSettimana = DateTimeHelper.PrimoGiornoSettimana(Anno, Settimana);
            DateTime dataFine = dataInizioSettimana.AddDays(7);
            GalvanicaReportModel report = bll.EstraiConsutivo(dataInizioSettimana, dataFine);
            ViewData.Add("dataInizio", dataInizioSettimana.ToShortDateString());
            ViewData.Add("dataFine", dataFine.ToShortDateString());
            return PartialView("GrigliaReportPartial", report);
        }

        public ActionResult ReportPDF(int Anno, int Settimana)
        {
            GalvanicaBLL bll = new GalvanicaBLL();
            DateTime dataInizioSettimana = DateTimeHelper.PrimoGiornoSettimana(Anno, Settimana);
            DateTime dataFine = dataInizioSettimana.AddDays(7);
            GalvanicaReportModel report = bll.EstraiConsutivo(dataInizioSettimana, dataFine);

            PDFHelper pdfHelper = new PDFHelper();
            byte[] fileContents = pdfHelper.EstraiGalvanicaReport(report, dataInizioSettimana, dataFine);

            return File(fileContents, "application/pdf", "Report.pdf");
        }
    }
}