using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Video
{
    public class VideoBusiness : ReportWebBusinessBase
    {
        public VideoBusiness() : base() { }

        [DataContext]
        public void FillRW_VIDEO(VideoDS ds)
        {
            VideoAdapter a = new VideoAdapter(DbConnection, DbTransaction);
            a.FillRW_VIDEO(ds);
        }

        [DataContext]
        public void FillRW_VIDEO_REPARTI(VideoDS ds)
        {
            VideoAdapter a = new VideoAdapter(DbConnection, DbTransaction);
            a.FillRW_VIDEO_REPARTI(ds);
        }

        [DataContext]
        public void FillRW_VIDEOByReparto(VideoDS ds, string Reparto, DateTime Giorno)
        {
            VideoAdapter a = new VideoAdapter(DbConnection, DbTransaction);
            a.FillRW_VIDEOByReparto(ds, Reparto, Giorno);
        }

        [DataContext(true)]
        public void UpdateVideoDS(string tablename, VideoDS ds)
        {
            VideoAdapter a = new VideoAdapter(DbConnection, DbTransaction);
            a.UpdateVideoDS(tablename, ds);
        }
    }
}
