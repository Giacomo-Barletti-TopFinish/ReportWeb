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
            int sequenzaLavorazione = 0;
            PreserieDS.USR_PRD_FASIRow faseRoot = ds.USR_PRD_FASI.Where(x => x.IDLANCIOD == lanciod.IDLANCIOD && x.ROOTSN == 1).FirstOrDefault();
            if (faseRoot != null)
            {
                Lavorazione lavorazioneRoot = CreaLavorazione(faseRoot, sequenzaLavorazione, ds);
                sequenzaLavorazione++;
                commessa.Lavorazioni.Add(lavorazioneRoot);

                List<PreserieDS.USR_PRD_FASIRow> faseFiglie = ds.USR_PRD_FASI.Where(x => !x.IsIDPRDFASEPADRENull() && x.IDPRDFASEPADRE == faseRoot.IDPRDFASE).ToList();
                EspandiAlberoFasi(faseFiglie, ds, sequenzaLavorazione, commessa);

            }

            return commessa;
        }

        private void EspandiAlberoFasi(List<PreserieDS.USR_PRD_FASIRow> faseFiglie, PreserieDS ds, int sequenzaLavorazione, Commessa commessa)
        {
            foreach (PreserieDS.USR_PRD_FASIRow faseFiglia in faseFiglie)
            {
                Lavorazione lavorazione = CreaLavorazione(faseFiglia, sequenzaLavorazione, ds);
                sequenzaLavorazione++;
                commessa.Lavorazioni.Add(lavorazione);
                List<PreserieDS.USR_PRD_FASIRow> figlie = ds.USR_PRD_FASI.Where(x => !x.IsIDPRDFASEPADRENull() && x.IDPRDFASEPADRE == faseFiglia.IDPRDFASE).ToList();
                EspandiAlberoFasi(figlie, ds, sequenzaLavorazione, commessa);
            }
        }

        private Lavorazione CreaLavorazione(PreserieDS.USR_PRD_FASIRow fase, int sequenza, PreserieDS ds)
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
    }
}
