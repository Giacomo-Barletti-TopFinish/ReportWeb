using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models
{
    public class MenuModel
    {
        public int IdMenu { get; set; }
        public string Etichetta { get; set; }
        public string OnClick { get; set; }
        public string HRef { get; set; }
        public string Font { get; set; }
        public bool Azione { get; set; }
        public bool Abilitato { get; set; }

        public List<MenuModel> MenuFiglio { get; set; }
    }
}
