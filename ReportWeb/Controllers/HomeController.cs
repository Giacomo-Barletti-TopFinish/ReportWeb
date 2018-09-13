using ReportWeb.BLL;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportWeb.Common;
using ReportWeb.Business;
using ReportWeb.Models.ALE;

namespace ReportWeb.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            List<string> messaggi = new List<string>();
            if(VerificaAbilitazioneUtente(16))
            {// ALE INSERIMENTO
                ALEBLL bll = new ALEBLL(RvlImageSite);
                int aux =bll.ContaSchede(ALEStatoDettaglio.INSERITO);
                if(aux>0)
                    messaggi.Add(string.Format("Ci sono {0} schede da addebitare",aux));
            }
            if (VerificaAbilitazioneUtente(17))
            {// ALE ADDEBITO
                ALEBLL bll = new ALEBLL(RvlImageSite);
                int aux = bll.ContaSchede(ALEStatoDettaglio.ADDEBITATO);
                if (aux > 0)
                    messaggi.Add(string.Format("Ci sono {0} schede da valorizzare", aux));
            }
            if (VerificaAbilitazioneUtente(18))
            {// ALE VALORIZZATO
                ALEBLL bll = new ALEBLL(RvlImageSite);
                int aux = bll.ContaSchede(ALEStatoDettaglio.VALORIZZATO);
                if (aux > 0)
                    messaggi.Add(string.Format("Ci sono {0} schede da approvare", aux));
            }
            if (VerificaAbilitazioneUtente(19))
            {// ALE APPROVATO
                ALEBLL bll = new ALEBLL(RvlImageSite);
                int aux = bll.ContaSchede(ALEStatoDettaglio.APPROVATO);
                if (aux > 0)
                    messaggi.Add(string.Format("Ci sono {0} schede da fatturare", aux));
            }
            return View(messaggi);
        }

    }
}