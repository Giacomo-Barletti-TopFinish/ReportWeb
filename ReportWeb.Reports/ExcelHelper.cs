using ReportWeb.Models.ALE;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ReportWeb.Models.Magazzino;

namespace ReportWeb.Reports
{
    public class ExcelHelper
    {
        public byte[] CreaExcelMancanti(AddebitiModel Addebiti)
        {
            byte[] content;
            MemoryStream ms = new MemoryStream();
            //string filename = @"c:\temp\mancanti.xlsx";
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                        new Column
                        {
                            Min = 1,
                            Max = 1,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 20,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 40,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 60,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 1,
                            Max = 1,
                            Width = 15,
                            CustomWidth = true
                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Mancanti" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("Data inserimento", CellValues.String, 2),
                    ConstructCell("Azienda", CellValues.String, 2),
                    ConstructCell("Lavorante", CellValues.String, 2),
                    ConstructCell("Modello", CellValues.String, 2),
                    ConstructCell("Descrizione", CellValues.String, 2),
                    ConstructCell("Quantità", CellValues.String, 2));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                foreach (AddebitoModel addebito in Addebiti.Addebiti)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(addebito.DataInserimento.ToShortDateString(), CellValues.String, 1),
                        ConstructCell(addebito.Azienda, CellValues.String, 1),
                        ConstructCell(addebito.LavoranteDescrizione, CellValues.String, 1),
                        ConstructCell(addebito.Modello, CellValues.String, 1),
                        ConstructCell(addebito.ModelloDescrizione, CellValues.String, 1),
                        ConstructCell(addebito.QuantitaDifettosi.ToString(), CellValues.Number, 1));

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
                document.Close();

                ms.Seek(0, SeekOrigin.Begin);
                content = ms.ToArray();
            }

