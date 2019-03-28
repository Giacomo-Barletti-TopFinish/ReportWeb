using ReportWeb.Data.Registrazione;
using ReportWeb.Entities;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class RegistrazioneBLL
    {
        public List<Registrazione> FillRW_REGISTRAZIONE()
        {
            RegistrazioneDS ds = new RegistrazioneDS();
            using (RegistrazioneBusiness bRegistrazione = new RegistrazioneBusiness())
            {
                bRegistrazione.FillRW_REGISTRAZIONE(ds);
            }

            List<Registrazione> utentiRegistrati = new List<Registrazione>();
            foreach (RegistrazioneDS.RW_REGISTRAZIONERow utenteRegistrato in ds.RW_REGISTRAZIONE.OrderBy(x => x.COGNOME))
            {
                string nome = string.Format("{0} {1}", utenteRegistrato.NOME, utenteRegistrato.COGNOME);
                string data = string.Format("{0} {1}", utenteRegistrato.INGRESSO.ToShortDateString(), utenteRegistrato.INGRESSO.ToShortTimeString());
                decimal tessera = utenteRegistrato.IsTESSERANull() ? -1 : utenteRegistrato.TESSERA;
                Registrazione utente = new Registrazione()
                {
                    Data = data,
                    Ditta = utenteRegistrato.DITTA,
                    IDREGISTRAZIONE = utenteRegistrato.IDREGISTRAZIONE,
                    Nome = nome,
                    Tessera = tessera
                };

                utentiRegistrati.Add(utente);
            }

            return utentiRegistrati;
        }


        public bool VerificaTesseraInUso(decimal tessera)
        {
            RegistrazioneDS ds = new RegistrazioneDS();
            using (RegistrazioneBusiness bRegistrazione = new RegistrazioneBusiness())
            {
                bRegistrazione.FillRW_REGISTRAZIONE(ds);
            }

            return ds.RW_REGISTRAZIONE.Any(x => x.IsUSCITANull() && !x.IsTESSERANull() && x.TESSERA == tessera);

        }

        public List<StoricoRegistrazioneModel> CaricaStorico(string Inizio, string Fine)
        {
            List<StoricoRegistrazioneModel> risultati = new List<StoricoRegistrazioneModel>();
            RegistrazioneDS ds = new RegistrazioneDS();
            using (RegistrazioneBusiness bRegistrazione = new RegistrazioneBusiness())
            {
                bRegistrazione.FillRW_REGISTRAZIONECompleta(ds);
            }

            DateTime dtInizio = DateTime.Parse(Inizio);
            DateTime dtFine = DateTime.Parse(Fine);

            List<RegistrazioneDS.RW_REGISTRAZIONERow> storico = ds.RW_REGISTRAZIONE.Where(x => x.INGRESSO >= dtInizio && x.INGRESSO <= dtFine).ToList();
            foreach (RegistrazioneDS.RW_REGISTRAZIONERow st in storico.OrderBy(x=>x.IDREGISTRAZIONE))
            {
                string nome = string.Format("{0} {1}", st.NOME, st.COGNOME);
                string azienda = st.IsAZIENDANull() ? string.Empty : st.AZIENDA;
                string doc = string.Format("{0} {1}", st.TIPODOCUMENTO, st.IsDOCUMENTONull() ? string.Empty : st.DOCUMENTO);
                string tessera = st.IsTESSERANull() ? string.Empty : st.TESSERA.ToString();
                string referente = st.REFERENTE;
                string ingresso = string.Format("{0} {1}",st.INGRESSO.ToShortDateString(),st.INGRESSO.ToShortTimeString());
                string uscita = string.Format("{0} {1}", st.IsUSCITANull() ? string.Empty : st.INGRESSO.ToShortDateString(),
                    st.IsUSCITANull() ? string.Empty : st.USCITA.ToShortTimeString());


                StoricoRegistrazioneModel elemento = new StoricoRegistrazioneModel();
                elemento.nome = nome;
                elemento.azienda = azienda;
                elemento.documento = doc;
                elemento.tessera = tessera;
                elemento.referente = referente;
                elemento.ingesso = ingresso;
                elemento.uscita = uscita;
                risultati.Add(elemento);

            }
            return risultati;

        }

        public bool RegistraIngresso(string Cognome, string Nome, string Azienda, string Tipo, string Numero, string Referente, Decimal Tessera, string Ditta, out string messaggio)
        {
            messaggio = string.Empty;
            RegistrazioneDS ds = new RegistrazioneDS();
            using (RegistrazioneBusiness bRegistrazione = new RegistrazioneBusiness())
            {
                bRegistrazione.FillRW_REGISTRAZIONE(ds);

                if (ds.RW_REGISTRAZIONE.Any(x => x.NOME.Trim() == Nome && x.COGNOME.Trim() == Cognome && Azienda.Trim() == Azienda && x.TIPODOCUMENTO.Trim() == Tipo && x.DOCUMENTO.Trim() == Numero))
                {
                    messaggio = "Utente già registrato";
                    return false;
                }
                RegistrazioneDS.RW_REGISTRAZIONERow registrazione = ds.RW_REGISTRAZIONE.NewRW_REGISTRAZIONERow();
                registrazione.NOME = Nome;
                registrazione.COGNOME = Cognome;
                registrazione.REFERENTE = Referente;
                registrazione.INGRESSO = DateTime.Now;

                if (!string.IsNullOrEmpty(Azienda))
                    registrazione.AZIENDA = Azienda;

                if (!string.IsNullOrEmpty(Tipo))
                    registrazione.TIPODOCUMENTO = Tipo;

                if (!string.IsNullOrEmpty(Numero))
                    registrazione.DOCUMENTO = Numero;

                registrazione.TESSERA = Tessera;
                registrazione.DITTA = Ditta;

                ds.RW_REGISTRAZIONE.AddRW_REGISTRAZIONERow(registrazione);
                bRegistrazione.UpdateRW_REGISTRAZIONE(ds);
                return true;
            }
        }

        public bool RegistraUscita(decimal IdRegistrazione, out string messaggio)
        {
            messaggio = string.Empty;
            RegistrazioneDS ds = new RegistrazioneDS();
            using (RegistrazioneBusiness bRegistrazione = new RegistrazioneBusiness())
            {
                bRegistrazione.FillRW_REGISTRAZIONE(ds);

                RegistrazioneDS.RW_REGISTRAZIONERow registrazione = ds.RW_REGISTRAZIONE.Where(x => x.IDREGISTRAZIONE == IdRegistrazione).FirstOrDefault();
                if (registrazione == null)
                {
                    messaggio = "Errore nella registrazione dell'uscita";
                    return false;
                }
                registrazione.USCITA = DateTime.Now;

                bRegistrazione.UpdateRW_REGISTRAZIONE(ds);
                return true;
            }
        }
    }
}
