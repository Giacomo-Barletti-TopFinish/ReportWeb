using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Magazzino
{
    public class MagazzinoBusiness : ReportWebBusinessBase
    {
        public MagazzinoBusiness() : base() { }

        [DataContext]
        public void FillMONITOR_GIACENZA(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillMONITOR_GIACENZA(ds);
        }

        [DataContext]
        public void FillMAGAZZ(MagazzinoDS ds, string filtro)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, filtro);
        }

        [DataContext]
        public void FillRW_MAGAZZINO_CAMPIONI(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillRW_MAGAZZINO_CAMPIONI(ds);
        }
        [DataContext]
        public void FillRW_POSIZIONE_CAMPIONI(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillRW_POSIZIONE_CAMPIONI(ds);

        }
        [DataContext]
        public void FillMAGAZZ(MagazzinoDS ds, List<string> filtro)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZ(ds, filtro);
        }

        [DataContext(true)]
        public void UpdateMONITOR_GIACENZA(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.UpdateMagazzinoDSTable(ds.MONITOR_GIACENZA.TableName, ds);
        }

        [DataContext(true)]
        public void UpdateMONITOR_APPROVVIGIONAMENTO(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.UpdateMagazzinoDSTable(ds.MONITOR_APPROVVIGIONAMENTO.TableName, ds);
        }

        [DataContext(true)]
        public void UpdateRW_MAGAZZINO_CAMPIONI(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.UpdateMagazzinoDSTable(ds.RW_MAGAZZINO_CAMPIONI.TableName, ds);
        }
        [DataContext(true)]
        public void UpdateRW_POSIZIONE_CAMPIONI(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.UpdateMagazzinoDSTable(ds.RW_POSIZIONE_CAMPIONI.TableName, ds);
        }

        [DataContext]
        public void FillMONITOR_APPROVVIGIONAMENTO(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillMONITOR_APPROVVIGIONAMENTO(ds);
        }

        [DataContext]
        public void FillMAGAZZINOESTERNO(String dataInizio, String dataFine, string codiceLavorante, MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.FillMAGAZZINOESTERNO(dataInizio, dataFine, codiceLavorante, ds);
        }
    }
}
