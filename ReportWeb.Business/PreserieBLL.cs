using ReportWeb.Data.Preserie;
using ReportWeb.Entities;
using ReportWeb.Models.Preserie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class PreserieBLL
    {
        public List<Commessa> TrovaCommessa(bool RicercaPerCommessa, string Commessa, string Articolo)
        {
            List<Commessa> commesse = new List<Commessa>();
            PreserieDS ds = new PreserieDS();

            using (PreserieBusiness bPreserie = new PreserieBusiness())
            {
                if (RicercaPerCommessa)
                    bPreserie.TrovaCommessaPerNome(Commessa, ds);
                else
                    bPreserie.TrovaCommessaPerModello(Articolo, ds);

                if (ds.USR_PRD_LANCIOD.Count == 0)
                    return commesse;

                List<string> IDMAGAZZ = ds.USR_PRD_LANCIOD.Select(x => x.IDMAGAZZ).ToList();
                bPreserie.FillMAGAZZ(ds, IDMAGAZZ);

                foreach (PreserieDS.USR_PRD_LANCIODRow lanciod in ds.USR_PRD_LANCIOD)
                {
                    Commessa commessa = CreaCommessa(lanciod, ds);
                    commesse.Add(commessa);
                }

            }
            return commesse;
        }


        private Commessa CreaCommessa(PreserieDS.USR_PRD_LANCIODRow lanciod, PreserieDS ds)
        {
            if (lanciod == null) return null;

            Commessa commessa = new Commessa();
            commessa.IDLANCIOT = lanciod.IDLANCIOD;
            commessa.IDLANCIOD = lanciod.IDLANCIOT;
            commessa.Articolo = new Articolo();
            if (!lanciod.IsIDMAGAZZNull())
            {
                PreserieDS.MAGAZZRow magazz = ds.MAGAZZ.Where(x => x.IDMAGAZZ == lanciod.IDMAGAZZ).FirstOrDefault();
                commessa.Articolo = CreaArticolo(magazz);
            }

            commessa.Quantita = lanciod.QTALANCIO;

            commessa.DataCommessa = string.Empty;
            commessa.DataInizio = string.Empty;
            commessa.DataFine = string.Empty;

            if (!lanciod.IsDATACOMMESSANull())
                commessa.DataCommessa = lanciod.DATACOMMESSA.ToShortDateString();

            if (!lanciod.IsDATAINIZIOPRODNull())
                commessa.DataInizio = lanciod.DATAINIZIOPROD.ToShortDateString();

            if (!lanciod.IsDATAFINEPRODNull())
                commessa.DataFine = lanciod.DATAFINEPROD.ToShortDateString();

            commessa.NomeCommessa = lanciod.IsNOMECOMMESSANull() ? string.Empty : lanciod.NOMECOMMESSA;
            commessa.Riferimento = lanciod.IsRIFERIMENTONull() ? string.Empty : lanciod.RIFERIMENTO;
            commessa.Lavorazioni = new List<Lavorazione>();
            return commessa;
        }

        private Articolo CreaArticolo(PreserieDS.MAGAZZRow magazz)
        {
            if (magazz == null) return null;

            Articolo articolo = new Articolo();

            articolo.IDMAGAZZ = magazz.IDMAGAZZ;
            articolo.Modello = magazz.MODELLO;
            articolo.Descrizione = magazz.IsDESMODELLOBASENull() ? string.Empty : magazz.DESMAGAZZ;

            return articolo;
        }
    }
}
