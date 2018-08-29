using ReportWeb.Data;
using ReportWeb.Entities;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebReport.Common;

namespace ReportWeb.BLL
{
    public class ODLHelper
    {
        public static List<ODLApertiModel> FillODLAperti(string Reparto)
        {
            ReportDS ds = new ReportDS();
            List<ODLApertiModel> odl = new List<ODLApertiModel>();

            switch (Reparto)
            {
                case Reparti.Piegafilo:
                    odl = EstraiODlApertiPerPiegafilo();
                    break;
                case Reparti.Stampaggio:
                    odl = EstraiODlApertiPerStampaggio();
                    break;
                default:
                    odl = EstraiODlApertiDaRepartoStandard(Reparto);
                    break;

            }


            return odl;
        }

        private static List<ODLApertiModel> EstraiODlApertiDaRepartoStandard(string Reparto)
        {
            ReportDS ds = new ReportDS();
            List<ODLApertiModel> odl = new List<ODLApertiModel>();
            using (ReportWebBusiness bWebReport = new ReportWebBusiness())
            {
                bWebReport.FillODL_APERTI(Reparto, TipoMovimentoFase.OrdineProduzione, ds);
                bWebReport.FillTV_SCADENZE_CLIFO(ds);
            }

            ReportDS.TV_SCADENZE_CLIFORow scadenza = ds.TV_SCADENZE_CLIFO.Where(x => x.CODICECLIFO.Trim() == Reparto).FirstOrDefault();
            int ritardo = 2;
            if (scadenza != null)
                ritardo = (int)scadenza.INTERVALLO_RITARDO;

            foreach (ReportDS.ODL_APERTIRow odlAperto in ds.ODL_APERTI.Where(x => x.PIANIFICATO_SN == "No").OrderBy(x => x.DATAFINE_ODL_E_MULTIPLA))
            {
                ODLApertiModel model = new ODLApertiModel()
                {
                    Azienda = odlAperto.IsAZIENDANull() ? string.Empty : odlAperto.AZIENDA,
                    Articolo = odlAperto.IsMODELLO_LANCIONull() ? string.Empty : odlAperto.MODELLO_LANCIO,
                    Commessa = odlAperto.IsNOMECOMMESSANull() ? string.Empty : odlAperto.NOMECOMMESSA,
                    Segnalatore = odlAperto.IsSEGNALATORENull() ? string.Empty : odlAperto.SEGNALATORE,
                    Fase = odlAperto.IsELENCOFASINull() ? string.Empty : odlAperto.ELENCOFASI,
                    Brand = string.Empty,
                    ODL = odlAperto.IsNUMMOVFASENull() ? string.Empty : odlAperto.NUMMOVFASE,
                    QtaDaTerminare = odlAperto.IsQTANull() ? (decimal?)null : odlAperto.QTADATER,
                    QtaTotale = odlAperto.IsQTADATERNull() ? (decimal?)null : odlAperto.QTA,
                    Wip = odlAperto.IsMODELLO_WIPNull() ? string.Empty : odlAperto.MODELLO_WIP,
                    DataFine = odlAperto.IsDATAFINE_ODL_E_MULTIPLANull() ? (DateTime?)null : odlAperto.DATAFINE_ODL_E_MULTIPLA,
                    Priority = (int)Priorità.Bassa,
                    DataCreazione = odlAperto.IsDATAINIZIO_ODLNull() ? (DateTime?)null : odlAperto.DATAINIZIO_ODL
                };

                if (odlAperto.DATAFINE_ODL_E_MULTIPLA < DateTime.Today) model.Priority = (int)Priorità.Alta;
                if (odlAperto.DATAFINE_ODL_E_MULTIPLA >= DateTime.Today && odlAperto.DATAFINE_ODL_E_MULTIPLA < DateTime.Today.AddDays(ritardo)) model.Priority = (int)Priorità.Media;
                odl.Add(model);
            }

            return odl;
        }

