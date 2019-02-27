using ReportWeb.Common;
using ReportWeb.Common.Helpers;
using ReportWeb.Data.Preserie;
using ReportWeb.Entities;
using ReportWeb.Models;
using ReportWeb.Models.Preserie;
using ReportWeb.Models.Preserie.JSON;
using ReportWeb.Views.Preserie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class PreserieBLL
    {
        public List<Commessa> TrovaCommessa(bool RicercaPerCommessa, string Commessa, string Articolo)
        {
            List<Commessa> commesse = new List<Commessa>();
            PreserieDS ds = new PreserieDS();

            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                if (RicercaPerCommessa)
                    bPreserie.TrovaCommessaPerNome(Commessa, ds);
                else
                    bPreserie.TrovaCommessaPerModello(Articolo, ds);

                if (ds.USR_PRD_LANCIOD.Count == 0)
                    return commesse;

                List<string> IDMAGAZZ = ds.USR_PRD_LANCIOD.Select(x => x.IDMAGAZZ).ToList();
                bPreserie.FillMAGAZZ(ds, IDMAGAZZ);

                foreach (PreserieDS.USR_PRD_LANCIODRow lanciod in ds.USR_PRD_LANCIOD)
                {
                    Commessa commessa = CreaCommessa(lanciod, ds);
                    commesse.Add(commessa);
                }

            }
            return commesse;
        }

        public Commessa CaricaCommessa(string IDLANCIOD)
        {

            PreserieDS ds = new PreserieDS();

            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                bPreserie.FillUSR_PRD_LANCIOD(IDLANCIOD, ds);
                bPreserie.FillUSR_PRD_FASI(IDLANCIOD, ds);
                bPreserie.FillUSR_PRD_MOVFASI(IDLANCIOD, ds);
                bPreserie.FillRW_PR_DETTAGLIOByLancio(IDLANCIOD, ds);
                bPreserie.FillTABFAS(ds);
                bPreserie.FillCLIFO(ds);

                if (ds.USR_PRD_LANCIOD.Count == 0)
                    return null;

                List<string> IDMAGAZZ = ds.USR_PRD_LANCIOD.Select(x => x.IDMAGAZZ).ToList();
                List<string> IDMAGAZZFASE = ds.USR_PRD_FASI.Select(x => x.IDMAGAZZ).ToList();
                List<string> IDMAGAZZMOVFASE = ds.USR_PRD_MOVFASI.Select(x => x.IDMAGAZZ).ToList();

                IDMAGAZZ.AddRange(IDMAGAZZFASE);
                IDMAGAZZ.AddRange(IDMAGAZZMOVFASE);
                IDMAGAZZ = IDMAGAZZ.Distinct().ToList();
                bPreserie.FillMAGAZZ(ds, IDMAGAZZ);

                PreserieDS.USR_PRD_LANCIODRow lanciod = ds.USR_PRD_LANCIOD.Where(x => x.IDLANCIOD == IDLANCIOD).FirstOrDefault();
                if (lanciod != null)
                    return CreaCommessa(lanciod, ds);

            }
            return null;
        }
        private Commessa CreaCommessa(PreserieDS.USR_PRD_LANCIODRow lanciod, PreserieDS ds)
        {
            if (lanciod == null) return null;

            Commessa commessa = new Commessa();
            commessa.IDLANCIOT = lanciod.IDLANCIOT;
            commessa.IDLANCIOD = lanciod.IDLANCIOD;
            commessa.Articolo = new Articolo();
            if (!lanciod.IsIDMAGAZZNull())
            {
                PreserieDS.MAGAZZRow magazz = ds.MAGAZZ.Where(x => x.IDMAGAZZ == lanciod.IDMAGAZZ).FirstOrDefault();
                commessa.Articolo = CreaArticolo(magazz);
            }

            commessa.Quantita = lanciod.QTALANCIO;

            commessa.DataCommessa = string.Empty;
            commessa.DataInizio = string.Empty;
            commessa.DataFine = string.Empty;

            if (!lanciod.IsDATACOMMESSANull())
                commessa.DataCommessa = lanciod.DATACOMMESSA.ToShortDateString();

            if (!lanciod.IsDATAINIZIOPRODNull())
                commessa.DataInizio = lanciod.DATAINIZIOPROD.ToShortDateString();

            if (!lanciod.IsDATAFINEPRODNull())
                commessa.DataFine = lanciod.DATAFINEPROD.ToShortDateString();

            commessa.NomeCommessa = lanciod.IsNOMECOMMESSANull() ? string.Empty : lanciod.NOMECOMMESSA;
            commessa.Riferimento = lanciod.IsRIFERIMENTONull() ? string.Empty : lanciod.RIFERIMENTO;
            commessa.Lavorazioni = new List<Lavorazione>();
            int sequenzaLavorazione = 1;
            PreserieDS.USR_PRD_FASIRow faseRoot = ds.USR_PRD_FASI.Where(x => x.IDLANCIOD == lanciod.IDLANCIOD && x.ROOTSN == 1).FirstOrDefault();
            if (faseRoot != null)
            {
                Lavorazione lavorazioneRoot = CreaLavorazione(faseRoot, sequenzaLavorazione.ToString(), ds);
                sequenzaLavorazione++;
                commessa.Lavorazioni.Add(lavorazioneRoot);

                List<PreserieDS.USR_PRD_FASIRow> faseFiglie = ds.USR_PRD_FASI.Where(x => !x.IsIDPRDFASEPADRENull() && x.IDPRDFASEPADRE == faseRoot.IDPRDFASE).ToList();
                EspandiAlberoFasi(faseFiglie, ds, sequenzaLavorazione.ToString(), commessa);

            }

            return commessa;
        }

        private void EspandiAlberoFasi(List<PreserieDS.USR_PRD_FASIRow> faseFiglie, PreserieDS ds, string radice, Commessa commessa)
        {
            int sequenzaLavorazione = 1;
            foreach (PreserieDS.USR_PRD_FASIRow faseFiglia in faseFiglie)
            {
                string nuovaRadice = string.Format("{0}.{1}", radice, sequenzaLavorazione);
                Lavorazione lavorazione = CreaLavorazione(faseFiglia, nuovaRadice, ds);
                sequenzaLavorazione++;
                commessa.Lavorazioni.Add(lavorazione);
                List<PreserieDS.USR_PRD_FASIRow> figlie = ds.USR_PRD_FASI.Where(x => !x.IsIDPRDFASEPADRENull() && x.IDPRDFASEPADRE == faseFiglia.IDPRDFASE).ToList();
                EspandiAlberoFasi(figlie, ds, nuovaRadice, commessa);
            }
        }

        private Lavorazione CreaLavorazione(PreserieDS.USR_PRD_FASIRow fase, string sequenza, PreserieDS ds)
        {
            if (fase == null) return null;

            Lavorazione lavorazione = new Lavorazione();

            lavorazione.IDPRDFASE = fase.IDPRDFASE;
            lavorazione.Sequenza = sequenza;
            lavorazione.IDMAGAZZ = fase.IDMAGAZZ;
            lavorazione.Reparto = fase.CODICECLIFO;
            lavorazione.Quantita = fase.QTA;
            lavorazione.QuantitaNetta = fase.QTANET;
            lavorazione.QuantitaTerminata = fase.QTATER;
            lavorazione.QuantitaDaTerminare = fase.QTADATER;

            lavorazione.DataInizio = fase.IsDATAINIZIONull() ? string.Empty : fase.DATAINIZIO.ToShortDateString();
            lavorazione.DataFine = fase.IsDATAFINENull() ? string.Empty : fase.DATAFINE.ToShortDateString();

            lavorazione.Offset = fase.OFFSETTIME;
            lavorazione.Leadtime = fase.LEADTIME;

            PreserieDS.USR_PRD_MOVFASIRow movFase = ds.USR_PRD_MOVFASI.Where(x => x.IDPRDFASE == fase.IDPRDFASE).FirstOrDefault();

            if (movFase != null)
            {
                lavorazione.Odl = CreaOdl(movFase);
                if (!movFase.IsBARCODENull())
                {
                    lavorazione.Dettagli = CreaListaDettaglio(movFase.BARCODE, ds);
                }

            }

            return lavorazione;
        }
        private Articolo CreaArticolo(PreserieDS.MAGAZZRow magazz)
        {
            if (magazz == null) return null;

            Articolo articolo = new Articolo();

            articolo.IDMAGAZZ = magazz.IDMAGAZZ;
            articolo.Modello = magazz.MODELLO;
            articolo.Descrizione = magazz.IsDESMODELLOBASENull() ? string.Empty : magazz.DESMAGAZZ;

            return articolo;
        }

        private ODL CreaOdl(PreserieDS.USR_PRD_MOVFASIRow movFase)
        {
            ODL odl = new ODL();

            odl.IDPRDMOVFASE = movFase.IDPRDMOVFASE;
            odl.NUMMOVFASE = movFase.IsNUMMOVFASENull() ? string.Empty : movFase.NUMMOVFASE;
            odl.DATAMOVFASE = movFase.IsDATAMOVFASENull() ? string.Empty : movFase.DATAMOVFASE.ToShortDateString();
            odl.Barcode = movFase.IsBARCODENull() ? string.Empty : movFase.BARCODE;
            odl.Reparto = movFase.IsCODICECLIFONull() ? string.Empty : movFase.CODICECLIFO;
            odl.Quantita = movFase.QTA;
            odl.QuantitaNetta = movFase.QTANET;
            odl.QuantitaTerminata = movFase.QTATER;
            odl.QuantitaDaTerminare = movFase.QTADATER;

            return odl;
        }

        private List<Dettaglio> CreaListaDettaglio(string barcode, PreserieDS ds)
        {
            List<Dettaglio> dettagli = new List<Dettaglio>();
            foreach (PreserieDS.RW_PR_DETTAGLIORow dettaglio in ds.RW_PR_DETTAGLIO.Where(x => x.BARCODE == barcode))
            {
                PreserieDS.TABFASRow fase = ds.TABFAS.Where(x => x.IDTABFAS == dettaglio.IDTABFAS).FirstOrDefault();
                if (fase == null) continue;


                Dettaglio d = new Dettaglio();
                d.IDDETTAGLIO = dettaglio.IDDETTAGLIO;
                d.idFase = fase.IDTABFAS;
                d.Fase = fase.CODICEFASE;

                if (!dettaglio.IsREPARTONull())
                {
                    PreserieDS.CLIFORow clifo = ds.CLIFO.Where(x => x.CODICE == dettaglio.REPARTO).FirstOrDefault();
                    if (clifo != null)
                    {
                        d.Lavorante = clifo.RAGIONESOC.Trim();
                        d.idLavorante = clifo.CODICE;
                    }
                }

                d.Nota = dettaglio.IsNOTANull() ? string.Empty : dettaglio.NOTA;
                d.PezziOra = dettaglio.PEZZI_ORARI;

                dettagli.Add(d);
            }

            return dettagli;
        }

        public ODLSchedaModel CaricaSchedaODL(string Barcode, string rvlImageSite)
        {
            ODLSchedaModel model = new ODLSchedaModel();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {

                bPreserie.FillCLIFO(ds);
                bPreserie.FillUSR_PRD_MOVFASIByBarcode(Barcode, ds);
                bPreserie.FillRW_PR_DETTAGLIO(Barcode, ds);
                bPreserie.FillTABFAS(ds);

                PreserieDS.USR_PRD_MOVFASIRow odl = ds.USR_PRD_MOVFASI.Where(x => x.BARCODE == Barcode).FirstOrDefault();
                if (odl == null)
                {
                    model.EsitoRicerca = 1;
                    return model;
                }
                model.EsitoRicerca = 2;
                model.Barcode = odl.BARCODE;
                model.IDPRDMOVFASE = odl.IDPRDMOVFASE;
                model.NumeroDocumento = odl.NUMMOVFASE;
                model.DataDocumento = odl.IsDATAMOVFASENull() ? string.Empty : odl.DATAMOVFASE.ToString("dd MMM yyyy");
                model.Quantita = odl.QTA;

                model.RepartoCodice = odl.IsCODICECLIFONull() ? string.Empty : odl.CODICECLIFO.Trim();


                model.Reparto = string.Empty;
                if (!odl.IsCODICECLIFONull())
                {
                    PreserieDS.CLIFORow reparto = ds.CLIFO.Where(x => x.CODICE == odl.CODICECLIFO).FirstOrDefault();
                    if (reparto != null)
                        model.Reparto = reparto.RAGIONESOC;
                }


                bPreserie.FillMAGAZZ(ds, new List<string>(new string[] { odl.IDMAGAZZ }));
                PreserieDS.MAGAZZRow modello = ds.MAGAZZ.Where(x => x.IDMAGAZZ == odl.IDMAGAZZ).FirstOrDefault();
                model.Modello = modello.MODELLO;
                model.ModelloDescrizione = modello.DESMAGAZZ;

                model.Commessa = string.Empty;
                model.DataCommessa = string.Empty;
                model.Riferimento = string.Empty;

                PreserieDS.USR_PRD_FASIRow fase = ds.USR_PRD_FASI.Where(x => x.IDPRDFASE == odl.IDPRDFASE).FirstOrDefault();
                if (fase != null)
                {

                    model.IdLancioD = fase.IDLANCIOD;

                    model.IdMagazz = fase.IDMAGAZZ;
                    model.IdTabFas = fase.IDTABFAS;

                    PreserieDS.USR_PRD_LANCIODRow lancio = ds.USR_PRD_LANCIOD.Where(x => x.IDLANCIOD == fase.IDLANCIOD).FirstOrDefault();
                    if (lancio != null)
                    {
                        model.Commessa = lancio.IsNOMECOMMESSANull() ? string.Empty : lancio.NOMECOMMESSA;
                        model.DataCommessa = lancio.IsDATACOMMESSANull() ? string.Empty : lancio.DATACOMMESSA.ToShortDateString();
                        model.Riferimento = lancio.IsRIFERIMENTONull() ? string.Empty : lancio.RIFERIMENTO;

                        bPreserie.FillMAGAZZ(ds, new List<string>(new string[] { lancio.IDMAGAZZ }));
                        PreserieDS.MAGAZZRow modelloLancio = ds.MAGAZZ.Where(x => x.IDMAGAZZ == lancio.IDMAGAZZ).FirstOrDefault();
                        model.ModelloFinale = modelloLancio.MODELLO;
                        model.ModelloFinaleDescrizione = modelloLancio.DESMAGAZZ;
                    }
                }

                bPreserie.FillUSR_PDM_FILES(ds, odl.IDMAGAZZ);
                model.ImageUrl = creaUrlImage(rvlImageSite, odl.IDMAGAZZ, ds);

                model.Dettagli = CreaListaDettaglio(Barcode, ds);

                return model;
            }
        }

        private string creaUrlImage(string RvlImageSite, string IDMAGAZZ, PreserieDS ds)
        {
            PreserieDS.USR_PDM_FILESRow immagine = ds.USR_PDM_FILES.Where(x => x.IDMAGAZZ == IDMAGAZZ).FirstOrDefault();
            if (immagine != null)
            {
                if (System.IO.Path.GetPathRoot(immagine.NOMEFILE) == "R:\\")
                {
                    string newUrl = RvlImageSite.Replace("rvlimmagini", "rvlimmaginir");
                    string newPath = immagine.NOMEFILE.ToUpper().Replace("R:\\", string.Empty);
                    newPath = newPath.Replace("\\", "/");
                    return newUrl + newPath;
                }
                return RvlImageSite + immagine.NOMEFILE;
            }

            return string.Empty;

        }

        public List<RWListItem> CaricaTabFas(string Reparto)
        {
            List<RWListItem> model = new List<RWListItem>();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                bPreserie.FillTABFAS(ds);
                model.Add(new RWListItem(string.Empty, string.Empty));

                foreach (PreserieDS.TABFASRow fase in ds.TABFAS.Where(x => !x.IsCODICECLIFOPREDFASENull() && x.CODICECLIFOPREDFASE.Trim() == Reparto).OrderBy(x => x.CODICEFASE))
                    model.Add(new RWListItem(fase.CODICEFASE, fase.IDTABFAS));

                return model;
            }
        }

        public List<RWListItem> CaricaListaMateriali()
        {
            List<RWListItem> model = new List<RWListItem>();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                bPreserie.FillRW_PR_MATERIALE(ds);
                model.Add(new RWListItem(string.Empty, string.Empty));

                foreach (PreserieDS.RW_PR_MATERIALERow materiale in ds.RW_PR_MATERIALE.OrderBy(x => x.IDMATERIALE))
                    model.Add(new RWListItem(materiale.MATERIALE, materiale.IDMATERIALE.ToString()));

                return model;
            }

        }

        public List<RWListItem> CaricaListaMacchine(string Reparto)
        {
            List<RWListItem> model = new List<RWListItem>();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                bPreserie.FillUSR_PRD_RESOURCESF(ds);
                model.Add(new RWListItem(string.Empty, string.Empty));

                foreach (PreserieDS.USR_PRD_RESOURCESFRow macchina in ds.USR_PRD_RESOURCESF.Where(x => x.IDRESOURCE != "0000000019" && x.CODICECLIFO == Reparto).OrderBy(x => x.CODRESOURCEF))
                    model.Add(new RWListItem(macchina.CODRESOURCEF, macchina.IDRESOURCEF));

                return model;
            }
        }

        public List<RWListItem> CaricaListaMetalliBase()
        {
            List<RWListItem> model = new List<RWListItem>();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                bPreserie.FillMetalliBase(ds);
                model.Add(new RWListItem(string.Empty, string.Empty));

                foreach (PreserieDS.MAGAZZRow metallo in ds.MAGAZZ.OrderBy(x => x.MODELLO))
                    model.Add(new RWListItem(metallo.MODELLO, metallo.IDMAGAZZ));

                return model;
            }
        }

        public List<RWListItem> CaricaListaPackaging()
        {
            List<RWListItem> model = new List<RWListItem>();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                bPreserie.FillRW_PR_PACKAGING(ds);
                model.Add(new RWListItem(string.Empty, string.Empty));

                foreach (PreserieDS.RW_PR_PACKAGINGRow fase in ds.RW_PR_PACKAGING)
                    model.Add(new RWListItem(fase.PACKAGING, fase.IDPACKAGING.ToString()));

                return model;
            }
        }

        public List<RWListItem> CaricaLavorazioni(string RepartoCodice)
        {
            List<RWListItem> model = new List<RWListItem>();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                bPreserie.FillRW_PR_LAVORAZIONE(ds);
                model.Add(new RWListItem(string.Empty, string.Empty));

                foreach (PreserieDS.RW_PR_LAVORAZIONERow lavorazione in ds.RW_PR_LAVORAZIONE.Where(x => x.REPARTO == RepartoCodice))
                    model.Add(new RWListItem(lavorazione.DESCRIZIONE, lavorazione.CODICE.ToString()));

                return model;
            }
        }

        public List<RWListItem> CreaListaLavorantiEsterni()
        {
            List<RWListItem> LavorantiEsterni = new List<RWListItem>();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                bPreserie.FillCLIFO(ds);
                LavorantiEsterni.Add(new RWListItem(string.Empty, string.Empty));
                int aux;
                foreach (PreserieDS.CLIFORow fornitore in ds.CLIFO.Where(x => !x.IsCODICENull() && !x.IsRAGIONESOCNull() && !x.IsTIPONull() && x.TIPO == "F" && int.TryParse(x.CODICE, out aux)))
                    LavorantiEsterni.Add(new RWListItem(fornitore.RAGIONESOC.Trim(), fornitore.CODICE.Trim()));
            }

            return LavorantiEsterni;
        }

        public void SalvaDettagli(string RepartoCodice, decimal Pezzi, string Packaging, decimal Peso,
            string Nota, string Dettagli, string IDPRDMOVFASE, string Barcode, string IdLancioD, string IdMagazz, string IDTABFAS, string IDUSER)
        {
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                //bPreserie.FillCLIFO(ds);
                //bPreserie.FillTABFAS(ds);
                //bPreserie.FillRW_PR_DETTAGLIO(Barcode, ds);


                long idDettaglio = bPreserie.GetID();
                PreserieDS.RW_PR_DETTAGLIORow dettaglioRow = null;// ds.RW_PR_DETTAGLIO.Where(x => x.IDDETTAGLIO == dettaglio.IDDETTAGLIO).FirstOrDefault();
                if (dettaglioRow == null)
                {

                    dettaglioRow = ds.RW_PR_DETTAGLIO.NewRW_PR_DETTAGLIORow();
                    dettaglioRow.IDDETTAGLIO = idDettaglio;
                    dettaglioRow.BARCODE = Barcode;
                    dettaglioRow.REPARTO = RepartoCodice;
                    dettaglioRow.DATACR = DateTime.Now;
                    dettaglioRow.IDPRDMOVFASE = IDPRDMOVFASE;
                    dettaglioRow.IDTABFAS = IDTABFAS;
                    dettaglioRow.IDUSER = IDUSER;
                    dettaglioRow.NOTA = Nota;
                    dettaglioRow.PEZZI_ORARI = Pezzi.ToString();
                    dettaglioRow.PESO = Peso;
                    dettaglioRow.PACKAGING = Packaging;
                    dettaglioRow.IDLANCIOD = IdLancioD;
                    dettaglioRow.IDMAGAZZ = IdMagazz;

                    ds.RW_PR_DETTAGLIO.AddRW_PR_DETTAGLIORow(dettaglioRow);
                }
                else
                {
                    dettaglioRow.DATACR = DateTime.Now;
                    dettaglioRow.IDTABFAS = IDTABFAS;
                    dettaglioRow.IDUSER = IDUSER;
                    dettaglioRow.NOTA = Nota;
                    dettaglioRow.PEZZI_ORARI = Pezzi.ToString();
                    dettaglioRow.PESO = Peso;
                    dettaglioRow.PACKAGING = Packaging;
                    dettaglioRow.IDLANCIOD = IdLancioD;
                    dettaglioRow.IDMAGAZZ = IdMagazz;
                }

                switch (RepartoCodice)
                {
                    case Reparti.Pulimentatura:
                        {
                            PulimentaturaJson[] dettagli = JSonSerializer.Deserialize<PulimentaturaJson[]>(Dettagli);
                            InserisciDettaglioPulimentatura(ds, dettagli, idDettaglio, bPreserie);
                            break;
                        }
                    case Reparti.Vibratura:
                        {
                            VibraturaJson[] dettagli = JSonSerializer.Deserialize<VibraturaJson[]>(Dettagli);
                            InserisciDettaglioVibratura(ds, dettagli, idDettaglio, bPreserie);
                            break;
                        }
                    case Reparti.Modelleria:
                        {
                            ModelleriaJson[] dettagli = JSonSerializer.Deserialize<ModelleriaJson[]>(Dettagli);
                            InserisciDettaglioModelleria(ds, dettagli, idDettaglio, bPreserie);
                            break;
                        }
                }

                bPreserie.UpdateRW_PR(ds.RW_PR_DETTAGLIO.TableName, ds);
                bPreserie.UpdateRW_PR(ds.RW_PR_PULIMENTATURA.TableName, ds);
                bPreserie.UpdateRW_PR(ds.RW_PR_VIBRATURA.TableName, ds);

            }
        }

        private void InserisciDettaglioPulimentatura(PreserieDS ds, PulimentaturaJson[] dettagli, long IDDETTAGLIO, PreserieBusiness bPreserie)
        {
            int sequenza = 1;
            foreach (PulimentaturaJson dettaglio in dettagli)
            {
                long idElemento = bPreserie.GetID();
                PreserieDS.RW_PR_PULIMENTATURARow pul = ds.RW_PR_PULIMENTATURA.NewRW_PR_PULIMENTATURARow();
                pul.IDELEMENTO = idElemento;
                pul.IDDETTAGLIO = IDDETTAGLIO;
                pul.SEQUENZA = sequenza;
                sequenza++;

                pul.LAVORAZIONE = dettaglio.Lavorazione;
                pul.AUTOMATICO = dettaglio.Automatico == "Automatico" ? "S" : "N";
                pul.SPAZZOLE = dettaglio.Spazzole;
                pul.PASTE = dettaglio.Paste;
                pul.PARTA_LAVORATA = dettaglio.ParteLavorata;

                ds.RW_PR_PULIMENTATURA.AddRW_PR_PULIMENTATURARow(pul);
            }
        }

        private void InserisciDettaglioVibratura(PreserieDS ds, VibraturaJson[] dettagli, long IDDETTAGLIO, PreserieBusiness bPreserie)
        {
            int sequenza = 1;
            foreach (VibraturaJson dettaglio in dettagli)
            {
                long idElemento = bPreserie.GetID();
                PreserieDS.RW_PR_VIBRATURARow vib = ds.RW_PR_VIBRATURA.NewRW_PR_VIBRATURARow();
                vib.IDELEMENTO = idElemento;
                vib.IDDETTAGLIO = IDDETTAGLIO;
                vib.SEQUENZA = sequenza;
                sequenza++;

                vib.LAVORAZIONE = dettaglio.Lavorazione;
                vib.TIPOLOGIA = dettaglio.AcquaSecco;
                vib.MATERIALE = dettaglio.Materiale;
                vib.ADDITIVI = dettaglio.Additivi;
                vib.MAXPEZZI = dettaglio.Pezzi;
                vib.TEMPO = dettaglio.Tempo;

                ds.RW_PR_VIBRATURA.AddRW_PR_VIBRATURARow(vib);
            }
        }

        private void InserisciDettaglioModelleria(PreserieDS ds, ModelleriaJson[] dettagli, long IDDETTAGLIO, PreserieBusiness bPreserie)
        {
            int sequenza = 1;
            foreach (ModelleriaJson dettaglio in dettagli)
            {
                long idElemento = bPreserie.GetID();
                PreserieDS.RW_PR_MODELLERIARow mod = ds.RW_PR_MODELLERIA.NewRW_PR_MODELLERIARow();
                mod.IDELEMENTO = idElemento;
                mod.IDDETTAGLIO = IDDETTAGLIO;
                mod.SEQUENZA = sequenza;
                sequenza++;

                mod.LAVORAZIONE = dettaglio.Lavorazione;
                if (string.IsNullOrEmpty(dettaglio.Materiale))
                    mod.MATERIALE = dettaglio.Materiale;

                if (string.IsNullOrEmpty(dettaglio.Programma))
                    mod.PROGRAMMA = dettaglio.Programma;

                if (string.IsNullOrEmpty(dettaglio.Utensili))
                    mod.UTENSILI = dettaglio.Utensili;

                if (string.IsNullOrEmpty(dettaglio.Attrezzaggio))
                    mod.ATTREZZAGGIO = dettaglio.Attrezzaggio;

                if (string.IsNullOrEmpty(dettaglio.Macchina))
                    mod.MACCHINA = dettaglio.Macchina;

                ds.RW_PR_MODELLERIA.AddRW_PR_MODELLERIARow(mod);
            }
        }
    }
}
