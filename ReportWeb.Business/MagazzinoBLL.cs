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
        public List<ModelloGiacenzaModel> TrovaModelloGiacenza(string Modello)
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

        public void SalvaApprovvigionamenti(string approvigionamenti, string Modello)
        {
            ApprovvigionamentoMagazzino[] Approvvigionamenti = JSonSerializer.Deserialize<ApprovvigionamentoMagazzino[]>(approvigionamenti);

            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                MagazzinoDS ds = new MagazzinoDS();
                bMagazzino.FillMONITOR_APPROVVIGIONAMENTO(ds);

                foreach (string idMagazz in Approvvigionamenti.Where(x => !x.Checked).Select(x => x.IDMAGAZZ))
                {
                    MagazzinoDS.MONITOR_APPROVVIGIONAMENTORow approvvigionamentoDaCancellare = ds.MONITOR_APPROVVIGIONAMENTO.Where(x => x.RowState != System.Data.DataRowState.Deleted && x.IDMAGAZZ == idMagazz).FirstOrDefault();
                    if (approvvigionamentoDaCancellare != null)
                        approvvigionamentoDaCancellare.Delete();
                }

                foreach (ApprovvigionamentoMagazzino approv in Approvvigionamenti.Where(x => x.Checked))
                {
                    MagazzinoDS.MONITOR_APPROVVIGIONAMENTORow approvvigionamento = ds.MONITOR_APPROVVIGIONAMENTO.Where(x => x.RowState != System.Data.DataRowState.Deleted && x.IDMAGAZZ == approv.IDMAGAZZ).FirstOrDefault();
                    if (approvvigionamento == null)
                    {
                        approvvigionamento = ds.MONITOR_APPROVVIGIONAMENTO.NewMONITOR_APPROVVIGIONAMENTORow();
                        approvvigionamento.IDMAGAZZ = approv.IDMAGAZZ;
                        approvvigionamento.NOTA = approv.Nota;
                        approvvigionamento.DATASCADENZA = DateTime.Parse(approv.DataScadenza);
                        ds.MONITOR_APPROVVIGIONAMENTO.AddMONITOR_APPROVVIGIONAMENTORow(approvvigionamento);
                    }
                    else
                    {
                        approvvigionamento.NOTA = approv.Nota;
                        approvvigionamento.DATASCADENZA = DateTime.Parse(approv.DataScadenza);
                    }
                }

                bMagazzino.UpdateMONITOR_APPROVVIGIONAMENTO(ds);
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

        public List<ModelloApprovvigionamentoModel> TrovaModelloApprovvigionamento(string Modello)
        {
            List<ModelloApprovvigionamentoModel> model = new List<ModelloApprovvigionamentoModel>();
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                MagazzinoDS ds = new MagazzinoDS();
                bMagazzino.FillMONITOR_APPROVVIGIONAMENTO(ds);
                bMagazzino.FillMAGAZZ(ds, Modello);

                foreach (MagazzinoDS.MAGAZZRow articolo in ds.MAGAZZ)
                {
                    ModelloApprovvigionamentoModel m = CreaModelloApprovvigionamentoModel(articolo, ds);

                    model.Add(m);
                }
            }

            return model;
        }

        private ModelloApprovvigionamentoModel CreaModelloApprovvigionamentoModel(MagazzinoDS.MAGAZZRow articolo, MagazzinoDS ds)
        {
            ModelloApprovvigionamentoModel m = new ModelloApprovvigionamentoModel();
            m.Descrizione = articolo.DESMAGAZZ;
            m.IDMAGAZZ = articolo.IDMAGAZZ;
            m.Modello = articolo.MODELLO;

            m.Nota = string.Empty;
            m.DataScadenza = string.Empty;
            m.Presente = false;
            MagazzinoDS.MONITOR_APPROVVIGIONAMENTORow approvvigionamento = ds.MONITOR_APPROVVIGIONAMENTO.Where(x => x.IDMAGAZZ == articolo.IDMAGAZZ).FirstOrDefault();
            if (approvvigionamento != null)
            {
                m.Nota = approvvigionamento.IsNOTANull() ? string.Empty : approvvigionamento.NOTA;
                m.DataScadenza = approvvigionamento.DATASCADENZA.ToString("yyyy-MM-dd");
                m.Presente = true;
            }

            return m;
        }
    }
}
