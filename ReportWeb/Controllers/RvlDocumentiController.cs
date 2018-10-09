using ReportWeb.Business;
using ReportWeb.Models;
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
            RvlDocumentiBLL bll = new RvlDocumentiBLL();
            List<RWListItem> model = bll.CaricaTipoDocumentoBolleVendita();
            model.Insert(0, new RWListItem(string.Empty, "-1"));
            ViewData.Add("tipoDocumento", model);
            List<RWListItem> fornitori = bll.CaricaListaFornitori();
            fornitori.Insert(0, new RWListItem(string.Empty, "-1"));
            ViewData.Add("fornitori", fornitori);
            return View();
        }

        public ActionResult TrovaBollaVendita(string NumeroDocumento, string TipoDocumento, string Data, string Cliente)
        {
            RvlDocumentiBLL bll = new RvlDocumentiBLL();
            List<BollaVenditaModel> model = bll.TrovaBollaVendita(NumeroDocumento, TipoDocumento, Data, Cliente);
            return PartialView("TabellaBolleVendita", model);
        }

        public ActionResult BolleCarico()
        {
            VerificaAbilitazioneUtenteConUscita(29);
            RvlDocumentiBLL bll = new RvlDocumentiBLL();
            List<RWListItem> model = bll.CaricaTipoDocumentoBolleCarico();
            model.Insert(0, new RWListItem(string.Empty, "-1"));
            ViewData.Add("tipoDocumento", model);
            List<RWListItem> fornitori = bll.CaricaListaFornitori();
            fornitori.Insert(0, new RWListItem(string.Empty, "-1"));
            ViewData.Add("fornitori", fornitori);
            return View();
        }

        public ActionResult TrovaBollaCarico(string NumeroDocumento, string TipoDocumento, string Data, string Riferimento, string Fornitore)
        {
            RvlDocumentiBLL bll = new RvlDocumentiBLL();
            List<BollaCaricoModel> model = bll.TrovaBollaCarico(NumeroDocumento, TipoDocumento, Data, Riferimento, Fornitore);
            return PartialView("TabellaBolleCarico", model);
        }
    }
}