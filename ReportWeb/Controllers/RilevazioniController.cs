using ReportWeb.Business;
using ReportWeb.Models;
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
        public ActionResult Reparto(string Reparto)
        {
            if (string.IsNullOrEmpty(Reparto)) return null;
            RilevazioneBLL bll = new RilevazioneBLL();

            List<string> utenti = bll.GetUtentiPerReparto(Reparto);
            if (utenti.Count == 0) return null;



            return View(utenti);
        }
        // GET: Rilevazioni
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUtentePartial(string Utente)
        {
            ViewData.Add("Utente", Utente);
            RilevazioneBLL bll = new RilevazioneBLL();
            string lavorazione = bll.CaricaSchedaAperto(Utente);
            if(string.IsNullOrEmpty(lavorazione))
            {
                List<RWListItem> lavorazioni = bll.CaricaListaLavorazioni();
                ViewData.Add("Lavorazioni", lavorazioni);
                return PartialView("ApriLavorazionePartial");
            }
            ViewData.Add("Lavorazione", lavorazione);
            return PartialView("ChiudiLavorazionePartial");
        }

        public ActionResult CaricaSchedaLavoratore(string Barcode)
        {
            RilevazioneBLL bll = new RilevazioneBLL();
            string lavoratore = bll.RilevaUtente(Barcode);
            ViewData.Add("BarcodeLavoratore", Barcode);
            ViewData.Add("Lavoratore", lavoratore);

            List<RWListItem> lavorazioni = bll.CaricaListaLavorazioni();
            ViewData.Add("Lavorazioni", lavorazioni);
            string barcodeODL;
            string lavorazione = bll.CaricaSchedaAperto(Barcode, out barcodeODL);
            if (string.IsNullOrEmpty(lavorazione))
            {
                return PartialView("CaricaSchedaLavoratore");
            }
            ViewData.Add("Lavorazione", lavorazione);

            PreserieBLL bll1 = new PreserieBLL();
            ODLSchedaModel model = bll1.CaricaSchedaODL(barcodeODL, Settings.Default.RvlImageSite);

            return PartialView("ChiudiSchedaODL", model);

        }

        public ActionResult ApriLavorazioneUtente(string Utente, string Lavorazione)
        {
            RilevazioneBLL bll = new RilevazioneBLL();
            bool esito = bll.InizioAttivita(Utente, Lavorazione);

            return Content(esito.ToString());
        }
        public ActionResult ChiudiLavorazioneUtente(string Utente, string Nota, decimal Quantita)
        {
            RilevazioneBLL bll = new RilevazioneBLL();
            bool esito = bll.TerminaAttivita(Utente, Nota, Quantita);

            return Content(esito.ToString());
        }
        public ActionResult CaricaSchedaODL(string Barcode)
        {
            PreserieBLL bll = new PreserieBLL();
            ODLSchedaModel model = bll.CaricaSchedaODL(Barcode, Settings.Default.RvlImageSite);

            return PartialView("CaricaSchedaODL", model);
        }

        public ActionResult InizioAttivita(string BarcodeLavoratore, string BarcodeOLD, string Lavorazione)
        {
            RilevazioneBLL bll = new RilevazioneBLL();
            bool esito = bll.InizioAttivita(BarcodeLavoratore, BarcodeOLD, Lavorazione);

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