            return content;
        }

        public byte[] CreaExcelFattureRitardate(FattureRitardateModel FattureRitardate)
        {
            byte[] content;
            MemoryStream ms = new MemoryStream();
            //string filename = @"c:\temp\mancanti.xlsx";
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                        new Column
                        {
                            Min = 1,
                            Max = 1,
                            Width = 40,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 20,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 20,
                            CustomWidth = true  


                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Fatture Ritardate" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("ODL", CellValues.String, 2),
                    ConstructCell("Data Creazione", CellValues.String, 2),
                    ConstructCell("Utente Inserimento", CellValues.String, 2),
                    ConstructCell("Lavorante", CellValues.String, 2),
                    ConstructCell("Data Scadenza", CellValues.String, 2));

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(row);

                foreach (FatturaRitardataModel FatturaRitardata in FattureRitardate.FattureRitardate)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(FatturaRitardata.ODL, CellValues.String, 1),
                        ConstructCell(FatturaRitardata.DATA_CREAZIONE.ToShortDateString(), CellValues.String, 1),
                        ConstructCell(FatturaRitardata.UIDUSER_INSERIMENTO, CellValues.String, 1),
                        ConstructCell(FatturaRitardata.LAVORANTE, CellValues.String, 1),
                        ConstructCell(FatturaRitardata.DATA_SCADENZA.ToShortDateString(), CellValues.String, 1));

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
                document.Close();

                ms.Seek(0, SeekOrigin.Begin);
                content = ms.ToArray();
            }

            return content;
        }

        private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }

                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } })
                    { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true } // header
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        public byte[] CreaExcelMagazziniEsterni(List<MagazzinoLavorantiEsterniModel> magazzini, string lavorante, string DataInizio, string DataFine)
        {
            byte[] content;
            MemoryStream ms = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                     new Column
                     {
                         Min = 1,
                         Max = 1,
                         Width = 15,
                         CustomWidth = true
                     },
                        new Column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 15,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 30,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 6,
                            Max = 6,
                            Width = 60,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 7,
                            Max = 7,
                            Width = 10,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 8,
                            Max = 8,
                            Width = 10,
                            CustomWidth = true
                        },
                          new Column
                          {
                              Min = 9,
                              Max = 9,
                              Width = 30,
                              CustomWidth = true
                          },
                        new Column
                        {
                            Min = 10,
                            Max = 10,
                            Width = 60,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 11,
                            Max = 11,
                            Width = 10,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 12,
                            Max = 12,
                            Width = 10,
                            CustomWidth = true
                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = lavorante };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                Row staticRow = new Row();
                staticRow.Append(
                   ConstructCell("Dal", CellValues.String, 2),
                   ConstructCell(DataInizio, CellValues.String, 1),
                   ConstructCell("Al", CellValues.String, 2),
                   ConstructCell(DataFine, CellValues.String, 1));
                sheetData.AppendChild(staticRow);
                sheetData.AppendChild(new Row());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("Azienda", CellValues.String, 2),
                    ConstructCell("ODL", CellValues.String, 2),
                    ConstructCell("Data inizio", CellValues.String, 2),
                    ConstructCell("Data fine", CellValues.String, 2),
                    ConstructCell("Modello", CellValues.String, 2),
                    ConstructCell("Descrizione", CellValues.String, 2),
                    ConstructCell("Quantità", CellValues.String, 2),
                    ConstructCell("Peso", CellValues.String, 2),
                    ConstructCell("Componente", CellValues.String, 2),
                    ConstructCell("Descrizione", CellValues.String, 2),
                    ConstructCell("Quantità", CellValues.String, 2),
                    ConstructCell("Peso", CellValues.String, 2));

                sheetData.AppendChild(row);

                foreach (MagazzinoLavorantiEsterniModel elemento in magazzini)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(elemento.Azienda, CellValues.String, 1),
                        ConstructCell(elemento.ODL, CellValues.String, 1),
                        ConstructCell(elemento.DataInizio, CellValues.String, 1),
                        ConstructCell(elemento.DataFine, CellValues.String, 1),
                        ConstructCell(elemento.Modello, CellValues.String, 1),
                        ConstructCell(elemento.ModelloDescrizione, CellValues.String, 1),
                        ConstructCell(elemento.Quanita.ToString(), CellValues.String, 1),
                        ConstructCell(elemento.Peso.ToString(), CellValues.String, 1),
                        ConstructCell(elemento.Componente, CellValues.String, 1),
                        ConstructCell(elemento.ComponenteDescrizione, CellValues.String, 1),
                        ConstructCell(elemento.QuanitaComponente.ToString(), CellValues.String, 1),
                        ConstructCell(elemento.PesoComponente.ToString(), CellValues.String, 1));


                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
                document.Close();

                ms.Seek(0, SeekOrigin.Begin);
                content = ms.ToArray();
            }

            return content;
        }


        public byte[] CreaExcelGiacenzeMagazzino(List<ModelloGiacenzaModel> giacenze)
        {
            byte[] content;
            MemoryStream ms = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                     new Column
                     {
                         Min = 1,
                         Max = 1,
                         Width = 35,
                         CustomWidth = true
                     },
                        new Column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 60,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 15,
                            CustomWidth = true
                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Giacenze" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("Modello", CellValues.String, 2),
                    ConstructCell("Dettaglio", CellValues.String, 2),
                    ConstructCell("Giacenze", CellValues.String, 2));

                sheetData.AppendChild(row);

                foreach (ModelloGiacenzaModel elemento in giacenze)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(elemento.Modello, CellValues.String, 1),
                        ConstructCell(elemento.Descrizione, CellValues.String, 1),
                        ConstructCell(elemento.Giacenza, CellValues.String, 1));

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
                document.Close();

                ms.Seek(0, SeekOrigin.Begin);
                content = ms.ToArray();
            }

            return content;
        }

        public byte[] CreaExcelCampionario(List<MagazzinoCampionarioModel> giacenze)
        {
            byte[] content;
            MemoryStream ms = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                     new Column
                     {
                         Min = 1,
                         Max = 1,
                         Width = 20,
                         CustomWidth = true
                     },
                        new Column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 15,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 80,
                            CustomWidth = true
                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Campionario" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("Codice", CellValues.String, 2),
                    ConstructCell("Finitura", CellValues.String, 2),
                    ConstructCell("Piano", CellValues.String, 2),
                    ConstructCell("Posizione", CellValues.String, 2),
                    ConstructCell("Descrizione", CellValues.String, 2));

                sheetData.AppendChild(row);

                foreach (MagazzinoCampionarioModel elemento in giacenze)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(elemento.Codice, CellValues.String, 1),
                        ConstructCell(elemento.Finitura, CellValues.String, 1),
                        ConstructCell(elemento.Piano, CellValues.String, 1),
                        ConstructCell(elemento.Posizione, CellValues.String, 1),
                        ConstructCell(elemento.Descrizione, CellValues.String, 1));

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
                document.Close();

                ms.Seek(0, SeekOrigin.Begin);
                content = ms.ToArray();
            }

            return content;
        }

        public byte[] CreaExcelPosizioneCampionario(List<PosizioneCampionarioModel> giacenze)
        {
            byte[] content;
            MemoryStream ms = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // Adding style
                WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                stylePart.Stylesheet = GenerateStylesheet();
                stylePart.Stylesheet.Save();

                // Setting up columns
                Columns columns = new Columns(
                     new Column
                     {
                         Min = 1,
                         Max = 1,
                         Width = 20,
                         CustomWidth = true
                     },
                        new Column
                        {
                            Min = 2,
                            Max = 2,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 3,
                            Max = 3,
                            Width = 20,
                            CustomWidth = false
                        },
                        new Column
                        {
                            Min = 4,
                            Max = 4,
                            Width = 70,
                            CustomWidth = true
                        },
                        new Column
                        {
                            Min = 5,
                            Max = 5,
                            Width = 15,
                            CustomWidth = true
                        });

                worksheetPart.Worksheet.AppendChild(columns);

                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());

                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Campionario" };

                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // Constructing header
                Row row = new Row();

                row.Append(
                    ConstructCell("Campione", CellValues.String, 2),
                    ConstructCell("Posizione", CellValues.String, 2),
                    ConstructCell("Progressivo", CellValues.String, 2),
                    ConstructCell("Seriale", CellValues.String, 2),
                    ConstructCell("Cliente", CellValues.String, 2));

                sheetData.AppendChild(row);

                foreach (PosizioneCampionarioModel elemento in giacenze)
                {
                    row = new Row();

                    row.Append(
                        ConstructCell(elemento.Campione, CellValues.String, 1),
                        ConstructCell(elemento.Posizione, CellValues.String, 1),
                        ConstructCell(elemento.Progressivo.ToString(), CellValues.String, 1),
                        ConstructCell(elemento.Seriale, CellValues.String, 1),
                        ConstructCell(elemento.Cliente, CellValues.String, 1));

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
                document.Save();
                document.Close();

                ms.Seek(0, SeekOrigin.Begin);
                content = ms.ToArray();
            }

            return content;
        }
    }
}
