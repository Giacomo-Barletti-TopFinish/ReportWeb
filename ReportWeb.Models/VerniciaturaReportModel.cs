using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class VerniciaturaReportModel
    {
        public List<VerniciaturaConsuntivoModel> Consuntivo { get; set; }
        public string QuantitaManualeTotale { get; set; }
        public string BarreTotali { get; set; }
    }
}
