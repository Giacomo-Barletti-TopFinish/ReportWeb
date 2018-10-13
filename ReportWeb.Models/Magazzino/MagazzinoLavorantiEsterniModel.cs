using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Magazzino
{
    public class MagazzinoLavorantiEsterniModel
    {
        public string IdMOdello { get; set; }
        public string Modello { get; set; }
        public string ModelloDescrizione { get; set; }
        public Decimal Quanita { get; set; }
        public Decimal Peso { get; set; }
        public string IdComponente { get; set; }
        public string Componente { get; set; }
        public string ComponenteDescrizione { get; set; }
        public Decimal QuanitaComponente { get; set; }
        public Decimal PesoComponente { get; set; }
        public string  DataInizio { get; set; }
        public string DataFine{ get; set; }
    }
}
