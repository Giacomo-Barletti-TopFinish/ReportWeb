using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class ODLApertiModel
    {
        public string Azienda { get; set; }
        public string Fase { get; set; }
        public string Articolo { get; set; }
        public string Wip { get; set; }
        public decimal? QtaTotale { get; set; }
        public decimal? QtaDaTerminare { get; set; }
        public string Segnalatore { get; set; }
        public string Brand { get; set; }
        public string ODL { get; set; }
        public string Commessa { get; set; }
        public DateTime? DataFine { get; set; }
        public int Priority { get; set; }
        public DateTime? DataCreazione { get; set; }
    }
}
