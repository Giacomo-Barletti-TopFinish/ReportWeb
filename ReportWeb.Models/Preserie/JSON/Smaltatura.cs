using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class SmaltaturaJson
    {
        [DataMember(Name = "Piazzatura")]
        public string Piazzatura { get; set; }

        [DataMember(Name = "Smalto")]
        public string Smalto { get; set; }

        [DataMember(Name = "Codice")]
        public string Codice{ get; set; }

    }
}
