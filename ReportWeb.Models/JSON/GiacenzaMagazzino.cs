using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.JSON
{

    [DataContract]
    public class GiacenzaMagazzino
    {
        [DataMember(Name = "Checked")]
        public bool Checked { get; set; }

        [DataMember(Name = "IDMAGAZZ")]
        public string IDMAGAZZ{ get; set; }

        [DataMember(Name = "Giacenza")]
        public decimal Giacenza { get; set; }

    }
}
