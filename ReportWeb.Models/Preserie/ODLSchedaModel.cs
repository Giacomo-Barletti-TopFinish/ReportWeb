using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie
{
    public class ODLSchedaModel
    {
        public string ImageUrl;
        public string Barcode { get; set; }
        public string NumeroDocumento { get; set; }
        public string DataDocumento { get; set; }
        public string Reparto { get; set; }
        public string RepartoCodice { get; set; }
        public string Modello { get; set; }
        public string ModelloDescrizione { get; set; }
        public decimal Quantita { get; set; }
        public string Commessa { get; set; }
        public string DataCommessa { get; set; }
        public int EsitoRicerca { get; set; }
        public string IDPRDMOVFASE { get; set; }
        public string Riferimento { get; set; }

        public string ModelloFinale { get; set; }
        public string ModelloFinaleDescrizione { get; set; }

        public List<Dettaglio> Dettagli { get; set; }
    }
}
