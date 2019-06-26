using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preziosi
{
    public class Movimenti
    {
        public DateTime Giorno { get; set; }
        public string Dare { get; set; }
        public string Avere{ get; set; }
        public string Causale { get; set; }
        public string Cassaforte { get; set; }
        public string Materiale { get; set; }
        public string Utente { get; set; }
    }
}
