using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class RipreseJson
    {
        [DataMember(Name = "Lavorazione")]
        public string Lavorazione { get; set; }

        [DataMember(Name = "PezziOrari")]
        public string PezziOrari { get; set; }

        [DataMember(Name = "Piazzatura")]
        public string Piazzatura { get; set; }

        [DataMember(Name = "Utensili")]
        public string Utensili { get; set; }

        [DataMember(Name = "Materiali")]
        public string Materiali { get; set; }

    }
}
