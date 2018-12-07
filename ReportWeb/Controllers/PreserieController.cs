using ReportWeb.Business;
using ReportWeb.Models;
using ReportWeb.Models.Preserie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class PreserieController : ControllerBase
    {
        // GET: Preserie
        public ActionResult Analisi()
        {
            VerificaAbilitazioneUtenteConUscita(33);
            return View();
        }

        public ActionResult TrovaCommesse(bool RicercaPerCommessa, string NomeCommessa, string Articolo)
        {
            PreserieBLL bll = new PreserieBLL();
            List<Commessa> commesse = bll.TrovaCommessa(RicercaPerCommessa, NomeCommessa, Articolo);

            if (commesse.Count == 1)
            {
                Commessa commessa = bll.CaricaCommessa(commesse[0].IDLANCIOD);
                return PartialView("CommessaPartial", commessa);
            }
            return PartialView("GrigliaCommessePartial", commesse);
        }

        public ActionResult CaricaCommessa(string IDLANCIOD)
        {
            PreserieBLL bll = new PreserieBLL();
            Commessa commessa = bll.CaricaCommessa(IDLANCIOD);
            return PartialView("CommessaPartial", commessa);
        }

        public ActionResult InserimentoDati()
        {
            VerificaAbilitazioneUtenteConUscita(34);
            return View();
        }

        public ActionResult CaricaScheda(string Barcode)
        {
            PreserieBLL bll = new PreserieBLL();
            ODLSchedaModel model = bll.CaricaSchedaODL(Barcode, RvlImageSite);
            List<RWListItem> fasi = bll.CaricaTabFas(model.RepartoCodice);
            List<RWListItem> lavorantiEsterni = bll.CreaListaLavorantiEsterni();

            ViewData.Add("Fasi", fasi);
            ViewData.Add("LavorantiEsterni", lavorantiEsterni);

            return PartialView("CaricaSchedaPartial", model);
        }
    }
}