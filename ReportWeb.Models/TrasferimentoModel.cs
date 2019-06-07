using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class TrasferimentoModel
    {
        public string DataPartenza { get; set; }
        public string OperatorePartenza { get; set; }
        public string DataArrivo { get; set; }
        public string OperatoreArrivo { get; set; }
        public string NumMovFase { get; set; }
        public string Reparto { get; set; }
        public string Modello { get; set; }
    }
}
