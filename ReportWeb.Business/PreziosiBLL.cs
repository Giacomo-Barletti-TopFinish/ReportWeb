using ReportWeb.Data.Preziosi;
using ReportWeb.Entities;
using ReportWeb.Models;
using ReportWeb.Models.Preziosi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class PreziosiBLL
    {
        public List<RWListItem> CreaListaPreziosi()
        {
            PreziosiDS ds = new PreziosiDS();
            List<RWListItem> preziosi = new List<RWListItem>();
            preziosi.Add(new RWListItem(string.Empty, "-1"));
            using (PreziosiBusiness bPreziosi = new PreziosiBusiness())
            {
                bPreziosi.FillRW_PREZIOSI(ds);
                preziosi.AddRange(ds.RW_PREZIOSI.Select(x => new RWListItem(x.MATERIALE, x.IDPREZIOSO.ToString())).ToArray());
            }

            return preziosi;
        }

        public List<SaldoCasseforti> GetSaldiCompleti()
        {
            List<SaldoCasseforti> saldi = new List<SaldoCasseforti>();
            PreziosiDS ds = new PreziosiDS();
            using (PreziosiBusiness bPreziosi = new PreziosiBusiness())
            {
                bPreziosi.FillRW_PREZIOSI(ds);
                bPreziosi.FillRW_MOVIMENTI_PREZIOSI(ds);

                foreach (PreziosiDS.RW_PREZIOSIRow prezioso in ds.RW_PREZIOSI.OrderBy(x => x.IDPREZIOSO))
                {
                    string saldoA = GetUltimoSaldo((int)prezioso.IDPREZIOSO, "A", ds).ToString();
                    string saldoB = GetUltimoSaldo((int)prezioso.IDPREZIOSO, "B", ds).ToString();
                    saldi.Add(new SaldoCasseforti(prezioso.MATERIALE.Trim(), saldoA, saldoB));
                }
                return saldi;
            }
        }

        public List<Movimenti> CaricaMovimenti(string DataInizio, string DataFine, int IdPrezioso)
        {
            DateTime inizio;
            DateTime fine;
            List<Movimenti> movimenti = new List<Movimenti>();
            if (DateTime.TryParse(DataInizio, out inizio) && DateTime.TryParse(DataFine, out fine))
            {
                inizio = new DateTime(inizio.Year, inizio.Month, inizio.Day, 0, 0, 0);
                fine = new DateTime(fine.Year, fine.Month, fine.Day, 23, 59, 59);
                PreziosiDS ds = new PreziosiDS();
                using (PreziosiBusiness bPreziosi = new PreziosiBusiness())
                {
                    bPreziosi.FillRW_MOVIMENTI_PREZIOSI(ds);
                    bPreziosi.FillRW_PREZIOSI(ds);
                    List<PreziosiDS.RW_MOVIMENTI_PREZIOSIRow> movimentiFiltrato = ds.RW_MOVIMENTI_PREZIOSI.Where(x => x.DATA >= inizio && x.DATA <= fine).ToList();
                    if (IdPrezioso > 0)
                        movimentiFiltrato = movimentiFiltrato.Where(x => x.IDPREZIOSO == IdPrezioso).ToList();

                    foreach (PreziosiDS.RW_MOVIMENTI_PREZIOSIRow mov in movimentiFiltrato.OrderBy(x => x.DATA))
                    {
                        string materiale = ds.RW_PREZIOSI.Where(x => x.IDPREZIOSO == mov.IDPREZIOSO).Select(x => x.MATERIALE.Trim()).FirstOrDefault();
                        Movimenti m = new Movimenti()
                        {
                            Cassaforte = mov.CASSAFORTE,
                            Causale = mov.IsNOTANull() ? string.Empty : mov.NOTA,
                            Giorno = mov.DATA,
                            Materiale = materiale,
                            Avere = mov.QUANTITA > 0 ? mov.QUANTITA.ToString() : string.Empty,
                            Dare = mov.QUANTITA < 0 ? (-1 * mov.QUANTITA).ToString() : string.Empty,
                            Utente = mov.UTENTE
                        };
                        movimenti.Add(m);
                    }
                }

            }
            return movimenti;
        }
        public Tuple<string, string> GetSaldoMateriale(int IdPrezioso)
        {

            PreziosiDS ds = new PreziosiDS();
            using (PreziosiBusiness bPreziosi = new PreziosiBusiness())
            {
                bPreziosi.FillRW_MOVIMENTI_PREZIOSI(ds);
                string saldoA = GetUltimoSaldo(IdPrezioso, "A", ds).ToString();
                string saldoB = GetUltimoSaldo(IdPrezioso, "B", ds).ToString();
                return new Tuple<string, string>(saldoA, saldoB);
            }
        }

        private decimal GetUltimoSaldo(int IdPrezioso, string Cassaforte, PreziosiDS ds)
        {
            if (!ds.RW_MOVIMENTI_PREZIOSI.Any(x => x.IDPREZIOSO == IdPrezioso && x.CASSAFORTE == Cassaforte)) return 0;
            DateTime data = ds.RW_MOVIMENTI_PREZIOSI.Where(x => x.IDPREZIOSO == IdPrezioso && x.CASSAFORTE == Cassaforte).Max(x => x.DATA);
            if (data != null)
            {
                PreziosiDS.RW_MOVIMENTI_PREZIOSIRow riga = ds.RW_MOVIMENTI_PREZIOSI.Where(x => x.IDPREZIOSO == IdPrezioso && x.DATA == data && x.CASSAFORTE == Cassaforte).FirstOrDefault();
                if (riga != null)
                {
                    return riga.SALDOFINALE;
                }
            }
            return 0;

        }

        public bool SalvaMovimentoPreziosoCassaforteA(int IdPrezioso, string Operazione, decimal Quantita, string Causale, string ConnectedUser)
        {
            try
            {
                PreziosiDS ds = new PreziosiDS();
                using (PreziosiBusiness bPreziosi = new PreziosiBusiness())
                {
                    bPreziosi.FillRW_PREZIOSI(ds);
                    bPreziosi.FillRW_MOVIMENTI_PREZIOSI(ds);
                    decimal saldoA = GetUltimoSaldo(IdPrezioso, "A", ds);
                    decimal saldoB = GetUltimoSaldo(IdPrezioso, "B", ds);
                    string prezioso = ds.RW_PREZIOSI.Where(x => x.IDPREZIOSO == IdPrezioso).Select(x => x.MATERIALE).FirstOrDefault();

                    if (Operazione == "P")
                    {

                        if (saldoA < Quantita) return false;
                        decimal nuovoSaldoA = saldoA - Quantita;
                        decimal nuovoSaldoB = saldoB + Quantita;
                        PreziosiDS.RW_MOVIMENTI_PREZIOSIRow movimentoA = ds.RW_MOVIMENTI_PREZIOSI.NewRW_MOVIMENTI_PREZIOSIRow();
                        movimentoA.CASSAFORTE = "A";
                        movimentoA.DATA = DateTime.Now;
                        movimentoA.IDPREZIOSO = IdPrezioso;
                        movimentoA.NOTA = Causale;
                        movimentoA.QUANTITA = -1 * Quantita;
                        movimentoA.SALDOFINALE = nuovoSaldoA;
                        movimentoA.SALDOINIZIALE = saldoA;
                        movimentoA.UTENTE = ConnectedUser;
                        ds.RW_MOVIMENTI_PREZIOSI.AddRW_MOVIMENTI_PREZIOSIRow(movimentoA);

                        PreziosiDS.RW_MOVIMENTI_PREZIOSIRow movimentoB = ds.RW_MOVIMENTI_PREZIOSI.NewRW_MOVIMENTI_PREZIOSIRow();
                        movimentoB.CASSAFORTE = "B";
                        movimentoB.DATA = DateTime.Now;
                        movimentoB.IDPREZIOSO = IdPrezioso;
                        movimentoB.NOTA = Causale;
                        movimentoB.QUANTITA = Quantita;
                        movimentoB.SALDOFINALE = nuovoSaldoB;
                        movimentoB.SALDOINIZIALE = saldoB;
                        movimentoB.UTENTE = ConnectedUser;
                        ds.RW_MOVIMENTI_PREZIOSI.AddRW_MOVIMENTI_PREZIOSIRow(movimentoB);

                        bPreziosi.UpdatePreziosiDS(ds.RW_MOVIMENTI_PREZIOSI.TableName, ds);
                        InviaMailCassaforteA(-1 * Quantita, Causale, ConnectedUser, prezioso, saldoA, nuovoSaldoA);
                        InviaMailCassaforteB(Quantita, Causale, ConnectedUser, prezioso, saldoB, nuovoSaldoB);
                    }
                    else
                    {
                        decimal nuovoSaldoA = saldoA + Quantita;
                        PreziosiDS.RW_MOVIMENTI_PREZIOSIRow movimentoA = ds.RW_MOVIMENTI_PREZIOSI.NewRW_MOVIMENTI_PREZIOSIRow();
                        movimentoA.CASSAFORTE = "A";
                        movimentoA.DATA = DateTime.Now;
                        movimentoA.IDPREZIOSO = IdPrezioso;
                        movimentoA.NOTA = Causale;
                        movimentoA.QUANTITA = Quantita;
                        movimentoA.SALDOFINALE = nuovoSaldoA;
                        movimentoA.SALDOINIZIALE = saldoA;
                        movimentoA.UTENTE = ConnectedUser;
                        ds.RW_MOVIMENTI_PREZIOSI.AddRW_MOVIMENTI_PREZIOSIRow(movimentoA);

                        bPreziosi.UpdatePreziosiDS(ds.RW_MOVIMENTI_PREZIOSI.TableName, ds);
                        InviaMailCassaforteA(Quantita, Causale, ConnectedUser, prezioso, saldoA, nuovoSaldoA);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        private void InviaMailCassaforteA(decimal Quantita, string Causale, string ConnectedUser, string prezioso, decimal saldoIniziale, decimal saldoFinale)
        {
            Decimal IDMAIL = 0;
            MailDispatcherBLL bllMD = new MailDispatcherBLL();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MOVIMENTO SULLA CASSAFORTE A");
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.Format("Operazione: {0}", Quantita < 0 ? "Prelievo" : "Versamento"));
            sb.AppendLine(string.Format("Quantità: {0}", Math.Abs(Quantita)));
            sb.AppendLine(string.Format("Nota: {0}", Causale));
            sb.AppendLine(string.Format("Operatore: {0}", ConnectedUser));
            sb.AppendLine(string.Format("Saldo iniziale: {0}", saldoIniziale));
            sb.AppendLine(string.Format("Saldo finale: {0}", saldoFinale));

            string oggetto = "PREZIONI - NUOVO MOVIMENTO";
            IDMAIL = bllMD.CreaEmail("PREZIOSI - CASSAFORTE A", oggetto, sb.ToString());

            bllMD.SottomettiEmail(IDMAIL);
        }

        private void InviaMailCassaforteB(decimal Quantita, string Causale, string ConnectedUser, string prezioso, decimal saldoIniziale, decimal saldoFinale)
        {
            Decimal IDMAIL = 0;
            MailDispatcherBLL bllMD = new MailDispatcherBLL();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MOVIMENTO SULLA CASSAFORTE B");
            sb.AppendLine(string.Empty);
            sb.AppendLine(string.Format("Operazione: {0}", Quantita < 0 ? "Prelievo" : "Versamento"));
            sb.AppendLine(string.Format("Quantità: {0}", Math.Abs(Quantita)));
            sb.AppendLine(string.Format("Nota: {0}", Causale));
            sb.AppendLine(string.Format("Operatore: {0}", ConnectedUser));
            sb.AppendLine(string.Format("Saldo iniziale: {0}", saldoIniziale));
            sb.AppendLine(string.Format("Saldo finale: {0}", saldoFinale));

            string oggetto = "PREZIONI - NUOVO MOVIMENTO";
            IDMAIL = bllMD.CreaEmail("PREZIOSI - CASSAFORTE B", oggetto, sb.ToString());

            bllMD.SottomettiEmail(IDMAIL);
        }

        public bool SalvaMovimentoPreziosoCassaforteB(int IdPrezioso, string Operazione, decimal Quantita, string Causale, string ConnectedUser)
        {
            try
            {
                PreziosiDS ds = new PreziosiDS();
                using (PreziosiBusiness bPreziosi = new PreziosiBusiness())
                {
                    bPreziosi.FillRW_MOVIMENTI_PREZIOSI(ds);
                    bPreziosi.FillRW_PREZIOSI(ds);

                    decimal saldoA = GetUltimoSaldo(IdPrezioso, "A", ds);
                    decimal saldoB = GetUltimoSaldo(IdPrezioso, "B", ds);
                    string prezioso = ds.RW_PREZIOSI.Where(x => x.IDPREZIOSO == IdPrezioso).Select(x => x.MATERIALE).FirstOrDefault();

                    if (Operazione == "P")
                    {

                        if (saldoB < Quantita) return false;
                        decimal nuovoSaldoB = saldoB - Quantita;

                        PreziosiDS.RW_MOVIMENTI_PREZIOSIRow movimentoB = ds.RW_MOVIMENTI_PREZIOSI.NewRW_MOVIMENTI_PREZIOSIRow();
                        movimentoB.CASSAFORTE = "B";
                        movimentoB.DATA = DateTime.Now;
                        movimentoB.IDPREZIOSO = IdPrezioso;
                        movimentoB.NOTA = Causale;
                        movimentoB.QUANTITA = -1 * Quantita;
                        movimentoB.SALDOFINALE = nuovoSaldoB;
                        movimentoB.SALDOINIZIALE = saldoB;
                        movimentoB.UTENTE = ConnectedUser;
                        ds.RW_MOVIMENTI_PREZIOSI.AddRW_MOVIMENTI_PREZIOSIRow(movimentoB);

                        bPreziosi.UpdatePreziosiDS(ds.RW_MOVIMENTI_PREZIOSI.TableName, ds);
                        InviaMailCassaforteB(-1 * Quantita, Causale, ConnectedUser, prezioso,saldoB,nuovoSaldoB);

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

    }
}
