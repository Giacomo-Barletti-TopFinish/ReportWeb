using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{

    public class ValorizzatoModel:AddebitoModel
    {
        public string NotaValorizzazione { get; set; }
        public decimal Prezzo{ get; set; }
    }
}
