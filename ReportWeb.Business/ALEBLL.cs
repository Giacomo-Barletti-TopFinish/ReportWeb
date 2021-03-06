﻿using ReportWeb.Common.Helpers;
using ReportWeb.Data.ALE;
using ReportWeb.Entities;
using ReportWeb.Models;
using ReportWeb.Models.ALE;
using ReportWeb.Models.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class ALEBLL
    {
        public ALEBLL(string RvlImageSite)
        {
            rvlImageSite = RvlImageSite;
        }
        private string rvlImageSite;

        public AddebitiModel TrovaMancanti(string dataInizio, string dataFine)
        {
            AddebitiModel model = new AddebitiModel();
            model.Addebiti = new List<AddebitoModel>();
            ALEDS ds = new ALEDS();
            using (ALEBusiness bALE = new ALEBusiness())
            {
                bALE.FillRW_ALE_DETTAGLIO(ds, dataInizio, dataFine, true);

                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                List<string> IDCHECKQT = ds.RW_ALE_DETTAGLIO.Select(x => x.IDCHECKQT).ToList();
                bALE.FillUSR_CHECKQ_T(ds, IDCHECKQT);
                bALE.FillUSR_CHECKQ_C(ds, IDCHECKQT);
                bALE.FillUSR_PRD_MOVFASI(ds, IDCHECKQT);
                List<string> IDMAGAZZ = ds.USR_CHECKQ_T.Select(x => x.IDMAGAZZ).ToList();
                bALE.FillMAGAZZ(ds, IDMAGAZZ);
                bALE.FillUSR_PDM_FILES(ds, IDMAGAZZ);
                bALE.FillTABFAS(ds);

                List<decimal> IDALEDETTAGLIO = ds.RW_ALE_DETTAGLIO.Select(x => x.IDALEDETTAGLIO).ToList();
                bALE.FillRW_ALE_DETT_COSTO(ds, IDALEDETTAGLIO);

                List<string> IDPRDMOVFASE = ds.USR_PRD_MOVFASI.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                bALE.FillUSR_PRD_FASI(ds, IDPRDMOVFASE);

                foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO)
                {
                    ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                    AddebitoModel m = CreaAddebitoModel(ds, riga);
                    model.Addebiti.Add(m);
                }

            }

            return model;
        }

        public InserimentoModel CaricaScheda(string Barcode)
        {
            InserimentoModel model = new InserimentoModel();
            ALEDS ds = new ALEDS();
            using (ALEBusiness bALE = new ALEBusiness())
            {
                if (VerificaBarcodeCaricato(Barcode))
                {
                    model.EsitoRicerca = 0;
                    return model;
                }

                bALE.FillUSR_CHECKQ_T(ds, Barcode);
                if (!ds.USR_CHECKQ_T.Any(x => x.BARCODE == Barcode))
                {
                    model.EsitoRicerca = 1;
                    return model;
                }

                ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.BARCODE == Barcode).FirstOrDefault();

                bALE.FillUSR_CHECKQ_D(ds, CHECKQ_T.IDCHECKQT);
                bALE.FillUSR_CHECKQ_C(ds, CHECKQ_T.IDCHECKQT);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                bALE.FillTABCAUMGT(ds);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_PRD_FLUSSO_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                ALEDS.USR_CHECKQ_DRow CHECKQ_D = ds.USR_CHECKQ_D.Where(x => x.IDCHECKQT == CHECKQ_T.IDCHECKQT).FirstOrDefault();
                List<ALEDS.USR_CHECKQ_CRow> CHECKQ_C = ds.USR_CHECKQ_C.Where(x => x.IDCHECKQT == CHECKQ_T.IDCHECKQT).ToList();

                model.EsitoRicerca = 2;
                model.IDCHECKQT = CHECKQ_T.IDCHECKQT;
                model.Barcode = CHECKQ_T.BARCODE;
                model.Azienda = CHECKQ_T.AZIENDA;
                model.NumeroDocumento = CHECKQ_T.NUMCHECKQT;
                model.DataDocumento = CHECKQ_T.DATACHECKQT.ToString("dd MMM yyyy");
                model.Quantita = CHECKQ_T.IsQTANull() ? 0 : CHECKQ_T.QTA;

                model.RepartoCodice = CHECKQ_T.IsCODICECLIFO_RILNull() ? string.Empty : CHECKQ_T.CODICECLIFO_RIL.Trim();


                model.Reparto = string.Empty;
                if (!CHECKQ_T.IsCODICECLIFO_RILNull())
                {
                    ALEDS.CLIFORow reparto = ds.CLIFO.Where(x => x.CODICE == CHECKQ_T.CODICECLIFO_RIL).FirstOrDefault();
                    if (reparto != null)
                        model.Reparto = reparto.RAGIONESOC;

                }
                model.Difetti = new List<DifettoRilevato>();
                foreach (ALEDS.USR_CHECKQ_CRow difettoRow in CHECKQ_C)
                {
                    DifettoRilevato difetto = new DifettoRilevato();

                    difetto.QuantitaDifforme = difettoRow == null ? 0 : (difettoRow.IsQTA_DIFNull() ? 0 : difettoRow.QTA_DIF);

                    difetto.Difetto = string.Empty;
                    difetto.TipoDifetto = string.Empty;
                    if (difettoRow != null)
                    {
                        ALEDS.USR_ANA_DIFETTIRow dif = ds.USR_ANA_DIFETTI.Where(x => x.IDDIFETTO == difettoRow.IDDIFETTO).FirstOrDefault();
                        ALEDS.USR_TAB_TIPODIFETTIRow tipoDifetto = ds.USR_TAB_TIPODIFETTI.Where(x => x.IDTIPODIFETTO == dif.IDTIPODIFETTO).FirstOrDefault();
                        difetto.TipoDifetto = tipoDifetto.DESTIPODIFETTO;
                        difetto.Difetto = dif.DESDIFETTO;
                    }
                    model.Difetti.Add(difetto);
                }


                bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                ALEDS.MAGAZZRow modello = ds.MAGAZZ.Where(x => x.IDMAGAZZ == CHECKQ_T.IDMAGAZZ).FirstOrDefault();
                model.Modello = modello.MODELLO;
                model.ModelloDescrizione = modello.DESMAGAZZ;

                ALEDS.USR_PRD_MOVFASIRow MovFase = ds.USR_PRD_MOVFASI.Where(x => x.IDCHECKQT == CHECKQ_T.IDCHECKQT).FirstOrDefault();
                if (MovFase != null)
                {
                    model.ODL = MovFase.IsNUMMOVFASENull() ? string.Empty : MovFase.NUMMOVFASE;
                    model.DataODL = MovFase.IsDATAMOVFASENull() ? string.Empty : MovFase.DATAMOVFASE.ToShortDateString();
                    model.Commessa = MovFase.IsRIFERIMENTO_INFRANull() ? string.Empty : MovFase.RIFERIMENTO_INFRA;
                    model.DataCommessa = MovFase.IsDATARIF_INFRANull() ? string.Empty : MovFase.DATARIF_INFRA.ToShortDateString();
                }

                bALE.FillUSR_PDM_FILES(ds, CHECKQ_T.IDMAGAZZ);
                model.ImageUrl = creaUrlImage(rvlImageSite, CHECKQ_T.IDMAGAZZ, ds);

                //ALEDS.USR_PDM_FILESRow immagine = ds.USR_PDM_FILES.Where(x => x.IDMAGAZZ == CHECKQ_T.IDMAGAZZ).FirstOrDefault();
                //if (immagine != null)
                //{

                //    model.ImageUrl = RvlImageSite + immagine.NOMEFILE;
                //}

                model.LavorantiEsterni = creaListaLavorantiEsterni(ds);

                return model;
            }
        }

        private string creaUrlImage(string RvlImageSite, string IDMAGAZZ, ALEDS ds)
        {
            ALEDS.USR_PDM_FILESRow immagine = ds.USR_PDM_FILES.Where(x => x.IDMAGAZZ == IDMAGAZZ).FirstOrDefault();
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
            return RvlImageSite + "NoImage.png";

        }

        public void SalvaInserimento(string Azienda, string Barcode, string IDCHECKQT, int Difettosi, int Inseriti, string Lavorante, string Nota, bool scartoDefinitivo,
                                     bool RiparazioneGratuita, string ODL, int EstensioneFattura, string UIDUSER)
        {
            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bool mancante = VerificaSeMancanti(IDCHECKQT);

                if (RiparazioneGratuita)
                {
                    bALE.SalvaInserimentoRW_ALE_RIPGRATUITA(Azienda, ODL, UIDUSER, EstensioneFattura, false);
                    return;
                }

                if (!mancante)
                {
                    bALE.FillCLIFO(ds);
                    ALEDS.CLIFORow lavoranteRow = ds.CLIFO.Where(x => x.CODICE.Trim() == Lavorante).FirstOrDefault();

                    MailDispatcherBLL bllMD = new MailDispatcherBLL();
                    string oggetto = "ALE - NUOVA SCHEDA INSERITA";
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Nuova scheda inserita");
                    sb.AppendLine();
                    if (lavoranteRow != null)
                        sb.AppendLine(string.Format("Lavorante: {0}", lavoranteRow.IsRAGIONESOCNull() ? string.Empty : lavoranteRow.RAGIONESOC.Trim()));
                    sb.AppendLine(string.Format("Quanità: {0}", Inseriti));
                    sb.AppendLine(string.Format("Inserita da: {0}", UIDUSER));

                    decimal IDMAIL = bllMD.CreaEmail("ALE - ADDEBITO", oggetto, sb.ToString());
                    bllMD.SottomettiEmail(IDMAIL);
                }
                bALE.SalvaInserimento(Azienda, Barcode, IDCHECKQT, Difettosi, Inseriti, Lavorante, Nota, UIDUSER, mancante, scartoDefinitivo);
            }
        }

        public bool VerificaSeMancanti(string IDCHECKQT)
        {
            ALEDS ds = new ALEDS();
            using (ALEBusiness bALE = new ALEBusiness())
            {
                bALE.FillUSR_CHECKQ_C(ds, IDCHECKQT);
                return ds.USR_CHECKQ_C.Any(x => IdMAncanti.ListaIdMancanti.Contains(x.IDDIFETTO));
            }
        }

        public int ContaSchede(string Stato)
        {
            using (ALEBusiness bAle = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bAle.FillRW_ALE_DETTAGLIO(ds, Stato);

                switch (Stato)
                {
                    case ALEStatoDettaglio.INSERITO:
                        return ds.RW_ALE_DETTAGLIO.Count();

                    case ALEStatoDettaglio.VALORIZZATO:
                    case ALEStatoDettaglio.APPROVATO:
                    case ALEStatoDettaglio.ADDEBITATO:
                        decimal[] gruppi = ds.RW_ALE_DETTAGLIO.Where(x => !x.IsIDALEGRUPPONull()).Select(x => x.IDALEGRUPPO).Distinct().ToArray();
                        return gruppi.Length;
                    default:
                        return 0;
                }

            }
        }

        private List<RWListItem> creaListaLavorantiEsterni(ALEDS ds)
        {
            List<RWListItem> LavorantiEsterni = new List<RWListItem>();
            LavorantiEsterni.Add(new RWListItem(string.Empty, string.Empty));

            int aux;
            foreach (ALEDS.CLIFORow fornitore in ds.CLIFO.Where(x => !x.IsCODICENull() && !x.IsRAGIONESOCNull() && !x.IsTIPONull() && x.TIPO == "F" && int.TryParse(x.CODICE, out aux)))
                LavorantiEsterni.Add(new RWListItem(fornitore.RAGIONESOC.Trim(), fornitore.CODICE.Trim()));
            return LavorantiEsterni;
        }

        public AddebitiModel LeggiSchedeDaAddebitare()
        {
            AddebitiModel model = new AddebitiModel();
            model.Addebiti = new List<AddebitoModel>();
            model.LavorantiEsterni = new List<RWListItem>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.INSERITO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                List<string> IDCHECKQT = ds.RW_ALE_DETTAGLIO.Select(x => x.IDCHECKQT).ToList();
                bALE.FillUSR_CHECKQ_T(ds, IDCHECKQT);
                bALE.FillUSR_CHECKQ_C(ds, IDCHECKQT);
                bALE.FillUSR_PRD_MOVFASI(ds, IDCHECKQT);
                List<string> IDMAGAZZ = ds.USR_CHECKQ_T.Select(x => x.IDMAGAZZ).ToList();
                bALE.FillMAGAZZ(ds, IDMAGAZZ);
                bALE.FillUSR_PDM_FILES(ds, IDMAGAZZ);
                bALE.FillTABFAS(ds);

                List<decimal> IDALEDETTAGLIO = ds.RW_ALE_DETTAGLIO.Select(x => x.IDALEDETTAGLIO).ToList();
                bALE.FillRW_ALE_DETT_COSTO(ds, IDALEDETTAGLIO);

                List<string> IDPRDMOVFASE = ds.USR_PRD_MOVFASI.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                bALE.FillUSR_PRD_FASI(ds, IDPRDMOVFASE);

                foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO)
                {
                    ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                    AddebitoModel m = CreaAddebitoModel(ds, riga);
                    model.Addebiti.Add(m);
                }

                model.LavorantiEsterni = creaListaLavorantiEsterni(ds);

            }
            return model;
        }

        public AddebitiModel LeggiSchedeNonAddebitate()
        {
            AddebitiModel model = new AddebitiModel();
            model.Addebiti = new List<AddebitoModel>();
            model.LavorantiEsterni = new List<RWListItem>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.NONADDEBITATO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                List<string> IDCHECKQT = ds.RW_ALE_DETTAGLIO.Select(x => x.IDCHECKQT).ToList();
                bALE.FillUSR_CHECKQ_T(ds, IDCHECKQT);
                bALE.FillUSR_CHECKQ_C(ds, IDCHECKQT);
                bALE.FillUSR_PRD_MOVFASI(ds, IDCHECKQT);
                List<string> IDMAGAZZ = ds.USR_CHECKQ_T.Select(x => x.IDMAGAZZ).ToList();
                bALE.FillMAGAZZ(ds, IDMAGAZZ);
                bALE.FillUSR_PDM_FILES(ds, IDMAGAZZ);
                bALE.FillTABFAS(ds);

                List<decimal> IDALEDETTAGLIO = ds.RW_ALE_DETTAGLIO.Select(x => x.IDALEDETTAGLIO).ToList();
                bALE.FillRW_ALE_DETT_COSTO(ds, IDALEDETTAGLIO);

                List<string> IDPRDMOVFASE = ds.USR_PRD_MOVFASI.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                bALE.FillUSR_PRD_FASI(ds, IDPRDMOVFASE);

                foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.DATA_NONADDEBITO > DateTime.Today.AddDays(-10)))
                {
                    ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                    AddebitoModel m = CreaAddebitoModel(ds, riga);
                    model.Addebiti.Add(m);
                }

                model.LavorantiEsterni = new List<RWListItem>();

            }
            return model;
        }

        public FattureRitardateModel LeggiFattureRitardate(string DataInizio, string DataFine, bool SoloVisibili)
        {
            FattureRitardateModel model = new FattureRitardateModel();
            model.FattureRitardate = new List<FatturaRitardataModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_RIPGRATUITA(ds, DataInizio, DataFine);

                if (SoloVisibili)
                {
                    foreach (ALEDS.RW_ALE_RIPGRATUITARow riga in ds.RW_ALE_RIPGRATUITA.Where(x => x.NASCONDI != "S").ToList())
                    {
                        FatturaRitardataModel m = CreaFatturaRitardataModel(ds, riga);
                        model.FattureRitardate.Add(m);
                    }
                }
                else
                {
                    foreach (ALEDS.RW_ALE_RIPGRATUITARow riga in ds.RW_ALE_RIPGRATUITA)
                    {
                        FatturaRitardataModel m = CreaFatturaRitardataModel(ds, riga);
                        model.FattureRitardate.Add(m);
                    }
                }

            }
            return model;
        }

        public void NascondiRiga(string IDRIPGRATUITA)
        {
            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.NascondiRiga(ds, IDRIPGRATUITA);
            }
        }

        private FatturaRitardataModel CreaFatturaRitardataModel(ALEDS ds, ALEDS.RW_ALE_RIPGRATUITARow riga)
        {
            FatturaRitardataModel m = new FatturaRitardataModel();
            m.IDRIPGRATUITA = riga.IDRIPGRATUITA;
            m.ODL = riga.IsODLNull() ? string.Empty : riga.ODL;
            m.DATA_CREAZIONE = riga.DATA_CREAZIONE;
            m.UIDUSER_INSERIMENTO = riga.UIDUSER_INSERIMENTO;
            m.LAVORANTE = riga.LAVORANTE;
            m.DATA_SCADENZA = riga.DATA_SCADENZA;
            m.NASCONDI = riga.NASCONDI;

            return m;
        }

        private AddebitoModel CreaAddebitoModel(ALEDS ds, ALEDS.RW_ALE_DETTAGLIORow riga)
        {
            ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
            AddebitoModel m = new AddebitoModel();
            m.IdAleDettaglio = riga.IDALEDETTAGLIO;
            m.Barcode = riga.BARCODE;
            m.Azienda = riga.AZIENDA;
            m.IDCHECKQT = riga.IDCHECKQT;
            m.IdAleGruppo = riga.IsIDALEGRUPPONull() ? -1 : riga.IDALEGRUPPO;

            m.QuantitaDifettosi = riga.QUANTITADIFETTOSI;
            m.QuantitaInseriti = riga.QUANTITAINSERITA;
            m.QuantitaAddebitata = riga.IsQUANTITAADDEBITATANull() ? 0 : riga.QUANTITAADDEBITATA;

            m.SCARTODEFINITIVO = riga.IsSCARTODEFINITIVONull() ? false : (riga.SCARTODEFINITIVO == "0") ? false : true;

            m.NotaInserimento = riga.IsNOTAINSERIMENTONull() ? string.Empty : riga.NOTAINSERIMENTO;
            m.NotaApprovazione = riga.IsNOTAAPPROVAZIONENull() ? string.Empty : riga.NOTAAPPROVAZIONE;
            m.NotaAddebito = riga.IsNOTAADDEBITONull() ? string.Empty : riga.NOTAADDEBITO;
            m.NotaValorizzazione = riga.IsNOTAVALORIZZAZIONENull() ? string.Empty : riga.NOTAVALORIZZAZIONE;

            m.Prezzo = riga.IsPREZZONull() ? 0 : riga.PREZZO;
            m.Valore = riga.IsVALORENull() ? 0 : riga.VALORE;
            m.PrezzoApprovato = riga.IsPREZZO_APPROVATONull() ? 0 : riga.PREZZO_APPROVATO;

            m.LavoranteCodice = riga.LAVORANTE.Trim();
            m.LavoranteDescrizione = string.Empty;
            ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == m.LavoranteCodice).FirstOrDefault();
            if (lavorante != null)
            {
                m.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC;
            }

            ALEDS.MAGAZZRow modello = ds.MAGAZZ.Where(x => x.IDMAGAZZ == CHECKQ_T.IDMAGAZZ).FirstOrDefault();
            m.Modello = modello.MODELLO;
            m.ModelloDescrizione = modello.DESMAGAZZ;

            ALEDS.USR_CHECKQ_CRow CHECKQ_C = ds.USR_CHECKQ_C.Where(x => x.IDCHECKQT == CHECKQ_T.IDCHECKQT).FirstOrDefault();

            m.Difetto = string.Empty;
            m.TipoDifetto = string.Empty;
            if (CHECKQ_C != null)
            {
                ALEDS.USR_ANA_DIFETTIRow difetto = ds.USR_ANA_DIFETTI.Where(x => x.IDDIFETTO == CHECKQ_C.IDDIFETTO).FirstOrDefault();
                ALEDS.USR_TAB_TIPODIFETTIRow tipoDifetto = ds.USR_TAB_TIPODIFETTI.Where(x => x.IDTIPODIFETTO == difetto.IDTIPODIFETTO).FirstOrDefault();
                m.TipoDifetto = tipoDifetto.DESTIPODIFETTO;
                m.Difetto = difetto.DESDIFETTO;
            }

            ALEDS.USR_PRD_MOVFASIRow MovFase = ds.USR_PRD_MOVFASI.Where(x => x.IDCHECKQT == CHECKQ_T.IDCHECKQT).FirstOrDefault();
            if (MovFase != null)
            {
                m.Commessa = MovFase.IsRIFERIMENTO_INFRANull() ? string.Empty : MovFase.RIFERIMENTO_INFRA;
                m.DataCommessa = MovFase.IsDATARIF_INFRANull() ? string.Empty : MovFase.DATARIF_INFRA.ToShortDateString();
            }

            m.Stato = riga.STATO;
            m.UidUserInserimento = riga.UIDUSER_INSERIMENTO;
            m.DataInserimento = riga.DATA_INSERIMENTO;
            m.UidUserNonAddebito = riga.IsUIDUSER_NONADDEBITONull() ? string.Empty : riga.UIDUSER_NONADDEBITO;
            m.DataNonAddebito = riga.IsDATA_NONADDEBITONull() ? (DateTime?)null : riga.DATA_NONADDEBITO;

            m.UrlImage = creaUrlImage(rvlImageSite, CHECKQ_T.IDMAGAZZ, ds);

            m.Costi = new List<CostiAddebitiModel>();
            List<ALEDS.RW_ALE_DETT_COSTORow> costiAssegnati = ds.RW_ALE_DETT_COSTO.Where(x => x.IDALEDETTAGLIO == riga.IDALEDETTAGLIO).ToList();
            if (costiAssegnati.Count > 0)
            {
                foreach (ALEDS.RW_ALE_DETT_COSTORow costo in costiAssegnati)
                {
                    CostiAddebitiModel cm = new CostiAddebitiModel()
                    {
                        CostoFase = costo.COSTO,
                        Fase = costo.FASE,
                        IdAleDettaglio = costo.IDALEDETTAGLIO,
                        IdAleDettCosto = costo.IDALEDETCOSTO
                    };
                    m.Costi.Add(cm);
                }

            }
            //else
            //{
            //    List<ALEDS.RW_ALE_COSTO_MAGAZZRow> costiModello = ds.RW_ALE_COSTO_MAGAZZ.Where(x => x.IDMAGAZZ == modello.IDMAGAZZ).ToList();
            //    foreach (ALEDS.RW_ALE_COSTO_MAGAZZRow costo in costiModello)
            //    {
            //        CostiAddebitiModel cm = new CostiAddebitiModel()
            //        {
            //            CostoFase = costo.COSTO,
            //            Fase = costo.FASE,
            //            IdAleDettaglio = -1,
            //            IdAleDettCosto = -1
            //        };
            //        m.Costi.Add(cm);
            //    }
            //}

            m.ListaFasi = CreaListaFasiPerValorizzazioneCosto(ds, modello.IDMAGAZZ);

            return m;
        }

        private List<FaseCosto> CreaListaFasiPerValorizzazioneCosto(ALEDS ds, string idmagazz)
        {
            List<FaseCosto> fasi = new List<FaseCosto>();

            List<ALEDS.RW_ALE_COSTO_MAGAZZRow> costiModello = ds.RW_ALE_COSTO_MAGAZZ.Where(x => x.IDMAGAZZ == idmagazz).ToList();
            fasi = (from fff in costiModello select new FaseCosto(fff.FASE, fff.COSTO.ToString().Replace(',', '.'))).ToList();

            if (fasi.Count > 0) return fasi;

            ALEDS.RW_ALE_FASI_DA_ES_DIBARow fasePadre = ds.RW_ALE_FASI_DA_ES_DIBA.Where(x => x.IDPADRE == idmagazz).FirstOrDefault();
            if (fasePadre == null) return fasi;
            List<ALEDS.RW_ALE_FASI_DA_ES_DIBARow> fasiFiglie = ds.RW_ALE_FASI_DA_ES_DIBA.Where(x => x.IDPRODOTTOFINITO == fasePadre.IDPRODOTTOFINITO && x.SEQUENZA >= fasePadre.SEQUENZA).OrderBy(x => x.SEQUENZA).ToList();

            ALEDS.RW_ALE_FASI_DA_ES_DIBARow faseFiglia = fasiFiglie.Where(x => x.IDPADRE == idmagazz).FirstOrDefault();

            while (faseFiglia != null)
            {
                fasi.Add(new FaseCosto(faseFiglia.FASE, faseFiglia.IsCOSTOUNINull() ? "0" : faseFiglia.COSTOUNI.ToString().Replace(',', '.')));
                faseFiglia = fasiFiglie.Where(x => x.IDPADRE == faseFiglia.IDARTICOLO).FirstOrDefault();
                // manca il caso di montaggio
            }

            //            fasi = (from fff in fasiFiglie select new FaseCosto(fff.FASE, fff.IsCOSTOUNINull() ? "0" : fff.COSTOUNI.ToString().Replace(',', '.'))).ToList();
            return fasi.Distinct().ToList();
        }

        public bool VerificaBarcodeCaricato(string Barcode)
        {
            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIOByBarcode(ds, Barcode);
                return ds.RW_ALE_DETTAGLIO.Any(x => x.BARCODE == Barcode);
            }
        }

        public void Addebita(string NotaGruppo, string Lavorante, string AddebitiJson, bool Rilavorazione, string UIDUSER)
        {
            ALEAddebitiJsonModel[] addebiti = JSonSerializer.Deserialize<ALEAddebitiJsonModel[]>(AddebitiJson);
            int IDGRUPPO = -1;
            using (ALEBusiness bALE = new ALEBusiness())
            {
                try
                {
                    ALEDS ds = new ALEDS();
                    bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.INSERITO);

                    IDGRUPPO = bALE.CreaGruppo(NotaGruppo, Lavorante.Trim(), UIDUSER, Rilavorazione);

                    foreach (ALEAddebitiJsonModel addebitoJ in addebiti)
                    {
                        ALEDS.RW_ALE_DETTAGLIORow dettaglio = ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEDETTAGLIO == addebitoJ.IdAleDettaglio).FirstOrDefault();
                        if (dettaglio != null)
                        {
                            dettaglio.IDALEGRUPPO = IDGRUPPO;
                            dettaglio.QUANTITAADDEBITATA = addebitoJ.Quantita;
                            dettaglio.NOTAADDEBITO = addebitoJ.Nota;
                            dettaglio.STATO = ALEStatoDettaglio.ADDEBITATO;
                        }
                    }

                    bALE.UpdateRW_ALE_DETTAGLIO(ds);

                }
                catch
                {
                    bALE.Rollback();
                    throw;
                }
            }

            GruppoModel gruppo = LeggiGruppi(ALEStatoDettaglio.ADDEBITATO).Where(x => x.IDALEGRUPPO == IDGRUPPO).FirstOrDefault();

            MailDispatcherBLL bllMD = new MailDispatcherBLL();
            string oggetto = "ALE - NUOVO GRUPPO ADDEBITO INSERITO ID GRUPPO:" + IDGRUPPO.ToString();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("E' stato creato il gruppo: {0} che deve essere valorizzato", IDGRUPPO));
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.Format("Lavorante: {0}", gruppo.LavoranteDescrizione.Trim()));
            sb.AppendLine(string.Format("Creato da: {0}", gruppo.UtenteAddebito.Trim()));
            if (!string.IsNullOrEmpty(gruppo.NotaAddebito))
                sb.AppendLine(string.Format("Nota addebito: {0}", gruppo.NotaAddebito.Trim()));
            sb.AppendLine(string.Empty);
            sb.AppendLine("Articoli addebitati");
            sb.AppendLine(string.Empty);
            foreach (AddebitoModel addebito in gruppo.Dettagli)
            {
                sb.AppendLine(string.Format("Modello: {0}  Numero pezzi: {1}  Motivo scarto: {2}", addebito.Modello, addebito.QuantitaAddebitata, addebito.Difetto));
                if (!string.IsNullOrEmpty(addebito.NotaInserimento))
                    sb.AppendLine(string.Format("         Nota inserimento: {0}", addebito.NotaInserimento));
                if (!string.IsNullOrEmpty(addebito.NotaAddebito))
                    sb.AppendLine(string.Format("         Nota addebito: {0}", addebito.NotaAddebito));
            }
            Decimal IDMAIL = 0;
            if (gruppo.Rilavorazione)
                IDMAIL = bllMD.CreaEmail("ALE - RILAVORAZIONE", oggetto, sb.ToString());
            else
                IDMAIL = bllMD.CreaEmail("ALE - VALORIZZAZIONE", oggetto, sb.ToString());

            bllMD.SottomettiEmail(IDMAIL);

        }

        public void NonAddebitare(string NotaGruppo, string AddebitiJson, string UIDUSER)
        {
            ALEAddebitiJsonModel[] addebiti = JSonSerializer.Deserialize<ALEAddebitiJsonModel[]>(AddebitiJson);

            using (ALEBusiness bALE = new ALEBusiness())
            {
                try
                {
                    ALEDS ds = new ALEDS();
                    bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.INSERITO);

                    foreach (ALEAddebitiJsonModel addebitoJ in addebiti)
                    {
                        ALEDS.RW_ALE_DETTAGLIORow dettaglio = ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEDETTAGLIO == addebitoJ.IdAleDettaglio).FirstOrDefault();
                        if (dettaglio != null)
                        {
                            dettaglio.QUANTITAADDEBITATA = 0;
                            dettaglio.NOTAADDEBITO = addebitoJ.Nota;
                            dettaglio.STATO = ALEStatoDettaglio.NONADDEBITATO;
                            dettaglio.DATA_NONADDEBITO = DateTime.Today;
                            dettaglio.UIDUSER_NONADDEBITO = UIDUSER;
                        }
                    }

                    bALE.UpdateRW_ALE_DETTAGLIO(ds);

                }
                catch
                {
                    bALE.Rollback();
                    throw;
                }
            }
        }

        public void ReinserisciDaAddebitare(decimal IdAleDettaglio, string UIDUSER)
        {

            using (ALEBusiness bALE = new ALEBusiness())
            {
                try
                {
                    ALEDS ds = new ALEDS();
                    bALE.FillRW_ALE_DETTAGLIOByPK(ds, IdAleDettaglio);

                    ALEDS.RW_ALE_DETTAGLIORow dettaglio = ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEDETTAGLIO == IdAleDettaglio).FirstOrDefault();
                    if (dettaglio != null)
                    {
                        dettaglio.QUANTITAADDEBITATA = 0;
                        dettaglio.STATO = ALEStatoDettaglio.INSERITO;
                        dettaglio.DATA_INSERIMENTO = DateTime.Today;
                        dettaglio.SetDATA_NONADDEBITONull();
                    }
                    bALE.UpdateRW_ALE_DETTAGLIO(ds);

                }
                catch
                {
                    bALE.Rollback();
                    throw;
                }
            }
        }

        public void AnnullaAddebita(int IDALEGRUPPO)
        {

            using (ALEBusiness bALE = new ALEBusiness())
            {
                try
                {
                    ALEDS ds = new ALEDS();
                    bALE.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
                    bALE.FillRW_ALE_DETTAGLIO(ds, IDALEGRUPPO);

                    foreach (ALEDS.RW_ALE_DETTAGLIORow dettaglio in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == IDALEGRUPPO))
                    {
                        dettaglio.SetIDALEGRUPPONull();
                        dettaglio.SetQUANTITAADDEBITATANull();
                        dettaglio.SetNOTAADDEBITONull();
                        dettaglio.STATO = ALEStatoDettaglio.INSERITO;
                    }

                    ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == IDALEGRUPPO).FirstOrDefault();
                    if (gruppo != null)
                        gruppo.Delete();

                    bALE.UpdateRW_ALE_DETTAGLIO(ds);
                    bALE.UpdateRW_ALE_GRUPPO(ds);

                    MailDispatcherBLL bllMD = new MailDispatcherBLL();
                    string oggetto = "ALE - ANNULLATO NUOVO GRUPPO ADDEBITO INSERITO ID GRUPPO:" + IDALEGRUPPO.ToString();
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("Il gruppo di addebito: {0} è stato annullato. Nessuna valorizzazione da fare", IDALEGRUPPO));

                    decimal IDMAIL = 0;
                    if (gruppo.RILAVORAZIONECOMPLESSA == "1")
                        IDMAIL = bllMD.CreaEmail("ALE - RILAVORAZIONE", oggetto, sb.ToString());
                    else
                        IDMAIL = bllMD.CreaEmail("ALE - VALORIZZAZIONE", oggetto, sb.ToString());
                    bllMD.SottomettiEmail(IDMAIL);


                }
                catch
                {
                    bALE.Rollback();
                    throw;
                }
            }
        }

        public List<GruppoModel> LeggiGruppi(string Stato)
        {
            List<GruppoModel> model = new List<GruppoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, Stato);
                List<decimal> IDALEGRUPPO = ds.RW_ALE_DETTAGLIO.Select(x => (decimal)x.IDALEGRUPPO).Distinct().ToList();

                bALE.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                List<string> IDCHECKQT = ds.RW_ALE_DETTAGLIO.Select(x => x.IDCHECKQT).ToList();
                bALE.FillUSR_CHECKQ_T(ds, IDCHECKQT);
                bALE.FillUSR_CHECKQ_C(ds, IDCHECKQT);
                bALE.FillUSR_PRD_MOVFASI(ds, IDCHECKQT);
                List<string> IDMAGAZZ = ds.USR_CHECKQ_T.Select(x => x.IDMAGAZZ).ToList();
                bALE.FillMAGAZZ(ds, IDMAGAZZ);
                bALE.FillUSR_PDM_FILES(ds, IDMAGAZZ);
                bALE.FillTABFAS(ds);
                bALE.FillRW_ALE_FASI_DA_ES_DIBA(ds, IDMAGAZZ);
                bALE.FillRW_ALE_COSTO_MAGAZZ(ds);
                List<string> IDPRDMOVFASE = ds.USR_PRD_MOVFASI.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                bALE.FillUSR_PRD_FASI(ds, IDPRDMOVFASE);

                List<decimal> IDALEDETTAGLIO = ds.RW_ALE_DETTAGLIO.Select(x => x.IDALEDETTAGLIO).ToList();
                bALE.FillRW_ALE_DETT_COSTO(ds, IDALEDETTAGLIO);

                bool annullaAddebito = false;
                bool annullaValorizzazione = false;
                bool annullaApprovazione = false;

                switch (Stato)
                {
                    case ALEStatoDettaglio.ADDEBITATO:
                        annullaAddebito = true;
                        break;
                    case ALEStatoDettaglio.VALORIZZATO:
                        annullaValorizzazione = true;
                        break;
                    case ALEStatoDettaglio.APPROVATO:
                        annullaApprovazione = true;
                        break;
                }

                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    GruppoModel grModel = CreaGruppoModel(gruppo, ds, annullaAddebito, annullaValorizzazione, annullaApprovazione);
                    model.Add(grModel);
                }
            }

            return model;
        }

        public List<GruppoModel> LeggiGruppiFatturati(DateTime DataInizio, DateTime DataFine)
        {
            List<GruppoModel> model = new List<GruppoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_GRUPPOFatturato(ds, DataInizio, DataFine);

                List<decimal> idGruppi = ds.RW_ALE_GRUPPO.Select(x => x.IDALEGRUPPO).Distinct().ToList();
                bALE.FillRW_ALE_DETTAGLIO(ds, idGruppi);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                List<string> IDCHECKQT = ds.RW_ALE_DETTAGLIO.Select(x => x.IDCHECKQT).ToList();
                bALE.FillUSR_CHECKQ_T(ds, IDCHECKQT);
                bALE.FillUSR_CHECKQ_C(ds, IDCHECKQT);
                bALE.FillUSR_PRD_MOVFASI(ds, IDCHECKQT);
                List<string> IDMAGAZZ = ds.USR_CHECKQ_T.Select(x => x.IDMAGAZZ).ToList();
                bALE.FillMAGAZZ(ds, IDMAGAZZ);
                bALE.FillUSR_PDM_FILES(ds, IDMAGAZZ);
                bALE.FillTABFAS(ds);

                List<string> IDPRDMOVFASE = ds.USR_PRD_MOVFASI.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                bALE.FillUSR_PRD_FASI(ds, IDPRDMOVFASE);

                List<decimal> IDALEDETTAGLIO = ds.RW_ALE_DETTAGLIO.Select(x => x.IDALEDETTAGLIO).ToList();
                bALE.FillRW_ALE_DETT_COSTO(ds, IDALEDETTAGLIO);

                bool annullaAddebito = false;
                bool annullaValorizzazione = false;
                bool annullaApprovazione = false;

                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    GruppoModel grModel = CreaGruppoModel(gruppo, ds, annullaAddebito, annullaValorizzazione, annullaApprovazione);
                    model.Add(grModel);
                }
            }

            return model;
        }

        private GruppoModel CreaGruppoModel(ALEDS.RW_ALE_GRUPPORow RWGruppo, ALEDS ds, bool addebitoAnnullabile, bool valorizzazioneAnnullabile, bool approvazioneAnnullabile)
        {
            GruppoModel grModel = new GruppoModel();
            grModel.IDALEGRUPPO = RWGruppo.IDALEGRUPPO;
            grModel.Aperto = (RWGruppo.APERTO == "0");
            grModel.Dettagli = new List<AddebitoModel>();

            grModel.LavoranteCodice = RWGruppo.LAVORANTE.Trim();
            grModel.LavoranteDescrizione = string.Empty;
            grModel.AddebitoAnnulabile = addebitoAnnullabile;
            grModel.ValorizzazioneAnnulabile = valorizzazioneAnnullabile;
            grModel.ApprovazioneAnnulabile = approvazioneAnnullabile;
            ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == grModel.LavoranteCodice).FirstOrDefault();
            if (lavorante != null)
                grModel.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC.Trim();

            grModel.NotaAddebito = RWGruppo.IsNOTA_ADDEBITONull() ? string.Empty : RWGruppo.NOTA_ADDEBITO;
            grModel.NotaValorizzazione = RWGruppo.IsNOTA_VALORIZZAZIONENull() ? string.Empty : RWGruppo.NOTA_VALORIZZAZIONE;
            grModel.NotaApprovazione = RWGruppo.IsNOTA_APPROVAZIONENull() ? string.Empty : RWGruppo.NOTA_APPROVAZIONE;
            grModel.NotaFatturazione = RWGruppo.IsNOTA_FATTURAZIONENull() ? string.Empty : RWGruppo.NOTA_FATTURAZIONE;

            grModel.DataAddebito = RWGruppo.IsDATA_ADDEBITONull() ? (DateTime?)null : RWGruppo.DATA_ADDEBITO;
            grModel.DataValorizzazione = RWGruppo.IsDATA_VALORIZZAZIONENull() ? (DateTime?)null : RWGruppo.DATA_VALORIZZAZIONE;
            grModel.DataApprovazione = RWGruppo.IsDATA_APPROVAZIONENull() ? (DateTime?)null : RWGruppo.DATA_APPROVAZIONE;
            grModel.DataFatturazione = RWGruppo.IsDATA_FATTURAZIONENull() ? (DateTime?)null : RWGruppo.DATA_FATTURAZIONE;

            grModel.UtenteAddebito = RWGruppo.UIDUSER_ADDEBITO;
            grModel.UtenteValorizzazione = RWGruppo.IsUIDUSER_VALORIZZAZIONENull() ? string.Empty : RWGruppo.UIDUSER_VALORIZZAZIONE;
            grModel.UtenteApprovazione = RWGruppo.IsUIDUSER_APPROVAZIONENull() ? string.Empty : RWGruppo.UIDUSER_APPROVAZIONE;
            grModel.UtenteFatturazione = RWGruppo.IsUIDUSER_FATTURAZIONENull() ? string.Empty : RWGruppo.UIDUSER_FATTURAZIONE;
            grModel.Rilavorazione = RWGruppo.IsRILAVORAZIONECOMPLESSANull() ? false : (RWGruppo.RILAVORAZIONECOMPLESSA == "0" ? false : true);

            foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == RWGruppo.IDALEGRUPPO))
            {
                ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                AddebitoModel m = CreaAddebitoModel(ds, riga);
                grModel.Dettagli.Add(m);
            }
            return grModel;
        }

        public void Valorizza(string IDALEGRUPPO, string Dettagli, string NotaGruppo, string UIDUSER)
        {
            ALEValorizzaJson[] Valorizzati = JSonSerializer.Deserialize<ALEValorizzaJson[]>(Dettagli);
            decimal idAleAgruppo = decimal.Parse(IDALEGRUPPO);
            using (ALEBusiness bALE = new ALEBusiness())
            {
                long idGruppo = long.Parse(IDALEGRUPPO);
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, idAleAgruppo);
                bALE.FillRW_ALE_GRUPPO(ds, new List<decimal>(new decimal[] { idGruppo }));
                List<decimal> idDettaglio = ds.RW_ALE_DETTAGLIO.Select(x => x.IDALEDETTAGLIO).ToList();
                bALE.FillRW_ALE_DETT_COSTO(ds, idDettaglio);

                foreach (ALEValorizzaJson val in Valorizzati)
                {
                    ALEDS.RW_ALE_DETTAGLIORow dettaglio = ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEDETTAGLIO == val.IdAleDettaglio).FirstOrDefault();
                    if (dettaglio != null)
                    {
                        if (val.Prezzo.HasValue)
                            dettaglio.PREZZO = val.Prezzo.Value;
                        else
                            dettaglio.SetPREZZONull();
                        dettaglio.STATO = ALEStatoDettaglio.VALORIZZATO;
                        dettaglio.NOTAVALORIZZAZIONE = val.Nota;
                        string idmagazz = bALE.GetIdMagazzFromIdDettaglio(dettaglio.IDALEDETTAGLIO);
                        foreach (CostoFaseJson costoFase in val.CostiFase)
                        {
                            ALEDS.RW_ALE_DETT_COSTORow dettCosto = ds.RW_ALE_DETT_COSTO.NewRW_ALE_DETT_COSTORow();
                            dettCosto.IDALEDETTAGLIO = dettaglio.IDALEDETTAGLIO;
                            dettCosto.FASE = (costoFase.Fase.Length > 50) ? costoFase.Fase.Substring(0, 50) : costoFase.Fase;
                            if (costoFase.Costo.HasValue)
                                dettCosto.COSTO = costoFase.Costo.Value;
                            else
                                dettCosto.COSTO = 0;
                            dettCosto.DATA_INSERIMENTO = DateTime.Now;
                            dettCosto.UIDUSER = UIDUSER;
                            ds.RW_ALE_DETT_COSTO.AddRW_ALE_DETT_COSTORow(dettCosto);
                        }

                        foreach (CostoFaseJson costoFase in val.CostiFase)
                        {
                            ALEDS.RW_ALE_COSTO_MAGAZZRow costoMagazz = ds.RW_ALE_COSTO_MAGAZZ.NewRW_ALE_COSTO_MAGAZZRow();
                            costoMagazz.IDMAGAZZ = idmagazz;
                            costoMagazz.FASE = (costoFase.Fase.Length > 50) ? costoFase.Fase.Substring(0, 50) : costoFase.Fase;
                            if (costoFase.Costo.HasValue)
                                costoMagazz.COSTO = costoFase.Costo.Value;
                            else
                                costoMagazz.COSTO = 0;
                            ds.RW_ALE_COSTO_MAGAZZ.AddRW_ALE_COSTO_MAGAZZRow(costoMagazz);
                        }
                    }
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.NOTA_VALORIZZAZIONE = NotaGruppo;
                    gruppo.DATA_VALORIZZAZIONE = DateTime.Now;
                    gruppo.UIDUSER_VALORIZZAZIONE = UIDUSER;
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_DETT_COSTO(ds);
                bALE.UpdateRW_ALE_COSTO_MAGAZZ(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);

                MailDispatcherBLL bllMD = new MailDispatcherBLL();
                string oggetto = "ALE - NUOVO GRUPPO DA APPROVARE:" + IDALEGRUPPO.ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("E' stato valorizzato il gruppo: {0}.", IDALEGRUPPO));

                decimal IDMAIL = bllMD.CreaEmail("ALE - APPROVAZIONE", oggetto, sb.ToString());
                bllMD.SottomettiEmail(IDMAIL);
            }
        }

        public void ApprovaGruppo(string IDALEGRUPPO, string Dettagli, string NotaGruppo, string UIDUSER)
        {
            ALEValorizzaJson[] Valorizzati = JSonSerializer.Deserialize<ALEValorizzaJson[]>(Dettagli);
            decimal idAleAgruppo = decimal.Parse(IDALEGRUPPO);
            using (ALEBusiness bALE = new ALEBusiness())
            {
                long idGruppo = long.Parse(IDALEGRUPPO);
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, idAleAgruppo);
                bALE.FillRW_ALE_GRUPPO(ds, new List<decimal>(new decimal[] { idGruppo }));

                foreach (ALEValorizzaJson val in Valorizzati)
                {
                    ALEDS.RW_ALE_DETTAGLIORow dettaglio = ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEDETTAGLIO == val.IdAleDettaglio).FirstOrDefault();
                    if (dettaglio != null)
                    {
                        if (val.Prezzo.HasValue)
                            dettaglio.PREZZO_APPROVATO = val.Prezzo.Value;
                        else
                            dettaglio.SetPREZZO_APPROVATONull();
                        dettaglio.STATO = ALEStatoDettaglio.APPROVATO;
                        dettaglio.NOTAAPPROVAZIONE = val.Nota;
                    }
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.NOTA_APPROVAZIONE = NotaGruppo;
                    gruppo.DATA_APPROVAZIONE = DateTime.Now;
                    gruppo.UIDUSER_APPROVAZIONE = UIDUSER;
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);

                MailDispatcherBLL bllMD = new MailDispatcherBLL();
                string oggetto = "ALE - NUOVO GRUPPO DA FATTURARE:" + IDALEGRUPPO.ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("E' stato approvato il gruppo: {0}.", IDALEGRUPPO));

                decimal IDMAIL = bllMD.CreaEmail("ALE - FATTURAZIONE", oggetto, sb.ToString());
                bllMD.SottomettiEmail(IDMAIL);

            }
        }

        public void AnnullaApprovazione(string IDALEGRUPPO, string UIDUSER)
        {
            decimal idAleAgruppo = decimal.Parse(IDALEGRUPPO);
            using (ALEBusiness bALE = new ALEBusiness())
            {
                long idGruppo = long.Parse(IDALEGRUPPO);
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, idAleAgruppo);
                bALE.FillRW_ALE_GRUPPO(ds, new List<decimal>(new decimal[] { idGruppo }));

                foreach (ALEDS.RW_ALE_DETTAGLIORow dettaglio in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == idAleAgruppo))
                {
                    dettaglio.SetPREZZO_APPROVATONull();
                    dettaglio.STATO = ALEStatoDettaglio.VALORIZZATO;
                    dettaglio.SetNOTAAPPROVAZIONENull();
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.SetNOTA_APPROVAZIONENull();
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);

                MailDispatcherBLL bllMD = new MailDispatcherBLL();
                string oggetto = "ALE - ANNULLATA APPROVAZIONE DEL GRUPPO:" + IDALEGRUPPO.ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("L'approvazione per il gruppo: {0} è stata annullata. Nessuna fatturazione da fare", IDALEGRUPPO));
                decimal IDMAIL = 0;
                if (gruppo.RILAVORAZIONECOMPLESSA == "1")
                    IDMAIL = bllMD.CreaEmail("ALE - RILAVORAZIONE", oggetto, sb.ToString());
                else
                    IDMAIL = bllMD.CreaEmail("ALE - FATTURAZIONE", oggetto, sb.ToString());
                bllMD.SottomettiEmail(IDMAIL);
            }
        }

        public void AnnullaValorizzazione(string IDALEGRUPPO, string UIDUSER)
        {
            decimal idAleAgruppo = decimal.Parse(IDALEGRUPPO);
            using (ALEBusiness bALE = new ALEBusiness())
            {
                long idGruppo = long.Parse(IDALEGRUPPO);
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, idAleAgruppo);
                List<decimal> idDettaglio = ds.RW_ALE_DETTAGLIO.Select(x => x.IDALEDETTAGLIO).ToList();
                bALE.FillRW_ALE_GRUPPO(ds, new List<decimal>(new decimal[] { idGruppo }));
                bALE.FillRW_ALE_DETT_COSTO(ds, idDettaglio);


                foreach (ALEDS.RW_ALE_DETTAGLIORow dettaglio in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == idGruppo))
                {
                    dettaglio.SetPREZZONull();
                    dettaglio.STATO = ALEStatoDettaglio.ADDEBITATO;
                    dettaglio.SetNOTAVALORIZZAZIONENull();

                    foreach (ALEDS.RW_ALE_DETT_COSTORow costo in ds.RW_ALE_DETT_COSTO.Where(x => x.RowState != System.Data.DataRowState.Deleted && x.IDALEDETTAGLIO == dettaglio.IDALEDETTAGLIO))
                        costo.Delete();
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.SetNOTA_VALORIZZAZIONENull();
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);
                bALE.UpdateRW_ALE_DETT_COSTO(ds);

                MailDispatcherBLL bllMD = new MailDispatcherBLL();
                string oggetto = "ALE - ANNULLATA VALORIZZAZIONE DEL GRUPPO:" + IDALEGRUPPO.ToString();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("La valorizzazione per il gruppo: {0} è stata annullata. Nessuna approvazione da fare", IDALEGRUPPO));

                decimal IDMAIL = bllMD.CreaEmail("ALE - APPROVAZIONE", oggetto, sb.ToString());
                bllMD.SottomettiEmail(IDMAIL);

            }
        }

        public void FatturaGruppo(string IDALEGRUPPO, string NotaGruppo, string UIDUSER)
        {
            decimal idAleAgruppo = decimal.Parse(IDALEGRUPPO);
            using (ALEBusiness bALE = new ALEBusiness())
            {
                long idGruppo = long.Parse(IDALEGRUPPO);
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, idAleAgruppo);
                bALE.FillRW_ALE_GRUPPO(ds, new List<decimal>(new decimal[] { idGruppo }));

                foreach (ALEDS.RW_ALE_DETTAGLIORow dettaglio in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == idAleAgruppo))
                {
                    if (dettaglio != null)
                    {
                        dettaglio.STATO = ALEStatoDettaglio.FATTURATO;
                    }
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.NOTA_FATTURAZIONE = NotaGruppo;
                    gruppo.APERTO = "1";
                    gruppo.UIDUSER_FATTURAZIONE = UIDUSER;
                    gruppo.DATA_FATTURAZIONE = DateTime.Now;
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);
            }
        }

        public void AnnullaFatturaGruppo(string IDALEGRUPPO, string UIDUSER)
        {
            decimal idAleAgruppo = decimal.Parse(IDALEGRUPPO);
            using (ALEBusiness bALE = new ALEBusiness())
            {
                long idGruppo = long.Parse(IDALEGRUPPO);
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, idAleAgruppo);
                bALE.FillRW_ALE_GRUPPO(ds, new List<decimal>(new decimal[] { idGruppo }));

                foreach (ALEDS.RW_ALE_DETTAGLIORow dettaglio in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == idGruppo))
                {
                    dettaglio.STATO = ALEStatoDettaglio.APPROVATO;
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.SetNOTA_FATTURAZIONENull();
                    gruppo.SetDATA_FATTURAZIONENull();
                    gruppo.APERTO = "0";

                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);
            }
        }

    }
}
