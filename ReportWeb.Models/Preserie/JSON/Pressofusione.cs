using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class PressofusioneJson
    {
        [DataMember(Name = "TipoStampo")]
        public string TipoStampo { get; set; }

        [DataMember(Name = "CodiceStampo")]
        public string CodiceStampo { get; set; }

        [DataMember(Name = "Impronte")]
        public decimal Impronte { get; set; }

        [DataMember(Name = "Batture")]
        public decimal Batture { get; set; }

        [DataMember(Name = "Materiale")]
        public string Materiale { get; set; }

    }
}
