using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Magazzino
{
    public class MagazzinoAdapter : ReportWebAdapterBase
    {
        public MagazzinoAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
            base(connection, transaction)
        { }


        public void FillMONITOR_GIACENZA(MagazzinoDS ds)
        {
            string select = @"SELECT * FROM MONITOR_GIACENZA ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MONITOR_GIACENZA);
            }
        }

        public void FillRW_MAGAZZINO_CAMPIONI(MagazzinoDS ds)
        {
            string select = @"SELECT * FROM RW_MAGAZZINO_CAMPIONI ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_MAGAZZINO_CAMPIONI);
            }
        }

        public void FillMONITOR_APPROVVIGIONAMENTO(MagazzinoDS ds)
        {
            string select = @"SELECT * FROM MONITOR_APPROVVIGIONAMENTO ";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MONITOR_APPROVVIGIONAMENTO);
            }
        }

        public void FillMAGAZZ(MagazzinoDS ds, string filtro)
        {
            string f = string.Format("%{0}%", filtro.Trim().ToUpper());
            string select = @"SELECT * FROM GRUPPO.MAGAZZ WHERE MODELLO LIKE $P{FILTRO}";

            ParamSet ps = new ParamSet();
            ps.AddParam("FILTRO", DbType.String, f);
            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {
                da.Fill(ds.MAGAZZ);
            }
        }

        public void FillMAGAZZ(MagazzinoDS ds, List<string> IDMAGAZZ)
        {

            if (IDMAGAZZ.Count == 0) return;

            string selezione = ConvertToStringForInCondition(IDMAGAZZ);
            string select = @"SELECT * FROM GRUPPO.MAGAZZ WHERE IDMAGAZZ IN ({0})";

            select = string.Format(select, selezione);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZ);
            }

        }

        public void UpdateMagazzinoDSTable(string tablename, MagazzinoDS ds)
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

        public void FillMAGAZZINOESTERNO(String dataInizio, String dataFine, string codiceLavorante, MagazzinoDS ds)
        {
            DateTime inizio = DateTime.Parse(dataInizio);
            DateTime fine = DateTime.Parse(dataFine);

            string select = @"SELECT * FROM MAGAZZINIESTERNI 
                                WHERE CODICECLIFO = '{2}'
                                AND INIZIO <= to_date('{1}','DD/MM/YYYY')
                                AND FINE > to_date('{0}','DD/MM/YYYY')
                                ORDER BY AZIENDA,NUMMOVFASE,MODELLO";

            //VECCHIA CONDIZIONE
            //AND(
            //                        (INIZIO <= to_date('{0}', 'DD/MM/YYYY')

            //                        AND FINE >= to_date('{0}', 'DD/MM/YYYY'))


            //                          OR
            //                          (INIZIO <= to_date('{0}', 'DD/MM/YYYY')
            //                          AND INIZIO > to_date('{1}', 'DD/MM/YYYY'))
            //                          )      

            string dtInizio = inizio.ToString("dd/MM/yyyy");
            string dtFine = fine.ToString("dd/MM/yyyy");
            select = string.Format(select, dtInizio, dtFine,codiceLavorante.Trim());

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.MAGAZZINIESTERNI);
            }

        }
    }
}
