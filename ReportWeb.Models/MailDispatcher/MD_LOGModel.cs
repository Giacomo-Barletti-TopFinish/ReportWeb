using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.MailDispatcher
{
    public class MD_LOGModel
    {
        public decimal IDMAIL { get; set; }
        public DateTime DataOperazione { get; set; }
        public string TipoOperazione { get; set; }
        public string Nota { get; set; }
    }
}
