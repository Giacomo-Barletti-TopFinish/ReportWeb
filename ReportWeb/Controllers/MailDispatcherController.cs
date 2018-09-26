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

            List<MD_EMAILModel> emails = bll.LeggiMailAppese();

            List<MD_RICHIEDENTEModel> richiedenti = bll.LeggiRichiedenti();
            List<MD_GRUPPOModel> gruppi = bll.LeggiGruppi();

            ViewData.Add("MDGRUPPO", gruppi);
            ViewData.Add("MDRICHIEDENTI", richiedenti);
            return View(emails);
        }

        public ActionResult LeggiLog(decimal IDMAIL)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_LOGModel> model = bll.LeggiLog(IDMAIL);

            return PartialView("TabellaLog", model);
        }

        public ActionResult CreaNuovoGruppo(string Gruppo)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_GRUPPOModel> gruppi = bll.CreaNuovoGruppo(Gruppo.ToUpper());

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
            List<MD_GRUPPO_DESTINATARIOModel> destinatari = bll.AggiungiDestinatario(IDGRUPPO, Destinatario.Trim().ToUpper());

            ViewData.Add("MDDESTINATARI", destinatari);
            return View("TabellaDestinatari");
        }

        public ActionResult LeggiGruppiRichiedenti(decimal IDRICHIEDENTE)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_RICHIEDENTEModel> richiedenti = bll.LeggiRichiedenti();
            MD_RICHIEDENTEModel richiedente = richiedenti.Where(x => x.IDRICHIEDENTE == IDRICHIEDENTE).FirstOrDefault();

            if (richiedente == null) return null;

            List<MD_GRUPPO_RICHIEDENTEModel> gruppiRichiedente = richiedente.GRUPPI;

            MD_GRUPPOModel gruppoVuoto = new MD_GRUPPOModel();
            gruppoVuoto.IDGRUPPO = -1;
            gruppoVuoto.Nome = string.Empty;
            gruppoVuoto.Destinatari = new List<MD_GRUPPO_DESTINATARIOModel>();

            List<MD_GRUPPOModel> gruppi = bll.LeggiGruppi();
            gruppi.Insert(0, gruppoVuoto);

            ViewData.Add("MDGRUPPIRICHIEDENTI", gruppiRichiedente);
            ViewData.Add("MDGRUPPO", gruppi);
            return View("TabellaGruppiRichiedenti");

        }

        public ActionResult CreaNuovoRichiedente(string Richiedente)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_RICHIEDENTEModel> richiedenti = bll.CreaNuovoRichiedente(Richiedente.Trim().ToUpper());

            ViewData.Add("MDRICHIEDENTI", richiedenti);
            return View("TabellaRichiedenti");
        }

        public ActionResult RimuoviRichiedente(decimal IDRICHIEDENTE)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_RICHIEDENTEModel> richiedenti = bll.RimuoviRichiedente(IDRICHIEDENTE);

            ViewData.Add("MDRICHIEDENTI", richiedenti);
            return View("TabellaRichiedenti");
        }

        public ActionResult AggiungiGruppoRichiedente(decimal IDRICHIEDENTE, decimal IDGRUPPO, bool CC)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_RICHIEDENTEModel> richiedenti = bll.AggiungiGruppoRichiedente(IDRICHIEDENTE, IDGRUPPO, CC);

            MD_RICHIEDENTEModel richiedente = richiedenti.Where(x => x.IDRICHIEDENTE == IDRICHIEDENTE).FirstOrDefault();
            List<MD_GRUPPO_RICHIEDENTEModel> gruppiRichiedente = richiedente.GRUPPI;

            MD_GRUPPOModel gruppoVuoto = new MD_GRUPPOModel();
            gruppoVuoto.IDGRUPPO = -1;
            gruppoVuoto.Nome = string.Empty;
            gruppoVuoto.Destinatari = new List<MD_GRUPPO_DESTINATARIOModel>();

            List<MD_GRUPPOModel> gruppi = bll.LeggiGruppi();
            gruppi.Insert(0, gruppoVuoto);

            ViewData.Add("MDGRUPPIRICHIEDENTI", gruppiRichiedente);
            ViewData.Add("MDGRUPPO", gruppi);
            return View("TabellaGruppiRichiedenti");
        }

        public ActionResult RimuoviGruppoRichiedente(decimal IDGRRICH, decimal IDRICHIEDENTE)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            List<MD_RICHIEDENTEModel> richiedenti = bll.RimuoviGruppoRichiedente(IDGRRICH);

            MD_GRUPPOModel gruppoVuoto = new MD_GRUPPOModel();
            gruppoVuoto.IDGRUPPO = -1;
            gruppoVuoto.Nome = string.Empty;
            gruppoVuoto.Destinatari = new List<MD_GRUPPO_DESTINATARIOModel>();

            List<MD_GRUPPOModel> gruppi = bll.LeggiGruppi();
            gruppi.Insert(0, gruppoVuoto);

            MD_RICHIEDENTEModel richiedente = richiedenti.Where(x => x.IDRICHIEDENTE == IDRICHIEDENTE).FirstOrDefault();
            List<MD_GRUPPO_RICHIEDENTEModel> gruppiRichiedente = richiedente.GRUPPI;

            ViewData.Add("MDGRUPPIRICHIEDENTI", gruppiRichiedente);
            ViewData.Add("MDGRUPPO", gruppi);
            return View("TabellaGruppiRichiedenti");
        }

        public ActionResult CreaMail(string Richiedente, string Soggetto, string Corpo)
        {
            MailDispatcherBLL bll = new MailDispatcherBLL();
            decimal IDMAIL = bll.CreaEmail(Richiedente, Soggetto.Trim().ToUpper(), Corpo.Trim().ToUpper());
            if (IDMAIL >= 0)
            {
                bll.SottomettiEmail(IDMAIL);
            }

            List<MD_EMAILModel> emails = bll.LeggiMailAppese();
            return PartialView("TabellaMail", emails);
        }

    }
}