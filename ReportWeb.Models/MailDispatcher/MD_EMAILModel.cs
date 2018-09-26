using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.MailDispatcher
{
    public class MD_EMAILModel
    {
        public decimal IDMAIL { get; set; }
        public decimal Tentativo { get; set; }
        public decimal IdRichiedente { get; set; }
        public string Richiedente { get; set; }
        public DateTime DataCreazione { get; set; }
        public string Stato { get; set; }
        public string Oggetto { get; set; }
    }
}
