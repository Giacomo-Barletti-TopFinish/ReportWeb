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
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

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
            foreach (MailDispatcherDS.MD_GRUPPI_DESTINATARIRow destinatario in ds.MD_GRUPPI_DESTINATARI.Where(x => x.RowState != System.Data.DataRowState.Deleted && x.IDGRUPPO == gruppo.IDGRUPPO))
            {
                MD_GRUPPO_DESTINATARIOModel des = new MD_GRUPPO_DESTINATARIOModel();
                des.Destinatario = destinatario.DESTINATARIO;
                des.IDDESTINATARIO = destinatario.IDDESTINATARIO;
                des.IDGRUPPO = destinatario.IDGRUPPO;

                gr.Destinatari.Add(des);
            }
            return gr;
        }

        public List<MD_RICHIEDENTEModel> LeggiRichiedenti()
        {
            List<MD_RICHIEDENTEModel> richiedenti = new List<MD_RICHIEDENTEModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                foreach (MailDispatcherDS.MD_RICHIEDENTIRow richiedente in ds.MD_RICHIEDENTI)
                {
                    MD_RICHIEDENTEModel r = CreaRichiedenteModel(ds, richiedente.IDRICHIEDENTE);
                    richiedenti.Add(r);
                }
            }

            return richiedenti;
        }

        public List<MD_GRUPPOModel> CreaNuovoGruppo(string Gruppo)
        {
            List<MD_GRUPPOModel> gruppi = new List<MD_GRUPPOModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.FillMD_GRUPPI(ds);
                if (!ds.MD_GRUPPI.Any(x => x.NOME.Trim() == Gruppo.Trim()))
                {
                    MailDispatcherDS.MD_GRUPPIRow gruppoRow = ds.MD_GRUPPI.NewMD_GRUPPIRow();
                    gruppoRow.NOME = Gruppo;
                    ds.MD_GRUPPI.AddMD_GRUPPIRow(gruppoRow);
                    bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI.TableName, ds);
                }

                ds.Clear();
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                foreach (MailDispatcherDS.MD_GRUPPIRow gruppo in ds.MD_GRUPPI)
                {
                    MD_GRUPPOModel gr = CreaGruppoModel(ds, gruppo.IDGRUPPO);
                    gruppi.Add(gr);
                }
            }

            return gruppi;
        }

        public List<MD_RICHIEDENTEModel> CreaNuovoRichiedente(string Richiedente)
        {
            List<MD_RICHIEDENTEModel> richiedenti = new List<MD_RICHIEDENTEModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.FillMD_RICHIEDENTI(ds);
                if (!ds.MD_RICHIEDENTI.Any(x => x.RICHIEDENTE.Trim() == Richiedente.Trim()))
                {
                    MailDispatcherDS.MD_RICHIEDENTIRow row = ds.MD_RICHIEDENTI.NewMD_RICHIEDENTIRow();
                    row.RICHIEDENTE = Richiedente;
                    ds.MD_RICHIEDENTI.AddMD_RICHIEDENTIRow(row);
                    bMD.UpdateMailDispatcherDSTable(ds.MD_RICHIEDENTI.TableName, ds);
                }

                ds.Clear();
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                foreach (MailDispatcherDS.MD_RICHIEDENTIRow richiedente in ds.MD_RICHIEDENTI)
                {
                    MD_RICHIEDENTEModel ri = CreaRichiedenteModel(ds, richiedente.IDRICHIEDENTE);
                    richiedenti.Add(ri);
                }
            }

            return richiedenti;
        }


        private MD_RICHIEDENTEModel CreaRichiedenteModel(MailDispatcherDS ds, decimal IDRICHIEDENTE)
        {
            MailDispatcherDS.MD_RICHIEDENTIRow richiedente = ds.MD_RICHIEDENTI.Where(x => x.IDRICHIEDENTE == IDRICHIEDENTE).FirstOrDefault();

            if (richiedente == null) return new MD_RICHIEDENTEModel();

            MD_RICHIEDENTEModel rm = new MD_RICHIEDENTEModel();
            rm.IDRICHIEDENTE = richiedente.IDRICHIEDENTE;
            rm.Richiedente = richiedente.RICHIEDENTE;

            rm.GRUPPI = new List<MD_GRUPPO_RICHIEDENTEModel>();
            foreach (MailDispatcherDS.MD_GRUPPI_RICHIEDENTIRow grRich in ds.MD_GRUPPI_RICHIEDENTI.Where(x => x.IDRICHIEDENTE == richiedente.IDRICHIEDENTE))
            {
                MD_GRUPPO_RICHIEDENTEModel gra = new MD_GRUPPO_RICHIEDENTEModel();
                gra.CC = (grRich.A_CC == "1") ? true : false;
                gra.Gruppo = CreaGruppoModel(ds, grRich.IDGRUPPO);
                gra.IDRICHIEDENTE = richiedente.IDRICHIEDENTE;
                rm.GRUPPI.Add(gra);
            }
            return rm;
        }

        public List<MD_GRUPPOModel> RimuoviGruppo(decimal IDGRUPPO)
        {
            List<MD_GRUPPOModel> gruppi = new List<MD_GRUPPOModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {

                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                foreach (MailDispatcherDS.MD_GRUPPI_RICHIEDENTIRow gra in ds.MD_GRUPPI_RICHIEDENTI.Where(x => x.IDGRUPPO == IDGRUPPO))
                    gra.Delete();

                foreach (MailDispatcherDS.MD_GRUPPI_DESTINATARIRow grd in ds.MD_GRUPPI_DESTINATARI.Where(x => x.IDGRUPPO == IDGRUPPO))
                    grd.Delete();

                foreach (MailDispatcherDS.MD_GRUPPIRow gr in ds.MD_GRUPPI.Where(x => x.IDGRUPPO == IDGRUPPO))
                    gr.Delete();

                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI_RICHIEDENTI.TableName, ds);
                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI_DESTINATARI.TableName, ds);
                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI.TableName, ds);

                ds.Clear();
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                foreach (MailDispatcherDS.MD_GRUPPIRow gruppo in ds.MD_GRUPPI)
                {
                    MD_GRUPPOModel gr = CreaGruppoModel(ds, gruppo.IDGRUPPO);
                    gruppi.Add(gr);
                }
            }

            return gruppi;
        }

        public List<MD_GRUPPO_DESTINATARIOModel> RimuoviDestinatario(decimal IDDESTINATARIO)
        {
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {

                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                MailDispatcherDS.MD_GRUPPI_DESTINATARIRow grd = ds.MD_GRUPPI_DESTINATARI.Where(x => x.IDDESTINATARIO == IDDESTINATARIO).FirstOrDefault();
                decimal IDGRUPPO = grd.IDGRUPPO;
                grd.Delete();

                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI_DESTINATARI.TableName, ds);
                ds.MD_GRUPPI_DESTINATARI.AcceptChanges();

                MailDispatcherDS.MD_GRUPPIRow gruppo = ds.MD_GRUPPI.Where(x => x.IDGRUPPO == IDGRUPPO).FirstOrDefault();
                MD_GRUPPOModel gr = CreaGruppoModel(ds, IDGRUPPO);

                return gr.Destinatari;
            }

        }

        public List<MD_GRUPPO_DESTINATARIOModel> AggiungiDestinatario(decimal IDGRUPPO, string Destinatario)
        {
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                if (!ds.MD_GRUPPI_DESTINATARI.Any(x => x.IDGRUPPO == IDGRUPPO && x.DESTINATARIO == Destinatario.Trim()))
                {
                    MailDispatcherDS.MD_GRUPPI_DESTINATARIRow destinatario = ds.MD_GRUPPI_DESTINATARI.NewMD_GRUPPI_DESTINATARIRow();
                    destinatario.IDGRUPPO = IDGRUPPO;
                    destinatario.DESTINATARIO = Destinatario;
                    ds.MD_GRUPPI_DESTINATARI.AddMD_GRUPPI_DESTINATARIRow(destinatario);

                    bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI_DESTINATARI.TableName, ds);
                }

                ds.Clear();
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI_DESTINATARI.TableName, ds);
                ds.MD_GRUPPI_DESTINATARI.AcceptChanges();

                MailDispatcherDS.MD_GRUPPIRow gruppo = ds.MD_GRUPPI.Where(x => x.IDGRUPPO == IDGRUPPO).FirstOrDefault();
                MD_GRUPPOModel gr = CreaGruppoModel(ds, IDGRUPPO);

                return gr.Destinatari;
            }

        }

        public List<MD_RICHIEDENTEModel> RimuoviRichiedente(decimal IDRICHIEDENTE)
        {
            List<MD_RICHIEDENTEModel> richiedenti = new List<MD_RICHIEDENTEModel>();
            MailDispatcherDS ds = new MailDispatcherDS();
            using (MailDispatcherBusiness bMD = new MailDispatcherBusiness())
            {

                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                foreach (MailDispatcherDS.MD_GRUPPI_RICHIEDENTIRow gra in ds.MD_GRUPPI_RICHIEDENTI.Where(x => x.IDRICHIEDENTE == IDRICHIEDENTE))
                    gra.Delete();

                foreach (MailDispatcherDS.MD_RICHIEDENTIRow gr in ds.MD_RICHIEDENTI.Where(x => x.IDRICHIEDENTE == IDRICHIEDENTE))
                    gr.Delete();

                bMD.UpdateMailDispatcherDSTable(ds.MD_GRUPPI_RICHIEDENTI.TableName, ds);
                bMD.UpdateMailDispatcherDSTable(ds.MD_RICHIEDENTI.TableName, ds);

                ds.Clear();
                bMD.FillMD_GRUPPI(ds);
                bMD.FillMD_GRUPPI_DESTINATARI(ds);
                bMD.FillMD_GRUPPI_RICHIEDENTI(ds);
                bMD.FillMD_RICHIEDENTI(ds);

                foreach (MailDispatcherDS.MD_RICHIEDENTIRow richiedente in ds.MD_RICHIEDENTI)
                {
                    MD_RICHIEDENTEModel ri = CreaRichiedenteModel(ds, richiedente.IDRICHIEDENTE);
                    richiedenti.Add(ri);
                }
            }

            return richiedenti;
        }
    }
}

