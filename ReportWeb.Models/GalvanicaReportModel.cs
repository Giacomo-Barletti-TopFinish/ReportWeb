using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class GalvanicaReportModel
    {
        public List<GalvanicaConsuntivoModel> Consuntivo { get; set; }
        public int BarreTotali { get; set; }
        public TimeSpan TempoTotale { get; set; }
        public TimeSpan FermoTotale { get; set; }
        public TimeSpan DurataEffettiva { get; set; }
        public decimal BarreHH { get; set; }
        public decimal MinBarre { get; set; }

    }
}
