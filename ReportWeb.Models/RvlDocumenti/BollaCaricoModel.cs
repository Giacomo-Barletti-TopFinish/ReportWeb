using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.RvlDocumenti
{
    public class BollaCaricoModel
    {
        public string IDACQUISTIT { get; set; }
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }
        public decimal Anno { get; set; }
        public string Data { get; set; }
        public string FullNumDoc { get; set; }
        public string Riferimento { get; set; }
        public string Fornitore { get; set; }
    }
}
