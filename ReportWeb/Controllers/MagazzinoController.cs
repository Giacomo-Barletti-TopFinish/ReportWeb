using ReportWeb.Business;
using ReportWeb.Models.Magazzino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class MagazzinoController : ControllerBase
    {
        // GET: Magazzino
        public ActionResult Index()
        {
            VerificaAbilitazioneUtenteConUscita(25);
            return View();
        }

        public ActionResult TrovaModello(string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.TrovaModelloGiacenza(Modello);
            return PartialView("TabellaGiacenze", model);
        }

        public ActionResult SalvaGiacenza(string Giacenze, string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.SalvaGiacenze(Giacenze, Modello);
            List<ModelloGiacenzaModel> model = bll.TrovaModelloGiacenza(Modello);
            return PartialView("TabellaGiacenze", model);
        }

        public ActionResult APPROVVIGIONAMENTI()
        {
            VerificaAbilitazioneUtenteConUscita(26);
            return View();
        }

        public ActionResult TrovaModelloApprovvigionamenti(string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloApprovvigionamentoModel> model = bll.TrovaModelloApprovvigionamento(Modello);
            return PartialView("TabellaApprovvigionamenti", model);
        }

        public ActionResult SalvaApprovvigionamenti(string Approvvigionamenti, string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.SalvaApprovvigionamenti(Approvvigionamenti, Modello);
            List<ModelloApprovvigionamentoModel> model = bll.TrovaModelloApprovvigionamento(Modello);
            return PartialView("TabellaApprovvigionamenti", model);
        }

    }
}