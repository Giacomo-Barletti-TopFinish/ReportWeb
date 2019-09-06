using ReportWeb.Common;
using ReportWeb.Data.Video;
using ReportWeb.Entities;
using ReportWeb.Models;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ReportWeb.Business
{
    public class VideoBLL
    {
        public bool SalvaVideoNelDatabase(string Filename, string ConnectedUser)
        {
            
            VideoDS ds = new VideoDS();
            using (VideoBusiness bVideo = new VideoBusiness())
            {
                try
                {
                    bVideo.FillRW_VIDEO(ds);

                    if (ds.RW_VIDEO.Where(x => x.NOMEVIDEO == Filename && x.PATHVIDEO ==@"~\Video").Count() == 0)
                    {

                        VideoDS.RW_VIDEORow VideoB = ds.RW_VIDEO.NewRW_VIDEORow();
                        VideoB.NOMEVIDEO = Filename;
                        VideoB.PATHVIDEO = @"~\Video";


                        VideoB.UTENTE = ConnectedUser;
                        VideoB.DATAINSERIMENTO = DateTime.Now;

                        ds.RW_VIDEO.AddRW_VIDEORow(VideoB);
                        bVideo.UpdateVideoDS(ds.RW_VIDEO.TableName, ds);
                    }
                    return true;
                    
                }                    
                catch //(Exception ex)
                {
                    bVideo.Rollback();
                    throw;
                }
            }           
        }
        public List<RWListItem> CreaListaVideo()
        {
            VideoDS ds = new VideoDS();
            List<RWListItem> video = new List<RWListItem>();
            
            using (VideoBusiness bVideo = new VideoBusiness())
            {                
                    bVideo.FillRW_VIDEO(ds);
                    video = ds.RW_VIDEO.Select(x => new RWListItem(x.NOMEVIDEO, x.IDVIDEO.ToString())).ToList();
            }
            
            return video;
        }

        public bool SalvaAssociazioneVideoReparto(decimal video, string reparto, string DataInizio, string DataFine, string ConnectedUser)
        {
            VideoDS ds = new VideoDS();
            using (VideoBusiness bVideo = new VideoBusiness())
            {
                try
                {
                    bVideo.FillRW_VIDEO_REPARTI(ds);

                    VideoDS.RW_VIDEO_REPARTIRow VideoB = ds.RW_VIDEO_REPARTI.NewRW_VIDEO_REPARTIRow();

                    VideoB.IDVIDEO = video;

                    VideoB.REPARTO = reparto;                   
                    VideoB.DATAINSERIMENTO = DateTime.Now;
                    VideoB.UTENTE = ConnectedUser;

                    DateTime di = DateTime.ParseExact(DataInizio + " 00:00:00", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime df = DateTime.ParseExact(DataFine + " 23:59:59", "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                    VideoB.DATAINIZIO = di;
                    VideoB.DATAFINE = df;


                    ds.RW_VIDEO_REPARTI.AddRW_VIDEO_REPARTIRow(VideoB);
                    bVideo.UpdateVideoDS(ds.RW_VIDEO_REPARTI.TableName, ds);

                    return true;
                }
                catch
                {
                    bVideo.Rollback();
                    return false;
                }
            }
        }
        public List<VideoRepartoModel> CreaListaVideoRepartoModel()
        {
            VideoDS ds = new VideoDS();
            List<VideoRepartoModel> model = new List<VideoRepartoModel>();

            using (VideoBusiness bVideo = new VideoBusiness())
            {
                bVideo.FillRW_VIDEO_REPARTI(ds);
                bVideo.FillRW_VIDEO(ds);
                List<RWListItem> Reparti_List = Reparti.CreaListaReparti();

                var VideoRepartoAbilitati = ds.RW_VIDEO_REPARTI.Where(x => x.DATAFINE >= System.DateTime.Today && x.DATAINIZIO <= System.DateTime.Today).ToList();

                foreach (var item in VideoRepartoAbilitati.OrderBy(x => x.DATAINIZIO))
                {                    
                    string NomeVideo = ds.RW_VIDEO.Where(x => x.IDVIDEO == item.IDVIDEO).Select(x => x.NOMEVIDEO).FirstOrDefault();
                    string NomeReparto = Reparti_List.Where(x => x.Value.Trim() == item.REPARTO).Select(x => x.Text).FirstOrDefault();

                    VideoRepartoModel vrm = new VideoRepartoModel();
                    vrm.Video = NomeVideo;
                    vrm.Reparto = NomeReparto;
                    vrm.IDVIDEOREPARTO = item.IDVIDEOREPARTO;
                    vrm.DataInizio = item.DATAINIZIO;
                    vrm.DataFine = item.DATAFINE;

                    model.Add(vrm);
                }


            }

            return model;
        }

        public void CancellaVideoReparto(decimal IDVIDEOREPARTO)
        {
            using (VideoBusiness bVideo = new VideoBusiness())
            {
                VideoDS ds = new VideoDS();
                bVideo.FillRW_VIDEO_REPARTI(ds);

                VideoDS.RW_VIDEO_REPARTIRow VideoB = ds.RW_VIDEO_REPARTI.NewRW_VIDEO_REPARTIRow();

                VideoB = ds.RW_VIDEO_REPARTI.Where(x => x.IDVIDEOREPARTO == IDVIDEOREPARTO).FirstOrDefault();
                if (VideoB != null)
                {
                    VideoB.Delete();
                    bVideo.UpdateVideoDS(ds.RW_VIDEO_REPARTI.TableName, ds);
                }
            }
        }

        public string LeggiVideo(string reparto)
        {
            string Path = string.Empty;
            using (VideoBusiness bVideo = new VideoBusiness())
            {
                VideoDS ds = new VideoDS();
                bVideo.FillRW_VIDEO(ds);
                bVideo.FillRW_VIDEO_REPARTI(ds);

                //VideoDS.RW_VIDEO_REPARTIRow VideoR = ds.RW_VIDEO_REPARTI.NewRW_VIDEO_REPARTIRow();

                VideoDS.RW_VIDEO_REPARTIRow  VideoR = ds.RW_VIDEO_REPARTI.Where(x => x.REPARTO == reparto
                                                                                  && x.DATAINIZIO <= System.DateTime.Today 
                                                                                  && x.DATAFINE >= System.DateTime.Today).FirstOrDefault();
                if (VideoR != null)
                {
                    //VideoDS.RW_VIDEORow Video = ds.RW_VIDEO.NewRW_VIDEORow();
                    VideoDS.RW_VIDEORow Video = ds.RW_VIDEO.Where(x => x.IDVIDEO == VideoR.IDVIDEO).FirstOrDefault();
                    if (Video != null)
                    {
                        Path = Video.PATHVIDEO+"\\"+Video.NOMEVIDEO;
                    }
                    
                }
            }
            return Path;
        }

    }
}
