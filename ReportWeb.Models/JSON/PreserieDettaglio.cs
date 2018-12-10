using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ReportWeb.Views.Preserie
{
    [DataContract]
    public class PreserieDettaglio
    {
        [DataMember(Name = "Fase")]
        public string Fase { get; set; }

        [DataMember(Name = "Pezzi")]
        public string Pezzi { get; set; }

        [DataMember(Name = "Lavorante")]
        public string Lavorante { get; set; }

        [DataMember(Name = "Nota")]
        public string Nota { get; set; }

        [DataMember(Name = "IDDETTAGLIO")]
        public decimal IDDETTAGLIO { get; set; }
    }
}
