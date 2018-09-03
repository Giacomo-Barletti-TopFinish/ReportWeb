using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class PVDReportModel
    {
        public List<PVDConsuntivoModel> Consuntivo { get; set; }
        public string DurataTotale { get; set; }
    }
}
