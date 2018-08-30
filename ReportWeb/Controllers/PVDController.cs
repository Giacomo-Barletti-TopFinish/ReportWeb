using ReportWeb.Business;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportWeb.Controllers
{
    public class PVDController : ControllerBase
    {
        // GET: PVD
        public ActionResult Consuntivo()
        {
            PVDBLL bll = new PVDBLL();
            List<RWListItem> macchine = bll.CreaListaMacchine();
            macchine.Insert(0, new RWListItem(string.Empty, string.Empty));
            ViewData.Add("macchine", macchine);
            return View();
        }

        public ActionResult GetGrigliaMacchine(string IDRESOURCEF)
        {
            PVDBLL bll = new PVDBLL();
            List<PVDConsuntivoModel> consunetivo = bll.EstraiConsutivoMacchina(IDRESOURCEF);
            return PartialView("GetGrigliaMacchinePartial", consunetivo);
        }

        public ActionResult CancellaConsuntivo(int IdConsuntivo, string IDRESOURCEF)
        {
            PVDBLL bll = new PVDBLL();
            bll.CancellaRigaConsuntivo(IdConsuntivo);
            List<PVDConsuntivoModel> consunetivo = bll.EstraiConsutivoMacchina(IDRESOURCEF);
            return PartialView("GetGrigliaMacchinePartial", consunetivo);
        }

        public ActionResult SalvaConsuntivo(string IDRESOURCEF, string FinituraCodice, string FinituraDescrizione, string Tipo, string Giorno, string Inizio, string Fine, int Quantita, string Clienti, string Articolo, int Impegno)
        {
            PVDBLL bll = new PVDBLL();
            bll.SalvaConsuntivo(IDRESOURCEF, FinituraCodice, FinituraDescrizione, Tipo, Giorno, Inizio, Fine, Quantita, Clienti, Articolo, Impegno, ConnectedUser);
            List<PVDConsuntivoModel> consunetivo = bll.EstraiConsutivoMacchina(IDRESOURCEF);
            return PartialView("GetGrigliaMacchinePartial", consunetivo);
        }
    }
}