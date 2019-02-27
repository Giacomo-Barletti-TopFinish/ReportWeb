using ReportWeb.Business;
using ReportWeb.Common;
using ReportWeb.Models;
using ReportWeb.Models.Preserie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class PreserieController : ControllerBase
    {
        // GET: Preserie
        public ActionResult Analisi()
        {
            VerificaAbilitazioneUtenteConUscita(33);
            return View();
        }

        public ActionResult TrovaCommesse(bool RicercaPerCommessa, string NomeCommessa, string Articolo)
        {
            PreserieBLL bll = new PreserieBLL();
            List<Commessa> commesse = bll.TrovaCommessa(RicercaPerCommessa, NomeCommessa, Articolo);

            if (commesse.Count == 1)
            {
                Commessa commessa = bll.CaricaCommessa(commesse[0].IDLANCIOD);
                return PartialView("CommessaPartial", commessa);
            }
            return PartialView("GrigliaCommessePartial", commesse);
        }

        public ActionResult CaricaCommessa(string IDLANCIOD)
        {
            PreserieBLL bll = new PreserieBLL();
            Commessa commessa = bll.CaricaCommessa(IDLANCIOD);
            return PartialView("CommessaPartial", commessa);
        }

        public ActionResult InserimentoDati()
        {
            VerificaAbilitazioneUtenteConUscita(34);
            return View();
        }

        public ActionResult ModificaDati(string barcode)
        {
            ViewData.Add("Barcode", barcode);
            return View();
        }

        public ActionResult CaricaScheda(string Barcode)
        {
            PreserieBLL bll = new PreserieBLL();
            ODLSchedaModel model = bll.CaricaSchedaODL(Barcode, RvlImageSite);
            List<RWListItem> Packaging = bll.CaricaListaPackaging();

            ViewData.Add("Packaging", Packaging);

            return PartialView("CaricaSchedaPartial", model);
        }

        public ActionResult SalvaDettagli(string RepartoCodice, decimal Pezzi, string Packaging, decimal Peso,
            string Nota, string Dettagli, string IDPRDMOVFASE, string Barcode, string IdLancioD, string IdMagazz, string IDTABFAS)
        {
            PreserieBLL bll = new PreserieBLL();
            bll.SalvaDettagli(RepartoCodice, Pezzi, Packaging, Peso,
             Nota, Dettagli, IDPRDMOVFASE, Barcode, IdLancioD, IdMagazz, IDTABFAS, ConnectedUser);
            return null;
        }

        public ActionResult CaricaSchedaDettaglioReparto(string RepartoCodice)
        {
            List<RWListItem> SiNoListItem = CreaListaSiNo(); ;
            ViewData.Add("SiNoListItem", SiNoListItem);

            PreserieBLL bll = new PreserieBLL();
            switch (RepartoCodice)
            {
                case Reparti.Pulimentatura:
                    {
                        List<RWListItem> Lavorazioni = bll.CaricaLavorazioni(RepartoCodice);
                        ViewData.Add("Lavorazioni", Lavorazioni);

                        List<RWListItem> Automatico = CreaListaAutomaticoManuale(); ;
                        ViewData.Add("Automatico", Automatico);

                        return PartialView("PulimentaturaPartial");
                    }
                case Reparti.Vibratura:
                    {
                        List<RWListItem> Lavorazioni = bll.CaricaLavorazioni(RepartoCodice);
                        ViewData.Add("Lavorazioni", Lavorazioni);

                        List<RWListItem> Automatico = CreaListaAcquaSecco();
                        ViewData.Add("AcquaSecco", Automatico);

                        List<RWListItem> Materiale = bll.CaricaListaMateriali();
                        ViewData.Add("Materiali", Materiale);

                        List<RWListItem> Vibratori = bll.CaricaListaMacchine(RepartoCodice);
                        ViewData.Add("Vibratori", Vibratori);

                        return PartialView("VibraturaPartial");
                    }

                case Reparti.Modelleria:
                    {
                        List<RWListItem> Lavorazioni = bll.CaricaLavorazioni(RepartoCodice);
                        ViewData.Add("Lavorazioni", Lavorazioni);

                        List<RWListItem> Attrezzaggio = CreaListaSiNo();
                        ViewData.Add("Attrezzaggio", Attrezzaggio);

                        List<RWListItem> MetalloBase = bll.CaricaListaMetalliBase();
                        ViewData.Add("MetalloBase", MetalloBase);

                        List<RWListItem> Macchine = bll.CaricaListaMacchine(RepartoCodice);
                        ViewData.Add("Macchine", Macchine);

                        return PartialView("ModelleriaPartial");
                    }
                default: return null;
            }

        }

        private List<RWListItem> CreaListaSiNo()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Si", "1"));
            result.Add(new RWListItem("No", "2"));

            return result;
        }

        private List<RWListItem> CreaListaAutomaticoManuale()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Automatico", "1"));
            result.Add(new RWListItem("Manuale", "2"));

            return result;
        }

        private List<RWListItem> CreaListaAcquaSecco()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Acqua", "1"));
            result.Add(new RWListItem("Secco", "2"));

            return result;
        }

    }
}