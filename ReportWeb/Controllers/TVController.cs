using ReportWeb.BLL;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportWeb.Common;
using ReportWeb.Business;

namespace ReportWeb.Controllers
{
    public class TVController : ControllerBase
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reparto(string Reparto)
        {

            if (string.IsNullOrEmpty(Reparto)) RedirectToAction("Index", "Home");
            ViewData.Add("Reparto", Reparto);
            List<ODLApertiModel> model = ODLHelper.FillODLAperti(Reparto);

            decimal qtaBassa = model.Where(x => x.Priority == (int)Priorità.Bassa).Sum(x => x.QtaDaTerminare.HasValue ? x.QtaDaTerminare.Value : 0);
            decimal qtaMedia = model.Where(x => x.Priority == (int)Priorità.Media).Sum(x => x.QtaDaTerminare.HasValue ? x.QtaDaTerminare.Value : 0);
            decimal qtaAlta = model.Where(x => x.Priority == (int)Priorità.Alta).Sum(x => x.QtaDaTerminare.HasValue ? x.QtaDaTerminare.Value : 0);
            decimal qtaTotale = qtaBassa + qtaMedia + qtaAlta;

            decimal percAlta = qtaTotale == 0 ? 0 : Math.Round(qtaAlta * 100 / qtaTotale, 0);
            decimal percMedia = qtaTotale == 0 ? 0 : Math.Round(qtaMedia * 100 / qtaTotale, 0);
            decimal percBassa = 100 - percMedia - percAlta;

            ViewData.Add("qtaBassa", string.Format("pz: {0} ({1}%)", qtaBassa, percBassa));
            ViewData.Add("qtaMedia", string.Format("pz: {0} ({1}%)", qtaMedia, percMedia));
            ViewData.Add("qtaAlta", string.Format("pz: {0} ({1}%)", qtaAlta, percAlta));

            string etichetta = Reparti.LeggiEtichetta(Reparto);
            ViewData.Add("Titolo", etichetta);
            ViewData.Add("TimeoutChangePage", LongTimeoutChangePage);

            return View(model);
        }

        public ActionResult Quadranti(string Reparto)
        {
            if (string.IsNullOrEmpty(Reparto)) RedirectToAction("Index", "Home");
            ViewData.Add("Reparto", Reparto);
            QuadrantiModel model = ODLHelper.GetDatiPerQuadranti(Reparto);

            string etichetta = Reparti.LeggiEtichetta(Reparto);
            ViewData.Add("Titolo", etichetta);

            ViewData.Add("TimeoutChangePage", ShortTimeoutChangePage);
            return View(model);
        }

        public ActionResult Video(string Reparto)
        {
            if (string.IsNullOrEmpty(Reparto))  return RedirectToAction("Index", "TV");
            VideoBLL bll = new VideoBLL();            
            string Path = bll.LeggiVideo(Reparto);
            //Path = Server.MapPath(Path);

            ViewData.Add("video", Path);
            
            return View();
        }
    }
}