using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie
{
    public class FaseLavoroInserita
    {
        public string Costo { get; set; }
        public decimal idElemento { get; set; }
        public string Lavorazione { get; set; }
        public List<Tuple<string, string>> FaseInserita { get; set; }
    }
}
