using ReportWeb.Business;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class RegistrazioneController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Ingresso()
        {
            return View();
        }
        public ActionResult Uscita()
        {
            RegistrazioneBLL bll = new RegistrazioneBLL();
            List<Registrazione> utenti = bll.FillRW_REGISTRAZIONE();

            return View(utenti);
        }

        public ActionResult RegistraIngresso(string Cognome, string Nome, string Azienda, string Tipo, string Numero, string Referente, decimal Tessera, string Ditta)
        {
            string messaggio;
            RegistrazioneBLL bll = new RegistrazioneBLL();
            if (bll.VerificaTesseraInUso(Tessera))
            {
                return Content("Tessera visitatore già in uso. Selezionare una tessera diversa.");
            }

            bool esito = bll.RegistraIngresso(Cognome.Trim().ToUpper(), Nome.Trim().ToUpper(), Azienda.Trim().ToUpper(), Tipo.Trim().ToUpper(), Numero.Trim().ToUpper(), Referente.Trim().ToUpper(), Tessera, Ditta, out messaggio);
            return Content(esito ? string.Empty : messaggio);

        }

        public ActionResult RegistraUscita(decimal IdRegistrazione)
        {
            string messaggio;
            RegistrazioneBLL bll = new RegistrazioneBLL();
            bool esito = bll.RegistraUscita(IdRegistrazione, out messaggio);
            return Content(esito ? string.Empty : messaggio);

        }

        public ActionResult Storico()
        {
            return View();
        }

        public ActionResult CaricaStorico(string Inizio, string Fine)
        {
            RegistrazioneBLL bll = new RegistrazioneBLL();
            List<StoricoRegistrazioneModel> risultati = bll.CaricaStorico(Inizio, Fine);
            return PartialView("StoricoPartial", risultati);
        }
    }
}