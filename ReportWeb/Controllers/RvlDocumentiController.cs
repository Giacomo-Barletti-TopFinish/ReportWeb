using ReportWeb.Business;
using ReportWeb.Models.RvlDocumenti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class RvlDocumentiController : ControllerBase
    {
        // GET: RvlDocumenti
        public ActionResult BolleVendita()
        {
            VerificaAbilitazioneUtenteConUscita(28);
            return View();
        }

        public ActionResult TrovaBollaVendita(string NumeroDocumento)
        {
            RvlDocumentiBLL bll = new RvlDocumentiBLL();
            List<BollaVenditaModel> model = bll.TrovaBollaVendita(NumeroDocumento);
            return PartialView("TabellaBolleVendita",model);
        }
    }
}