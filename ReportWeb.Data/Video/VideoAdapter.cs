using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Video
{
    public class VideoAdapter : ReportWebAdapterBase
    {
        public VideoAdapter(System.Data.IDbConnection connection, IDbTransaction transaction) :
         base(connection, transaction)
        { }

        public void FillRW_VIDEO(VideoDS ds)
        {
            string select = @"SELECT * FROM RW_VIDEO";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_VIDEO);
            }
        }       

        public void FillRW_VIDEO_REPARTI(VideoDS ds)
        {
            string select = @"SELECT * FROM RW_VIDEO_REPARTI";

            using (DbDataAdapter da = BuildDataAdapter(select))
            {
                da.Fill(ds.RW_VIDEO_REPARTI);
            }
        }

        public void FillRW_VIDEOByReparto(VideoDS ds, string Reparto, DateTime Giorno)
        {
            string select = @"SELECT VI.* FROM RW_VIDEO VI
                             INNER JOIN RW_VIDEO_REPARTI VR ON VR.IDVIDEO = VI.IDVIDEO
                             WHERE VR.REPARTO = $P{REPARTO}
                             AND DATAINIZIO >= to_date('{0}','DD/MM/YYYY HH24:MI:SS')
                             AND DATAFINE <= to_date('{1}','DD/MM/YYYY HH24:MI:SS') ";

            ParamSet ps = new ParamSet();
            ps.AddParam("REPARTO", DbType.String, Reparto);

            string dtInizio = Giorno.ToString("dd/MM/yyyy");
            dtInizio += " 00:00:00";
            string dtFine = Giorno.ToString("dd/MM/yyyy");
            dtFine += " 23:59:59";
            select = string.Format(select, dtInizio, dtFine);

            using (DbDataAdapter da = BuildDataAdapter(select, ps))
            {            
                da.Fill(ds.RW_VIDEO_REPARTI);
            }
        }

        public void UpdateVideoDS(string tablename, VideoDS ds)
        {
            string query = string.Format(CultureInfo.InvariantCulture, "SELECT * FROM {0}", tablename);

            using (DbDataAdapter a = BuildDataAdapter(query))
            {
                a.ContinueUpdateOnError = false;
                DataTable dt = ds.Tables[tablename];
                DbCommandBuilder cmd = BuildCommandBuilder(a);
                a.UpdateCommand = cmd.GetUpdateCommand();
                a.DeleteCommand = cmd.GetDeleteCommand();
                a.InsertCommand = cmd.GetInsertCommand();
                a.Update(dt);
            }
        }

    }
}
