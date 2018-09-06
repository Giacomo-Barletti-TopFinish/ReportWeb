using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{
    public class AddebitoModel
    {
        public decimal IdAleDettaglio { get; set; }
        public decimal IdAleGruppo { get; set; }
        public decimal QuantitaDifettosi { get; set; }
        public decimal QuantitaInseriti { get; set; }
        public decimal QuantitaAddebitata { get; set; }
        public string Nota{ get; set; }
        public string NotaAddebito { get; set; }
        public string LavoranteCodice{ get; set; }
        public string LavoranteDescrizione { get; set; }
        public string Modello { get; set; }
        public string ModelloDescrizione { get; set; }
        public string TipoDifetto { get; set; }
        public string Difetto { get; set; }

    }

    public class AddebitiModel
    {
        public List<AddebitoModel> Addebiti{ get; set; }
        public List<RWListItem> LavorantiEsterni{ get; set; }

    }
}
