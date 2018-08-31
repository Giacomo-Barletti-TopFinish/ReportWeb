using ReportWeb.Common.Helpers;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data
{
    public class PVDAdapter : ReportWebAdapterBase
    {
        public PVDAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillUSR_PRD_RESOURCESF(PVDDS ds)
        {
            string select = @"
                SELECT RF.* FROM GRUPPO.USR_PRD_RESOURCESF RF
                INNER JOIN GRUPPO.USR_ANA_RESOURCES AR ON AR.IDRESOURCE = RF.IDRESOURCE
                INNER JOIN GRUPPO.USR_TAB_TIPORESOURCES TR ON TR.IDTIPORESOURCE = AR.IDTIPORESOURCE
                WHERE TR.CODTIPORESOURCE = 'MACCHINE'
                AND RF.CODICECLIFO = 'PVD'
                AND AR.CODRESOURCE = 'PVD'";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_RESOURCESF);
            }
        }

        public void CancellaRigaConsuntivo(int IdConsuntivo)
        {
            string delete = @"
                DELETE FROM RW_PVD_CONSUNTIVO WHERE IDCONSUNTIVO = $P{IdConsuntivo}";
            ParamSet ps = new ParamSet();
            ps.AddParam("IdConsuntivo", DbType.Int16, IdConsuntivo);

            using (DbCommand cmd = BuildCommand(delete, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void FillRW_PVD_CONSUNTIVO(PVDDS ds)
        {
            string select = @"select co.*,rf.desresourcef as macchina from rw_pvd_consuntivo co
                                inner join GRUPPO.USR_PRD_RESOURCESF RF on co.idresourcef = rf.idresourcef
                                order by giorno, macchina,inizio,idconsuntivo";


            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_PVD_CONSUNTIVO);
            }
        }

        public void SalvaConsuntivo(string IDRESOURCEF, string FinituraCodice, string FinituraDescrizione, string Tipo, string Giorno, string Inizio, string Fine, int Quantita, string Clienti,
            string Articolo, int Impegno, string UIDUSER)
        {
            LogManagerHelper.WriteMessage("Salva consuntivo PVDADAPTER");

            string insert = @"INSERT INTO RW_PVD_CONSUNTIVO (IDRESOURCEF,FINITURA_COD,FINITURA_DESC,TIPO,GIORNO,INIZIO, FINE, QUANTITA, CLIENTI, ARTICOLO, IMPEGNO, DATA_INSERIMENTO,UIDUSER) VALUES
                                            ($P<IDRESOURCEF>,$P<FinituraCodice>,$P<FinituraDescrizione>,$P<Tipo>,$P<GIORNO>,$P<Inizio>, $P<Fine>,$P<Quantita>, $P<Clienti>, $P<Articolo>,$P<Impegno>,$P<NOW>,$P<UIDUSER>)";
            DateTime giorno = DateTime.Parse(Giorno);
            ParamSet ps = new ParamSet();
            ps.AddParam("IDRESOURCEF", DbType.String, IDRESOURCEF.ToUpper());
            ps.AddParam("FinituraCodice", DbType.String, FinituraCodice.ToUpper());
            ps.AddParam("FinituraDescrizione", DbType.String, FinituraDescrizione.ToUpper());
            ps.AddParam("Tipo", DbType.String, Tipo.ToUpper());
            ps.AddParam("Inizio", DbType.String, Inizio);
            ps.AddParam("Fine", DbType.String, Fine);
            ps.AddParam("Quantita", DbType.Int32, Quantita);
            ps.AddParam("Clienti", DbType.String, Clienti.ToUpper());
            ps.AddParam("Articolo", DbType.String, Articolo.ToUpper());
            ps.AddParam("Impegno", DbType.Int32, Impegno);
            ps.AddParam("NOW", DbType.DateTime, DateTime.Now);
            ps.AddParam("UIDUSER", DbType.String, UIDUSER);
            ps.AddParam("GIORNO", DbType.DateTime, giorno);

            LogManagerHelper.WriteMessage("Salva consuntivo PVDADAPTER PRIMA DI USING");
            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                LogManagerHelper.WriteMessage("Salva consuntivo PVDADAPTER PRIMA DI EXECUTE");
                cmd.ExecuteNonQuery();
                LogManagerHelper.WriteMessage("Salva consuntivo PVDADAPTER DOPO EXECUTE");
            }
        }



    }
}
