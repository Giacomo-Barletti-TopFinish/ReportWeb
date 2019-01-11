
using ReportWeb.Common.Helpers;
using ReportWeb.Data.Core;
using ReportWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Data.Registrazione
{
    public class RegistrazioneBusiness : ReportWebBusinessBase
    {
        public RegistrazioneBusiness() : base() { }

        [DataContext]
        public void FillRW_REGISTRAZIONE(RegistrazioneDS ds)
        {
            RegistrazioneAdapter a = new RegistrazioneAdapter(DbConnection, DbTransaction);
            a.FillRW_REGISTRAZIONE(ds);
        }



        [DataContext(true)]
        public void UpdateRW_REGISTRAZIONE(RegistrazioneDS ds)
        {
            RegistrazioneAdapter a = new RegistrazioneAdapter(DbConnection, DbTransaction);
            a.UpdateRegistrazioneDS(ds.RW_REGISTRAZIONE.TableName, ds);
        }
    }
}
