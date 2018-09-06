using ReportWeb.Business;
using ReportWeb.Models.ALE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class ALEController : ControllerBase
    {
        // GET: ALE
        public ActionResult Inserimento()
        {
            VerificaAbilitazioneUtente(16);
            return View();
        }
        public ActionResult Addebito()
        {
            VerificaAbilitazioneUtente(17);
            return View();
        }
        public ActionResult Valorizzazione()
        {
            VerificaAbilitazioneUtente(18);
            return View();
        }
        public ActionResult Conferma()
        {
            VerificaAbilitazioneUtente(19);
            return View();
        }
        public ActionResult Fatturazione()
        {
            VerificaAbilitazioneUtente(20);
            return View();
        }

        public ActionResult CaricaScheda(string Barcode)
        {
            ALEBLL bll = new ALEBLL();
            InserimentoModel model = bll.CaricaScheda(Barcode, RvlImageSite);


            return PartialView("CaricaSchedaPartial", model);
        }

        public ActionResult SalvaInserimento(string Barcode, string IDCHECKQT, int Difettosi, int Inseriti, string Lavorante, string Nota)
        {
            ALEBLL bll = new ALEBLL();
            bll.SalvaInserimento(Barcode, IDCHECKQT, Difettosi, Inseriti, Lavorante, Nota, ConnectedUser);

            return null;
        }
    }
}