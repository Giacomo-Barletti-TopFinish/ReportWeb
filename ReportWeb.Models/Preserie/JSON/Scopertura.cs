using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class ScoperturaJson
    {
        [DataMember(Name = "Lavorazione")]
        public string Lavorazione { get; set; }

        [DataMember(Name = "Passaggi")]
        public string Passaggi { get; set; }

        [DataMember(Name = "Materiale")]
        public string Materiale{ get; set; }

        [DataMember(Name = "Vibratore")]
        public string Vibratore{ get; set; }

        [DataMember(Name = "Additivi")]
        public string Additivi { get; set; }

        [DataMember(Name = "Pezzi")]
        public decimal Pezzi { get; set; }

        [DataMember(Name = "Tempo")]
        public string Tempo { get; set; }
    }
}
