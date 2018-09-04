using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{
    public class InserimentoModel
    {
        public bool NonTrovato { get; set; }

        public string IDCHECKQT { get; set; }

        public string NumeroDocumento { get; set; }
        public string Reparto { get; set; }
        public string Modello { get; set; }
        public string ModelloDescrizione { get; set; }
        public string ModelloProdottoFinito { get; set; }
        public string ModelloProdottoFinitoDescrizione { get; set; }
        public decimal Quantita { get; set; }
        public decimal QuantitaDifforme { get; set; }
        public string TipoDifetto { get; set; }
        public string Difetto { get; set; }

        public List<RWListItem> LavorantiEsterni { get; set; }
    }
}
