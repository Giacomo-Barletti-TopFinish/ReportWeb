using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class VideoRepartoModel
    {
        public decimal IDVIDEOREPARTO { get; set; }
        public string Reparto { get; set; }
        public string Video { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }        
    }
}
