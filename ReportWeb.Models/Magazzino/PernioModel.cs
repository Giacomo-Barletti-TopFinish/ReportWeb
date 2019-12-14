using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Magazzino
{
    public class PernioModel
    {
        public decimal IdPosizPerno { get; set; }
        public string Cliente { get; set; }
        public string Posizione { get; set; }
        public string Articolo { get; set; }
        public string CodiceInterno { get; set; }
        public string ProgressivoStampo { get; set; }
        public string Componente { get; set; }
        public string Descrizione{ get; set; }
        public decimal Diametro{ get; set; }
        public decimal Lunghezza{ get; set; }
        public decimal Quantita{ get; set; }
        public decimal GiacenzaMinima{ get; set; }
    }
}
