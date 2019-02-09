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
    }
}
