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
            VerificaAbilitazioneUtenteConUscita(16);
            return View();
        }
        public ActionResult Addebito()
        {
            VerificaAbilitazioneUtenteConUscita(17);

           
            return View();
        }
        public ActionResult Valorizzazione()
        {
            VerificaAbilitazioneUtenteConUscita(18);
            return View();
        }
        public ActionResult Conferma()
        {
            VerificaAbilitazioneUtenteConUscita(19);
            return View();
        }
        public ActionResult Fatturazione()
        {
            VerificaAbilitazioneUtenteConUscita(20);
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

        public ActionResult CaricaAddebitoNuoviElementiPartial()
        {
            ALEBLL bAle = new ALEBLL();
            AddebitiModel model = bAle.LeggiSchedeDaAddebitare();

            return PartialView("AddebitoNuoviElementiPartial", model);
        }

        public ActionResult Addebita(string NotaGruppo, string Lavorante, string Addebiti)
        {
            ALEBLL bll = new ALEBLL();
            bll.Addebita(NotaGruppo, Lavorante, Addebiti, ConnectedUser);
            return null;
        }

        public ActionResult CaricaAddebitoGruppiPartial()
        {
            ALEBLL bll = new ALEBLL();
            List<GruppoModel> model = bll.LeggiGruppiAddebito();
            List<GruppoModel> altri = bll.LeggiAltriGruppiNonAddebito();
            model.AddRange(altri);
            return PartialView("AddebitoGruppiPartial", model);
        }

        public ActionResult AnnullaAddebito(int IDALEGRUPPO)
        {
            ALEBLL bll = new ALEBLL();
            bll.AnnullaAddebita(IDALEGRUPPO);
            return null;
        }

        public ActionResult CaricaGruppiDaValorizzare()
        {
            ALEBLL bll = new ALEBLL();
            List<GruppoModel> model = bll.LeggiGruppiAddebito();
            return PartialView("GruppiDaValorizzarePartial", model);
        }


        public ActionResult CaricaGruppiValorizzati()
        {
            ALEBLL bll = new ALEBLL();
            List<GruppoValorizzatoModel> model = bll.LeggiGruppiValorizzati();
            List<GruppoValorizzatoModel> altri = bll.LeggiAltriGruppiNonValorizzati();
            model.AddRange(altri);
            return PartialView("GruppiValorizzatiPartial", model);
        }

        public ActionResult Valorizza(string IDALEGRUPPO, string Dettagli, string Nota)
        {
            ALEBLL bll = new ALEBLL();
            bll.Valorizza( IDALEGRUPPO,  Dettagli,Nota,  ConnectedUser);
            return null;
        }

        public ActionResult AnnullaValorizzazione(string IDALEGRUPPO)
        {
            ALEBLL bll = new ALEBLL();
            bll.AnnullaValorizzazione(IDALEGRUPPO,ConnectedUser);
            return null;
        }
    }
}