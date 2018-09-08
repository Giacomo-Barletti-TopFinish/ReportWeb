using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{
    public class ApprovatoModel : ValorizzatoModel
    {
        public string NotaApprovazione{ get; set; }
        public decimal PrezzoApprovato { get; set; }
    }
}
