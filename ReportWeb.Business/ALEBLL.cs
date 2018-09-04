using ReportWeb.Data.ALE;
using ReportWeb.Entities;
using ReportWeb.Models;
using ReportWeb.Models.ALE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class ALEBLL
    {
        public InserimentoModel CaricaScheda(string Barcode)
        {
            InserimentoModel model = new InserimentoModel();
            ALEDS ds = new ALEDS();
            using (ALEBusiness bALE = new ALEBusiness())
            {
                bALE.FillUSR_CHECKQ_T(ds, Barcode);
                if (!ds.USR_CHECKQ_T.Any(x => x.BARCODE == Barcode))
                {
                    model.NonTrovato = true;
                    return model;
                }

                ALEDS.USR_CHECKQ_TRow CHECKQ_T = ds.USR_CHECKQ_T.Where(x => x.BARCODE == Barcode).FirstOrDefault();
                ALEDS.USR_CHECKQ_DRow CHECKQ_D = ds.USR_CHECKQ_D.Where(x => x.IDCHECKQT == CHECKQ_T.IDCHECKQT).FirstOrDefault();
                ALEDS.USR_CHECKQ_CRow CHECKQ_C = ds.USR_CHECKQ_C.Where(x => x.IDCHECKQT == CHECKQ_T.IDCHECKQT).FirstOrDefault();

                bALE.FillUSR_CHECKQ_D(ds, CHECKQ_T.IDCHECKQT);
                bALE.FillUSR_CHECKQ_C(ds, CHECKQ_T.IDCHECKQT);
                bALE.FillUSR_TAB_TIPODIFETTI(ds);
                bALE.FillUSR_ANA_DIFETTI(ds);
                bALE.FillTABCAUMGT(ds);
                bALE.FillCLIFO(ds);

                model.NonTrovato = false;
                model.IDCHECKQT = CHECKQ_T.IDCHECKQT;
                model.Quantita = CHECKQ_T.IsQTANull() ? 0 : CHECKQ_T.QTA;
                model.QuantitaDifforme = CHECKQ_D == null ? 0 : (CHECKQ_D.IsQTAGRP_DIFNull() ? 0 : CHECKQ_D.QTAGRP_DIF);

                ALEDS.CLIFORow reparto = ds.CLIFO.Where(x => x.CODICE == CHECKQ_T.CODICECLIFO_RIL).FirstOrDefault();
                if (reparto != null)
                    model.Reparto = reparto.RAGIONESOC;
                else
                    model.Reparto = string.Empty;

                model.Difetto = string.Empty;
                model.TipoDifetto = string.Empty;

                ALEDS.USR_ANA_DIFETTIRow difetto = ds.USR_ANA_DIFETTI.Where(x => x.IDDIFETTO == CHECKQ_C.IDDIFETTO).FirstOrDefault();
                ALEDS.USR_TAB_TIPODIFETTIRow tipoDifetto = ds.USR_TAB_TIPODIFETTI.Where(x => x.IDTIPODIFETTO == difetto.IDTIPODIFETTO).FirstOrDefault();

                model.TipoDifetto = tipoDifetto.DESTIPODIFETTO;
                model.Difetto = difetto.DESDIFETTO;

                int aux;
                foreach (ALEDS.CLIFORow fornitore in ds.CLIFO.Where(x => int.TryParse(x.CODICE, out aux)))
                    model.LavorantiEsterni.Add(new RWListItem(fornitore.RAGIONESOC, fornitore.CODICE));


                return model;
            }
        }
    }
}
