using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class MontaggioJson
    {
        [DataMember(Name = "Difficolta")]
        public decimal Difficolta { get; set; }

        [DataMember(Name = "Attrezzi")]
        public string Attrezzi { get; set; }

        [DataMember(Name = "Colle")]
        public string Colle { get; set; }

        [DataMember(Name = "Attesa")]
        public string Attesa { get; set; }

        [DataMember(Name = "Colore")]
        public string Colore { get; set; }

    }
}
