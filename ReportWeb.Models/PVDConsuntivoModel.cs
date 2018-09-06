using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class PVDConsuntivoModel
    {
        public DateTime Giorno { get; set; }
        public string IDRESOURCEF { get; set; }
        public string Macchina{ get; set; }
        public string FinituraCodice { get; set; }
        public string FinituraDescrizione { get; set; }
        public string TipoCiclo { get; set; }
        public string Inizio { get; set; }
        public string Fine { get; set; }
        public int Quantita{ get; set; }
        public string Clienti { get; set; }
        public string Articolo { get; set; }
        public string Impegno { get; set; }
        public int IdConsuntivo{ get; set; }
        public string Durata{ get; set; }
    }
}
