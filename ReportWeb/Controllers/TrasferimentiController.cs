using ReportWeb.Business;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class TrasferimentiController : ControllerBase
    {
        // GET: Trasferimenti
        public ActionResult Index()
        {
            VerificaAbilitazioneUtenteConUscita(41);
            TrasferimentiBLL bll = new TrasferimentiBLL();
            List<RWListItem> fornitori = bll.CaricaListaOperatori();
            fornitori.Insert(0, new RWListItem(string.Empty, "-1"));
            ViewData.Add("operatori", fornitori);
            return View();
        }

        public ActionResult EstraiDati(string DataInizio, string DataFine, string OperatoreInvio, string OperatoreRicezione, string ODL)
        {
            TrasferimentiBLL bll = new TrasferimentiBLL();
            List<TrasferimentoModel> model = bll.EstraiTrasferimenti(DataInizio, DataFine, OperatoreInvio, OperatoreRicezione, ODL.ToUpper());
            return PartialView("TrasferimentiPartial", model);
        }
    }
}