using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie
{
    public class Commessa
    {
        public string IDLANCIOT;
        public string IDLANCIOD;
        public Articolo Articolo;
        public decimal Quantita;
        public string DataInizio;
        public string DataFine;
        public string DataCommessa;
        public string NomeCommessa;
        public string Riferimento;
        public List<Lavorazione> Lavorazioni;
    }
}
