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
            byte[] fileContents = excel.CreaExcelMagazziniEsterni(model, lavorante, DataInizio, DataFine);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MagazziniEsterni.xlsx");
        }

        public ActionResult ReportExcelGiacenze()
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.CaricaGiacenze();
            ExcelHelper excel = new ExcelHelper();
            byte[] fileContents = excel.CreaExcelGiacenzeMagazzino(model);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "GiacenzeMagazzini.xlsx");
        }

        public ActionResult ReportCampionario()
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<MagazzinoCampionarioModel> model = bll.TrovaCampionario(string.Empty, string.Empty, string.Empty, string.Empty);
            ExcelHelper excel = new ExcelHelper();
            byte[] fileContents = excel.CreaExcelCampionario(model);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Campionario.xlsx");
        }

        public ActionResult ReportPosizioneCampionario()
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<PosizioneCampionarioModel> model = bll.TrovaPosizioneCampionario(string.Empty, string.Empty, string.Empty, string.Empty);
            ExcelHelper excel = new ExcelHelper();
            byte[] fileContents = excel.CreaExcelPosizioneCampionario(model);

            return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PosizioneCampionario.xlsx");
        }

        public ActionResult CAMPIONARIO()
        {
            VerificaAbilitazioneUtenteConUscita(52);
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.CaricaGiacenze();
            return View(model);
        }

        public ActionResult MOVIMENTOCAMPIONARIO()
        {
            VerificaAbilitazioneUtenteConUscita(53);
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.CaricaGiacenze();
            return View(model);
        }

        public ActionResult POSIZIONECAMPIONI()
        {
            VerificaAbilitazioneUtenteConUscita(54);
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.CaricaGiacenze();
            return View(model);
        }

        public ActionResult MOVIMENTOPOSIZIONECAMPIONARIO()
        {
            VerificaAbilitazioneUtenteConUscita(55);
            MagazzinoBLL bll = new MagazzinoBLL();
            List<ModelloGiacenzaModel> model = bll.CaricaGiacenze();
            return View(model);
        }
        public ActionResult TrovaCampioni(string Codice, string Finitura, string Piano, string Descrizione)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<MagazzinoCampionarioModel> model = bll.TrovaCampionario(Codice.ToUpper(), Finitura.ToUpper(), Piano.ToUpper(), Descrizione.ToUpper());
            return PartialView("GrigliaMagazzinoCampioniPartial", model);
        }
        public ActionResult TrovaPosizioneCampioni(string Seriale, string Cliente, string Posizione, string Campione)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<PosizioneCampionarioModel> model = bll.TrovaPosizioneCampionario(Campione.ToUpper(), Seriale.ToUpper(), Posizione.ToUpper(), Cliente.ToUpper());
            return PartialView("GrigliaPosizioneCampioniPartial", model);
        }
        public ActionResult SalvaCampioni(string Id, string Codice, string Finitura, string Piano, string Posizione, string Descrizione)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.SalvaCampioni(Id.ToUpper(), Codice.ToUpper(), Finitura.ToUpper(), Piano.ToUpper(), Posizione.ToUpper(), Descrizione.ToUpper(), ConnectedUser);
            List<MagazzinoCampionarioModel> model = bll.TrovaCampionario(Codice.ToUpper(), Finitura.ToUpper(), Piano.ToUpper(), Descrizione.ToUpper());
            return PartialView("GrigliaMagazzinoCampioniPartial", model);
        }

        public ActionResult CancellaCampioni(string Id, string Codice, string Finitura)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.CancellaCampioni(Id.ToUpper(), Codice.ToUpper(), Finitura.ToUpper());
            List<MagazzinoCampionarioModel> model = bll.TrovaCampionario(Codice.ToUpper(), Finitura.ToUpper(), string.Empty, string.Empty);
            return PartialView("GrigliaMagazzinoCampioniPartial", model);
        }

        public ActionResult SalvaPosizioneCampioni(string Id, string Campione, string Posizione, decimal Progressivo, string Seriale, string Cliente)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.SalvaPosizioneCampioni(Id.ToUpper(), Campione.ToUpper(), Posizione.ToUpper(), Progressivo, Seriale.ToUpper(), Cliente.ToUpper(), ConnectedUser);
            List<PosizioneCampionarioModel> model = bll.TrovaPosizioneCampionario(Campione.ToUpper(), Seriale.ToUpper(), Posizione.ToUpper(), Cliente.ToUpper());
            return PartialView("GrigliaPosizioneCampioniPartial", model);
        }

        public ActionResult CancellaPosizioneCampioni(string Id, string Seriale, string Cliente)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.CancellaPosizioneCampioni(Id.ToUpper(), Seriale.ToUpper(), Cliente.ToUpper());
            List<PosizioneCampionarioModel> model = bll.TrovaPosizioneCampionario(string.Empty, Seriale.ToUpper(), string.Empty, Cliente.ToUpper());
            return PartialView("GrigliaPosizioneCampioniPartial", model);
        }

        public ActionResult PERNI()
        {
            VerificaAbilitazioneUtenteConUscita(56);
            MagazzinoBLL bll = new MagazzinoBLL();
            List<PernioModel> model = bll.CaricaPerni();
            return View(model);
        }

        public ActionResult TrovaPerni(string Articolo, string Cliente, string Posizione, string Componente, decimal Lunghezza, decimal Diametro)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            List<PernioModel> model = bll.TrovaPerni(Articolo.ToUpper(), Cliente.ToUpper(), Posizione.ToUpper(), Componente.ToUpper(), Lunghezza, Diametro);
            return PartialView("GrigliaPerni", model);
        }

        public ActionResult SalvaPernio(string Id, string Articolo, string Cliente, string Posizione, string Componente, string Interno, string Stampo, string Descrizione, decimal Diametro, decimal Lunghezza, decimal Quantita, decimal Giacenza)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.SalvaPerno(Id.ToUpper(), Articolo.ToUpper(), Cliente.ToUpper(), Posizione.ToUpper(), Componente.ToUpper(), Interno.ToUpper(), Stampo.ToUpper(), Descrizione.ToUpper(), Diametro, Lunghezza, Quantita, Giacenza, ConnectedUser);
            List<PernioModel> model = bll.TrovaPerni(string.Empty, Cliente.ToUpper(), Posizione.ToUpper(), string.Empty, -1, -1);
            return PartialView("GrigliaPerni", model);
        }

        public ActionResult CancellaPernio(string Id, string Cliente, string Posizione)
        {
            MagazzinoBLL bll = new MagazzinoBLL();
            bll.CancellaPernio(Id.ToUpper(), Cliente.ToUpper(), Posizione.ToUpper());
            List<PernioModel> model = bll.TrovaPerni(string.Empty, Cliente.ToUpper(), Posizione.ToUpper(), string.Empty, -1, -1);
            return PartialView("GrigliaPerni", model);
        }


    }
}