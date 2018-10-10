using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.RvlDocumenti
{
    public class BollaVenditaModel
    {
        public string IDVENDITET { get; set; }
        public string NumeroDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public decimal Anno { get; set; }
        public string Data { get; set; }
        public string Cliente { get; set; }
        public string FullNumDoc { get; set; }
        public string Azienda { get; set; }

        public List<PrdMovFasiModel> PRDMOVFASI { get; set; }
        public List<BollaVenditaDettaglioModel> Dettagli { get; set; }
    }
}
