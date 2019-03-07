using ReportWeb.Business;
using ReportWeb.Common;
using ReportWeb.Models;
using ReportWeb.Models.Preserie;
using ReportWeb.Models.Preserie.JSON;
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

        public ActionResult SalvaDettagli(decimal IDDettaglio, string RepartoCodice, decimal Pezzi, decimal Packaging, decimal Peso,
            string Nota, string Dettagli, string IDPRDMOVFASE, string Barcode, string IdLancioD, string IdMagazz, string IDTABFAS)
        {
            PreserieBLL bll = new PreserieBLL();
            bll.SalvaDettagli(IDDettaglio, RepartoCodice, Pezzi, Packaging, Peso,
             Nota, Dettagli, IDPRDMOVFASE, Barcode, IdLancioD, IdMagazz, IDTABFAS, ConnectedUser);
            return null;
        }

        public ActionResult CaricaSchedaDettaglioReparto(string RepartoCodice, string IDTABFASS, string Barcode)
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

                        List<PulimentaturaJson> model = bll.FillRW_PR_PULIMENTATURA(Barcode);
                        return PartialView("PulimentaturaPartial", model);

                    }
                case Reparti.Vibratura:
                    {
                        if (IDTABFASS == "0000000060")//decapaggio
                        {
                            List<RWListItem> Lavorazioni = bll.CaricaLavorazioni(RepartoCodice);
                            ViewData.Add("Lavorazioni", Lavorazioni);

                            List<RWListItem> Interno = CreaListaSiNo();
                            ViewData.Add("Interno", Interno);

                            List<RWListItem> Tipologia = CreaListaTipoDecapaggio();
                            ViewData.Add("Tipologia", Tipologia);

                            List<DecapaggioJson> model = bll.FillRW_PR_DECAPAGGIO(Barcode);
                            return PartialView("DecapaggioPartial", model);
                        }
                        else
                        {
                            List<RWListItem> Lavorazioni = bll.CaricaLavorazioni(RepartoCodice);
                            ViewData.Add("Lavorazioni", Lavorazioni);

                            List<RWListItem> Automatico = CreaListaAcquaSecco();
                            ViewData.Add("AcquaSecco", Automatico);

                            List<RWListItem> Materiale = CaricaListaMaterialiVibratura();
                            ViewData.Add("Materiali", Materiale);

                            List<RWListItem> Vibratori = bll.CaricaListaMacchine(RepartoCodice);
                            ViewData.Add("Vibratori", Vibratori);

                            List<VibraturaJson> model = bll.FillRW_PR_VIBRATURA(Barcode);
                            return PartialView("VibraturaPartial", model);
                        }
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
                        List<ModelleriaJson> model = bll.FillRW_PR_MODELLERIA(Barcode);
                        return PartialView("ModelleriaPartial", model);
                    }

                case Reparti.Pressofusione:
                    {
                        List<RWListItem> Impronte = CreaLiCreaListaImpronte();
                        ViewData.Add("Impronte", Impronte);

                        List<RWListItem> Materiale = bll.CaricaListaMateriali();
                        ViewData.Add("Materiali", Materiale);

                        List<PressofusioneJson> model = bll.FillRW_PR_PRESSOFUSIONE(Barcode);
                        return PartialView("PressofusionePartial", model);
                    }

                case Reparti.Tornitura:
                    {
                        List<RWListItem> Macchine = bll.CaricaListaMacchine(RepartoCodice);
                        ViewData.Add("Macchine", Macchine);

                        List<RWListItem> Materiale = bll.CaricaListaMateriali();
                        ViewData.Add("Materiali", Materiale);

                        List<TornituraJson> model = bll.FillRW_PR_TORNITURA(Barcode);
                        return PartialView("TornituraPartial",model);
                    }

                case Reparti.Riprese:
                    {
                        if (IDTABFASS == "0000000146")//laser
                        {
                            List<RWListItem> TipoLaseratura = CreaListaLaseratura();
                            ViewData.Add("TipoLaseratura", TipoLaseratura);

                            List<RWListItem> Piazzatura = CreaListaPiazzaturaLaser();
                            ViewData.Add("Piazzatura", Piazzatura);

                            List<RWListItem> Magazzino = CreaListaSiNo();
                            ViewData.Add("Magazzino", Magazzino);

                            return PartialView("LaserPartial");
                        }
                        else
                        {
                            List<RWListItem> Lavorazioni = bll.CaricaLavorazioni(RepartoCodice);
                            ViewData.Add("Lavorazioni", Lavorazioni);

                            List<RWListItem> Piazzatura = CreaListaSiNo();
                            ViewData.Add("Piazzatura", Piazzatura);

                            return PartialView("RipresePartial");
                        }
                    }

                case Reparti.Verniciatura:
                    {

                        return PartialView("VerniciaturaPartial");
                    }

                case Reparti.GalvanicaAuto:
                    {

                        return PartialView("GalvanicaPartial");
                    }


                case Reparti.Smaltatura:
                    {
                        List<RWListItem> Piazzatura = CreaListaSiNo();
                        ViewData.Add("Piazzatura", Piazzatura);

                        return PartialView("SmaltaturaPartial");
                    }
                //case Reparti.Scopertura:
                //    {
                //        List<RWListItem> Lavorazioni = bll.CaricaLavorazioni("VIBR");
                //        ViewData.Add("Lavorazioni", Lavorazioni);

                //        List<RWListItem> Materiale = CaricaListaMaterialiVibratura();
                //        ViewData.Add("Materiali", Materiale);

                //        List<RWListItem> Vibratori = bll.CaricaListaMacchine(RepartoCodice);
                //        ViewData.Add("Vibratori", Vibratori);

                //        return PartialView("ScoperturaPartial");
                //    }
                case Reparti.Stampaggio:
                    {
                        List<RWListItem> MetalloBase = bll.CaricaListaMetalliBase();
                        ViewData.Add("MetalloBase", MetalloBase);

                        List<RWListItem> Materiali = CaricaListaMaterialiStampaggio();
                        ViewData.Add("Materiali", Materiali);

                        List<RWListItem> Impronte = CaricaListaMaterialiStampaggio();
                        ViewData.Add("Impronte", Impronte);

                        List<RWListItem> Tranciature = CaricaListaTranciature();
                        ViewData.Add("Tranciature", Tranciature);

                        return PartialView("StampaggioPartial");
                    }
                case Reparti.Saldatura:
                    {
                        List<RWListItem> Piazzatura = CreaListaSiNo();
                        ViewData.Add("Piazzatura", Piazzatura);

                        return PartialView("SaldaturaPartial");
                    }
                case Reparti.Montaggio:
                    {
                        List<RWListItem> difficolta = CaricaListaDifficoltà();
                        ViewData.Add("Difficolta", difficolta);

                        List<RWListItem> Colori = CaricaListaColori();
                        ViewData.Add("Colori", Colori);

                        return PartialView("MontaggioPartial");
                    }
                default: return null;
            }

        }

        private List<RWListItem> CaricaListaMaterialiStampaggio()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Billetta", "Billetta"));
            result.Add(new RWListItem("Piatto", "Piatto"));
            result.Add(new RWListItem("Piastra", "Piastra"));

            return result;
        }

        private List<RWListItem> CaricaListaDifficoltà()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("1", "1"));
            result.Add(new RWListItem("2", "2"));
            result.Add(new RWListItem("3", "3"));
            result.Add(new RWListItem("4", "4"));
            result.Add(new RWListItem("5", "5"));

            return result;
        }

        private List<RWListItem> CaricaListaColori()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Rosso - Alemar", "Rosso - Alemar"));
            result.Add(new RWListItem("Nero - NuovaVS", "Nero - NuovaVS"));
            result.Add(new RWListItem("Blu - RBL", "Blu - RBL"));

            return result;
        }

        private List<RWListItem> CaricaListaImpronteStampo()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("1", "1"));
            result.Add(new RWListItem("2", "2"));
            result.Add(new RWListItem("3", "3"));
            result.Add(new RWListItem("4", "4"));
            result.Add(new RWListItem("5", "5"));
            result.Add(new RWListItem("6", "6"));
            result.Add(new RWListItem("7", "7"));
            result.Add(new RWListItem("8", "8"));
            result.Add(new RWListItem("9", "9"));
            result.Add(new RWListItem("10", "10"));

            return result;
        }

        private List<RWListItem> CaricaListaTranciature()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("1", "1"));
            result.Add(new RWListItem("2", "2"));
            result.Add(new RWListItem("3", "3"));
            result.Add(new RWListItem("4", "4"));
            result.Add(new RWListItem("5", "5"));

            return result;
        }
        private List<RWListItem> CaricaListaMaterialiVibratura()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Sassi", "Sassi"));
            result.Add(new RWListItem("Ceramica", "Ceramica"));
            result.Add(new RWListItem("Legnetti", "Legnetti"));
            result.Add(new RWListItem("materiale1", "materiale1"));
            result.Add(new RWListItem("materiale2", "materiale2"));
            result.Add(new RWListItem("materiale3", "materiale3"));

            return result;
        }
        private List<RWListItem> CreaListaSiNo()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Si", "Si"));
            result.Add(new RWListItem("No", "No"));

            return result;
        }

        private List<RWListItem> CreaListaTipoDecapaggio()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Statico", "Statico"));
            result.Add(new RWListItem("Roto", "Roto"));

            return result;
        }

        private List<RWListItem> CreaListaLaseratura()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Incisione", "Incisione"));
            result.Add(new RWListItem("Satinatura", "Satinatura"));

            return result;
        }

        private List<RWListItem> CreaListaPiazzaturaLaser()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Divisore", "Divisore"));
            result.Add(new RWListItem("Pallet", "Pallet"));
            result.Add(new RWListItem("Piazzatura", "Piazzatura"));

            return result;
        }

        private List<RWListItem> CreaLiCreaListaImpronte()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("1", "1"));
            result.Add(new RWListItem("2", "2"));
            result.Add(new RWListItem("3", "3"));
            result.Add(new RWListItem("4", "4"));
            result.Add(new RWListItem("5", "5"));
            result.Add(new RWListItem("6", "6"));
            result.Add(new RWListItem("7", "7"));
            result.Add(new RWListItem("8", "8"));
            result.Add(new RWListItem("9", "9"));
            result.Add(new RWListItem("10", "10"));
            result.Add(new RWListItem("11", "11"));
            result.Add(new RWListItem("12", "12"));
            result.Add(new RWListItem("13", "13"));
            result.Add(new RWListItem("14", "14"));
            result.Add(new RWListItem("15", "15"));

            return result;
        }


        private List<RWListItem> CreaListaAutomaticoManuale()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Automatico", "Automatico"));
            result.Add(new RWListItem("Manuale", "Manuale"));

            return result;
        }

        private List<RWListItem> CreaListaAcquaSecco()
        {
            List<RWListItem> result = new List<RWListItem>();
            result.Add(new RWListItem(string.Empty, string.Empty));
            result.Add(new RWListItem("Acqua", "Acqua"));
            result.Add(new RWListItem("Secco", "Secco"));

            return result;
        }

    }
}