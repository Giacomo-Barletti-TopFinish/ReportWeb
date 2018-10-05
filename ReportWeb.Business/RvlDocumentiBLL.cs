using ReportWeb.Data.RvlDocumenti;
using ReportWeb.Entities;
using ReportWeb.Models.RvlDocumenti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Business
{
    public class RvlDocumentiBLL
    {
        public List<BollaVenditaModel> TrovaBollaVendita(string NumeroDocumento)
        {
            List<BollaVenditaModel> bolleVendita = new List<BollaVenditaModel>();

            using (RvlDocumentiBusiness bRvlDocumenti = new RvlDocumentiBusiness())
            {
                RvlDocumentiDS ds = new RvlDocumentiDS();

                bRvlDocumenti.FillUSR_VENDITET(ds, NumeroDocumento);

                List<string> IDVENDITET = ds.USR_VENDITET.Select(x => x.IDVENDITET).ToList();
                if (IDVENDITET.Count > 0)
                {
                    bRvlDocumenti.FillCLIFO(ds);
                    bRvlDocumenti.FillTABTIPDOC(ds);
                }

                bRvlDocumenti.FillUSR_VENDITED(ds, IDVENDITET);

                List<string> IDVENDITED = ds.USR_VENDITED.Select(x => x.IDVENDITED).Distinct().ToList();
                bRvlDocumenti.FillUSR_PRD_FLUSSO_MOVMATE(ds, IDVENDITED);

                List<string> IDPRDMOVMATE = ds.USR_PRD_FLUSSO_MOVMATE.Select(x => x.IDPRDMOVMATE).Distinct().ToList();
                bRvlDocumenti.FillUSR_PRD_MOVMATE(ds, IDPRDMOVMATE);

                List<string> IDPRDMOVFASE = ds.USR_PRD_MOVMATE.Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                bRvlDocumenti.FillUSR_PRD_FLUSSO_MOVFASI(ds, IDPRDMOVFASE);
                bRvlDocumenti.FillUSR_PRD_MOVFASI(ds, IDPRDMOVFASE);

                List<string> IDACQUISTID = ds.USR_PRD_FLUSSO_MOVFASI.Where(x => !x.IsIDACQUISTIDNull()).Select(x => x.IDACQUISTID).Distinct().ToList();
                bRvlDocumenti.FillUSR_ACQUISTID(ds, IDACQUISTID);

                List<string> IDACQUISTIT = ds.USR_ACQUISTID.Select(x => x.IDACQUISTIT).Distinct().ToList();
                bRvlDocumenti.FillUSR_ACQUISTIT(ds, IDACQUISTIT);

                List<string> IDMAGAZZ = ds.USR_PRD_MOVFASI.Select(x => x.IDMAGAZZ).Distinct().ToList();
                bRvlDocumenti.FillMAGAZZ(ds, IDMAGAZZ);

                foreach (RvlDocumentiDS.USR_VENDITETRow testata in ds.USR_VENDITET)
                {
                    BollaVenditaModel bollaVendita = new BollaVenditaModel();
                    bollaVendita.IDVENDITET = testata.IDVENDITET;
                    bollaVendita.NumeroDocumento = testata.IsNUMDOCNull() ? string.Empty : testata.NUMDOC;

                    RvlDocumentiDS.TABTIPDOCRow tipDoc = ds.TABTIPDOC.Where(x => x.IDTABTIPDOC == testata.IDTABTIPDOC).FirstOrDefault();
                    if (tipDoc != null)
                        bollaVendita.TipoDocumento = tipDoc.CODICETIPDOC;
                    bollaVendita.Anno = testata.ANNODOC;
                    bollaVendita.Data = testata.IsDATDOCNull() ? string.Empty : testata.DATDOC.ToShortDateString();
                    bollaVendita.FullNumDoc = testata.IsFULLNUMDOCNull() ? string.Empty : testata.FULLNUMDOC;

                    bollaVendita.Cliente = string.Empty;
                    if (!testata.IsCODICECLIFONull())
                    {
                        RvlDocumentiDS.CLIFORow cliente = ds.CLIFO.Where(x => x.CODICE == testata.CODICECLIFO).FirstOrDefault();
                        if (cliente != null)
                            bollaVendita.Cliente = cliente.RAGIONESOC.Trim();
                    }

                    bollaVendita.PRDMOVFASI = new List<PrdMovFasiModel>();

                    IDVENDITED = ds.USR_VENDITED.Where(x => x.IDVENDITET == testata.IDVENDITET).Select(x => x.IDVENDITED).ToList();
                    IDPRDMOVMATE = ds.USR_PRD_FLUSSO_MOVMATE.Where(x => IDVENDITED.Contains(x.IDVENDITED)).Select(x => x.IDPRDMOVMATE).ToList();
                    IDPRDMOVFASE = ds.USR_PRD_MOVMATE.Where(x => IDPRDMOVMATE.Contains(x.IDPRDMOVMATE)).Select(x => x.IDPRDMOVFASE).ToList();

                    foreach (RvlDocumentiDS.USR_PRD_MOVFASIRow movFaseRow in ds.USR_PRD_MOVFASI.Where(x => IDPRDMOVFASE.Contains(x.IDPRDMOVFASE)))
                    {
                        PrdMovFasiModel movFase = new PrdMovFasiModel();
                        movFase.IDPRDMOVFASE = movFaseRow.IDPRDMOVFASE;
                        movFase.NumeroMovimentoFase = movFaseRow.IsNUMMOVFASENull() ? string.Empty : movFaseRow.NUMMOVFASE;
                        movFase.Modello = string.Empty;
                        if (!movFaseRow.IsIDMAGAZZNull())
                        {
                            RvlDocumentiDS.MAGAZZRow magaz = ds.MAGAZZ.Where(x => x.IDMAGAZZ == movFaseRow.IDMAGAZZ).FirstOrDefault();
                            if (magaz != null)
                                movFase.Modello = magaz.MODELLO;
                        }
                        movFase.Quantita = movFaseRow.QTA;
                        bollaVendita.PRDMOVFASI.Add(movFase);

                        movFase.Acquisti = new List<BollaCaricoModel>();
                        IDACQUISTID = ds.USR_PRD_FLUSSO_MOVFASI.Where(x => !x.IsIDACQUISTIDNull() && x.IDPRDMOVFASE == movFaseRow.IDPRDMOVFASE).Select(x => x.IDACQUISTID).Distinct().ToList();
                        IDACQUISTIT = ds.USR_ACQUISTID.Where(x => IDACQUISTID.Contains(x.IDACQUISTID)).Select(x => x.IDACQUISTIT).Distinct().ToList();

                        foreach (RvlDocumentiDS.USR_ACQUISTITRow acquistoRow in ds.USR_ACQUISTIT.Where(x => IDACQUISTIT.Contains(x.IDACQUISTIT)))
                        {
                            BollaCaricoModel bollaCarico = new BollaCaricoModel();
                            bollaCarico.IDACQUISTIT = acquistoRow.IDACQUISTIT;

                            RvlDocumentiDS.TABTIPDOCRow tipDocAcquisto = ds.TABTIPDOC.Where(x => x.IDTABTIPDOC == acquistoRow.IDTABTIPDOC).FirstOrDefault();
                            if (tipDocAcquisto != null)
                                bollaCarico.TipoDocumento = tipDocAcquisto.CODICETIPDOC;

                            bollaCarico.FullNumDoc = acquistoRow.IsFULLNUMDOCNull() ? string.Empty : acquistoRow.FULLNUMDOC;
                            bollaCarico.NumeroDocumento = acquistoRow.IsNUMDOCNull() ? string.Empty : acquistoRow.NUMDOC;
                            bollaCarico.Anno = acquistoRow.ANNODOC;
                            bollaCarico.Data = acquistoRow.IsDATDOCNull() ? string.Empty : acquistoRow.DATDOC.ToShortDateString();
                            bollaCarico.Riferimento = acquistoRow.IsRIFERIMENTONull() ? string.Empty : acquistoRow.RIFERIMENTO;

                            bollaCarico.Fornitore = string.Empty;
                            if (!acquistoRow.IsCODICECLIFONull())
                            {
                                RvlDocumentiDS.CLIFORow cliente = ds.CLIFO.Where(x => x.CODICE == acquistoRow.CODICECLIFO).FirstOrDefault();
                                if (cliente != null)
                                    bollaCarico.Fornitore = cliente.RAGIONESOC.Trim();
                            }
                            movFase.Acquisti.Add(bollaCarico);
                        }
                    }

                    bolleVendita.Add(bollaVendita);
                }
            }
            return bolleVendita;
        }
    }
}