using ReportWeb.Data.PVD;
using ReportWeb.Entities;
using ReportWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class PVDBLL
    {
        public List<RWListItem> CreaListaMacchine()
        {
            List<RWListItem> macchine = new List<RWListItem>();

            PVDDS ds = new PVDDS();
            using (PVDBusiness bPDV = new PVDBusiness())
            {
                bPDV.FillUSR_PRD_RESOURCESF(ds);
            }

            foreach (PVDDS.USR_PRD_RESOURCESFRow m in ds.USR_PRD_RESOURCESF)
            {
                macchine.Add(new RWListItem(m.DESRESOURCEF, m.IDRESOURCEF));
            }
            return macchine;
        }

        public List<PVDConsuntivoModel> EstraiConsutivoMacchina(string IDRESOURCEF)
        {
            List<PVDConsuntivoModel> consuntivo = new List<PVDConsuntivoModel>();

            PVDDS ds = new PVDDS();
            using (PVDBusiness bPDV = new PVDBusiness())
            {
                bPDV.FillRW_PVD_CONSUNTIVO(ds);
            }

            foreach (PVDDS.RW_PVD_CONSUNTIVORow m in ds.RW_PVD_CONSUNTIVO.Where(X => X.IDRESOURCEF == IDRESOURCEF))
            {
                PVDConsuntivoModel model = new PVDConsuntivoModel();

                model.Giorno = m.GIORNO;
                model.IDRESOURCEF = m.IDRESOURCEF;
                model.Macchina = m.MACCHINA;
                model.FinituraCodice = m.IsFINITURA_CODNull()?string.Empty:m.FINITURA_COD;
                model.FinituraDescrizione = m.IsFINITURA_DESCNull()?string.Empty:m.FINITURA_DESC;
                model.TipoCiclo = m.IsTIPONull()?string.Empty:m.TIPO;
                model.Inizio = m.INIZIO;
                model.Fine = m.FINE;
                model.Quantita = (int)m.QUANTITA;
                model.Clienti = m.IsCLIENTINull()?string.Empty:m.CLIENTI;
                model.Articolo = m.IsARTICOLONull()?string.Empty:m.ARTICOLO;
                model.Impegno = (int)m.IMPEGNO;
                model.IdConsuntivo = (int)m.IDCONSUNTIVO;
                consuntivo.Add(model);
            }
            return consuntivo;
        }

        public void CancellaRigaConsuntivo(int IdConsuntivo)
        {
            using (PVDBusiness bPDV = new PVDBusiness())
            {
                bPDV.CancellaRigaConsuntivo(IdConsuntivo);
            }
        }

        public void SalvaConsuntivo(string IDRESOURCEF, string FinituraCodice, string FinituraDescrizione, string Tipo, string Giorno, string Inizio, string Fine, int Quantita, string Clienti, string Articolo, int Impegno, string UIDUSER)
        {
            DateTime giorno = DateTime.Parse(Giorno);
            using (PVDBusiness bPDV = new PVDBusiness())
            {
                bPDV.SalvaConsuntivo(IDRESOURCEF, FinituraCodice, FinituraDescrizione, Tipo, Giorno, Inizio, Fine, Quantita, Clienti, Articolo, Impegno, UIDUSER);
            }

        }
    }
}
