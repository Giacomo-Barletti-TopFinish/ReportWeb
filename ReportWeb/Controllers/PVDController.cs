using ReportWeb.Business;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportWeb.Common.Helpers;

namespace ReportWeb.Controllers
{
    public class PVDController : ControllerBase
    {
        // GET: PVD
        public ActionResult Consuntivo()
        {
            VerificaAbilitazioneUtenteConUscita(5);

            PVDBLL bll = new PVDBLL();
            List<RWListItem> macchine = bll.CreaListaMacchine();
            macchine.Insert(0, new RWListItem(string.Empty, string.Empty));
            ViewData.Add("macchine", macchine);
            return View();
        }

        public ActionResult GetGrigliaMacchine(string IDRESOURCEF)
        {
            PVDBLL bll = new PVDBLL();
            List<PVDConsuntivoModel> consuntivo = bll.EstraiConsutivoMacchina(IDRESOURCEF);
            return PartialView("GetGrigliaMacchinePartial", consuntivo);
        }

        public ActionResult CancellaConsuntivo(int IdConsuntivo, string IDRESOURCEF)
        {
            PVDBLL bll = new PVDBLL();
            bll.CancellaRigaConsuntivo(IdConsuntivo);
            List<PVDConsuntivoModel> consunetivo = bll.EstraiConsutivoMacchina(IDRESOURCEF);
            return PartialView("GetGrigliaMacchinePartial", consunetivo);
        }

        public ActionResult SalvaConsuntivo(string IDRESOURCEF, string FinituraCodice, string FinituraDescrizione, string Tipo, string Giorno, string Inizio, string Fine, int Quantita, string Clienti, string Articolo, string Impegno)
        {
            PVDBLL bll = new PVDBLL();
            bll.SalvaConsuntivo(IDRESOURCEF, FinituraCodice, FinituraDescrizione, Tipo, Giorno, Inizio, Fine, Quantita, Clienti, Articolo, Impegno, ConnectedUser);
            List<PVDConsuntivoModel> consunetivo = bll.EstraiConsutivoMacchina(IDRESOURCEF);
            return PartialView("GetGrigliaMacchinePartial", consunetivo);
        }

        public ActionResult Report()
        {
            VerificaAbilitazioneUtenteConUscita(6);

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
            PVDBLL bll = new PVDBLL();
            List<RWListItem> macchine = bll.CreaListaMacchine();
            macchine.Insert(0, new RWListItem(string.Empty, string.Empty));
            ViewData.Add("settimane", settimane);
            ViewData.Add("anni", Anni);
            ViewData.Add("macchine", macchine);
            return View();
        }

        public ActionResult TrovaConsuntivo(int Anno, int Settimana, string Macchina)
        {
            PVDBLL bll = new PVDBLL();
            DateTime dataInizioSettimana = DateTimeHelper.PrimoGiornoSettimana(Anno, Settimana);
            DateTime dataFine = dataInizioSettimana.AddDays(7);
            PVDReportModel report = bll.EstraiConsutivo(dataInizioSettimana, dataFine,Macchina);
            ViewData.Add("dataInizio", dataInizioSettimana.ToShortDateString());
            ViewData.Add("dataFine", dataFine.ToShortDateString());
            return PartialView("GrigliaReportPartial", report);
        }
    }
}