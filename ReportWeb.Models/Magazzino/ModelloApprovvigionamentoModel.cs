using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Magazzino
{
    public class ModelloApprovvigionamentoModel
    {
        public string IDMAGAZZ { get; set; }
        public string Modello { get; set; }
        public string Descrizione { get; set; }
        public string Nota { get; set; }
        public string DataScadenza{ get; set; }
        public bool Presente { get; set; }
    }
}
