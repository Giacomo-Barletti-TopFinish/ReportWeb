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

        public ActionResult LeggiDestinatari(decimal IDGRUPPO)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_GRUPPOModel> gruppi = bll.LeggiGruppi();
            MD_GRUPPOModel gruppo = gruppi.Where(x => x.IDGRUPPO == IDGRUPPO).FirstOrDefault();

            if (gruppo == null) return null;

            List<MD_GRUPPO_DESTINATARIOModel> destinatari = gruppo.Destinatari;

            ViewData.Add("MDDESTINATARI", destinatari);
            return View("TabellaDestinatari");

        }

        public ActionResult RimuoviDestinatario(decimal IDDESTINATARIO)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_GRUPPO_DESTINATARIOModel> destinatari = bll.RimuoviDestinatario(IDDESTINATARIO);

            ViewData.Add("MDDESTINATARI", destinatari);
            return View("TabellaDestinatari");
        }

        public ActionResult AggiungiDestinatario(decimal IDGRUPPO, string Destinatario)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_GRUPPO_DESTINATARIOModel> destinatari = bll.AggiungiDestinatario(IDGRUPPO, Destinatario);

            ViewData.Add("MDDESTINATARI", destinatari);
            return View("TabellaDestinatari");
        }
    }
}