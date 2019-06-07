using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Trasferimenti
{
    public class TrasferimentiAdapter : ReportWebAdapterBase
    {
        public TrasferimentiAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
           base(connection, transaction)
        { }

        public void FillUSR_PRD_RESOURCESF(TrasferimentiDS ds)
        {
            string select = @"select rf.* 
                                from gruppo.usr_prd_resourcesf rf
                                inner join gruppo.usr_ana_resources rs on rs.idresource=rf.idresource
                                where rs.idtiporesource = '0000000019'
                                order by rf.CODRESOURCEF";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.USR_PRD_RESOURCESF);
            }
        }

        public void FillAP_GRIGLIA(TrasferimentiDS ds, DateTime dal, DateTime al, string barcodePartenza, string barcodeArrivo)
        {
            string select = @"select tr.data_partenza,rp.codresourcef operatore_partenza,tr.data_arrivo,ra.codresourcef operatore_arrivo,td.nummovfase,td.reparto,ma.modello
                                from ap_ttrasferimenti tr
                                inner join ap_dtrasferimenti td on td.idtrasferimento = tr.idtrasferimento
                                inner join usr_prd_movfasi mf on mf.barcode = td.barcode_odl
                                inner join gruppo.magazz ma on ma.idmagazz = mf.idmagazz
                                inner join gruppo.USR_PRD_RESOURCESF rp on rp.barcode=tr.barcode_partenza
                                inner join gruppo.USR_PRD_RESOURCESF ra on ra.barcode=tr.barcode_arrivo
                              WHERE ((data_partenza >= to_date('{0}','DD/MM/YYYY HH24:MI:SS') AND data_partenza <= to_date('{1}','DD/MM/YYYY HH24:MI:SS'))
                                    OR ( data_arrivo >= to_date('{2}','DD/MM/YYYY HH24:MI:SS') AND data_arrivo <= to_date('{3}','DD/MM/YYYY HH24:MI:SS')))";

            string dtInizio = dal.ToString("dd/MM/yyyy");
            dtInizio += " 00:00:01";
            string dtFine = al.ToString("dd/MM/yyyy");
            dtFine += " 23:59:59";
            select = string.Format(select, dtInizio, dtFine, dtInizio, dtFine);

            if (barcodePartenza != "-1")
                select = string.Format("{0} AND tr.barcode_partenza = '{1}'", select, barcodePartenza);

            if (barcodeArrivo != "-1")
                select = string.Format("{0} AND tr.barcode_arrivo = '{1}'", select, barcodeArrivo);

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.AP_GRIGLIA);
            }
        }
    }
}
