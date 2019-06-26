using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preziosi
{
    public class SaldoCasseforti
    {
        public string Materiale { get; set; }
        public string SaldoA { get; set; }
        public string SaldoB { get; set; }

        public SaldoCasseforti(string Materiale, string SaldoA, string SaldoB)
        {
            this.Materiale = Materiale;
            this.SaldoA = SaldoA;
            this.SaldoB = SaldoB;
        }
    }
}
