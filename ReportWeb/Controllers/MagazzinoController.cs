using ReportWeb.Business;
using ReportWeb.Models;
using ReportWeb.Models.Magazzino;
using ReportWeb.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class MagazzinoController : ControllerBase
    {
        // GET: Magazzino
        public ActionResult Index()
        {
            VerificaAbilitazioneUtenteConUscita(25);
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.CaricaGiacenze();
            return View(model);
        }

        public ActionResult TrovaModello(string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.TrovaModelloGiacenza(Modello);
            return PartialView("TabellaGiacenze", model);
        }

        public ActionResult SalvaGiacenza(string Giacenze, string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.SalvaGiacenze(Giacenze, Modello);
            List<ModelloGiacenzaModel> model = new List<ModelloGiacenzaModel>();
            return PartialView("TabellaGiacenze", model);
        }

        public ActionResult APPROVVIGIONAMENTI()
        {
            VerificaAbilitazioneUtenteConUscita(26);
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloApprovvigionamentoModel> model = bll.CaricaApprovvigionamento();
            return View(model);
        }

        public ActionResult TrovaModelloApprovvigionamenti(string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloApprovvigionamentoModel> model = bll.TrovaModelloApprovvigionamento(Modello);
            return PartialView("TabellaApprovvigionamenti", model);
        }

        public ActionResult SalvaApprovvigionamenti(string Approvvigionamenti, string Modello)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.SalvaApprovvigionamenti(Approvvigionamenti, Modello);
            List<ModelloApprovvigionamentoModel> model = new List<ModelloApprovvigionamentoModel>();// bll.TrovaModelloApprovvigionamento(Modello);
            return PartialView("TabellaApprovvigionamenti", model);
        }

        public ActionResult MagazziniEsterni()
        {
            VerificaAbilitazioneUtenteConUscita(30);
            RvlDocumentiBLL bll = new RvlDocumentiBLL();
            List<RWListItem> fornitori = bll.CaricaListaFornitori(true);
            fornitori.Insert(0, new RWListItem(string.Empty, "-1"));
            ViewData.Add("lavoranti", fornitori);
            return View();
        }

        public ActionResult EstraiDati(string DataInizio, string DataFine, string Lavorante)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<MagazzinoLavorantiEsterniModel> model = bll.EstraiMagazzinoLavorantiEsterni(DataInizio, DataFine, Lavorante);
            return PartialView("MagazzinoEsternoPartial", model);
        }

        public ActionResult ReportExcel(string DataInizio, string DataFine, string Lavorante)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            RvlDocumentiBLL bll2 = new RvlDocumentiBLL();
            List<RWListItem> fornitori = bll2.CaricaListaFornitori(true);
            RWListItem RWlavorante = fornitori.Where(x => x.Value.Trim() == Lavorante).FirstOrDefault();
            string lavorante = string.Empty;
            if (RWlavorante != null)
                lavorante = RWlavorante.Text;
            List<MagazzinoLavorantiEsterniModel> model = bll.EstraiMagazzinoLavorantiEsterni(DataInizio, DataFine, Lavorante);

            ExcelHelper excel = new ExcelHelper();
            byte[] fileContents = excel.CreaExcelMagazziniEsterni(model, lavorante);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MagazziniEsterni.xlsx");
        }
    }
}