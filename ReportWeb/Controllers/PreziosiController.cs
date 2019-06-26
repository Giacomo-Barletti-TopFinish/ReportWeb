using ReportWeb.Business;
using ReportWeb.Models;
using ReportWeb.Models.Preziosi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class PreziosiController : ControllerBase
    {
        // GET: Preziosi
        public ActionResult INSERIMENTOCASSAFORTEA()
        {
            VerificaAbilitazioneUtenteConUscita(43);
            PreziosiBLL bll = new PreziosiBLL();
            List<RWListItem> preziosi = bll.CreaListaPreziosi();
            List<RWListItem> dareAvere = CreaListaMenuDareAvere();
            ViewData.Add("preziosi", preziosi);
            ViewData.Add("dareAvere", dareAvere);
            return View();
        }

        public ActionResult INSERIMENTOCASSAFORTEB()
        {
            VerificaAbilitazioneUtenteConUscita(44);
            PreziosiBLL bll = new PreziosiBLL();
            List<RWListItem> preziosi = bll.CreaListaPreziosi();
            ViewData.Add("preziosi", preziosi);
            return View();
        }

        public ActionResult MOVIMENTI()
        {
            VerificaAbilitazioneUtenteConUscita(45);
            PreziosiBLL bll = new PreziosiBLL();
            List<RWListItem> preziosi = bll.CreaListaPreziosi();
            ViewData.Add("preziosi", preziosi);
            return View();
        }

        public ActionResult GetSaldoMateriale(int IdPrezioso)
        {
            PreziosiBLL bll = new PreziosiBLL();
            Tuple<string, string> t = bll.GetSaldoMateriale(IdPrezioso);
            return Json(t);
        }
        public List<RWListItem> CreaListaMenuDareAvere()
        {
            List<RWListItem> dareAvere = new List<RWListItem>();
            dareAvere.Add(new RWListItem(string.Empty, "-1"));
            dareAvere.Add(new RWListItem("Versamento", "V"));
            dareAvere.Add(new RWListItem("Prelievo", "P"));

            return dareAvere;
        }

        public ActionResult SalvaMovimentoPreziosoCassaforteA(int IdPrezioso, string Operazione, decimal Quantita, string Causale)
        {
            PreziosiBLL bll = new PreziosiBLL();
            bool esito = bll.SalvaMovimentoPreziosoCassaforteA(IdPrezioso, Operazione, Quantita, Causale, ConnectedUser);
            return Content(esito.ToString());
        }

        public ActionResult SalvaMovimentoPreziosoCassaforteB(int IdPrezioso, string Operazione, decimal Quantita, string Causale)
        {
            PreziosiBLL bll = new PreziosiBLL();
            bool esito = bll.SalvaMovimentoPreziosoCassaforteB(IdPrezioso, Operazione, Quantita, Causale, ConnectedUser);
            return Content(esito.ToString());
        }

        public ActionResult CaricaSaldiCasseforti()
        {
            PreziosiBLL bll = new PreziosiBLL();
            List<SaldoCasseforti> saldi = bll.GetSaldiCompleti();

            return PartialView("SaldiCassefortiPartial", saldi);
        }

        public ActionResult CaricaMovimenti(string DataInizio, string DataFine)
        {
            PreziosiBLL bll = new PreziosiBLL();
            List<Movimenti> movimenti = bll.CaricaMovimenti(DataInizio, DataFine);

            return PartialView("CaricaMovimentiPartial", movimenti);
        }
    }
}