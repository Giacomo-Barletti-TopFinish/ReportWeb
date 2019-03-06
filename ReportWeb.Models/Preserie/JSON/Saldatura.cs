using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class SaldaturaJson
    {
        [DataMember(Name = "Piazzatura")]
        public string Piazzatura { get; set; }

    }
}
