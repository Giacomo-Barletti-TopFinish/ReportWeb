using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{
    public class AddebitiModel
    {
        public List<AddebitoModel> Addebiti { get; set; }
        public List<RWListItem> LavorantiEsterni { get; set; }
    }

    public class AddebitoModel
    {
        public decimal IdAleDettaglio { get; set; }
        public string Barcode { get; set; }
        public string IDCHECKQT { get; set; }
        public string Azienda { get; set; }


        public decimal IdAleGruppo { get; set; }

        public decimal QuantitaDifettosi { get; set; }
        public decimal QuantitaInseriti { get; set; }
        public decimal QuantitaAddebitata { get; set; }
        public string NotaInserimento { get; set; }
        public string NotaAddebito { get; set; }
        public string NotaValorizzazione { get; set; }
        public string NotaApprovazione { get; set; }
        public decimal Prezzo { get; set; }
        public decimal Valore { get; set; }
        public decimal PrezzoApprovato { get; set; }

        public string LavoranteCodice { get; set; }
        public string LavoranteDescrizione { get; set; }

        public string Modello { get; set; }
        public string ModelloDescrizione { get; set; }
        public string TipoDifetto { get; set; }
        public string Difetto { get; set; }
        public string Commessa { get; set; }
        public string DataCommessa { get; set; }

        public string Stato { get; set; }
        public string UidUserInserimento { get; set; }
        public DateTime DataInserimento { get; set; }

        public string UidUserNonAddebito{ get; set; }
        public DateTime? DataNonAddebito { get; set; }

        public string UrlImage { get; set; }

        public List<CostiAddebitiModel> Costi { get; set; }

        public List<FaseCosto> ListaFasi { get; set; }

        public bool SCARTODEFINITIVO { get; set; }
    }


    public class CostiAddebitiModel
    {
        public decimal IdAleDettCosto { get; set; }
        public decimal IdAleDettaglio { get; set; }
        public string Fase { get; set; }
        public decimal CostoFase { get; set; }
    }
}
