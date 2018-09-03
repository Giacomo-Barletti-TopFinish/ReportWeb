using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Galvanica
{
    public class GalvanicaAdapter : ReportWebAdapter
    {
        public GalvanicaAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }

        public void FillRW_GALV_CONSUNTIVO(GalvanicaDS ds)
        {
            string query = @"SELECT * FROM $T<RW_GALV_CONSUNTIVO> ORDER BY INIZIO_TURNO";
            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                a.Fill(ds.RW_GALV_CONSUNTIVO);
            }
        }

        public void FillRW_GALV_FERMI(GalvanicaDS ds)
        {
            string query = @"SELECT * FROM $T<RW_GALV_FERMI> ORDER BY ORA";
            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                a.Fill(ds.RW_GALV_FERMI);
            }
        }

        public void CancellaConsuntivo(long IdConsuntivo)
        {

            string insert = @"DELETE FROM RW_GALV_CONSUNTIVO WHERE IDCONSUNTIVO = $P{IDCONSUNTIVO}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDCONSUNTIVO", DbType.Int64, IdConsuntivo);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void CancellaFermi(long IdConsuntivo)
        {

            string insert = @"DELETE FROM RW_GALV_FERMI WHERE IDCONSUNTIVO = $P{IDCONSUNTIVO}";

            ParamSet ps = new ParamSet();
            ps.AddParam("IDCONSUNTIVO", DbType.Int64, IdConsuntivo);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }


        public void SalvaConsuntivo(long IdConsuntivo, string Inizio_turno, string Fine_turno, int Barre, string UIDUSER)
        {

            string insert = @"INSERT INTO RW_GALV_CONSUNTIVO ( IDCONSUNTIVO ,INIZIO_TURNO ,FINE_TURNO ,BARRE ,  DATA_INSERIMENTO ,UIDUSER ) VALUES (
                        $P{IDCONSUNTIVO},$P{INIZIO_TURNO},$P{FINE_TURNO},$P{BARRE},$P{DATA_INSERIMENTO},$P{UIDUSER})";

            DateTime dtInizio = DateTime.Parse(Inizio_turno);
            DateTime dtFine = DateTime.Parse(Fine_turno);

            ParamSet ps = new ParamSet();
            ps.AddParam("IDCONSUNTIVO", DbType.Int64, IdConsuntivo);
            ps.AddParam("INIZIO_TURNO", DbType.DateTime, dtInizio);
            ps.AddParam("FINE_TURNO", DbType.DateTime, dtFine);
            ps.AddParam("BARRE", DbType.Int32, Barre);
            ps.AddParam("DATA_INSERIMENTO", DbType.DateTime, DateTime.Now);
            ps.AddParam("UIDUSER", DbType.String, UIDUSER);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }

        public void SalvaFermo(long IdConsuntivo, string Tipo, string Ora, string Durata, string Motivo, string UIDUSER)
        {

            string insert = @"INSERT INTO RW_GALV_FERMI ( IDCONSUNTIVO ,TIPO ,ORA,DURATA,  MOTIVO,DATA_INSERIMENTO,UIDUSER ) VALUES (
                        $P{IDCONSUNTIVO},$P{TIPO},$P{ORA},$P{DURATA},$P{MOTIVO},$P{DATA_INSERIMENTO},$P{UIDUSER})";


            ParamSet ps = new ParamSet();
            ps.AddParam("IDCONSUNTIVO", DbType.Int64, IdConsuntivo);
            ps.AddParam("TIPO", DbType.String, Tipo);
            ps.AddParam("ORA", DbType.String, Ora);
            ps.AddParam("DURATA", DbType.String, Durata);
            ps.AddParam("MOTIVO", DbType.String, Motivo);
            ps.AddParam("DATA_INSERIMENTO", DbType.DateTime, DateTime.Now);
            ps.AddParam("UIDUSER", DbType.String, UIDUSER);

            using (DbCommand cmd = BuildCommand(insert, ps))
            {
                cmd.ExecuteNonQuery();
            }
        }
    }
}
