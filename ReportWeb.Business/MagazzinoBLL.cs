using ReportWeb.Common.Helpers;
using ReportWeb.Data.Magazzino;
using ReportWeb.Entities;
using ReportWeb.Models.JSON;
using ReportWeb.Models.Magazzino;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class MagazzinoBLL
    {
        public List<ModelloGiacenzaModel> TrovaModello(string Modello)
        {
            List<ModelloGiacenzaModel> model = new List<ModelloGiacenzaModel>();
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                MagazzinoDS ds = new MagazzinoDS();
                bMagazzino.FillMONITOR_GIACENZA(ds);
                bMagazzino.FillMAGAZZ(ds, Modello);

                foreach (MagazzinoDS.MAGAZZRow articolo in ds.MAGAZZ)
                {
                    ModelloGiacenzaModel m = CreaModelloGiacenzeModel(articolo, ds);

                    model.Add(m);
                }
            }

            return model;
        }

        public void SalvaGiacenze(string giacenze, string Modello)
        {
            GiacenzaMagazzino[] Giacenze = JSonSerializer.Deserialize<GiacenzaMagazzino[]>(giacenze);

            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                MagazzinoDS ds = new MagazzinoDS();
                bMagazzino.FillMONITOR_GIACENZA(ds);

                foreach (string idMagazz in Giacenze.Where(x => !x.Checked).Select(x => x.IDMAGAZZ))
                {
                    MagazzinoDS.MONITOR_GIACENZARow giacenzaDaCancellare = ds.MONITOR_GIACENZA.Where(x => x.RowState != System.Data.DataRowState.Deleted && x.IDMAGAZZ == idMagazz).FirstOrDefault();
                    if (giacenzaDaCancellare != null)
                        giacenzaDaCancellare.Delete();
                }

                foreach (GiacenzaMagazzino giac in Giacenze.Where(x => x.Checked))
                {
                    MagazzinoDS.MONITOR_GIACENZARow giacenza = ds.MONITOR_GIACENZA.Where(x => x.RowState != System.Data.DataRowState.Deleted && x.IDMAGAZZ == giac.IDMAGAZZ).FirstOrDefault();
                    if (giacenza == null)
                    {
                        giacenza = ds.MONITOR_GIACENZA.NewMONITOR_GIACENZARow();
                        giacenza.IDMAGAZZ = giac.IDMAGAZZ;
                        giacenza.GIACENZA = giac.Giacenza;
                        ds.MONITOR_GIACENZA.AddMONITOR_GIACENZARow(giacenza);
                    }
                    else
                        giacenza.GIACENZA = giac.Giacenza;
                }

                bMagazzino.UpdateMONITOR_GIACENZA(ds);
            }


        }

        private ModelloGiacenzaModel CreaModelloGiacenzeModel(MagazzinoDS.MAGAZZRow articolo, MagazzinoDS ds)
        {
            ModelloGiacenzaModel m = new ModelloGiacenzaModel();
            m.Descrizione = articolo.DESMAGAZZ;
            m.IDMAGAZZ = articolo.IDMAGAZZ;
            m.Modello = articolo.MODELLO;

            m.Giacenza = string.Empty;
            m.Presente = false;
            MagazzinoDS.MONITOR_GIACENZARow giacenza = ds.MONITOR_GIACENZA.Where(x => x.IDMAGAZZ == articolo.IDMAGAZZ).FirstOrDefault();
            if (giacenza != null)
            {
                m.Giacenza = giacenza.GIACENZA.ToString();
                m.Presente = true;
            }

            return m;
        }
    }
}
