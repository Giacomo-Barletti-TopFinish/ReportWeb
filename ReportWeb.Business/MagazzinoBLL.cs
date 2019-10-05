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
                List<string> IDMAGAZZ = ds.MONITOR_GIACENZA.Select(X => X.IDMAGAZZ).Distinct().ToList();

                foreach (MagazzinoDS.MAGAZZRow articolo in ds.MAGAZZ.Where(x => !IDMAGAZZ.Contains(x.IDMAGAZZ)))
                {
                    ModelloGiacenzaModel m = CreaModelloGiacenzeModel(articolo, ds);

                    model.Add(m);
                }
            }

            return model;
        }

        public List<MagazzinoCampionarioModel> TrovaCampionario(string Codice, string Finitura)
        {
            List<MagazzinoCampionarioModel> model = new List<MagazzinoCampionarioModel>();
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                MagazzinoDS ds = new MagazzinoDS();
                bMagazzino.FillRW_MAGAZZINO_CAMPIONI(ds);

                List<MagazzinoDS.RW_MAGAZZINO_CAMPIONIRow> elementi = ds.RW_MAGAZZINO_CAMPIONI.ToList();
                if (!string.IsNullOrEmpty(Codice))
                {
                    elementi = elementi.Where(x => x.CODICE.Contains(Codice)).ToList();
                }

                if (!string.IsNullOrEmpty(Finitura))
                {
                    elementi = elementi.Where(x => !x.IsFINITURANull() && x.FINITURA.Contains(Finitura)).ToList();
                }

                foreach (MagazzinoDS.RW_MAGAZZINO_CAMPIONIRow articolo in elementi)
                {
                    MagazzinoCampionarioModel m = new MagazzinoCampionarioModel()
                    {
                        Descrizione = articolo.IsDESCRIZIONENull() ? string.Empty : articolo.DESCRIZIONE,
                        Codice = articolo.CODICE,
                        Finitura = articolo.IsFINITURANull() ? string.Empty : articolo.FINITURA,
                        Piano = articolo.PIANO,
                        Posizione = articolo.POSIZIONE,
                        IDMAGAZCAMP = articolo.IDMAGAZCAMP
                    };

                    model.Add(m);
                }
            }

            return model;

        }
        public List<ModelloGiacenzaModel> CaricaGiacenze()
        {
            List<ModelloGiacenzaModel> model = new List<ModelloGiacenzaModel>();
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                MagazzinoDS ds = new MagazzinoDS();
                bMagazzino.FillMONITOR_GIACENZA(ds);
                List<string> magazz = ds.MONITOR_GIACENZA.Select(x => x.IDMAGAZZ).ToList();
                bMagazzino.FillMAGAZZ(ds, magazz);

                foreach (MagazzinoDS.MAGAZZRow articolo in ds.MAGAZZ.OrderBy(x => x.MODELLO))
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

        public List<ModelloApprovvigionamentoModel> CaricaApprovvigionamento()
        {
            List<ModelloApprovvigionamentoModel> model = new List<ModelloApprovvigionamentoModel>();
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                MagazzinoDS ds = new MagazzinoDS();
                bMagazzino.FillMONITOR_APPROVVIGIONAMENTO(ds);
                List<string> magazz = ds.MONITOR_APPROVVIGIONAMENTO.Select(x => x.IDMAGAZZ).ToList();
                bMagazzino.FillMAGAZZ(ds, magazz);

                foreach (MagazzinoDS.MAGAZZRow articolo in ds.MAGAZZ.OrderBy(x => x.MODELLO))
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

        public List<MagazzinoLavorantiEsterniModel> EstraiMagazzinoLavorantiEsterni(string dataInizio, string dataFine, string lavorante)
        {
            List<MagazzinoLavorantiEsterniModel> model = new List<MagazzinoLavorantiEsterniModel>();
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                MagazzinoDS ds = new MagazzinoDS();
                bMagazzino.FillMAGAZZINOESTERNO(dataInizio, dataFine, lavorante, ds);

                foreach (MagazzinoDS.MAGAZZINIESTERNIRow magaz in ds.MAGAZZINIESTERNI.OrderBy(x => x.MODELLO))
                {
                    MagazzinoLavorantiEsterniModel m = new MagazzinoLavorantiEsterniModel();
                    m.ODL = magaz.NUMMOVFASE;
                    m.Azienda = magaz.AZIENDA;
                    m.IdModello = magaz.IDMODELLO;
                    m.Modello = magaz.MODELLO.Trim();
                    m.ModelloDescrizione = magaz.MODDESC.Trim();
                    m.Quanita = magaz.QTA;
                    m.Peso = magaz.PESO;
                    m.IdComponente = magaz.IDCOMPONENTE;
                    m.Componente = magaz.COMPONENTE.Trim();
                    m.ComponenteDescrizione = magaz.COMDESC.Trim();
                    m.QuanitaComponente = magaz.FABBITOT;
                    m.PesoComponente = magaz.PESOCOMPONENTE;
                    m.DataInizio = magaz.INIZIO.ToShortDateString();
                    m.DataFine = magaz.FINE.ToShortDateString();

                    model.Add(m);
                }
            }
            return model;
        }

        public void SalvaCampioni(string Id, string Codice, string Finitura, string Piano, string Posizione, string Descrizione, string User)
        {
            MagazzinoDS ds = new MagazzinoDS();
            using (MagazzinoBusiness bMagazzino = new MagazzinoBusiness())
            {
                bMagazzino.FillRW_MAGAZZINO_CAMPIONI(ds);
                MagazzinoDS.RW_MAGAZZINO_CAMPIONIRow elemento = null;
                if (string.IsNullOrEmpty(Id))
                {
                    elemento = ds.RW_MAGAZZINO_CAMPIONI.NewRW_MAGAZZINO_CAMPIONIRow();
                    elemento.CODICE = Codice;
                    elemento.FINITURA = Finitura;
                    elemento.PIANO = Piano;
                    elemento.POSIZIONE = Posizione;
                    elemento.DESCRIZIONE = Descrizione;
                    elemento.UTENTE = User;
                    elemento.DATAINSERIMENTO = DateTime.Now;
                    ds.RW_MAGAZZINO_CAMPIONI.AddRW_MAGAZZINO_CAMPIONIRow(elemento);
                }
                else
                {
                    decimal id = decimal.Parse(Id);
                    elemento = ds.RW_MAGAZZINO_CAMPIONI.Where(x => x.IDMAGAZCAMP == id).FirstOrDefault();
                    if (elemento == null)
                        throw new ArgumentException(string.Format("IDMAGAZCAMP non trovato il valore {0} impossibile salvare", Id));
                    elemento.CODICE = Codice;
                    elemento.FINITURA = Finitura;
                    elemento.PIANO = Piano;
                    elemento.POSIZIONE = Posizione;
                    elemento.DESCRIZIONE = Descrizione;
                    elemento.UTENTE = User;
                    elemento.DATAINSERIMENTO = DateTime.Now;
                }
                bMagazzino.UpdateRW_MAGAZZINO_CAMPIONI(ds);
            }
        }
    }
}
