using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.RvlDocumenti
{
    public class PrdMovFasiModel
    {
        public string IDPRDMOVFASE { get; set; }
        public string NumeroMovimentoFase { get; set; }
        public string Modello { get; set; }
        public decimal Quantita { get; set; }

        public List<BollaCaricoModel> Acquisti { get; set; }
    }
}