        private static List<ODLApertiModel> EstraiODlApertiPerPiegafilo()
        {
            string Reparto = Reparti.Stampaggio;

            List<string> fasi = new List<string>();
            fasi.Add("PIEGF;");
            fasi.Add("STAF;");

            ReportDS ds = new ReportDS();
            List<ODLApertiModel> odl = new List<ODLApertiModel>();
            using (ReportWebBusiness bWebReport = new ReportWebBusiness())
            {
                bWebReport.FillODL_APERTI(Reparto, TipoMovimentoFase.OrdineProduzione, ds);
                bWebReport.FillTV_SCADENZE_CLIFO(ds);
            }
            ReportDS.TV_SCADENZE_CLIFORow scadenza = ds.TV_SCADENZE_CLIFO.Where(x => x.CODICECLIFO.Trim() == Reparto).FirstOrDefault();
            int ritardo = 2;
            if (scadenza != null)
                ritardo = (int)scadenza.INTERVALLO_RITARDO;

            foreach (ReportDS.ODL_APERTIRow odlAperto in ds.ODL_APERTI.Where(x => x.PIANIFICATO_SN == "No" && fasi.Contains(x.ELENCOFASI)).OrderBy(x => x.DATAFINE_ODL_E_MULTIPLA))
            {
                ODLApertiModel model = new ODLApertiModel()
                {
                    Azienda = odlAperto.IsAZIENDANull() ? string.Empty : odlAperto.AZIENDA,
                    Articolo = odlAperto.IsMODELLO_LANCIONull() ? string.Empty : odlAperto.MODELLO_LANCIO,
                    Commessa = odlAperto.IsNOMECOMMESSANull() ? string.Empty : odlAperto.NOMECOMMESSA,
                    Segnalatore = odlAperto.IsSEGNALATORENull() ? string.Empty : odlAperto.SEGNALATORE,
                    Fase = odlAperto.IsELENCOFASINull() ? string.Empty : odlAperto.ELENCOFASI,
                    Brand = string.Empty,
                    ODL = odlAperto.IsNUMMOVFASENull() ? string.Empty : odlAperto.NUMMOVFASE,
                    QtaDaTerminare = odlAperto.IsQTANull() ? (decimal?)null : odlAperto.QTADATER,
                    QtaTotale = odlAperto.IsQTADATERNull() ? (decimal?)null : odlAperto.QTA,
                    Wip = odlAperto.IsMODELLO_WIPNull() ? string.Empty : odlAperto.MODELLO_WIP,
                    DataFine = odlAperto.IsDATAFINE_ODL_E_MULTIPLANull() ? (DateTime?)null : odlAperto.DATAFINE_ODL_E_MULTIPLA,
                    Priority = (int)Priorità.Bassa,
                    DataCreazione = odlAperto.IsDATAINIZIO_ODLNull() ? (DateTime?)null : odlAperto.DATAINIZIO_ODL
                };

                if (odlAperto.DATAFINE_ODL_E_MULTIPLA < DateTime.Today) model.Priority = (int)Priorità.Alta;
                if (odlAperto.DATAFINE_ODL_E_MULTIPLA >= DateTime.Today && odlAperto.DATAFINE_ODL_E_MULTIPLA < DateTime.Today.AddDays(ritardo)) model.Priority = (int)Priorità.Media;
                odl.Add(model);
            }

            return odl;
        }

