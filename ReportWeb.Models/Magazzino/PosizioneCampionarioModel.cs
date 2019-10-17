using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Magazzino
{
    public class PosizioneCampionarioModel
    {
        public string Campione { get; set; }
        public string Posizione { get; set; }
        public string Seriale { get; set; }
        public decimal Progressivo { get; set; }
        public string Cliente{ get; set; }
        public decimal IDPOSIZCAMP { get; set; }

    }
}
