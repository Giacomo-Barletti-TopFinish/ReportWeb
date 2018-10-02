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

        [DataContext(true)]
        public void UpdateMONITOR_GIACENZA(MagazzinoDS ds)
        {
            MagazzinoAdapter a = new MagazzinoAdapter(DbConnection, DbTransaction);
            a.UpdateMagazzinoDSTable(ds.MONITOR_GIACENZA.TableName, ds);
        }
    }
}
