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

        public decimal CreaMail(decimal idRichiedente)
        {
            string insert = @"INSERT INTO MD_EMAIL (IDRICHIEDENTE,DATACREAZIONE, STATO) VALUES ($P{IDRICHIEDENTE},$P{DATA},${STATO}) RETURNING IDMAIL INTO $P{IDMAIL}";
            ParamSet ps = new ParamSet();
            ps.AddParam("IDRICHIEDENTE", DbType.Decimal, idRichiedente);
            ps.AddParam("DATA", DbType.DateTime, DateTime.Now);
            ps.AddParam("STATO", DbType.String, "CRT");
            ps.AddOutputParam("IDMAIL", DbType.Decimal);
            decimal IDMAIL = -1;
            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
                IDMAIL = RetrieveParamValue<int>(cmd, "IDMAIL");
            }
            return IDMAIL;
        }

        public void InserisciOggettoeCopro(decimal IDMAIL, string oggetto, string corpo)
        {
            if (oggetto.Length > 50)
                oggetto = oggetto.Substring(0, 50);

            if (corpo.Length > 4000)
                corpo = corpo.Substring(0, 4000);

            string insert = @"UPDATE MD_EMAIL SET OGGETTO = $P{OGGETTO}, CORPO = ${CORPO} WHERE IDMAIL = $P{IDMAIL}";
            ParamSet ps = new ParamSet();
            ps.AddParam("OGGETTO", DbType.String, oggetto);
            ps.AddParam("CORPO", DbType.String, corpo);
            ps.AddParam("IDMAIL", DbType.Decimal, IDMAIL);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void SottomettiEmail(decimal IDMAIL)
        {
            string insert = @"UPDATE MD_EMAIL SET STATO = $P{STATO} WHERE IDMAIL = $P{IDMAIL}";
            ParamSet ps = new ParamSet();
            ps.AddParam("STATO", DbType.String, "DIN");

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }


    }
}
