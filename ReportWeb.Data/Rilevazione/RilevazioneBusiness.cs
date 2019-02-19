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

        [DataContext]
        public void FillRW_TEMPI_APERTI(RilevazioniDS ds, string BARCODE)
        {
            RilevazioneAdapter a = new RilevazioneAdapter(DbConnection, DbTransaction);
            a.FillRW_TEMPI_APERTI(ds, BARCODE);
        }

        [DataContext]
        public void FillRW_TEMPI_APERTI_PER_UTENTE(RilevazioniDS ds, string Utente)
        {
            RilevazioneAdapter a = new RilevazioneAdapter(DbConnection, DbTransaction);
            a.FillRW_TEMPI_APERTI_PER_UTENTE(ds, Utente);
        }

        [DataContext]
        public void FillRW_TEMPI_LAVORAZIONI(RilevazioniDS ds)
        {
            RilevazioneAdapter a = new RilevazioneAdapter(DbConnection, DbTransaction);
            a.FillRW_TEMPI_LAVORAZIONI(ds);
        }

        [DataContext(true)]
        public void UpdateRW_TEMPI(RilevazioniDS ds)
        {
            RilevazioneAdapter a = new RilevazioneAdapter(DbConnection, DbTransaction);
            a.UpdateRegistrazioneDS(ds.RW_TEMPI.TableName, ds);
        }

        [DataContext]
        public void FillRW_TEMPI_REPARTI(RilevazioniDS ds)
        {
            RilevazioneAdapter a = new RilevazioneAdapter(DbConnection, DbTransaction);
            a.FillRW_TEMPI_REPARTI(ds);
        }
    }
}
