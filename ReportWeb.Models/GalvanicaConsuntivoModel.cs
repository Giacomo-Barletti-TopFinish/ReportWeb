using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class GalvanicaConsuntivoModel
    {
        public int IdConsuntivo { get; set; }
        public DateTime InizioTurno { get; set; }
        public DateTime FineTurno { get; set; }
        public int Barre { get; set; }
        public List<FermoModel> Fermi { get; set; }
        public TimeSpan FermoTotale { get; set; }
        public TimeSpan Durata { get; set; }
        public TimeSpan DurataEffettiva { get; set; }
        public decimal BarreHH{ get; set; }
        public decimal MinBarre { get; set; }
        public string UIDUSER { get; set; }
    }

    public class FermoModel
    {
        public int IdFermo { get; set; }
        public int IdConsuntivo { get; set; }
        public string Tipo { get; set; }
        public string Ora { get; set; }
        public string Durata { get; set; }
        public string Motivo { get; set; }
    }
}
