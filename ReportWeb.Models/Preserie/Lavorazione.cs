using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Models.Preserie
{
    public class Lavorazione
    {
        public string IDPRDFASE;
        public int Sequenza;
        public string IDMAGAZZ;
        public string Reparto;
        public decimal Quantita;
        public decimal QuantitaNetta;
        public decimal QuantitaTerminata;
        public decimal QuantitaDaTerminare;

        public string DataInizio;
        public string DatFine;
        public decimal Offset;
        public decimal Leadtime;
        List<DettaglioBase> Dettagli;

    }

    public class ODL
    {
        public string IDPRDMOVFASE;
        public string NUMMOVFASE;
        public string DATAMOVFASE;
        public string Barcode;
        public string Reparto;
        public decimal Quantita;
        public decimal QuantitaNetta;
        public decimal QuantitaTerminata;
        public decimal QuantitaDaTerminare;
    }
}
