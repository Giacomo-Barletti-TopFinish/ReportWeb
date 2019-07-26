using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using ReportWeb.Common.Helpers;
using ReportWeb.Models;
using ReportWeb.Models.ALE;
using ReportWeb.Models.Preziosi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportWeb.Reports
{
    public class PDFHelper
    {
        private Document _document;

        public byte[] EstraiReportALEMancanti(AddebitiModel report, string dataInizio, string dataFine)
        {
            InizializzaDocumento("Report Mancanti ALE", "Report", "MetalWeb");

            _document.DefaultPageSetup.Orientation = Orientation.Portrait;
            _document.DefaultPageSetup.RightMargin = 20;
            _document.DefaultPageSetup.LeftMargin = 20;
            _document.AddSection();
            _document.LastSection.AddParagraph("Report Mancanti ALE", "Heading2");

            Paragraph paragraph = _document.LastSection.AddParagraph();
            paragraph.AddText("Report dal ");
            paragraph.AddFormattedText(dataInizio, TextFormat.Bold);
            paragraph.AddText(" al ");
            paragraph.AddFormattedText(dataFine, TextFormat.Bold);
            paragraph.AddText(".");

            paragraph.Format.SpaceAfter = "1cm";

            CreaTabellaALEMancanti(report);

            byte[] fileContents = EstraiByteDaDocumento();
            return fileContents;
        }

        public byte[] EstraiGalvanicaReport(GalvanicaReportModel report, DateTime dataInizio, DateTime dataFine)
        {
            InizializzaDocumento("Report Galvanica", "Report settimanale", "MetalWeb");

            _document.DefaultPageSetup.Orientation = Orientation.Landscape;
            _document.DefaultPageSetup.RightMargin = 20;
            _document.DefaultPageSetup.LeftMargin = 20;
            _document.AddSection();
            _document.LastSection.AddParagraph("Report Galvanica", "Heading2");

            Paragraph paragraph = _document.LastSection.AddParagraph();
            paragraph.AddText("Report per la settimana dal ");
            paragraph.AddFormattedText(dataInizio.ToShortDateString(), TextFormat.Bold);
            paragraph.AddText(" al ");
            paragraph.AddFormattedText(dataFine.ToShortDateString(), TextFormat.Bold);
            paragraph.AddText(".");

            paragraph = _document.LastSection.AddParagraph();
            paragraph.AddText("Numero barre: ");
            paragraph.AddFormattedText(report.BarreTotali.ToString(), TextFormat.Bold);
            paragraph.AddText("  Durata totale: ");
            paragraph.AddFormattedText(DateTimeHelper.ToHoursMin(report.TempoTotale), TextFormat.Bold);
            paragraph.AddText("  Durata complessiva fermi: ");
            paragraph.AddFormattedText(DateTimeHelper.ToHoursMin(report.FermoTotale), TextFormat.Bold);
            paragraph.AddText("  Durata effettiva: ");
            paragraph.AddFormattedText(DateTimeHelper.ToHoursMin(report.DurataEffettiva), TextFormat.Bold);

            paragraph = _document.LastSection.AddParagraph();
            paragraph.AddText("  Numero barre in 1 ora: ");
            paragraph.AddFormattedText(report.BarreHH.ToString(), TextFormat.Bold);
            paragraph.AddText("  Minuti per barra: ");
            paragraph.AddFormattedText(report.MinBarre.ToString(), TextFormat.Bold);

            paragraph.Format.SpaceAfter = "1cm";

            CreaTabellaGalvanica(report);

            byte[] fileContents = EstraiByteDaDocumento();
            return fileContents;
        }

        public byte[] EstraiMovimentiPreziosi(List<Movimenti> movimenti, List<SaldoCasseforti> saldi, string dataInizio, string dataFine)
        {
            InizializzaDocumento("Movimenti prezioso", "Movimenti", "MetalWeb");

            _document.DefaultPageSetup.Orientation = Orientation.Landscape;
            _document.DefaultPageSetup.RightMargin = 20;
            _document.DefaultPageSetup.LeftMargin = 20;
            _document.AddSection();
            _document.LastSection.AddParagraph("Movimenti prezioso", "Heading2");

            Paragraph paragraph = _document.LastSection.AddParagraph();
            paragraph.AddText("Movimenti dal ");
            paragraph.AddFormattedText(dataInizio, TextFormat.Bold);
            paragraph.AddText(" al ");
            paragraph.AddFormattedText(dataFine, TextFormat.Bold);
            paragraph.AddText(". ");
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();

            paragraph = _document.LastSection.AddParagraph();
            paragraph.AddText("MOVIMENTI CASSAFORTE GRANDE");
            paragraph.AddLineBreak();
            CreaTabellaMovimentiCassaforte(movimenti, "A");
            paragraph.Format.SpaceAfter = "1cm";

            paragraph = _document.LastSection.AddParagraph();
            paragraph.AddLineBreak();
            paragraph.AddText("MOVIMENTI CASSAFORTE PICCOLA");
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            CreaTabellaMovimentiCassaforte(movimenti, "B");
            paragraph.Format.SpaceAfter = "1cm";

            paragraph = _document.LastSection.AddParagraph();
            paragraph.AddLineBreak();
            paragraph.AddText("SALDI");
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            CreaTabellaSaldiPrezioso(saldi);

            byte[] fileContents = EstraiByteDaDocumento();
            return fileContents;
        }

        public byte[] EstraiPDVReport(PVDReportModel report, DateTime dataInizio, DateTime dataFine)
        {
            InizializzaDocumento("Report PDV", "Report settimanale", "MetalWeb");

            _document.DefaultPageSetup.Orientation = Orientation.Landscape;
            _document.DefaultPageSetup.RightMargin = 20;
            _document.DefaultPageSetup.LeftMargin = 20;
            _document.AddSection();
            _document.LastSection.AddParagraph("Report PVD", "Heading2");

            Paragraph paragraph = _document.LastSection.AddParagraph();
            paragraph.AddText("Report per la settimana dal ");
            paragraph.AddFormattedText(dataInizio.ToShortDateString(), TextFormat.Bold);
            paragraph.AddText(" al ");
            paragraph.AddFormattedText(dataFine.ToShortDateString(), TextFormat.Bold);
            paragraph.AddText(". Durata complessiva: ");
            paragraph.AddFormattedText(report.DurataTotale, TextFormat.Bold);
            paragraph.Format.SpaceAfter = "1cm";

            CreaTabellaPVD(report);

            byte[] fileContents = EstraiByteDaDocumento();
            return fileContents;
        }

        public byte[] EstraiVerniciaturaReport(VerniciaturaReportModel report, DateTime dataInizio, DateTime dataFine)
        {
            InizializzaDocumento("Report Verniciatura", "Report settimanale", "MetalWeb");

            _document.DefaultPageSetup.Orientation = Orientation.Landscape;
            _document.DefaultPageSetup.RightMargin = 20;
            _document.DefaultPageSetup.LeftMargin = 20;
            _document.AddSection();
            _document.LastSection.AddParagraph("Report Verniciatura", "Heading2");

            Paragraph paragraph = _document.LastSection.AddParagraph();
            paragraph.AddText("Report per la settimana dal ");
            paragraph.AddFormattedText(dataInizio.ToShortDateString(), TextFormat.Bold);
            paragraph.AddText(" al ");
            paragraph.AddFormattedText(dataFine.ToShortDateString(), TextFormat.Bold);
            paragraph.AddText(". Quantità manuale complessiva: ");
            paragraph.AddFormattedText(report.QuantitaManualeTotale, TextFormat.Bold);
            paragraph.AddText(". NUmero di barre complessivo: ");
            paragraph.AddFormattedText(report.BarreTotali, TextFormat.Bold);
            paragraph.Format.SpaceAfter = "1cm";


            CreaTabellaVerniciatura(report);

            byte[] fileContents = EstraiByteDaDocumento();
            return fileContents;
        }

        private byte[] EstraiByteDaDocumento()
        {
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true);// parametro obsoleto , PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = _document;

            renderer.RenderDocument();

            byte[] fileContents = null;
            using (MemoryStream stream = new MemoryStream())
            {
                renderer.Save(stream, true);
                fileContents = stream.ToArray();
            }
            return fileContents;
        }

        private void DefinisciStili()
        {
            // Get the predefined style Normal.
            Style style = _document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole _document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";

            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = _document.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 14;
            style.Font.Bold = true;
            style.Font.Color = Colors.DarkBlue;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;

            style = _document.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;

            style = _document.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;

            style = _document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = _document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called TextBox based on style Normal
            style = _document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

            // Create a new style called TOC based on style Normal
            style = _document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right, TabLeader.Dots);
            style.ParagraphFormat.Font.Color = Colors.Blue;
        }

        private void CreaTabellaMovimentiCassaforte(List<Movimenti> movimenti, string cassaforte)
        {
            //12 colonne
            Table table = new Table();
            table.Borders.Width = 0.75;

            Column column = table.AddColumn(Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(Unit.FromCentimeter(5));
            table.AddColumn(Unit.FromCentimeter(2.5));
            table.AddColumn(Unit.FromCentimeter(2.5));

            table.AddColumn(Unit.FromCentimeter(3));

            table.AddColumn(Unit.FromCentimeter(8));

            table.Rows.Height = 10;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("Giorno");
            cell = row.Cells[1];
            cell.AddParagraph("Materiale");
            cell = row.Cells[2];
            cell.AddParagraph("Dare (gr.)");
            cell = row.Cells[3];
            cell.AddParagraph("Avere");
            cell = row.Cells[4];
            cell.AddParagraph("Utente");
            cell = row.Cells[5];
            cell.AddParagraph("Causale");

            foreach (Movimenti movimento in movimenti.Where(x => x.Cassaforte == cassaforte))
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(movimento.Giorno.ToShortDateString());
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell = row.Cells[1];
                cell.AddParagraph(movimento.Materiale);
                cell = row.Cells[2];
                cell.AddParagraph(movimento.Dare);
                cell = row.Cells[3];
                cell.AddParagraph(movimento.Avere);
                cell = row.Cells[4];
                cell.AddParagraph(movimento.Utente);
                cell = row.Cells[5];
                cell.AddParagraph(movimento.Causale);

            }


            table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            _document.LastSection.Add(table);
        }
        private void CreaTabellaSaldiPrezioso(List<SaldoCasseforti> saldi)
        {
            //12 colonne
            Table table = new Table();
            table.Borders.Width = 0.75;

            Column column = table.AddColumn(Unit.FromCentimeter(6));
            table.AddColumn(Unit.FromCentimeter(2.5));
            table.AddColumn(Unit.FromCentimeter(2.5));

            table.Rows.Height = 10;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("Materiale");
            cell = row.Cells[1];
            cell.AddParagraph("Cassaforte Grande");
            cell = row.Cells[2];
            cell.AddParagraph("Cassaforte Piccola");

            foreach (SaldoCasseforti saldo in saldi)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(saldo.Materiale);
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell = row.Cells[1];
                cell.AddParagraph(saldo.SaldoA);
                cell = row.Cells[2];
                cell.AddParagraph(saldo.SaldoB);

            }


            table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            _document.LastSection.Add(table);
        }
        private void InizializzaDocumento(string Titolo, string Soggetto, string Autore)
        {
            _document = new Document();
            _document.Info.Title = Titolo;
            _document.Info.Subject = Soggetto;
            _document.Info.Author = Autore;

            DefinisciStili();
        }

        private void CreaTabellaPVD(PVDReportModel report)
        {
            //12 colonne
            Table table = new Table();
            table.Borders.Width = 0.75;

            Column column = table.AddColumn(Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(Unit.FromCentimeter(4.5));
            table.AddColumn(Unit.FromCentimeter(4.5));
            table.AddColumn(Unit.FromCentimeter(3));

            table.AddColumn(Unit.FromCentimeter(2));
            table.AddColumn(Unit.FromCentimeter(2));
            table.AddColumn(Unit.FromCentimeter(2));
            table.AddColumn(Unit.FromCentimeter(2));

            table.AddColumn(Unit.FromCentimeter(2.5));

            table.Rows.Height = 10;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("Giorno");
            cell = row.Cells[1];
            cell.AddParagraph("Macchina");
            cell = row.Cells[2];
            cell.AddParagraph("Finitura");
            cell = row.Cells[3];
            cell.AddParagraph("Ciclo");
            cell = row.Cells[4];
            cell.AddParagraph("Inizio");
            cell = row.Cells[5];
            cell.AddParagraph("Fine");
            cell = row.Cells[6];
            cell.AddParagraph("Durata");
            cell = row.Cells[7];
            cell.AddParagraph("Quantità");
            cell = row.Cells[8];
            cell.AddParagraph("Impegno");


            foreach (PVDConsuntivoModel consuntivo in report.Consuntivo)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(consuntivo.Giorno.ToShortDateString());
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell.MergeDown = 1;
                cell = row.Cells[1];
                cell.AddParagraph(consuntivo.Macchina);
                cell = row.Cells[2];
                cell.AddParagraph(string.Format("{0}-{1}", consuntivo.FinituraCodice, consuntivo.FinituraDescrizione));
                cell = row.Cells[3];
                cell.AddParagraph(consuntivo.TipoCiclo);
                cell = row.Cells[4];
                cell.AddParagraph(consuntivo.Inizio);
                cell = row.Cells[5];
                cell.AddParagraph(consuntivo.Fine);
                cell = row.Cells[6];
                cell.AddParagraph(consuntivo.Durata);
                cell = row.Cells[7];
                cell.AddParagraph(consuntivo.Quantita.ToString());
                cell = row.Cells[8];
                cell.AddParagraph(consuntivo.Impegno);

                row = table.AddRow();
                cell = row.Cells[1];
                cell.MergeRight = 2;
                Paragraph p = cell.AddParagraph();
                p.AddFormattedText("Clienti: ", TextFormat.Bold);
                p.AddText(consuntivo.Clienti);
                cell = row.Cells[4];
                cell.MergeRight = 4;
                p = cell.AddParagraph();
                p.AddFormattedText("Articoli: ", TextFormat.Bold);
                p.AddText(consuntivo.Articolo);


            }


            table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            _document.LastSection.Add(table);
        }

        private void CreaTabellaVerniciatura(VerniciaturaReportModel report)
        {
            //12 colonne
            Table table = new Table();
            table.Borders.Width = 0.75;

            Column column = table.AddColumn(Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            column = table.AddColumn(Unit.FromCentimeter(3.5));
            column.Format.Alignment = ParagraphAlignment.Center;
            column = table.AddColumn(Unit.FromCentimeter(2.5));
            column.Format.Alignment = ParagraphAlignment.Center;

            table.Rows.Height = 10;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("Giorno");
            cell = row.Cells[1];
            cell.AddParagraph("Quantità manuale");
            cell = row.Cells[2];
            cell.AddParagraph("Barre");

            foreach (VerniciaturaConsuntivoModel consuntivo in report.Consuntivo)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(consuntivo.Giorno.ToShortDateString());
                cell.VerticalAlignment = VerticalAlignment.Center;
                cell = row.Cells[1];
                cell.AddParagraph(consuntivo.QuantitaManuale.ToString());
                cell = row.Cells[2];
                cell.AddParagraph(consuntivo.Barre.ToString());

            }


            table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            _document.LastSection.Add(table);
        }
        private void CreaTabellaALEMancanti(AddebitiModel report)
        {
            //12 colonne
            Table table = new Table();
            table.Borders.Width = 0.75;

            Column column = table.AddColumn(Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(Unit.FromCentimeter(2.5));
            table.AddColumn(Unit.FromCentimeter(3.5));

            table.AddColumn(Unit.FromCentimeter(4.0));
            table.AddColumn(Unit.FromCentimeter(5.5));
            table.AddColumn(Unit.FromCentimeter(1.5));


            table.Rows.Height = 10;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("Giorno");
            cell = row.Cells[1];
            cell.AddParagraph("Azienda");
            cell = row.Cells[2];
            cell.AddParagraph("Lavorante");
            cell = row.Cells[3];
            cell.AddParagraph("Modello");
            cell = row.Cells[4];
            cell.AddParagraph("Descrizione");
            cell = row.Cells[5];
            cell.AddParagraph("Quantità");


            foreach (AddebitoModel mancante in report.Addebiti)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(mancante.DataInserimento.ToShortDateString());
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[1];
                cell.AddParagraph(mancante.Azienda);
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[2];
                cell.AddParagraph(mancante.LavoranteDescrizione);
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[3];
                cell.AddParagraph(mancante.Modello);
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[4];
                cell.AddParagraph(mancante.ModelloDescrizione);
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[5];
                cell.AddParagraph(mancante.QuantitaDifettosi.ToString());
                cell.VerticalAlignment = VerticalAlignment.Top;

            }

            table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            _document.LastSection.Add(table);
        }

        private void CreaTabellaGalvanica(GalvanicaReportModel report)
        {
            //12 colonne
            Table table = new Table();
            table.Borders.Width = 0.75;

            Column column = table.AddColumn(Unit.FromCentimeter(2));
            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(Unit.FromCentimeter(3));
            table.AddColumn(Unit.FromCentimeter(3));
            table.AddColumn(Unit.FromCentimeter(1.5));

            table.AddColumn(Unit.FromCentimeter(1.5));
            table.AddColumn(Unit.FromCentimeter(1.5));
            table.AddColumn(Unit.FromCentimeter(1.5));
            table.AddColumn(Unit.FromCentimeter(1.5));
            table.AddColumn(Unit.FromCentimeter(1.5));

            table.AddColumn(Unit.FromCentimeter(9));

            table.AddColumn(Unit.FromCentimeter(2));

            table.Rows.Height = 10;
            table.TopPadding = 5;
            table.BottomPadding = 5;

            Row row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            Cell cell = row.Cells[0];
            cell.AddParagraph("Giorno");
            cell = row.Cells[1];
            cell.AddParagraph("Inizio turno");
            cell = row.Cells[2];
            cell.AddParagraph("Fine turno");
            cell = row.Cells[3];
            cell.AddParagraph("Barre");
            cell = row.Cells[4];
            cell.AddParagraph("Durata");
            cell = row.Cells[5];
            cell.AddParagraph("Fermo");
            cell = row.Cells[6];
            cell.AddParagraph("Durata effettiva");
            cell = row.Cells[7];
            cell.AddParagraph("Barre/h");
            cell = row.Cells[8];
            cell.AddParagraph("Minuti/ Barra");
            cell = row.Cells[9];
            cell.AddParagraph("Fermi");
            cell = row.Cells[10];
            cell.AddParagraph("Operatore");


            foreach (GalvanicaConsuntivoModel consuntivo in report.Consuntivo)
            {
                row = table.AddRow();

                cell = row.Cells[0];
                cell.AddParagraph(consuntivo.InizioTurno.ToShortDateString());
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[1];
                cell.AddParagraph(consuntivo.InizioTurno.ToString("dd/MM/yyyy HH:mm"));
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[2];
                cell.AddParagraph(consuntivo.FineTurno.ToString("dd/MM/yyyy HH:mm"));
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[3];
                cell.AddParagraph(consuntivo.Barre.ToString());
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[4];
                cell.AddParagraph(consuntivo.Durata.ToString(@"hh\:mm"));
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[5];
                cell.AddParagraph(consuntivo.FermoTotale.ToString(@"hh\:mm"));
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[6];
                cell.AddParagraph(consuntivo.DurataEffettiva.ToString(@"hh\:mm"));
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[7];
                cell.AddParagraph(consuntivo.BarreHH.ToString());
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[8];
                cell.AddParagraph(consuntivo.MinBarre.ToString());
                cell.VerticalAlignment = VerticalAlignment.Top;


                cell = row.Cells[10];
                cell.AddParagraph(consuntivo.UIDUSER);
                cell.VerticalAlignment = VerticalAlignment.Top;


                for (int i = 0; i < consuntivo.Fermi.Count; i++)
                {
                    cell = row.Cells[9];
                    string fermo = string.Format("{0} {1} {2} {3}", consuntivo.Fermi[i].Tipo, consuntivo.Fermi[i].Ora, consuntivo.Fermi[i].Durata, consuntivo.Fermi[i].Motivo);
                    cell.AddParagraph(fermo);
                }
            }

            table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            _document.LastSection.Add(table);
        }
    }
}
