using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class ModelleriaJson
    {
        [DataMember(Name = "Lavorazione")]
        public string Lavorazione { get; set; }

        [DataMember(Name = "Attrezzaggio")]
        public string Attrezzaggio { get; set; }

        [DataMember(Name = "Materiale")]
        public string Materiale{ get; set; }

        [DataMember(Name = "Programma")]
        public string Programma{ get; set; }

        [DataMember(Name = "Macchine")]
        public string Macchina { get; set; }

        [DataMember(Name = "Utensili")]
        public string Utensili { get; set; }
    }
}
