using MetalPlus.Kernel.Data.Core;
using ReportWeb.Common.Helpers;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.ALE
{
    public class ALEBusiness : ReportWebBusinessBase
    {
        public ALEBusiness() : base() { }

        [DataContext]
        public void FillUSR_CHECKQ_T(ALEDS ds, string Barcode)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_CHECKQ_T(ds, Barcode);
        }

        [DataContext]
        public void FillUSR_CHECKQ_D(ALEDS ds, string IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_CHECKQ_D(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillUSR_CHECKQ_C(ALEDS ds, string IDCHECKQT)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_CHECKQ_C(ds, IDCHECKQT);
        }

        [DataContext]
        public void FillUSR_TAB_TIPODIFETTI(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_TAB_TIPODIFETTI(ds);
        }

        [DataContext]
        public void FillUSR_ANA_DIFETTI(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillUSR_ANA_DIFETTI(ds);
        }

        [DataContext]
        public void FillTABCAUMGT(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillTABCAUMGT(ds);
        }

        [DataContext]
        public void FillCLIFO(ALEDS ds)
        {
            ALEAdapter a = new ALEAdapter(DbConnection, DbTransaction);
            a.FillCLIFO(ds);
        }
    }
}
