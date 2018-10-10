using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.RvlDocumenti
{
    public class BollaCaricoDettaglioModel
    {
        public string IDACQUISTID { get; set; }
        public string Modello { get; set; }
        public decimal Quantita{ get; set; }
        public string Causale { get; set; }
    }
}
