using ReportWeb.Business;
using ReportWeb.Common.Helpers;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class VerniciaturaController : ControllerBase
    {
        private const int _numeroGiorni = 15;
        // GET: Verniciatura
        public ActionResult Consuntivo()
        {
            VerificaAbilitazioneUtente(13);

            return View();
        }

        public ActionResult Report()
        {
            VerificaAbilitazioneUtente(14);

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
            VerniciaturaBLL bll = new VerniciaturaBLL();
            List<VerniciaturaConsuntivoModel> consuntivo = bll.EstraiConsutivoUltimoPeriodo(_numeroGiorni);
            return PartialView("GetGrigliaPartial", consuntivo);
        }

        public ActionResult CancellaConsuntivo(int IdConsuntivo)
        {
            VerniciaturaBLL bll = new VerniciaturaBLL();
            bll.CancellaRigaConsuntivo(IdConsuntivo);
            List<VerniciaturaConsuntivoModel> consuntivo = bll.EstraiConsutivoUltimoPeriodo(_numeroGiorni);
            return PartialView("GetGrigliaPartial", consuntivo);
        }

        public ActionResult SalvaConsuntivo(string Giorno, int QuantitaManuale, int Barre)
        {
            VerniciaturaBLL bll = new VerniciaturaBLL();
            bll.SalvaConsuntivo(Giorno, QuantitaManuale, Barre, ConnectedUser);
            List<VerniciaturaConsuntivoModel> consuntivo = bll.EstraiConsutivoUltimoPeriodo(_numeroGiorni);
            return PartialView("GetGrigliaPartial", consuntivo);
        }

        public ActionResult TrovaConsuntivo(int Anno, int Settimana)
        {
            VerniciaturaBLL bll = new VerniciaturaBLL();
            DateTime dataInizioSettimana = DateTimeHelper.PrimoGiornoSettimana(Anno, Settimana);
            DateTime dataFine = dataInizioSettimana.AddDays(7);
            VerniciaturaReportModel report = bll.EstraiConsutivo(dataInizioSettimana, dataFine);
            ViewData.Add("dataInizio", dataInizioSettimana.ToShortDateString());
            ViewData.Add("dataFine", dataFine.ToShortDateString());
            return PartialView("GrigliaReportPartial", report);
        }
    }
}