using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class StampaggioJson
    {
        [DataMember(Name = "TipoMateriale")]
        public string TipoMateriale { get; set; }

        [DataMember(Name = "Materiale")]
        public string Materiale { get; set; }

        [DataMember(Name = "Lunghezza")]
        public decimal Lunghezza { get; set; }

        [DataMember(Name = "Larghezza")]
        public decimal Larghezza { get; set; }

        [DataMember(Name = "Altezza")]
        public decimal Altezza { get; set; }

        [DataMember(Name = "Stampo")]
        public string Stampo { get; set; }

        [DataMember(Name = "Impronte")]
        public decimal Impronte { get; set; }

        [DataMember(Name = "Battute")]
        public decimal Battute { get; set; }

        [DataMember(Name = "Tranciature")]
        public decimal Tranciature { get; set; }

        [DataMember(Name = "Trancia1")]
        public decimal Trancia1 { get; set; }

        [DataMember(Name = "Trancia2")]
        public decimal Trancia2 { get; set; }

        [DataMember(Name = "Certificato")]
        public string Certificato { get; set; }
    }
}
