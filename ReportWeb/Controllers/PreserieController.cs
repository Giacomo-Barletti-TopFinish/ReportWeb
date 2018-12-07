using ReportWeb.Business;
using ReportWeb.Models.Preserie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class PreserieController : Controller
    {
        // GET: Preserie
        public ActionResult Analisi()
        {
            return View();
        }

        public ActionResult TrovaCommesse(bool RicercaPerCommessa, string NomeCommessa, string Articolo)
        {
            PreserieBLL bll = new PreserieBLL();
            List<Commessa> commesse = bll.TrovaCommessa(RicercaPerCommessa, NomeCommessa, Articolo);

            if(commesse.Count==1)
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
    }
}