using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{
    public class InserimentoModel
    {
        public int EsitoRicerca { get; set; }

        public string IDCHECKQT { get; set; }
        public string Barcode { get; set; }

        public string NumeroDocumento { get; set; }
        public string DataDocumento { get; set; }
        public string Reparto { get; set; }
        public string RepartoCodice { get; set; }
        public string Modello { get; set; }
        public string ModelloDescrizione { get; set; }
        public decimal Quantita { get; set; }
        public decimal QuantitaDifforme { get; set; }
        public string TipoDifetto { get; set; }
        public string Difetto { get; set; }
        public string ODL { get; set; }
        public string DataODL { get; set; }
        public string Commessa { get; set; }
        public string DataCommessa { get; set; }
        public string ImageUrl { get; set; }
        public List<RWListItem> LavorantiEsterni { get; set; }
    }
}
