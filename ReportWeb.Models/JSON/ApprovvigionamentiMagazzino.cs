using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.JSON
{
    [DataContract]
    public class ApprovvigionamentoMagazzino
    {
        [DataMember(Name = "Checked")]
        public bool Checked { get; set; }

        [DataMember(Name = "IDMAGAZZ")]
        public string IDMAGAZZ { get; set; }

        [DataMember(Name = "DataScadenza")]
        public string DataScadenza { get; set; }

        [DataMember(Name = "Nota")]
        public string Nota { get; set; }
    }
}
