using ReportWeb.Models;
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
            List<RWListItem> tipoFermo = new List<RWListItem>();
            tipoFermo.Add(new RWListItem(string.Empty, string.Empty));
            tipoFermo.Add(new RWListItem("Pianificato", "Pianificato"));
            tipoFermo.Add(new RWListItem("Guasto", "Guasto"));

            ViewData.Add("tipoFermo", tipoFermo);
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