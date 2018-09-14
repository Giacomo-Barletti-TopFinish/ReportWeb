using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.JSON
{
    [DataContract]
    public class ALEValorizzaJson
    {
        [DataMember(Name = "IdAleDettaglio")]
        public decimal IdAleDettaglio { get; set; }

        [DataMember(Name = "Prezzo")]
        public decimal? Prezzo{ get; set; }

        [DataMember(Name = "Costo")]
        public decimal? Costo { get; set; }

        [DataMember(Name = "Nota")]
        public string Nota { get; set; }

        [DataMember(Name = "CostiFase")]
        public CostoFaseJson[] CostiFase { get; set; }
    }


    [DataContract]
    public class CostoFaseJson
    {
        [DataMember(Name = "IdAleDettaglio")]
        public decimal IdAleDettaglio { get; set; }

        [DataMember(Name = "Fase")]
        public string Fase { get; set; }

        [DataMember(Name = "Costo")]
        public decimal? Costo { get; set; }

    }
}
