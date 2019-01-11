using ReportWeb.Data.Registrazione;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class RegistrazioneBLL
    {
        public List<Tuple<decimal, string, string>> FillRW_REGISTRAZIONE()
        {
            RegistrazioneDS ds = new RegistrazioneDS();
            using (RegistrazioneBusiness bRegistrazione = new RegistrazioneBusiness())
            {
                bRegistrazione.FillRW_REGISTRAZIONE(ds);
            }

            List<Tuple<decimal, string, string>> utentiRegistrati = new List<Tuple<decimal, string, string>>();
            foreach (RegistrazioneDS.RW_REGISTRAZIONERow utenteRegistrato in ds.RW_REGISTRAZIONE.OrderBy(x => x.COGNOME))
            {
                string nome = string.Format("{0} {1}", utenteRegistrato.NOME, utenteRegistrato.COGNOME);
                string data = string.Format("{0} {1}", utenteRegistrato.INGRESSO.ToShortDateString(), utenteRegistrato.INGRESSO.ToShortTimeString());
                Tuple<decimal, string, string> utente = new Tuple<decimal, string, string>(utenteRegistrato.IDREGISTRAZIONE, nome, data);
                utentiRegistrati.Add(utente);
            }

            return utentiRegistrati;
        }

        public bool RegistraIngresso(string Cognome, string Nome, string Azienda, string Tipo, string Numero, string Referente, out string messaggio)
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
