using ReportWeb.Data.MailDispatcher;
using ReportWeb.Entities;
using ReportWeb.Models.MailDispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class MailDispatcherBLL
    {
        public List<MD_GRUPPOModel> LeggiGruppi()
        {
            List<MD_GRUPPOModel> gruppi = new List<MD_GRUPPOModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_APP(ds);

                foreach (MailDispatcherDS.MD_GRUPPIRow gruppo in ds.MD_GRUPPI)
                {
                    MD_GRUPPOModel gr = CreaGruppoModel(ds, gruppo.IDGRUPPO);
                    gruppi.Add(gr);
                }
            }

            return gruppi;
        }

        private MD_GRUPPOModel CreaGruppoModel(MailDispatcherDS ds, decimal IdGruppo)
        {
            MD_GRUPPOModel gr = new MD_GRUPPOModel();
            MailDispatcherDS.MD_GRUPPIRow gruppo = ds.MD_GRUPPI.Where(x => x.IDGRUPPO == IdGruppo).FirstOrDefault();
            if (gruppo == null) return gr;

            gr.IDGRUPPO = gruppo.IDGRUPPO;
            gr.Nome = gruppo.NOME.ToUpper();
            gr.Destinatari = new List<MD_GRUPPO_DESTINATARIOModel>();
            foreach (MailDispatcherDS.MD_GRUPPI_DESTINATARIRow destinatario in ds.MD_GRUPPI_DESTINATARI.Where(x => x.IDGRUPPO == gruppo.IDGRUPPO))
            {
                MD_GRUPPO_DESTINATARIOModel des = new MD_GRUPPO_DESTINATARIOModel();
                des.Destinatario = destinatario.DESTINATARIO;
                des.IDDESTINATARIO = destinatario.IDDESTINATARIO;
                des.IDGRUPPO = destinatario.IDGRUPPO;

                gr.Destinatari.Add(des);
            }
            return gr;
        }

        public List<MD_APPLICAZIONEModel> LeggiGruppiApplicazioni()
        {
            List<MD_APPLICAZIONEModel> gruppi = new List<MD_APPLICAZIONEModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_APP(ds);

                foreach (MailDispatcherDS.MD_GRUPPI_APPRow gruppo in ds.MD_GRUPPI_APP)
                {
                    MD_APPLICAZIONEModel gr = CreaApplicazioneModel(ds, gruppo);
                    gruppi.Add(gr);
                }
            }

            return gruppi;
        }

        public List<MD_GRUPPOModel> CreaNuovoGruppo(string Gruppo)
        {
            List<MD_GRUPPOModel> gruppi = new List<MD_GRUPPOModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {

                MailDispatcherDS.MD_GRUPPIRow gruppoRow = ds.MD_GRUPPI.NewMD_GRUPPIRow();
                gruppoRow.NOME = Gruppo;

                ds.MD_GRUPPI.AddMD_GRUPPIRow(gruppoRow);

                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI.TableName, ds);

                ds.Clear();
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_APP(ds);

                foreach (MailDispatcherDS.MD_GRUPPIRow gruppo in ds.MD_GRUPPI)
                {
                    MD_GRUPPOModel gr = CreaGruppoModel(ds, gruppo.IDGRUPPO);
                    gruppi.Add(gr);
                }
            }

            return gruppi;
        }

        private MD_APPLICAZIONEModel CreaApplicazioneModel(MailDispatcherDS ds, MailDispatcherDS.MD_GRUPPI_APPRow gruppo)
        {
            MD_APPLICAZIONEModel ap = new MD_APPLICAZIONEModel();
            ap.Applicazione = gruppo.APPLICAZIONE;
            ap.Operazione = gruppo.OPERAZIONE;
            ap.GRUPPI = new List<MD_GRUPPO_APPLICAZIONE>();
            foreach (MailDispatcherDS.MD_GRUPPI_APPRow grApp in ds.MD_GRUPPI_APP.Where(x => x.APPLICAZIONE == gruppo.APPLICAZIONE && x.OPERAZIONE == gruppo.OPERAZIONE))
            {
                MD_GRUPPO_APPLICAZIONE gra = new MD_GRUPPO_APPLICAZIONE();
                gra.CC = (grApp.A_CC == "1") ? true : false;
                gra.Gruppo = CreaGruppoModel(ds, grApp.IDGRUPPO);
                ap.GRUPPI.Add(gra);
            }
            return ap;
        }

        public List<MD_GRUPPOModel> RimuoviGruppo(decimal IDGRUPPO)
        {
            List<MD_GRUPPOModel> gruppi = new List<MD_GRUPPOModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {

                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_APP(ds);

                foreach (MailDispatcherDS.MD_GRUPPI_APPRow gra in ds.MD_GRUPPI_APP.Where(x => x.IDGRUPPO == IDGRUPPO))
                    gra.Delete();

                foreach (MailDispatcherDS.MD_GRUPPI_DESTINATARIRow grd in ds.MD_GRUPPI_DESTINATARI.Where(x => x.IDGRUPPO == IDGRUPPO))
                    grd.Delete();

                foreach (MailDispatcherDS.MD_GRUPPIRow gr in ds.MD_GRUPPI.Where(x => x.IDGRUPPO == IDGRUPPO))
                    gr.Delete();

                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI_APP.TableName,ds);
                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI_DESTINATARI.TableName, ds);
                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI.TableName, ds);

                ds.Clear();
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_APP(ds);

                foreach (MailDispatcherDS.MD_GRUPPIRow gruppo in ds.MD_GRUPPI)
                {
                    MD_GRUPPOModel gr = CreaGruppoModel(ds, gruppo.IDGRUPPO);
                    gruppi.Add(gr);
                }
            }

            return gruppi;
        }
    }
}
