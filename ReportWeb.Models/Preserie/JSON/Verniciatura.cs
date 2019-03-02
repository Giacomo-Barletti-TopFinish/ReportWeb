using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie.JSON
{
    [DataContract]
    public class VerniciaturaJson
    {
        [DataMember(Name = "Telaio")]
        public string Telaio { get; set; }

        [DataMember(Name = "PezziTelaio")]
        public decimal PezziTelaio { get; set; }

        [DataMember(Name = "Durata")]
        public string Durata { get; set; }

        [DataMember(Name = "Ricetta")]
        public string Ricetta { get; set; }
    }
}
