using ReportWeb.Data.Magazzino;
using ReportWeb.Entities;
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
                    ModelloGiacenzaModel m = new ModelloGiacenzaModel();
                    m.Descrizione = articolo.DESMAGAZZ;
                    m.IDMAGAZZ = articolo.IDMAGAZZ;
                    m.Modello = articolo.MODELLO;

                    m.Giacenza = string.Empty;
                    MagazzinoDS.MONITOR_GIACENZARow giacenza = ds.MONITOR_GIACENZA.Where(x => x.IDMAGAZZ == articolo.IDMAGAZZ).FirstOrDefault();
                    if (giacenza != null)
                        m.Giacenza = giacenza.GIACENZA.ToString();

                    model.Add(m);
                }
            }

            return model;
        }
    }
}
