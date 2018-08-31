using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Verniciatura
{

    public class VerniciaturaAdapter : ReportWebAdapterBase
    {
        public VerniciaturaAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void CancellaRigaConsuntivo(int IdConsuntivo)
        {
            string delete = @"
                DELETE FROM RW_VERNICIATURA_CONSUNTIVO WHERE IDCONSUNTIVO = $P{IdConsuntivo}";
            ParamSet ps = new ParamSet();
            ps.AddParam("IdConsuntivo", DbType.Int16, IdConsuntivo);

            using (DbCommand cmd = BuildCommand(delete, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void FillRW_VERNICIATURA_CONSUNTIVO(VerniciaturaDS ds)
        {
            string select = @"select co.* from RW_VERNICIATURA_CONSUNTIVO co order by giorno,idconsuntivo";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_VERNICIATURA_CONSUNTIVO);
            }
        }

        public void SalvaConsuntivo(string Giorno, int QuantitaManuale, int Barre, string UIDUSER)
        {

            string insert = @"INSERT INTO RW_VERNICIATURA_CONSUNTIVO (GIORNO,QUANTITA_MANUALE, BARRE, DATA_INSERIMENTO,UIDUSER) VALUES
                                            ($P<GIORNO>,$P<QUANTITA_MANUALE>, $P<BARRE>,$P<NOW>,$P<UIDUSER>)";
            DateTime giorno = DateTime.Parse(Giorno);
            ParamSet ps = new ParamSet();
            ps.AddParam("QUANTITA_MANUALE", DbType.Int32, QuantitaManuale);
            ps.AddParam("BARRE", DbType.Int32, Barre);
            ps.AddParam("NOW", DbType.DateTime, DateTime.Now);
            ps.AddParam("UIDUSER", DbType.String, UIDUSER);
            ps.AddParam("GIORNO", DbType.DateTime, giorno);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }



    }
}
