using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Trasferimenti
{
    public class TrasferimentiBusiness : ReportWebBusinessBase
    {
        public TrasferimentiBusiness() : base() { }

        [DataContext]
        public void FillUSR_PRD_RESOURCESF(TrasferimentiDS ds)
        {
            TrasferimentiAdapter a = new TrasferimentiAdapter(DbConnection, DbTransaction);
            a.FillUSR_PRD_RESOURCESF(ds);
        }

        [DataContext]
        public void FillAP_GRIGLIA(TrasferimentiDS ds, DateTime dal, DateTime al, string barcodePartenza, string barcodeArrivo)
        {
            TrasferimentiAdapter a = new TrasferimentiAdapter(DbConnection, DbTransaction);
            a.FillAP_GRIGLIA(ds, dal, al, barcodePartenza, barcodeArrivo);
        }
    }
}
