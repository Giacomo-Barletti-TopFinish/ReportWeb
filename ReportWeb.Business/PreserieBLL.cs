using ReportWeb.Data.Preserie;
using ReportWeb.Entities;
using ReportWeb.Models.Preserie;
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
            //if (faseFiglie.Count == 0) return;
            //if (faseFiglie.Count == 1)
            //{
            //    string[] radiceStr = radice.Split('.');
            //    int indice = Int32.Parse(radiceStr[radiceStr.Length - 1]);
            //    string radiceNuova = string.Empty;
            //    if (radice.Length > 1)
            //        for (int i = 0; i < radiceStr.Length - 1; i++)
            //            radiceNuova = radiceStr[i] + ".";
            //    radiceNuova += (indice+1).ToString();
            //    Lavorazione lavorazione = CreaLavorazione(faseFiglie[0], radiceNuova, ds);
            //    commessa.Lavorazioni.Add(lavorazione);
            //    List<PreserieDS.USR_PRD_FASIRow> figlie = ds.USR_PRD_FASI.Where(x => !x.IsIDPRDFASEPADRENull() && x.IDPRDFASEPADRE == faseFiglie[0].IDPRDFASE).ToList();
            //    EspandiAlberoFasi(figlie, ds, radiceNuova, commessa);
            //    return;
            //}

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
                lavorazione.Odl = CreaOdl(movFase);

            List<DettaglioBase> Dettagli = new List<DettaglioBase>();

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

        public ODLSchedaModel CaricaSchedaODL(string Barcode, string rvlImageSite)
        {
            ODLSchedaModel model = new ODLSchedaModel();
            PreserieDS ds = new PreserieDS();
            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {

                bPreserie.FillCLIFO(ds);
                bPreserie.FillUSR_PRD_MOVFASIByBarcode(Barcode, ds);

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
                    PreserieDS.USR_PRD_LANCIODRow lancio = ds.USR_PRD_LANCIOD.Where(x => x.IDLANCIOD == fase.IDLANCIOD).FirstOrDefault();
                    if (lancio != null)
                    {
                        model.Commessa = lancio.IsNOMECOMMESSANull() ? string.Empty : lancio.NOMECOMMESSA;
                        model.DataCommessa = lancio.IsDATACOMMESSANull() ? string.Empty : lancio.DATACOMMESSA.ToShortDateString();
                        model.Riferimento = lancio.IsRIFERIMENTONull() ? string.Empty : lancio.RIFERIMENTO;
                    }
                }

                bPreserie.FillUSR_PDM_FILES(ds, odl.IDMAGAZZ);
                model.ImageUrl = creaUrlImage(rvlImageSite, odl.IDMAGAZZ, ds);

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
    }
}
