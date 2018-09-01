using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class GalvanicaController : ControllerBase
    {
        // GET: Galvanica
        public ActionResult Consuntivo()
        {
            VerificaAbilitazioneUtente(10);
            return View();
        }

        public ActionResult Report()
        {
            VerificaAbilitazioneUtente(11);
            return View();
        }
    }
}