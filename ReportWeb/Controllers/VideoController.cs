using ReportWeb.Business;
using ReportWeb.Common.Helpers;
using ReportWeb.Models;
using ReportWeb.Reports;
using ReportWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;



namespace ReportWeb.Controllers
{
    public class VideoController : ControllerBase
    {
        [HttpGet]
        public ActionResult AttivaVideo()
        {

            return View();
        }
        
        [HttpPost]
        public ActionResult AttivaVideo(HttpPostedFileBase file)
        {
            VerificaAbilitazioneUtenteConUscita(47);

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string Path_Default_Folder = "~/Video";
                   
                    string path = Path.Combine(Server.MapPath(Path_Default_Folder), Path.GetFileName(file.FileName));
                    file.SaveAs(path);

                    VideoBLL bll = new VideoBLL();

                    if (bll.SalvaVideoNelDatabase(file.FileName, ConnectedUser))
                    {                        
                        ViewBag.Message = "File caricato con successo";
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERRORE:" + ex.Message.ToString();
                }
            }
            else
            {
                ViewBag.Message = "Occorre specificare un file";
            }
            
            return View();
        }

        [HttpPost]
        public ActionResult CaricaAssociaVideoPartial()
        {
            VideoBLL bll = new VideoBLL();
            List<RWListItem> Video_List = bll.CreaListaVideo();
            List<RWListItem> Reparti_List = Reparti.CreaListaReparti();

            ViewData.Add("video", Video_List);
            ViewData.Add("reparti", Reparti_List);

            return PartialView("AssociaVideoRepartiPartial");


        }

        [HttpPost]
        public ActionResult AssociaVideoReparto(decimal Video, string Reparto, string DataInizio, string DataFine)
        {
            bool esito = true;

            DateTime di = DateTime.ParseExact(DataInizio, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            DateTime df = DateTime.ParseExact(DataFine, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            if (df < di)
            {
                esito = false;
            }

            if (esito)
            {
                VideoBLL bll = new VideoBLL();
                esito = bll.SalvaAssociazioneVideoReparto(Video, Reparto, DataInizio, DataFine, ConnectedUser);               
            }

            return Content(esito.ToString());
        }

        [HttpPost]
        public ActionResult GrigliaVideoRepartoPartial()
        {
            VideoBLL bll = new VideoBLL();
            List<VideoRepartoModel> model = bll.CreaListaVideoRepartoModel();            

            return PartialView("GrigliaVideoRepartoPartial", model);
        }

        [HttpPost]
        public ActionResult CancellaVideoReparto(decimal IDVIDEOREPARTO)
        {
            VideoBLL bll = new VideoBLL();
            bll.CancellaVideoReparto(IDVIDEOREPARTO);

            return null;
        }
    }
}