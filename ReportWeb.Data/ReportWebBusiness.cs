using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportWeb.Common;
using ReportWeb.Data.Core;

namespace ReportWeb.Data
{
    public class ReportWebBusiness : ReportWebBusinessBase
    {
        public ReportWebBusiness() : base() { }

        [DataContext]
        public void FillODL_APERTI(ReportDS ds)
        {
            ReportWebAdapter a = new ReportWebAdapter(DbConnection, DbTransaction);
            a.FillODL_APERTI(ds);
        }

        [DataContext]
        public void FillODL_APERTI(string codiceTipoMovimentoFase, ReportDS ds)
        {
            ReportWebAdapter a = new ReportWebAdapter(DbConnection, DbTransaction);
            a.FillODL_APERTI(codiceTipoMovimentoFase, ds);
        }

        [DataContext]
        public void FillODL_APERTI(string reparto, string codiceTipoMovimentoFase, ReportDS ds)
        {
            ReportWebAdapter a = new ReportWebAdapter(DbConnection, DbTransaction);
            a.FillODL_APERTI(reparto, codiceTipoMovimentoFase, ds);
        }

        [DataContext]
        public void FillTV_SCADENZE_CLIFO(ReportDS ds)
        {
            ReportWebAdapter a = new ReportWebAdapter(DbConnection, DbTransaction);
            a.FillTV_SCADENZE_CLIFO(ds);
        }
    }
}
