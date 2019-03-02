using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class TornituraJson
    {
        [DataMember(Name = "Macchina")]
        public string Macchina { get; set; }

        [DataMember(Name = "Materiale")]
        public string Materiale{ get; set; }

        [DataMember(Name = "Diametro")]
        public decimal Diametro { get; set; }

        [DataMember(Name = "Utensile")]
        public string Utensile { get; set; }

    }
}
