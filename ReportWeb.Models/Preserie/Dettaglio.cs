using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie
{
    public abstract class DettaglioBase
    {
        public decimal PezziOra;
        public string Lavorazione;
        public string Nota;

        public abstract string Contenuto();

    }
}
