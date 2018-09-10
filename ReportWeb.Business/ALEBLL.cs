using ReportWeb.Common.Helpers;
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
        public InserimentoModel CaricaScheda(string Barcode, string RvlImageSite)
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
                ALEDS.USR_CHECKQ_CRow CHECKQ_C = ds.USR_CHECKQ_C.Where(x => x.IDCHECKQT == CHECKQ_T.IDCHECKQT).FirstOrDefault();

                model.EsitoRicerca = 2;
                model.IDCHECKQT = CHECKQ_T.IDCHECKQT;
                model.Barcode = CHECKQ_T.BARCODE;
                model.NumeroDocumento = CHECKQ_T.NUMCHECKQT;
                model.DataDocumento = CHECKQ_T.DATACHECKQT.ToString("dd MMM yyyy");
                model.Quantita = CHECKQ_T.IsQTANull() ? 0 : CHECKQ_T.QTA;
                model.QuantitaDifforme = CHECKQ_C == null ? 0 : (CHECKQ_C.IsQTA_DIFNull() ? 0 : CHECKQ_C.QTA_DIF);

                model.RepartoCodice = CHECKQ_T.IsCODICECLIFO_RILNull() ? string.Empty : CHECKQ_T.CODICECLIFO_RIL.Trim();
                ALEDS.CLIFORow reparto = ds.CLIFO.Where(x => x.CODICE == CHECKQ_T.CODICECLIFO_RIL).FirstOrDefault();
                if (reparto != null)
                    model.Reparto = reparto.RAGIONESOC;
                else
                    model.Reparto = string.Empty;

                model.Difetto = string.Empty;
                model.TipoDifetto = string.Empty;
                if (CHECKQ_C != null)
                {
                    ALEDS.USR_ANA_DIFETTIRow difetto = ds.USR_ANA_DIFETTI.Where(x => x.IDDIFETTO == CHECKQ_C.IDDIFETTO).FirstOrDefault();
                    ALEDS.USR_TAB_TIPODIFETTIRow tipoDifetto = ds.USR_TAB_TIPODIFETTI.Where(x => x.IDTIPODIFETTO == difetto.IDTIPODIFETTO).FirstOrDefault();
                    model.TipoDifetto = tipoDifetto.DESTIPODIFETTO;
                    model.Difetto = difetto.DESDIFETTO;
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
                ALEDS.USR_PDM_FILESRow immagine = ds.USR_PDM_FILES.Where(x => x.IDMAGAZZ == CHECKQ_T.IDMAGAZZ).FirstOrDefault();
                if (immagine != null)
                {

                    model.ImageUrl = RvlImageSite + immagine.NOMEFILE;
                }

                model.LavorantiEsterni = creaListaLavorantiEsterni(ds);

                return model;
            }
        }

        public void SalvaInserimento(string Barcode, string IDCHECKQT, int Difettosi, int Inseriti, string Lavorante, string Nota, string UIDUSER)
        {
            using (ALEBusiness bALE = new ALEBusiness())
            {
                bALE.SalvaInserimento(Barcode, IDCHECKQT, Difettosi, Inseriti, Lavorante, Nota, UIDUSER);
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

                foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO)
                {
                    bALE.FillUSR_CHECKQ_C(ds, riga.IDCHECKQT);
                    bALE.FillUSR_CHECKQ_T(ds, riga.BARCODE);
                    ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                    bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                    bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                    AddebitoModel m = CreaAddebitoModel(ds, riga, CHECKQ_T);
                    model.Addebiti.Add(m);
                }

                model.LavorantiEsterni = creaListaLavorantiEsterni(ds);

            }
            return model;
        }

        private AddebitoModel CreaAddebitoModel(ALEDS ds, ALEDS.RW_ALE_DETTAGLIORow riga, ALEDS.USR_CHECKQ_TRow CHECKQ_T)
        {
            AddebitoModel m = new AddebitoModel();
            m.IdAleDettaglio = riga.IDALEDETTAGLIO;
            m.IdAleGruppo = riga.IsIDALEGRUPPONull() ? -1 : riga.IDALEGRUPPO;
            m.QuantitaDifettosi = riga.QUANTITADIFETTOSI;
            m.QuantitaInseriti = riga.QUANTITAINSERITA;
            m.QuantitaAddebitata = riga.IsQUANTITAADDEBITATANull() ? 0 : riga.QUANTITAADDEBITATA;
            m.Nota = riga.IsNOTANull() ? string.Empty : riga.NOTA;
            m.NotaAddebito = riga.IsNOTAADDEBITONull() ? string.Empty : riga.NOTAADDEBITO;
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
            return m;
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

        public void Addebita(string NotaGruppo, string Lavorante, string AddebitiJson, string UIDUSER)
        {
            ALEAddebitiJsonModel[] addebiti = JSonSerializer.Deserialize<ALEAddebitiJsonModel[]>(AddebitiJson);

            using (ALEBusiness bALE = new ALEBusiness())
            {
                try
                {
                    ALEDS ds = new ALEDS();
                    bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.INSERITO);

                    int IDGRUPPO = bALE.CreaGruppo(NotaGruppo, Lavorante.Trim(), UIDUSER);

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
        }

        public void AnnullaAddebita(int IDALEGRUPPO)
        {

            using (ALEBusiness bALE = new ALEBusiness())
            {
                try
                {
                    ALEDS ds = new ALEDS();
                    bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.ADDEBITATO);
                    bALE.FillRW_ALE_GRUPPO(ds, new List<long>(new long[] { IDALEGRUPPO }));



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

                }
                catch
                {
                    bALE.Rollback();
                    throw;
                }
            }
        }

        public List<GruppoModel> LeggiGruppiAddebito()
        {
            List<GruppoModel> model = new List<GruppoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.ADDEBITATO);

                List<long> IDALEGRUPPO = ds.RW_ALE_DETTAGLIO.Select(x => (long)x.IDALEGRUPPO).Distinct().ToList();
                bALE.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    GruppoModel grModel = new GruppoModel();
                    grModel.IDALEGRUPPO = gruppo.IDALEGRUPPO;
                    grModel.Aperto = (gruppo.APERTO == "0");
                    grModel.Dettagli = new List<AddebitoModel>();

                    grModel.LavoranteCodice = gruppo.LAVORANTE.Trim();
                    grModel.LavoranteDescrizione = string.Empty;
                    grModel.AddebitoAnnulabile = true;
                    ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == grModel.LavoranteCodice).FirstOrDefault();
                    if (lavorante != null)
                        grModel.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC.Trim();

                    grModel.NotaAddebito = gruppo.IsNOTAADDEBITONull() ? string.Empty : gruppo.NOTAADDEBITO;

                    foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == gruppo.IDALEGRUPPO))
                    {
                        bALE.FillUSR_CHECKQ_C(ds, riga.IDCHECKQT);
                        bALE.FillUSR_CHECKQ_T(ds, riga.BARCODE);
                        ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                        bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                        bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                        AddebitoModel m = CreaAddebitoModel(ds, riga, CHECKQ_T);
                        grModel.Dettagli.Add(m);
                    }
                    model.Add(grModel);
                }
            }

            return model;
        }

        public List<GruppoModel> LeggiGruppiDaValorizzare()
        {
            List<GruppoModel> model = new List<GruppoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.ADDEBITATO);

                List<long> IDALEGRUPPO = ds.RW_ALE_DETTAGLIO.Select(x => (long)x.IDALEGRUPPO).Distinct().ToList();
                bALE.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    GruppoModel grModel = new GruppoModel();
                    grModel.IDALEGRUPPO = gruppo.IDALEGRUPPO;
                    grModel.Aperto = (gruppo.APERTO == "0");
                    grModel.Dettagli = new List<AddebitoModel>();

                    grModel.LavoranteCodice = gruppo.LAVORANTE.Trim();
                    grModel.LavoranteDescrizione = string.Empty;
                    grModel.AddebitoAnnulabile = true;
                    ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == grModel.LavoranteCodice).FirstOrDefault();
                    if (lavorante != null)
                        grModel.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC.Trim();

                    grModel.NotaAddebito = gruppo.IsNOTAADDEBITONull() ? string.Empty : gruppo.NOTAADDEBITO;

                    foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == gruppo.IDALEGRUPPO))
                    {
                        bALE.FillUSR_CHECKQ_C(ds, riga.IDCHECKQT);
                        bALE.FillUSR_CHECKQ_T(ds, riga.BARCODE);
                        ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                        bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                        bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                        AddebitoModel m = CreaAddebitoModel(ds, riga, CHECKQ_T);
                        grModel.Dettagli.Add(m);
                    }
                    model.Add(grModel);
                }
            }

            return model;
        }

        public List<GruppoModel> LeggiAltriGruppiNonAddebito()
        {
            List<GruppoModel> model = new List<GruppoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_GRUPPO(ds, true);

                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.ADDEBITATO);
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.VALORIZZATO);
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.APPROVATO);

                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    if (ds.RW_ALE_DETTAGLIO.Any(x => x.IDALEGRUPPO == gruppo.IDALEGRUPPO))
                    {
                        GruppoModel grModel = new GruppoModel();
                        grModel.IDALEGRUPPO = gruppo.IDALEGRUPPO;
                        grModel.Aperto = (gruppo.APERTO == "0");
                        grModel.Dettagli = new List<AddebitoModel>();

                        grModel.LavoranteCodice = gruppo.LAVORANTE.Trim();
                        grModel.LavoranteDescrizione = string.Empty;
                        grModel.AddebitoAnnulabile = false;
                        ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == grModel.LavoranteCodice).FirstOrDefault();
                        if (lavorante != null)
                            grModel.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC.Trim();

                        grModel.NotaAddebito = gruppo.IsNOTAADDEBITONull() ? string.Empty : gruppo.NOTAADDEBITO;

                        foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == gruppo.IDALEGRUPPO))
                        {
                            bALE.FillUSR_CHECKQ_C(ds, riga.IDCHECKQT);
                            bALE.FillUSR_CHECKQ_T(ds, riga.BARCODE);
                            ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                            bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                            bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                            AddebitoModel m = CreaAddebitoModel(ds, riga, CHECKQ_T);
                            grModel.Dettagli.Add(m);
                        }
                        model.Add(grModel);
                    }
                }
            }

            return model;
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
                bALE.FillRW_ALE_GRUPPO(ds, new List<long>(new long[] { idGruppo }));

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
                        dettaglio.UIDUSER = UIDUSER;
                        dettaglio.NOTAVALORIZZAZIONE = val.Nota;
                    }
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.NOTAVALORIZZAZIONE = NotaGruppo;
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);
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
                bALE.FillRW_ALE_GRUPPO(ds, new List<long>(new long[] { idGruppo }));

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
                        dettaglio.UIDUSER = UIDUSER;
                        dettaglio.NOTAAPPROVAZIONE = val.Nota;
                    }
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.NOTAAPPROVAZIONE = NotaGruppo;
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);
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
                bALE.FillRW_ALE_GRUPPO(ds, new List<long>(new long[] { idGruppo }));

                foreach (ALEDS.RW_ALE_DETTAGLIORow dettaglio in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == idAleAgruppo))
                {
                    dettaglio.SetPREZZO_APPROVATONull();
                    dettaglio.STATO = ALEStatoDettaglio.VALORIZZATO;
                    dettaglio.UIDUSER = UIDUSER;
                    dettaglio.SetNOTAAPPROVAZIONENull();
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.SetNOTAAPPROVAZIONENull();
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);
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
                bALE.FillRW_ALE_GRUPPO(ds, new List<long>(new long[] { idGruppo }));

                foreach (ALEDS.RW_ALE_DETTAGLIORow dettaglio in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == idGruppo))
                {
                    dettaglio.SetPREZZONull();
                    dettaglio.STATO = ALEStatoDettaglio.ADDEBITATO;
                    dettaglio.UIDUSER = UIDUSER;
                    dettaglio.SetNOTAVALORIZZAZIONENull();
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.SetNOTAVALORIZZAZIONENull();
                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);
            }
        }

        public List<GruppoValorizzatoModel> LeggiGruppiValorizzati()
        {
            List<GruppoValorizzatoModel> model = new List<GruppoValorizzatoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.VALORIZZATO);

                List<long> IDALEGRUPPO = ds.RW_ALE_DETTAGLIO.Select(x => (long)x.IDALEGRUPPO).Distinct().ToList();
                bALE.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    GruppoValorizzatoModel grModel = new GruppoValorizzatoModel();
                    grModel.IDALEGRUPPO = gruppo.IDALEGRUPPO;
                    grModel.Aperto = (gruppo.APERTO == "0");
                    grModel.Dettagli = new List<ValorizzatoModel>();

                    grModel.LavoranteCodice = gruppo.LAVORANTE.Trim();
                    grModel.LavoranteDescrizione = string.Empty;
                    grModel.ValorizzazioneAnnullabile = true;
                    ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == grModel.LavoranteCodice).FirstOrDefault();
                    if (lavorante != null)
                        grModel.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC.Trim();

                    grModel.NotaValorizzazione = gruppo.IsNOTAVALORIZZAZIONENull() ? string.Empty : gruppo.NOTAVALORIZZAZIONE;
                    grModel.ValoreTotale = 0;
                    foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == gruppo.IDALEGRUPPO))
                    {
                        bALE.FillUSR_CHECKQ_C(ds, riga.IDCHECKQT);
                        bALE.FillUSR_CHECKQ_T(ds, riga.BARCODE);
                        ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                        bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                        bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                        AddebitoModel am = CreaAddebitoModel(ds, riga, CHECKQ_T);
                        ValorizzatoModel m = new ValorizzatoModel()
                        {
                            Commessa = am.Commessa,
                            DataCommessa = am.DataCommessa,
                            Difetto = am.Difetto,
                            IdAleDettaglio = am.IdAleDettaglio,
                            IdAleGruppo = am.IdAleGruppo,
                            LavoranteCodice = am.LavoranteCodice,
                            LavoranteDescrizione = am.LavoranteDescrizione,
                            Modello = am.Modello,
                            ModelloDescrizione = am.ModelloDescrizione,
                            Nota = am.Nota,
                            NotaAddebito = am.NotaAddebito,
                            QuantitaAddebitata = am.QuantitaAddebitata,
                            QuantitaDifettosi = am.QuantitaDifettosi,
                            QuantitaInseriti = am.QuantitaInseriti,
                            TipoDifetto = am.TipoDifetto
                        };
                        m.NotaValorizzazione = riga.IsNOTAVALORIZZAZIONENull() ? string.Empty : riga.NOTAVALORIZZAZIONE;
                        m.Prezzo = riga.IsPREZZONull() ? 0 : riga.PREZZO;
                        grModel.ValoreTotale += m.Prezzo * m.QuantitaAddebitata;
                        grModel.Dettagli.Add(m);
                    }
                    model.Add(grModel);
                }
            }

            return model;
        }

        public List<GruppoValorizzatoModel> LeggiAltriGruppiNonValorizzati()
        {
            List<GruppoValorizzatoModel> model = new List<GruppoValorizzatoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.APPROVATO);
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.FATTURATO);

                List<long> IDALEGRUPPO = ds.RW_ALE_DETTAGLIO.Select(x => (long)x.IDALEGRUPPO).Distinct().ToList();
                bALE.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    GruppoValorizzatoModel grModel = new GruppoValorizzatoModel();
                    grModel.IDALEGRUPPO = gruppo.IDALEGRUPPO;
                    grModel.Aperto = (gruppo.APERTO == "0");
                    grModel.Dettagli = new List<ValorizzatoModel>();

                    grModel.LavoranteCodice = gruppo.LAVORANTE.Trim();
                    grModel.LavoranteDescrizione = string.Empty;
                    grModel.ValorizzazioneAnnullabile = false;
                    grModel.ValoreTotale = 0;
                    ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == grModel.LavoranteCodice).FirstOrDefault();
                    if (lavorante != null)
                        grModel.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC.Trim();

                    grModel.NotaValorizzazione = gruppo.IsNOTAVALORIZZAZIONENull() ? string.Empty : gruppo.NOTAVALORIZZAZIONE;

                    foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == gruppo.IDALEGRUPPO))
                    {
                        bALE.FillUSR_CHECKQ_C(ds, riga.IDCHECKQT);
                        bALE.FillUSR_CHECKQ_T(ds, riga.BARCODE);
                        ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                        bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                        bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                        AddebitoModel am = CreaAddebitoModel(ds, riga, CHECKQ_T);
                        ValorizzatoModel m = new ValorizzatoModel()
                        { 
                           Commessa=am.Commessa,
                           DataCommessa=am.DataCommessa,
                           Difetto=am.Difetto,
                           IdAleDettaglio = am.IdAleDettaglio,
                           IdAleGruppo=am.IdAleGruppo,
                           LavoranteCodice=am.LavoranteCodice,
                           LavoranteDescrizione=am.LavoranteDescrizione,
                           Modello=am.Modello,
                           ModelloDescrizione=am.ModelloDescrizione,
                           Nota=am.Nota,
                           NotaAddebito=am.NotaAddebito,
                           QuantitaAddebitata=am.QuantitaAddebitata,
                           QuantitaDifettosi=am.QuantitaDifettosi,
                           QuantitaInseriti=am.QuantitaInseriti,
                           TipoDifetto=am.TipoDifetto
                        };
                        m.NotaValorizzazione = riga.IsNOTAVALORIZZAZIONENull() ? string.Empty : riga.NOTAVALORIZZAZIONE;
                        m.Prezzo = riga.IsPREZZONull() ? 0 : riga.PREZZO;
                        grModel.ValoreTotale += m.Prezzo * m.QuantitaAddebitata;

                        grModel.Dettagli.Add(m);

                    }
                    model.Add(grModel);
                }
            }

            return model;
        }

        public List<GruppoApprovatoModel> LeggiGruppiApprovati()
        {
            List<GruppoApprovatoModel> model = new List<GruppoApprovatoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.APPROVATO);

                List<long> IDALEGRUPPO = ds.RW_ALE_DETTAGLIO.Select(x => (long)x.IDALEGRUPPO).Distinct().ToList();
                bALE.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    GruppoApprovatoModel grModel = new GruppoApprovatoModel();
                    grModel.IDALEGRUPPO = gruppo.IDALEGRUPPO;
                    grModel.Aperto = (gruppo.APERTO == "0");
                    grModel.Dettagli = new List<ApprovatoModel>();

                    grModel.LavoranteCodice = gruppo.LAVORANTE.Trim();
                    grModel.LavoranteDescrizione = string.Empty;
                    grModel.ApprovazioneAnnullabile = true;
                    ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == grModel.LavoranteCodice).FirstOrDefault();
                    if (lavorante != null)
                        grModel.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC.Trim();

                    grModel.NotaApprovazione = gruppo.IsNOTAAPPROVAZIONENull() ? string.Empty : gruppo.NOTAAPPROVAZIONE;
                    grModel.ValoreTotale = 0;
                    grModel.ValoreApprovatoTotale = 0;
                    foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == gruppo.IDALEGRUPPO))
                    {
                        bALE.FillUSR_CHECKQ_C(ds, riga.IDCHECKQT);
                        bALE.FillUSR_CHECKQ_T(ds, riga.BARCODE);
                        ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                        bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                        bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                        AddebitoModel am = CreaAddebitoModel(ds, riga, CHECKQ_T);
                        ApprovatoModel m = new ApprovatoModel()
                        {
                            Commessa = am.Commessa,
                            DataCommessa = am.DataCommessa,
                            Difetto = am.Difetto,
                            IdAleDettaglio = am.IdAleDettaglio,
                            IdAleGruppo = am.IdAleGruppo,
                            LavoranteCodice = am.LavoranteCodice,
                            LavoranteDescrizione = am.LavoranteDescrizione,
                            Modello = am.Modello,
                            ModelloDescrizione = am.ModelloDescrizione,
                            Nota = am.Nota,
                            NotaAddebito = am.NotaAddebito,
                            QuantitaAddebitata = am.QuantitaAddebitata,
                            QuantitaDifettosi = am.QuantitaDifettosi,
                            QuantitaInseriti = am.QuantitaInseriti,
                            TipoDifetto = am.TipoDifetto
                        };
                        m.NotaValorizzazione = riga.IsNOTAVALORIZZAZIONENull() ? string.Empty : riga.NOTAVALORIZZAZIONE;
                        m.Prezzo = riga.IsPREZZONull() ? 0 : riga.PREZZO;
                        m.NotaApprovazione = riga.IsNOTAAPPROVAZIONENull() ? string.Empty : riga.NOTAAPPROVAZIONE;
                        m.PrezzoApprovato = riga.IsPREZZO_APPROVATONull() ? 0 : riga.PREZZO_APPROVATO;
                        grModel.ValoreTotale += m.Prezzo * m.QuantitaAddebitata;
                        grModel.ValoreApprovatoTotale += m.PrezzoApprovato * m.QuantitaAddebitata;
                        grModel.Dettagli.Add(m);
                    }
                    model.Add(grModel);
                }
            }

            return model;
        }

        public void FatturaGruppo(string IDALEGRUPPO, string Dettagli, string NotaGruppo, string UIDUSER)
        {
            ALEValorizzaJson[] Valorizzati = JSonSerializer.Deserialize<ALEValorizzaJson[]>(Dettagli);
            decimal idAleAgruppo = decimal.Parse(IDALEGRUPPO);
            using (ALEBusiness bALE = new ALEBusiness())
            {
                long idGruppo = long.Parse(IDALEGRUPPO);
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, idAleAgruppo);
                bALE.FillRW_ALE_GRUPPO(ds, new List<long>(new long[] { idGruppo }));

                foreach (ALEValorizzaJson val in Valorizzati)
                {
                    ALEDS.RW_ALE_DETTAGLIORow dettaglio = ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEDETTAGLIO == val.IdAleDettaglio).FirstOrDefault();
                    if (dettaglio != null)
                    {
                        dettaglio.STATO = ALEStatoDettaglio.FATTURATO;
                        dettaglio.UIDUSER = UIDUSER;
                        dettaglio.NOTAFATTURAZIONE = val.Nota;
                    }
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.NOTAFATTURAZIONE = NotaGruppo;
                    gruppo.APERTO = "1";
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
                bALE.FillRW_ALE_GRUPPO(ds, new List<long>(new long[] { idGruppo }));

                foreach (ALEDS.RW_ALE_DETTAGLIORow dettaglio in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == idGruppo))
                {
                    dettaglio.STATO = ALEStatoDettaglio.APPROVATO;
                    dettaglio.UIDUSER = UIDUSER;
                    dettaglio.SetNOTAFATTURAZIONENull();
                }
                ALEDS.RW_ALE_GRUPPORow gruppo = ds.RW_ALE_GRUPPO.Where(x => x.IDALEGRUPPO == idGruppo).FirstOrDefault();
                if (gruppo != null)
                {
                    gruppo.SetNOTAFATTURAZIONENull();
                    gruppo.APERTO = "0";

                }
                bALE.UpdateRW_ALE_DETTAGLIO(ds);
                bALE.UpdateRW_ALE_GRUPPO(ds);
            }
        }

        public List<GruppoFatturatoModel> LeggiGruppiFatturati()
        {
            List<GruppoFatturatoModel> model = new List<GruppoFatturatoModel>();

            using (ALEBusiness bALE = new ALEBusiness())
            {
                ALEDS ds = new ALEDS();
                bALE.FillRW_ALE_DETTAGLIO(ds, ALEStatoDettaglio.FATTURATO);

                List<long> IDALEGRUPPO = ds.RW_ALE_DETTAGLIO.Select(x => (long)x.IDALEGRUPPO).Distinct().ToList();
                bALE.FillRW_ALE_GRUPPO(ds, IDALEGRUPPO);
                bALE.FillCLIFO(ds);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                foreach (ALEDS.RW_ALE_GRUPPORow gruppo in ds.RW_ALE_GRUPPO)
                {
                    GruppoFatturatoModel grModel = new GruppoFatturatoModel();
                    grModel.IDALEGRUPPO = gruppo.IDALEGRUPPO;
                    grModel.Aperto = (gruppo.APERTO == "0");
                    grModel.Dettagli = new List<FatturatoModel>();

                    grModel.LavoranteCodice = gruppo.LAVORANTE.Trim();
                    grModel.LavoranteDescrizione = string.Empty;
                    grModel.FatturazioneAnnullabile = true;
                    ALEDS.CLIFORow lavorante = ds.CLIFO.Where(x => x.CODICE.Trim() == grModel.LavoranteCodice).FirstOrDefault();
                    if (lavorante != null)
                        grModel.LavoranteDescrizione = lavorante.IsRAGIONESOCNull() ? string.Empty : lavorante.RAGIONESOC.Trim();

                    grModel.NotaFatturazione = gruppo.IsNOTAFATTURAZIONENull() ? string.Empty : gruppo.NOTAFATTURAZIONE;
                    grModel.ValoreApprovatoTotale = 0;
                    foreach (ALEDS.RW_ALE_DETTAGLIORow riga in ds.RW_ALE_DETTAGLIO.Where(x => x.IDALEGRUPPO == gruppo.IDALEGRUPPO))
                    {
                        bALE.FillUSR_CHECKQ_C(ds, riga.IDCHECKQT);
                        bALE.FillUSR_CHECKQ_T(ds, riga.BARCODE);
                        ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.IDCHECKQT == riga.IDCHECKQT).FirstOrDefault();
                        bALE.FillMAGAZZ(ds, CHECKQ_T.IDMAGAZZ);
                        bALE.FillUSR_PRD_MOVFASI(ds, CHECKQ_T.IDCHECKQT);
                        AddebitoModel am = CreaAddebitoModel(ds, riga, CHECKQ_T);
                        FatturatoModel m = new FatturatoModel()
                        {
                            Commessa = am.Commessa,
                            DataCommessa = am.DataCommessa,
                            Difetto = am.Difetto,
                            IdAleDettaglio = am.IdAleDettaglio,
                            IdAleGruppo = am.IdAleGruppo,
                            LavoranteCodice = am.LavoranteCodice,
                            LavoranteDescrizione = am.LavoranteDescrizione,
                            Modello = am.Modello,
                            ModelloDescrizione = am.ModelloDescrizione,
                            Nota = am.Nota,
                            NotaAddebito = am.NotaAddebito,
                            QuantitaAddebitata = am.QuantitaAddebitata,
                            QuantitaDifettosi = am.QuantitaDifettosi,
                            QuantitaInseriti = am.QuantitaInseriti,
                            TipoDifetto = am.TipoDifetto
                        };
                        m.NotaValorizzazione = riga.IsNOTAVALORIZZAZIONENull() ? string.Empty : riga.NOTAVALORIZZAZIONE;
                        m.Prezzo = riga.IsPREZZONull() ? 0 : riga.PREZZO;
                        m.NotaApprovazione = riga.IsNOTAAPPROVAZIONENull() ? string.Empty : riga.NOTAAPPROVAZIONE;
                        m.PrezzoApprovato = riga.IsPREZZO_APPROVATONull() ? 0 : riga.PREZZO_APPROVATO;
                        m.NotaApprovazione = riga.IsNOTAFATTURAZIONENull() ? string.Empty : riga.NOTAFATTURAZIONE;
                        grModel.ValoreApprovatoTotale += m.PrezzoApprovato * m.QuantitaAddebitata;
                        grModel.Dettagli.Add(m);
                    }
                    model.Add(grModel);
                }
            }

            return model;
        }
    }
}
