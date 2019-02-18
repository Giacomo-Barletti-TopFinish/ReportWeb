using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class Registrazione
    {
        public decimal IDREGISTRAZIONE { get; set; }
        public decimal Tessera { get; set; }
        public string Nome { get; set; }
        public string Data{ get; set; }
        public string Ditta{ get; set; }
    }
}
