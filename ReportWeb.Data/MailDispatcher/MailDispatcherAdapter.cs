using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.MailDispatcher
{

    public class MailDispatcherAdapter : ReportWebAdapterBase
    {
        public MailDispatcherAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillMD_GRUPPI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI);
            }
        }

        public void FillMD_GRUPPI_DESTINATARI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI_DESTINATARI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI_DESTINATARI);
            }
        }

        public void FillMD_RICHIEDENTI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_RICHIEDENTI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_RICHIEDENTI);
            }
        }

        public void FillMD_GRUPPI_RICHIEDENTI(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_GRUPPI_RICHIEDENTI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_GRUPPI_RICHIEDENTI);
            }
        }

        public void UpdateMailDispatcherDSTable(string tablename, MailDispatcherDS ds)
        {
            string query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}", tablename);

            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                a.ContinueUpdateOnError = false;
                DataTable dt = ds.Tables[tablename];
                DbCommandBuilder cmd = BuildCommandBuilder(a);
                a.UpdateCommand = cmd.GetUpdateCommand();
                a.DeleteCommand = cmd.GetDeleteCommand();
                a.InsertCommand = cmd.GetInsertCommand();
                a.Update(dt);
            }
        }

        public decimal CreaMail(decimal idRichiedente, string oggetto, string corpo)
        {
            if (oggetto.Length > 50)
                oggetto = oggetto.Substring(0, 50);

            if (corpo.Length > 4000)
                corpo = corpo.Substring(0, 4000);
            string insert = @"INSERT INTO MD_EMAIL (IDRICHIEDENTE,DATACREAZIONE, STATO, TENTATIVO,OGGETTO,CORPO) VALUES ($P{IDRICHIEDENTE},$P{DATA},$P{STATO},0,$P{OGGETTO},$P{CORPO}) RETURNING IDMAIL INTO $P{IDMAIL}";
            ParamSet ps = new ParamSet();
            ps.AddParam("IDRICHIEDENTE", DbType.Decimal, idRichiedente);
            ps.AddParam("DATA", DbType.DateTime, DateTime.Now);
            ps.AddParam("STATO", DbType.String, "CRT");
            ps.AddParam("OGGETTO", DbType.String, oggetto);
            ps.AddParam("CORPO", DbType.String, corpo);
            ps.AddOutputParam("IDMAIL", DbType.Decimal);
            decimal IDMAIL = -1;
            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
                IDMAIL = RetrieveParamValue<decimal>(cmd, "IDMAIL");
            }
            return IDMAIL;
        }

        public void SottomettiEmail(decimal IDMAIL)
        {
            string insert = @"UPDATE MD_EMAIL SET STATO = $P{STATO} WHERE IDMAIL = $P{IDMAIL}";
            ParamSet ps = new ParamSet();
            ps.AddParam("STATO", DbType.String, "DIN");
            ps.AddParam("IDMAIL", DbType.Decimal, IDMAIL);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void FillMD_EMAIL_APPESE(MailDispatcherDS ds)
        {
            string select = @"SELECT * FROM MD_EMAIL WHERE STATO IN ('ERR','DIN','BLK') ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MD_EMAIL);
            }
        }

        public void FillMD_LOG(MailDispatcherDS ds, decimal IDMAIL)
        {
            string select = @"SELECT * FROM MD_LOG WHERE IDMAIL = $P{IDMAIL}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDMAIL", DbType.Decimal, IDMAIL);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MD_LOG);
            }
        }
    }
}
