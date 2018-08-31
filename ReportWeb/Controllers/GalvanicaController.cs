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
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }
    }
}