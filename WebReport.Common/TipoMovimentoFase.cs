using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebReport.Common
{
    public static class TipoMovimentoFase
    {
        public const string OrdinePrototipia = "ODP";
        public const string OrdinePreserie = "ODS";
        public const string OrdineProduzione = "ODL";
        public const string OrdineCampionatura = "ODC";
        public const string OrdineRiparazione = "ODR";
        public const string ControlloCollaudo = "CHK";
        public const string OrdineUrgente = "ODU";
        public const string OrdineRiparazioneCommessa = "ORC";
        public const string ResoRiordinoGratuitoFornitore = "RRF";
        public const string OrdineManuale = "ODM";

        public static string LeggiEtichetta(string CodiceTipoovimentoFase)
        {
            switch (CodiceTipoovimentoFase)
            {
                case TipoMovimentoFase.OrdinePrototipia:
                    return "ORDINE DI PROTOTIPIA";
                case TipoMovimentoFase.OrdinePreserie:
                    return "ORDINE DI PRESERIE";
                case TipoMovimentoFase.OrdineCampionatura:
                    return "ORDINE DI CAMPIONATURA";
                case TipoMovimentoFase.OrdineProduzione:
                    return "ORDINE DI PRODUZIONE";
                case TipoMovimentoFase.OrdineRiparazione:
                    return "ORDINE DI RIPARAZIONE";
                case TipoMovimentoFase.ControlloCollaudo:
                    return "CONTROLLO E COLLAUDO";
                case TipoMovimentoFase.OrdineUrgente:
                    return "RDINE PRODUZIONE URGENTE";
                case TipoMovimentoFase.OrdineRiparazioneCommessa:
                    return "ORDINE DI RIPARAZIONE SU COMMESSA";
                case TipoMovimentoFase.ResoRiordinoGratuitoFornitore:
                    return "RESO/RIORDINE GRATUITO A FORNITORE";
                case TipoMovimentoFase.OrdineManuale:
                    return "ORDINE MANUALE";
                default:
                    return string.Empty;
            }
        }
    }

}
