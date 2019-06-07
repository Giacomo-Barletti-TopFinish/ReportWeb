using ReportWeb.Data.Trasferimenti;
using ReportWeb.Entities;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class TrasferimentiBLL
    {
        public List<RWListItem> CaricaListaOperatori()
        {
            List<RWListItem> model = new List<RWListItem>();
            using (TrasferimentiBusiness bTrasferimenti = new TrasferimentiBusiness())
            {
                TrasferimentiDS ds = new TrasferimentiDS();
                bTrasferimenti.FillUSR_PRD_RESOURCESF(ds);
                foreach (TrasferimentiDS.USR_PRD_RESOURCESFRow risorsa in ds.USR_PRD_RESOURCESF)
                {
                    model.Add(new RWListItem(risorsa.CODRESOURCEF.Trim(), risorsa.BARCODE));
                }
            }
            return model;
        }

        public List<TrasferimentoModel> EstraiTrasferimenti(string DataInizio, string DataFine, string OperatoreInvio, string OperatoreRicezione)
        {
            DateTime dtInizio = DateTime.Parse(DataInizio);
            DateTime dtFine = DateTime.Parse(DataFine);

            List<TrasferimentoModel> model = new List<TrasferimentoModel>();
            using (TrasferimentiBusiness bTrasferimenti = new TrasferimentiBusiness())
            {
                TrasferimentiDS ds = new TrasferimentiDS();
                bTrasferimenti.FillAP_GRIGLIA(ds, dtInizio, dtFine, OperatoreInvio, OperatoreRicezione);

                foreach (TrasferimentiDS.AP_GRIGLIARow trasferimento in ds.AP_GRIGLIA.OrderBy(x => x.DATA_PARTENZA))
                {
                    string dtArrivo = string.Empty;
                    if (!trasferimento.IsDATA_ARRIVONull())
                        dtArrivo = string.Format("{0} {1}", trasferimento.DATA_ARRIVO.ToShortDateString(), trasferimento.DATA_ARRIVO.ToShortTimeString());

                    string dtPartenza = string.Format("{0} {1}", trasferimento.DATA_PARTENZA.ToShortDateString(), trasferimento.DATA_PARTENZA.ToShortTimeString());

                    TrasferimentoModel m = new TrasferimentoModel()
                    {
                        DataArrivo = dtArrivo,
                        DataPartenza = dtPartenza,
                        Modello = trasferimento.MODELLO,
                        NumMovFase = trasferimento.NUMMOVFASE,
                        OperatoreArrivo = trasferimento.IsOPERATORE_ARRIVONull() ? string.Empty : trasferimento.OPERATORE_ARRIVO,
                        OperatorePartenza = trasferimento.IsOPERATORE_PARTENZANull() ? string.Empty : trasferimento.OPERATORE_PARTENZA,
                        Reparto = trasferimento.REPARTO
                    };

                    model.Add(m);
                }
            }
            return model;

        }
    }
}
