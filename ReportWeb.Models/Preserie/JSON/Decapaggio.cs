using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class DecapaggioJson
    {
        [DataMember(Name = "Lavorazione")]
        public string Lavorazione { get; set; }

        [DataMember(Name = "Tipologia")]
        public string Tipologia { get; set; }

        [DataMember(Name = "Interno")]
        public string Interno { get; set; }

        [DataMember(Name = "Programma")]
        public string Programma { get; set; }

    }
}
