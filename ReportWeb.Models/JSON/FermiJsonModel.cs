using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.JSON
{
 
    [DataContract]
    public class FermiJsonModel
    {
        [DataMember(Name = "Tipo")]
        public string Tipo { get; set; }

        [DataMember(Name = "Ora")]
        public string Ora { get; set; }

        [DataMember(Name = "Durata")]
        public string Durata { get; set; }

        [DataMember(Name = "Motivo")]
        public string Motivo { get; set; }
    }
}
