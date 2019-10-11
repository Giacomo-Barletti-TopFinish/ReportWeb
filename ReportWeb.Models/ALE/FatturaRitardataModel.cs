using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{
    public class FattureRitardateModel
    {
        public List<FatturaRitardataModel> FattureRitardate { get; set; }
    }

    public class FatturaRitardataModel
    {
        public decimal IDRIPGRATUITA { get; set;}
        public string ODL { get; set; }
        public DateTime DATA_CREAZIONE { get; set; }
        public string UIDUSER_INSERIMENTO {get; set;}

        public string LAVORANTE {get; set;}
        public DateTime DATA_SCADENZA { get; set; }
        public string NASCONDI {get; set;}        
    }
}
