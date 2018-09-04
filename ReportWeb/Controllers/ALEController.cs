using ReportWeb.Business;
using ReportWeb.Models.ALE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class ALEController : Controller
    {
        // GET: ALE
        public ActionResult Inserimento()
        {
            return View();
        }
        public ActionResult Addebito()
        {
            return View();
        }
        public ActionResult Valorizzazione()
        {
            return View();
        }
        public ActionResult Conferma()
        {
            return View();
        }
        public ActionResult Fatturazione()
        {
            return View();
        }

        public ActionResult CaricaScheda( string Barcode)
        {
            ALEBLL bll = new ALEBLL();
            InserimentoModel model = bll.CaricaScheda(Barcode);


            return View("CaricaSchedaPartial",model);
        }
    }
}