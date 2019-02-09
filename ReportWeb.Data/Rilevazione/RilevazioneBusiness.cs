using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Rilevazione
{

    public class RilevazioneBusiness : ReportWebBusinessBase
    {
        public RilevazioneBusiness() : base() { }

        [DataContext]
        public void FillUSR_PRD_RESOURCESF(RilevazioniDS ds, string BARCODE)
        {
            RilevazioneAdapter a = new RilevazioneAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_RESOURCESF(ds, BARCODE);
        }



        [DataContext(true)]
        public void UpdateRW_TEMPI(RilevazioniDS ds)
        {
            RilevazioneAdapter a = new RilevazioneAdapter(DbConnection, DbTransaction);
            a.UpdateRegistrazioneDS(ds.RW_TEMPI.TableName, ds);
        }
    }
}
