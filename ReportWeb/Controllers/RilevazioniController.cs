using ReportWeb.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class RilevazioniController : Controller
    {
        // GET: Rilevazioni
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CaricaSchedaLavoratore(string Barcode)
        {
            RilevazioneBLL bll = new RilevazioneBLL();
            string lavoratore = bll.RilevaUtente(Barcode);

            ViewData.Add("BarcodeLavoratore", Barcode);
            ViewData.Add("Lavoratore", lavoratore);

            return PartialView("CaricaSchedaLavoratore");
        }
    }
}