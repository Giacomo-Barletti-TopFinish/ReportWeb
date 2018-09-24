using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.MailDispatcher
{
    public class MD_GRUPPOModel
    {
        public decimal IDGRUPPO { get; set; }
        public string Nome { get; set; }
        public List<MD_GRUPPO_DESTINATARIOModel> Destinatari { get; set; }
    }

    public class MD_GRUPPO_DESTINATARIOModel
    {
        public decimal IDDESTINATARIO { get; set; }
        public string Destinatario { get; set; }
        public decimal IDGRUPPO { get; set; }
    }
    
    public class MD_RICHIEDENTEModel
    {
        public decimal IDRICHIEDENTE { get; set; }
        public string Richiedente { get; set; }
        public List<MD_GRUPPO_RICHIEDENTEModel> GRUPPI { get; set; }
    }
    public class MD_GRUPPO_RICHIEDENTEModel
    {
        public decimal IDGRRICH { get; set; }
        public decimal IDRICHIEDENTE { get; set; }
        public MD_GRUPPOModel Gruppo { get; set; }
        public bool CC { get; set; }
    }
}
