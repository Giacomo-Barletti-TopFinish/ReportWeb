using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.ALE
{
    public class FaseCosto
    {
        public string Fase { get; set; }
        public string Costo { get; set; }

        public FaseCosto(string fase, string costo)
        {
            this.Fase = fase;
            this.Costo = costo;
        }
    }
}
