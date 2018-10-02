using ReportWeb.Business;
using ReportWeb.Models.Magazzino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class MagazzinoController : Controller
    {
        // GET: Magazzino
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TrovaModello(string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.TrovaModello(Modello);
            return PartialView("TabellaGiacenze", model);
        }

        public ActionResult SalvaGiacenza(string Giacenze, string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.SalvaGiacenze(Giacenze, Modello);
            List<ModelloGiacenzaModel> model = bll.TrovaModello(Modello);
            return PartialView("TabellaGiacenze", model);
        }
    }
}