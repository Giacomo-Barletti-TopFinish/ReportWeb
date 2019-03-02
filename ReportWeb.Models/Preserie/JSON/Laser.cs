using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class LaseraturaJson
    {
        [DataMember(Name = "TipoLaseratura")]
        public string TipoLaseratura { get; set; }

        [DataMember(Name = "Parametri")]
        public string Parametri { get; set; }

        [DataMember(Name = "Piazzatura")]
        public string Piazzatura { get; set; }

        [DataMember(Name = "Magazzino")]
        public string Magazzino { get; set; }

        [DataMember(Name = "Laser")]
        public string Laser { get; set; }
    }
}
