using ReportWeb.Business;
using ReportWeb.Models.Preserie;
using ReportWeb.Properties;
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

            string barcodeODL = bll.CaricaSchedaAperto(Barcode);
            if (string.IsNullOrEmpty(barcodeODL))
            {
                return PartialView("CaricaSchedaLavoratore");
            }

            PreserieBLL bllPr = new PreserieBLL();
            ODLSchedaModel model = bllPr.CaricaSchedaODL(barcodeODL, Settings.Default.RvlImageSite);

            return PartialView("ChiudiSchedaODL",model);

        }

        public ActionResult CaricaSchedaODL(string Barcode)
        {
            PreserieBLL bll = new PreserieBLL();
            ODLSchedaModel model = bll.CaricaSchedaODL(Barcode, Settings.Default.RvlImageSite);

            return PartialView("CaricaSchedaODL", model);
        }

        public ActionResult InizioAttivita(string BarcodeLavoratore, string BarcodeOLD)
        {
            RilevazioneBLL bll = new RilevazioneBLL();
            bool esito = bll.InizioAttivita(BarcodeLavoratore, BarcodeOLD);

            return Content(esito.ToString());
        }

        public ActionResult TerminaAttivita(string BarcodeLavoratore, string BarcodeOLD, string Nota, decimal Quantita)
        {
            RilevazioneBLL bll = new RilevazioneBLL();
            bool esito = bll.TerminaAttivita(BarcodeLavoratore, BarcodeOLD, Nota, Quantita);

            return Content(esito.ToString());
        }
    }
}