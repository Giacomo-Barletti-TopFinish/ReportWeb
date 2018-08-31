using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class VerniciaturaConsuntivoModel
    {
        public int IdConsuntivo { get; set; }
        public DateTime Giorno { get; set; }
        public int QuantitaManuale { get; set; }
        public int Barre { get; set; }
    }
}
