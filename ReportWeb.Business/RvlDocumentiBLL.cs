using ReportWeb.Data.RvlDocumenti;
using ReportWeb.Entities;
using ReportWeb.Models;
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
        public List<RWListItem> CaricaTipoDocumentoBolleCarico()
        {
            List<RWListItem> model = new List<RWListItem>();
            using (RvlDocumentiBusiness bRvlDocumenti = new RvlDocumentiBusiness())
            {
                RvlDocumentiDS ds = new RvlDocumentiDS();
                bRvlDocumenti.FillTABTIPDOC(ds);

                List<string> documentiAmmessi = new List<string>();
                documentiAmmessi.Add("0000000002");
                documentiAmmessi.Add("0000000004");
                documentiAmmessi.Add("0000000006");
                documentiAmmessi.Add("0000000008");

                foreach (RvlDocumentiDS.TABTIPDOCRow tipoDoc in ds.TABTIPDOC.Where(x => documentiAmmessi.Contains(x.IDTABTIPDOC)))
                {
                    model.Add(new RWListItem(tipoDoc.CODICETIPDOC, tipoDoc.IDTABTIPDOC));
                }
            }
            return model;
        }

        public List<RWListItem> CaricaTipoDocumentoBolleVendita()
        {
            List<RWListItem> model = new List<RWListItem>();
            using (RvlDocumentiBusiness bRvlDocumenti = new RvlDocumentiBusiness())
            {
                RvlDocumentiDS ds = new RvlDocumentiDS();
                bRvlDocumenti.FillTABTIPDOC(ds);

                List<string> documentiAmmessi = new List<string>();
                documentiAmmessi.Add("0000000022");
                documentiAmmessi.Add("0000000024");
                documentiAmmessi.Add("0000000026");
                documentiAmmessi.Add("0000000028");

                foreach (RvlDocumentiDS.TABTIPDOCRow tipoDoc in ds.TABTIPDOC.Where(x => documentiAmmessi.Contains(x.IDTABTIPDOC)))
                {
                    model.Add(new RWListItem(tipoDoc.CODICETIPDOC, tipoDoc.IDTABTIPDOC));
                }
            }
            return model;
        }

        public List<RWListItem> CaricaListaFornitori()
        {
            List<RWListItem> model = new List<RWListItem>();
            using (RvlDocumentiBusiness bRvlDocumenti = new RvlDocumentiBusiness())
            {
                RvlDocumentiDS ds = new RvlDocumentiDS();
                bRvlDocumenti.FillCLIFO(ds);

                foreach (RvlDocumentiDS.CLIFORow fornitore in ds.CLIFO.OrderBy(x => x.RAGIONESOC))
                {
                    model.Add(new RWListItem(fornitore.RAGIONESOC.Trim(), fornitore.CODICE));
                }
            }
            return model;
        }
        public List<BollaVenditaModel> TrovaBollaVendita(string NumeroDocumento, string TipoDocumento, string Data, string Cliente)
        {
            List<BollaVenditaModel> bolleVendita = new List<BollaVenditaModel>();

            using (RvlDocumentiBusiness bRvlDocumenti = new RvlDocumentiBusiness())
            {
                RvlDocumentiDS ds = new RvlDocumentiDS();

                bRvlDocumenti.FillUSR_VENDITET(ds, NumeroDocumento, TipoDocumento, Data, Cliente);

                List<string> IDVENDITET = ds.USR_VENDITET.Select(x => x.IDVENDITET).ToList();
                if (IDVENDITET.Count > 0)
                {
                    bRvlDocumenti.FillCLIFO(ds);
                    bRvlDocumenti.FillTABTIPDOC(ds);
                    bRvlDocumenti.FillTABCAUMGT(ds);
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
                IDMAGAZZ.AddRange(ds.USR_VENDITED.Select(x => x.IDMAGAZZ).Distinct().ToList());
                bRvlDocumenti.FillMAGAZZ(ds, IDMAGAZZ);

                foreach (RvlDocumentiDS.USR_VENDITETRow testata in ds.USR_VENDITET)
                {
                    BollaVenditaModel bollaVendita = creaBollaVenditaModel(ds, testata);

                    IDVENDITED = ds.USR_VENDITED.Where(x => x.IDVENDITET == testata.IDVENDITET).Select(x => x.IDVENDITED).ToList();
                    IDPRDMOVMATE = ds.USR_PRD_FLUSSO_MOVMATE.Where(x => IDVENDITED.Contains(x.IDVENDITED)).Select(x => x.IDPRDMOVMATE).ToList();
                    IDPRDMOVFASE = ds.USR_PRD_MOVMATE.Where(x => IDPRDMOVMATE.Contains(x.IDPRDMOVMATE)).Select(x => x.IDPRDMOVFASE).ToList();

                    foreach (RvlDocumentiDS.USR_PRD_MOVFASIRow movFaseRow in ds.USR_PRD_MOVFASI.Where(x => IDPRDMOVFASE.Contains(x.IDPRDMOVFASE)))
                    {
                        PrdMovFasiModel movFase = creaPrdMovFasiModel(ds, movFaseRow);
                        bollaVendita.PRDMOVFASI.Add(movFase);

                        IDACQUISTID = ds.USR_PRD_FLUSSO_MOVFASI.Where(x => !x.IsIDACQUISTIDNull() && x.IDPRDMOVFASE == movFaseRow.IDPRDMOVFASE).Select(x => x.IDACQUISTID).Distinct().ToList();
                        IDACQUISTIT = ds.USR_ACQUISTID.Where(x => IDACQUISTID.Contains(x.IDACQUISTID)).Select(x => x.IDACQUISTIT).Distinct().ToList();

                        foreach (RvlDocumentiDS.USR_ACQUISTITRow acquistoRow in ds.USR_ACQUISTIT.Where(x => IDACQUISTIT.Contains(x.IDACQUISTIT)))
                        {
                            BollaCaricoModel bollaCarico = creaBollaCaricoModel(ds, acquistoRow);
                            movFase.Acquisti.Add(bollaCarico);
                        }
                    }

                    bolleVendita.Add(bollaVendita);
                }
            }
            return bolleVendita;
        }

        private BollaVenditaDettaglioModel creaBollaVenditaDettaglioModel(RvlDocumentiDS ds, RvlDocumentiDS.USR_VENDITEDRow venditad)
        {
            BollaVenditaDettaglioModel dettaglio = new BollaVenditaDettaglioModel();
            dettaglio.IDVENDITED = venditad.IDVENDITED;
            dettaglio.NRIGA = venditad.NRRIGA;
            dettaglio.Modello = string.Empty;
            if (!venditad.IsIDMAGAZZNull())
            {
                RvlDocumentiDS.MAGAZZRow magaz = ds.MAGAZZ.Where(x => x.IDMAGAZZ == venditad.IDMAGAZZ).FirstOrDefault();
                if (magaz != null)
                    dettaglio.Modello = magaz.MODELLO.Trim();
            }
            dettaglio.Quantita = venditad.QTATOT;

            return dettaglio;
        }

        private BollaVenditaModel creaBollaVenditaModel(RvlDocumentiDS ds, RvlDocumentiDS.USR_VENDITETRow testata)
        {
            BollaVenditaModel bollaVendita = new BollaVenditaModel();
            bollaVendita.IDVENDITET = testata.IDVENDITET;
            bollaVendita.NumeroDocumento = testata.IsNUMDOCNull() ? string.Empty : testata.NUMDOC;
            bollaVendita.Azienda = testata.IsAZIENDANull() ? string.Empty : testata.AZIENDA;
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
            bollaVendita.Dettagli = new List<BollaVenditaDettaglioModel>();

            foreach (RvlDocumentiDS.USR_VENDITEDRow venditad in ds.USR_VENDITED.Where(x => x.IDVENDITET == bollaVendita.IDVENDITET))
            {
                bollaVendita.Dettagli.Add(creaBollaVenditaDettaglioModel(ds, venditad));
            }
            return bollaVendita;
        }

        private PrdMovFasiModel creaPrdMovFasiModel(RvlDocumentiDS ds, RvlDocumentiDS.USR_PRD_MOVFASIRow movFaseRow)
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
            movFase.Acquisti = new List<BollaCaricoModel>();
            movFase.Vendite = new List<BollaVenditaModel>();

            return movFase;
        }

        private BollaCaricoModel creaBollaCaricoModel(RvlDocumentiDS ds, RvlDocumentiDS.USR_ACQUISTITRow acquistoRow)
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
            bollaCarico.Azienda = acquistoRow.IsAZIENDANull() ? string.Empty : acquistoRow.AZIENDA;
            bollaCarico.Fornitore = string.Empty;
            if (!acquistoRow.IsCODICECLIFONull())
            {
                RvlDocumentiDS.CLIFORow cliente = ds.CLIFO.Where(x => x.CODICE == acquistoRow.CODICECLIFO).FirstOrDefault();
                if (cliente != null)
                    bollaCarico.Fornitore = cliente.RAGIONESOC.Trim();
            }
            bollaCarico.Fasi = new List<PrdMovFasiModel>();
            bollaCarico.Dettagli = new List<BollaCaricoDettaglioModel>();

            foreach (RvlDocumentiDS.USR_ACQUISTIDRow acquistidRow in ds.USR_ACQUISTID.Where(x => x.IDACQUISTIT == acquistoRow.IDACQUISTIT))
            {
                BollaCaricoDettaglioModel dettaglio = new BollaCaricoDettaglioModel();

                dettaglio.IDACQUISTID = acquistidRow.IDACQUISTID;
                if (!acquistidRow.IsIDMAGAZZNull())
                {
                    RvlDocumentiDS.MAGAZZRow magaz = ds.MAGAZZ.Where(x => x.IDMAGAZZ == acquistidRow.IDMAGAZZ).FirstOrDefault();
                    if (magaz != null)
                        dettaglio.Modello = magaz.MODELLO.Trim();
                }
                dettaglio.Quantita = acquistidRow.QTATOT;

                dettaglio.Causale = string.Empty;
                if (!acquistidRow.IsIDTABCAUMGTNull())
                {
                    RvlDocumentiDS.TABCAUMGTRow cau = ds.TABCAUMGT.Where(x => x.IDTABCAUMGT == acquistidRow.IDTABCAUMGT).FirstOrDefault();
                    if (cau != null)
                        dettaglio.Causale = cau.IsDESTABCAUMGTNull() ? string.Empty : cau.DESTABCAUMGT;
                }

                bollaCarico.Dettagli.Add(dettaglio);
            }

            return bollaCarico;
        }

        public List<BollaCaricoModel> TrovaBollaCarico(string NumeroDocumento, string TipoDocumento, string Data, string Riferimento, string Fornitore)
        {
            List<BollaCaricoModel> bolleCarico = new List<BollaCaricoModel>();

            using (RvlDocumentiBusiness bRvlDocumenti = new RvlDocumentiBusiness())
            {
                RvlDocumentiDS ds = new RvlDocumentiDS();

                bRvlDocumenti.FillUSR_ACQUISTIT(ds, NumeroDocumento, TipoDocumento, Data, Riferimento, Fornitore);

                List<string> IDACQUISTIT = ds.USR_ACQUISTIT.Select(x => x.IDACQUISTIT).Distinct().ToList();
                if (IDACQUISTIT.Count > 0)
                {
                    bRvlDocumenti.FillCLIFO(ds);
                    bRvlDocumenti.FillTABTIPDOC(ds);
                    bRvlDocumenti.FillTABCAUMGT(ds);
                }

                bRvlDocumenti.FillUSR_ACQUISTIDByIDUSRACQUISTIT(ds, IDACQUISTIT);

                List<string> IDACQUISTID = ds.USR_ACQUISTID.Select(x => x.IDACQUISTID).Distinct().ToList();
                bRvlDocumenti.FillUSR_PRD_FLUSSO_MOVFASIByIDACQUISTID(ds, IDACQUISTID);

                List<string> IDPRDMOVFASE = ds.USR_PRD_FLUSSO_MOVFASI.Where(x => !x.IsIDPRDMOVFASENull()).Select(x => x.IDPRDMOVFASE).Distinct().ToList();
                bRvlDocumenti.FillUSR_PRD_MOVMATEByIDPRDMOVFASE(ds, IDPRDMOVFASE);

                List<string> IDPRDMOVMATE = ds.USR_PRD_MOVMATE.Select(x => x.IDPRDMOVMATE).Distinct().ToList();
                bRvlDocumenti.FillUSR_PRD_FLUSSO_MOVMATEByIDPRDMOVMATE(ds, IDPRDMOVMATE);

                List<string> IDVENDITET = ds.USR_PRD_FLUSSO_MOVMATE.Where(x => !x.IsIDVENDITETNull()).Select(x => x.IDVENDITET).Distinct().ToList();
                bRvlDocumenti.FillUSR_VENDITET(ds, IDVENDITET);
                bRvlDocumenti.FillUSR_VENDITED(ds, IDVENDITET);

                bRvlDocumenti.FillUSR_PRD_MOVFASI(ds, IDPRDMOVFASE);

                List<string> IDMAGAZZ = ds.USR_PRD_MOVFASI.Select(x => x.IDMAGAZZ).Distinct().ToList();
                IDMAGAZZ.AddRange(ds.USR_VENDITED.Select(x => x.IDMAGAZZ).Distinct().ToList());
                bRvlDocumenti.FillMAGAZZ(ds, IDMAGAZZ);

                foreach (RvlDocumentiDS.USR_ACQUISTITRow testata in ds.USR_ACQUISTIT)
                {
                    BollaCaricoModel bollaCarico = creaBollaCaricoModel(ds, testata);

                    IDACQUISTID = ds.USR_ACQUISTID.Where(x => x.IDACQUISTIT == testata.IDACQUISTIT).Select(x => x.IDACQUISTID).Distinct().ToList();

                    IDPRDMOVFASE = ds.USR_PRD_FLUSSO_MOVFASI.Where(x => !x.IsIDPRDMOVFASENull() && IDACQUISTID.Contains(x.IDACQUISTID)).Select(x => x.IDPRDMOVFASE).Distinct().ToList();

                    foreach (RvlDocumentiDS.USR_PRD_MOVFASIRow movFaseRow in ds.USR_PRD_MOVFASI.Where(x => IDPRDMOVFASE.Contains(x.IDPRDMOVFASE)))
                    {
                        PrdMovFasiModel movFase = creaPrdMovFasiModel(ds, movFaseRow);
                        bollaCarico.Fasi.Add(movFase);

                        IDPRDMOVMATE = ds.USR_PRD_MOVMATE.Where(x => x.IDPRDMOVFASE == movFaseRow.IDPRDMOVFASE).Select(x => x.IDPRDMOVMATE).Distinct().ToList();
                        IDVENDITET = ds.USR_PRD_FLUSSO_MOVMATE.Where(x => !x.IsIDVENDITETNull() && IDPRDMOVMATE.Contains(x.IDPRDMOVMATE)).Select(x => x.IDVENDITET).Distinct().ToList();

                        foreach (RvlDocumentiDS.USR_VENDITETRow vendita in ds.USR_VENDITET.Where(x => x.IDVENDITET.Contains(x.IDVENDITET)))
                        {
                            BollaVenditaModel bollaVendita = creaBollaVenditaModel(ds, vendita);
                            movFase.Vendite.Add(bollaVendita);
                        }
                    }

                    bolleCarico.Add(bollaCarico);
                }

            }
            return bolleCarico;
        }
    }
}