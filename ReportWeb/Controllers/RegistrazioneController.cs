using ReportWeb.Business;
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
            List<Tuple<decimal, string, string>> utenti = bll.FillRW_REGISTRAZIONE();



            return View(utenti);
        }

        public ActionResult RegistraIngresso(string Cognome, string Nome, string Azienda, string Tipo, string Numero, string Referente)
        {
            string messaggio;
            RegistrazioneBLL bll = new RegistrazioneBLL();
            bool esito = bll.RegistraIngresso(Cognome.Trim().ToUpper(), Nome.Trim().ToUpper(), Azienda.Trim().ToUpper(), Tipo.Trim().ToUpper(), Numero.Trim().ToUpper(), Referente.Trim().ToUpper(), out messaggio);
            return Content(esito ? string.Empty : messaggio);

        }

        public ActionResult RegistraUscita(decimal IdRegistrazione)
        {
            string messaggio;
            RegistrazioneBLL bll = new RegistrazioneBLL();
            bool esito = bll.RegistraUscita(IdRegistrazione, out messaggio);
            return Content(esito ? string.Empty : messaggio);

        }
    }
}