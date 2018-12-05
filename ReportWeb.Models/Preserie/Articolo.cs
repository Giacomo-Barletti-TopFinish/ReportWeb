using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie
{
    public class Articolo
    {
        public string IDMAGAZZ;
        public string Modello;
        public string Descrizione;

        public override string ToString()
        {
            return string.Format("{0} - {1}", Modello, Descrizione);
        }
    }
}
