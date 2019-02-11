using ReportWeb.Data.Rilevazione;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class RilevazioneBLL
    {
        public string RilevaUtente(string barcode)
        {

            RilevazioniDS ds = new RilevazioniDS();
            using (RilevazioneBusiness bRilevazione = new RilevazioneBusiness())
            {
                bRilevazione.FillUSR_PRD_RESOURCESF(ds, barcode);

                RilevazioniDS.USR_PRD_RESOURCESFRow risorsa = ds.USR_PRD_RESOURCESF.Where(x => x.BARCODE == barcode).FirstOrDefault();
                if (risorsa == null) return string.Empty;

                return risorsa.IsDESRESOURCEFNull() ? string.Empty : risorsa.DESRESOURCEF;

            }

        }

        public bool InizioAttivita(string BarcodeLavoratore, string BarcodeOLD)
        {
            try
            {
                RilevazioniDS ds = new RilevazioniDS();
                using (RilevazioneBusiness bRilevazione = new RilevazioneBusiness())
                {
                    RilevazioniDS.RW_TEMPIRow tempo = ds.RW_TEMPI.NewRW_TEMPIRow();
                    tempo.APERTO = 1;
                    tempo.BARCODE_ODL = BarcodeOLD;
                    tempo.BARCODE_UTENTE = BarcodeLavoratore;
                    tempo.IDDATO = bRilevazione.GetID();
                    tempo.INIZIO = DateTime.Now;

                    ds.RW_TEMPI.AddRW_TEMPIRow(tempo);
                    bRilevazione.UpdateRW_TEMPI(ds);

                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public bool TerminaAttivita(string BarcodeLavoratore, string BarcodeOLD, string Nota, decimal Quantita)
        {
            try
            {
                RilevazioniDS ds = new RilevazioniDS();
                using (RilevazioneBusiness bRilevazione = new RilevazioneBusiness())
                {
                    bRilevazione.FillRW_TEMPI_APERTI(ds, BarcodeLavoratore);
                    RilevazioniDS.RW_TEMPIRow tempo = ds.RW_TEMPI.FirstOrDefault();

                    if (tempo.BARCODE_ODL != BarcodeOLD) return false;

                    tempo.APERTO = 0;
                    tempo.FINE = DateTime.Now;
                    tempo.NOTA = Nota.Length > 100 ? Nota.Substring(0, 100) : Nota;
                    tempo.QUANTITA = Quantita;

                    bRilevazione.UpdateRW_TEMPI(ds);
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public string CaricaSchedaAperto(string BarcodeLavoratore)
        {
            RilevazioniDS ds = new RilevazioniDS();
            using (RilevazioneBusiness bRilevazione = new RilevazioneBusiness())
            {
                bRilevazione.FillRW_TEMPI_APERTI(ds, BarcodeLavoratore);

                RilevazioniDS.RW_TEMPIRow tempoAperto = ds.RW_TEMPI.FirstOrDefault();
                if (tempoAperto == null) return string.Empty;

                return tempoAperto.BARCODE_ODL;
            }
        }
    }
}
