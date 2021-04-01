using ReportWeb.Business;
using ReportWeb.Common.Helpers;
using ReportWeb.Models;
using ReportWeb.Models.Preziosi;
using ReportWeb.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class PreziosiController : ControllerBase
    {
        // GET: Preziosi
        public ActionResult INSERIMENTOCASSAFORTEA()
        {
            VerificaAbilitazioneUtenteConUscita(43);
            PreziosiBLL bll = new PreziosiBLL();
            List<RWListItem> preziosi = bll.CreaListaPreziosi();
            List<RWListItem> dareAvere = CreaListaMenuDareAvere();
            ViewData.Add("preziosi", preziosi);
            ViewData.Add("dareAvere", dareAvere);
            return View();
        }

        public ActionResult INSERIMENTOCASSAFORTEB()
        {
            VerificaAbilitazioneUtenteConUscita(44);
            PreziosiBLL bll = new PreziosiBLL();
            List<RWListItem> preziosi = bll.CreaListaPreziosi();
            ViewData.Add("preziosi", preziosi);
            return View();
        }

        public ActionResult MOVIMENTI()
        {
            VerificaAbilitazioneUtenteConUscita(45);
            PreziosiBLL bll = new PreziosiBLL();
            List<RWListItem> preziosi = bll.CreaListaPreziosi();
            ViewData.Add("preziosi", preziosi);
            return View();
        }

        public ActionResult GetSaldoMateriale(int IdPrezioso)
        {
            PreziosiBLL bll = new PreziosiBLL();
            Tuple<string, string> t = bll.GetSaldoMateriale(IdPrezioso);
            return Json(t);
        }
        public List<RWListItem> CreaListaMenuDareAvere()
        {
            List<RWListItem> dareAvere = new List<RWListItem>();
            dareAvere.Add(new RWListItem(string.Empty, "-1"));
            dareAvere.Add(new RWListItem("Versamento", "V"));
            dareAvere.Add(new RWListItem("Prelievo", "P"));

            return dareAvere;
        }

        public ActionResult SalvaMovimentoPreziosoCassaforteA(int IdPrezioso, string Operazione, string Quantita, string Causale)
        {
            decimal quantita = decimal.Parse(Quantita, System.Globalization.CultureInfo.InvariantCulture);
            PreziosiBLL bll = new PreziosiBLL();
            bool esito = bll.SalvaMovimentoPreziosoCassaforteA(IdPrezioso, Operazione, quantita, Causale, ConnectedUser);
            return Content(esito.ToString());
        }

        public ActionResult SalvaMovimentoPreziosoCassaforteB(int IdPrezioso, string Operazione, string Quantita, string Causale)
        {
            decimal quantita = decimal.Parse(Quantita, System.Globalization.CultureInfo.InvariantCulture);
            PreziosiBLL bll = new PreziosiBLL();
            bool esito = bll.SalvaMovimentoPreziosoCassaforteB(IdPrezioso, Operazione, quantita, Causale, ConnectedUser);
            return Content(esito.ToString());
        }

        public ActionResult CaricaSaldiCasseforti()
        {
            PreziosiBLL bll = new PreziosiBLL();
            List<SaldoCasseforti> saldi = bll.GetSaldiCompleti();

            return PartialView("SaldiCassefortiPartial", saldi);
        }

        public ActionResult CaricaMovimenti(string DataInizio, string DataFine, int IdPrezioso)
        {
            PreziosiBLL bll = new PreziosiBLL();
            List<Movimenti> movimenti = bll.CaricaMovimenti(DataInizio, DataFine, IdPrezioso);

            return PartialView("CaricaMovimentiPartial", movimenti);
        }

        public ActionResult ReportPDF(string Tipo, string DataInizio, string DataFine, int IdPrezioso)
        {
            PreziosiBLL bll = new PreziosiBLL();
            List<Movimenti> movimenti = bll.CaricaMovimenti(DataInizio, DataFine, IdPrezioso);
            List<RWListItem> preziosi = bll.CreaListaPreziosi();
            List<SaldoCasseforti> saldi = bll.GetSaldiCompleti();

            if (Tipo == "PDF")
            {
                PDFHelper pdfHelper = new PDFHelper();
                byte[] fileContents = pdfHelper.EstraiMovimentiPreziosi(movimenti, saldi, DataInizio, DataFine);

                return File(fileContents, "application/pdf", "MovimentiPreziosi.pdf");
            }
            if (Tipo == "EXCEL")
            {

                ExcelHelper excelHelper = new ExcelHelper();
                byte[] fileContents = excelHelper.EstraiMovimentiPreziosi(movimenti, saldi, DataInizio, DataFine);

                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MovimentiPreziosi.xlsx");
            }

            throw new ArgumentException("ERRORE TIPO ESTRAZIONE NON VALIDA");

        }
    }
}