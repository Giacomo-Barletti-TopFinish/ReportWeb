using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class GalvanicaJson
    {
        [DataMember(Name = "Legatura")]
        public string Legatura { get; set; }

        [DataMember(Name = "Telaio")]
        public string Telaio { get; set; }

        [DataMember(Name = "PezziFilo")]
        public decimal PezziFilo { get; set; }

        [DataMember(Name = "FiliTealio")]
        public decimal FiliTealio { get; set; }

        [DataMember(Name = "Spessore")]
        public string Spessore { get; set; }

    }
}