        private static List<ODLApertiModel> EstraiODlApertiPerStampaggio()
        {
            string Reparto = Reparti.Stampaggio;

            List<string> fasi = new List<string>();
            fasi.Add("CTRL.QUAL.;");
            fasi.Add("STAC;");

            ReportDS ds = new ReportDS();
            List<ODLApertiModel> odl = new List<ODLApertiModel>();
            using (ReportWebBusiness bWebReport = new ReportWebBusiness())
            {
                bWebReport.FillODL_APERTI(Reparto, TipoMovimentoFase.OrdineProduzione, ds);
                bWebReport.FillTV_SCADENZE_CLIFO(ds);
            }
            ReportDS.TV_SCADENZE_CLIFORow scadenza = ds.TV_SCADENZE_CLIFO.Where(x => x.CODICECLIFO.Trim() == Reparto).FirstOrDefault();
            int ritardo = 2;
            if (scadenza != null)
                ritardo = (int)scadenza.INTERVALLO_RITARDO;

            foreach (ReportDS.ODL_APERTIRow odlAperto in ds.ODL_APERTI.Where(x => x.PIANIFICATO_SN == "No" && fasi.Contains(x.ELENCOFASI)).OrderBy(x => x.DATAFINE_ODL_E_MULTIPLA))
            {
                ODLApertiModel model = new ODLApertiModel()
                {
                    Azienda = odlAperto.IsAZIENDANull() ? string.Empty : odlAperto.AZIENDA,
                    Articolo = odlAperto.IsMODELLO_LANCIONull() ? string.Empty : odlAperto.MODELLO_LANCIO,
                    Commessa = odlAperto.IsNOMECOMMESSANull() ? string.Empty : odlAperto.NOMECOMMESSA,
                    Segnalatore = odlAperto.IsSEGNALATORENull() ? string.Empty : odlAperto.SEGNALATORE,
                    Fase = odlAperto.IsELENCOFASINull() ? string.Empty : odlAperto.ELENCOFASI,
                    Brand = string.Empty,
                    ODL = odlAperto.IsNUMMOVFASENull() ? string.Empty : odlAperto.NUMMOVFASE,
                    QtaDaTerminare = odlAperto.IsQTANull() ? (decimal?)null : odlAperto.QTADATER,
                    QtaTotale = odlAperto.IsQTADATERNull() ? (decimal?)null : odlAperto.QTA,
                    Wip = odlAperto.IsMODELLO_WIPNull() ? string.Empty : odlAperto.MODELLO_WIP,
                    DataFine = odlAperto.IsDATAFINE_ODL_E_MULTIPLANull() ? (DateTime?)null : odlAperto.DATAFINE_ODL_E_MULTIPLA,
                    Priority = (int)Priorità.Bassa,
                    DataCreazione = odlAperto.IsDATAINIZIO_ODLNull() ? (DateTime?)null : odlAperto.DATAINIZIO_ODL
                };

                if (odlAperto.DATAFINE_ODL_E_MULTIPLA < DateTime.Today) model.Priority = (int)Priorità.Alta;
                if (odlAperto.DATAFINE_ODL_E_MULTIPLA >= DateTime.Today && odlAperto.DATAFINE_ODL_E_MULTIPLA < DateTime.Today.AddDays(ritardo)) model.Priority = (int)Priorità.Media;
                odl.Add(model);
            }

            return odl;
        }
        public static QuadrantiModel GetDatiPerQuadranti(string Reparto)
        {
            ReportDS ds = new ReportDS();
            using (ReportWebBusiness bWebReport = new ReportWebBusiness())
            {
                bWebReport.FillODL_APERTI(Reparto, TipoMovimentoFase.OrdineProduzione, ds);
            }

            QuadrantiModel model = new QuadrantiModel();


            model.InScadenza = (int)ds.ODL_APERTI.Where(x => x.DATAFINE_ODL_E_MULTIPLA >= DateTime.Today && x.DATAFINE_ODL_E_MULTIPLA < DateTime.Today.AddDays(+2) && x.PIANIFICATO_SN == "No").Sum(x => x.QTADATER);
            model.Scaduti = (int)ds.ODL_APERTI.Where(x => x.DATAFINE_ODL_E_MULTIPLA < DateTime.Today).Sum(x => x.QTADATER);
            model.DaPrendereInCarico = (int)ds.ODL_APERTI.Where(x => x.IsDATAPRIMOINVIO_ODLNull() && !x.IsDATAMOVFASENull() && DateTime.Today.Subtract(x.DATAMOVFASE).TotalDays > 1 && x.PIANIFICATO_SN == "No").Sum(x => x.QTADATER);

            return model;
        }
    }
}
