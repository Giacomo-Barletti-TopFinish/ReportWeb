using ReportWeb.Business;
using ReportWeb.Models.MailDispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class MailDispatcherController : ControllerBase
    {
        // GET: MailDispatcher
        public ActionResult Index()
        {
            VerificaAbilitazioneUtenteConUscita(22);
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_APPLICAZIONEModel> applicazioni = bll.LeggiGruppiApplicazioni();
            List<MD_GRUPPOModel> gruppi = bll.LeggiGruppi();

            ViewData.Add("MDGRUPPO", gruppi);
            ViewData.Add("MDGRUPPIAPP", applicazioni);
            return View();
        }

        public ActionResult CreaNuovoGruppo(string Gruppo)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();            
            List<MD_GRUPPOModel> gruppi = bll.CreaNuovoGruppo(Gruppo);

            ViewData.Add("MDGRUPPO", gruppi);
            return View("TabellaGruppi");
        }

        public ActionResult RimuoviGruppo(decimal IDGRUPPO)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_GRUPPOModel> gruppi = bll.RimuoviGruppo(IDGRUPPO);

            ViewData.Add("MDGRUPPO", gruppi);
            return View("TabellaGruppi");
        }
    }
}