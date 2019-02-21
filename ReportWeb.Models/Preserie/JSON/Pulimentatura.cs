using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class PulimentaturaJson
    {
        [DataMember(Name = "Lavorazione")]
        public string Lavorazione { get; set; }

        [DataMember(Name = "Automatico")]
        public string Automatico { get; set; }

        [DataMember(Name = "ParteLavorata")]
        public string ParteLavorata { get; set; }

        [DataMember(Name = "Spazzole")]
        public string Spazzole { get; set; }

        [DataMember(Name = "Paste")]
        public string Paste { get; set; }

    }
}
