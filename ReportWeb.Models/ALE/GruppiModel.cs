using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{

    public class GruppoModel
    {
        public List<AddebitoModel> Dettagli { get; set; }
        public string NotaAddebito { get; set; }
        public string LavoranteCodice { get; set; }
        public string LavoranteDescrizione { get; set; }
        public bool Aperto { get; set; }
        public decimal IDALEGRUPPO { get; set; }
        public bool AddebitoAnnulabile{ get; set; }
    }

    public class GruppoValorizzatoModel
    {
        public List<ValorizzatoModel> Dettagli { get; set; }
        public string NotaValorizzazione { get; set; }
        public string LavoranteCodice { get; set; }
        public string LavoranteDescrizione { get; set; }
        public bool Aperto { get; set; }
        public decimal IDALEGRUPPO { get; set; }
        public bool ValorizzazioneAnnullabile { get; set; }
        public decimal ValoreTotale{ get; set; }
    }

    public class GruppoApprovatoModel
    {
        public List<ApprovatoModel> Dettagli { get; set; }
        public string NotaApprovazione { get; set; }
        public string LavoranteCodice { get; set; }
        public string LavoranteDescrizione { get; set; }
        public bool Aperto { get; set; }
        public decimal IDALEGRUPPO { get; set; }
        public bool ApprovazioneAnnullabile { get; set; }
        public decimal ValoreTotale { get; set; }
        public decimal ValoreApprovatoTotale { get; set; }
    }

    public class GruppoFatturatoModel
    {
        public List<FatturatoModel> Dettagli { get; set; }
        public string NotaFatturazione { get; set; }
        public string LavoranteCodice { get; set; }
        public string LavoranteDescrizione { get; set; }
        public bool Aperto { get; set; }
        public decimal IDALEGRUPPO { get; set; }
        public bool FatturazioneAnnullabile { get; set; }
        public decimal ValoreApprovatoTotale { get; set; }
    }
}
