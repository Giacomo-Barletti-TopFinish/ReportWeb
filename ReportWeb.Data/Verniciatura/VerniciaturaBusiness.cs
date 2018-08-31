using MetalPlus.Kernel.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Verniciatura
{

    public class VerniciaturaBusiness : ReportWebBusinessBase
    {
        public VerniciaturaBusiness() : base() { }

        [DataContext]
        public void FillRW_VERNICIATURA_CONSUNTIVO(VerniciaturaDS ds)
        {
            VerniciaturaAdapter a = new VerniciaturaAdapter(DbConnection, DbTransaction);
            a.FillRW_VERNICIATURA_CONSUNTIVO(ds);
        }

        [DataContext(true)]
        public void CancellaRigaConsuntivo(int IdConsuntivo)
        {
            VerniciaturaAdapter a = new VerniciaturaAdapter(DbConnection, DbTransaction);
            a.CancellaRigaConsuntivo(IdConsuntivo);
        }

        [DataContext(true)]
        public void SalvaConsuntivo(string Giorno, int QuantitaManuale, int Barre, string UIDUSER)
        {
            VerniciaturaAdapter a = new VerniciaturaAdapter(DbConnection, DbTransaction);
            a.SalvaConsuntivo(Giorno, QuantitaManuale, Barre, UIDUSER);
        }

    }
}
