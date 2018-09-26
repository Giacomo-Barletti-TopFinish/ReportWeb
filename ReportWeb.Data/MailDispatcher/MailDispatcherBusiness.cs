using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.MailDispatcher
{
    public class MailDispatcherBusiness : ReportWebBusinessBase
    {
        public MailDispatcherBusiness() : base() { }

        [DataContext]
        public void FillMD_GRUPPI(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_GRUPPI(ds);
        }

        [DataContext]
        public void FillMD_GRUPPI_DESTINATARI(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_GRUPPI_DESTINATARI(ds);
        }

        [DataContext]
        public void FillMD_GRUPPI_RICHIEDENTI(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_GRUPPI_RICHIEDENTI(ds);
        }

        [DataContext]
        public void FillMD_RICHIEDENTI(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_RICHIEDENTI(ds);
        }

        [DataContext]
        public void FillMD_EMAIL_APPESE(MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_EMAIL_APPESE(ds);
        }

        [DataContext]
        public void FillMD_LOG(MailDispatcherDS ds, decimal IDMAIL)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.FillMD_LOG(ds, IDMAIL);
        }

        [DataContext(true)]
        public void UpdateMailDispatcherDSTable(string Tablename, MailDispatcherDS ds)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.UpdateMailDispatcherDSTable(Tablename, ds);
        }

        [DataContext(true)]
        public decimal CreaMail(decimal idRichiedente, string oggetto, string corpo)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            return a.CreaMail(idRichiedente, oggetto, corpo);
        }

        [DataContext(true)]
        public void SottomettiEmail(decimal IDMAIL)
        {
            MailDispatcherAdapter a = new MailDispatcherAdapter(DbConnection, DbTransaction);
            a.SottomettiEmail(IDMAIL);
        }
    }
}
