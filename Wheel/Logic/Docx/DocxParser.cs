using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using Wheel.Logic.Projects;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using PdfDocument = PdfSharp.Pdf.PdfDocument;




namespace Wheel.Logic.Docx
{
    internal class DocxParser
    {
        public static string GetLocalPagePath(string pageName) => Path.Combine("Templates\\Docx", pageName) + ".docx";

        /// <summary>
        /// Searches for "{WheelValue}" entries in a .docx document and retrieves the entry at the specified index.
        /// </summary>
        /// <param name="path">The path to the .docx file.</param>
        /// <param name="idx">The index of the entry to retrieve.</param>
        /// <returns>A string array containing the Entry Name and optionally the Base Value, or null if the index is out of bounds.</returns>
        public static string[] GetEntryByIdx(string path, int idx)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("The specified file does not exist.", path);

            // Open the .docx file
            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, false))
            {
                // Find the MainDocumentPart
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                if (mainPart == null)
                    throw new InvalidOperationException("The document does not contain a main part.");

                // Access the document body text
                var body = mainPart.Document.Body;
                if (body == null)
                    throw new InvalidOperationException("The document does not contain a body.");

                // Extract all text
                string documentText = string.Join(" ", body.Descendants<Text>().Select(t => t.Text));

                // Regex to match {WheelValue (Entry Name, Base Value)} or { WheelValue (Entry Name) }
                var regex = new Regex(@"\{\s*WheelValue\s*\(\s*([^,]+?)\s*(?:,\s*([^)]*?)\s*)?\)\s*\}");

                // Find all matches
                var matches = regex.Matches(documentText);

                // Check if the index is within bounds
                if (idx < 0 || idx >= matches.Count)
                    return null;

                // Get the match at the specified index
                var match = matches[idx];
                string entryName = match.Groups[1].Value.Trim();
                string baseValue = match.Groups[2].Success ? match.Groups[2].Value.Trim() : null;

                // Return as a string array
                return baseValue != null ? new[] { entryName, baseValue } : new[] { entryName };
            }
        }

        /// <summary>
        /// Replaces the specified "{Wheel(entry_name)}" placeholder in a .docx file with the given value, 
        /// preserving formatting and structure.
        /// </summary>
        /// <param name="path">The path to the .docx file.</param>
        /// <param name="entryName">The entry name to replace inside "{Wheel(entry_name)}".</param>
        /// <param name="entryValue">The value to insert in place of "{Wheel(entry_name)}".</param>
        public static void SetEntryByName(string path, string entryName, string entryValue)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("The specified file does not exist.", path);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                if (mainPart == null)
                    throw new InvalidOperationException("The document does not contain a main part.");

                var body = mainPart.Document.Body;
                if (body == null)
                    throw new InvalidOperationException("The document does not contain a body.");

                string placeholder = $"{{WheelValue({entryName})}}";

                foreach (var para in body.Descendants<Paragraph>())
                {
                    var runs = para.Elements<Run>().ToList();
                    if (!runs.Any()) continue;

                    string fullText = string.Join("", runs.Select(r => r.GetFirstChild<Text>()?.Text ?? ""));
                    int placeholderIndex = fullText.IndexOf(placeholder);

                    if (placeholderIndex != -1)
                    {

                        int currentIndex = 0;
                        bool replaced = false;
                        foreach (var run in runs)
                        {
                            var textElement = run.GetFirstChild<Text>();
                            if (textElement == null) continue;

                            string runText = textElement.Text;

                            if (currentIndex + runText.Length >= placeholderIndex &&
                                currentIndex <= placeholderIndex + placeholder.Length)
                            {
                                // If this is the first run where replacement starts, modify its text
                                if (!replaced)
                                {
                                    // Remove placeholder from the first affected run and insert new value
                                    int startReplaceIndex = Math.Max(0, placeholderIndex - currentIndex);
                                    int endReplaceIndex = Math.Min(runText.Length, placeholderIndex + placeholder.Length - currentIndex);

                                    string beforePlaceholder = runText.Substring(0, startReplaceIndex);
                                    string afterPlaceholder = runText.Substring(endReplaceIndex);

                                    textElement.Text = beforePlaceholder + entryValue + afterPlaceholder;
                                    replaced = true;
                                }
                                else
                                {
                                    // Clear text in following runs if they were part of the placeholder
                                    textElement.Text = "";
                                }
                            }

                            currentIndex += runText.Length;
                        }

                        break; // Stop after first replacement
                    }
                }

                // Save changes
                mainPart.Document.Save();
            }
        }

        public static void SetEntryByNameAndAddLines(string path, string entryName, string entryValue)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("The specified file does not exist.", path);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                if (mainPart == null)
                    throw new InvalidOperationException("The document does not contain a main part.");

                var body = mainPart.Document.Body;
                if (body == null)
                    throw new InvalidOperationException("The document does not contain a body.");

                string placeholder = $"{{WheelValue({entryName})}}";

                foreach (var para in body.Descendants<Paragraph>())
                {
                    var runs = para.Elements<Run>().ToList();
                    if (!runs.Any()) continue;

                    string fullText = string.Join("", runs.Select(r => r.GetFirstChild<Text>()?.Text ?? ""));
                    int placeholderIndex = fullText.IndexOf(placeholder);

                    if (placeholderIndex != -1)
                    {
                        int currentIndex = 0;
                        bool replaced = false;
                        Run firstRun = null;
                        foreach (var run in runs)
                        {
                            var textElement = run.GetFirstChild<Text>();
                            if (textElement == null) continue;

                            string runText = textElement.Text;

                            if (currentIndex + runText.Length >= placeholderIndex &&
                                currentIndex <= placeholderIndex + placeholder.Length)
                            {
                                if (!replaced)
                                {
                                    firstRun = run;
                                    int startReplaceIndex = Math.Max(0, placeholderIndex - currentIndex);
                                    int endReplaceIndex = Math.Min(runText.Length, placeholderIndex + placeholder.Length - currentIndex);

                                    string beforePlaceholder = runText.Substring(0, startReplaceIndex);
                                    string afterPlaceholder = runText.Substring(endReplaceIndex);

                                    // Split by new lines first
                                    var lines = entryValue.Split('\n');
                                    textElement.Text = beforePlaceholder + ProcessTabs(lines[0]) + afterPlaceholder;

                                    replaced = true;
                                }
                                else
                                {
                                    // Clear text in following runs if they were part of the placeholder
                                    textElement.Text = "";
                                }
                            }
                            currentIndex += runText.Length;
                        }

                        // Insert new paragraphs for each new line and process tabs
                        if (firstRun != null)
                        {
                            Paragraph parentParagraph = firstRun.Parent as Paragraph;
                            var lines = entryValue.Split('\n');

                            for (int i = 1; i < lines.Length; i++)
                            {
                                Paragraph newParagraph = new Paragraph(new Run(new Text(ProcessTabs(lines[i]))));
                                body.InsertAfter(newParagraph, parentParagraph);
                                parentParagraph = newParagraph; // Move reference to new paragraph
                            }
                        }

                        break; // Stop after first replacement
                    }
                }

                // Save changes
                mainPart.Document.Save();
            }
        }

        private static string ProcessTabs(string input)
        {
            return input.Replace("\t", "    "); // Replace tab with 4 spaces (or customize as needed)
        }


        public static void SetEntryByName(string path, string entryName, string[] column1Values, string[] column2Values)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("The specified file does not exist.", path);

            if (column1Values.Length > column2Values.Length)
            {
                string[] temp = new string[column1Values.Length];
                Array.Copy(column2Values, temp, column2Values.Length);
                column2Values = temp;
            }
            else if (column1Values.Length < column2Values.Length)
            {
                string[] temp = new string[column2Values.Length];
                Array.Copy(column1Values, temp, column1Values.Length);
                column1Values = temp;
            }


            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
            {
                MainDocumentPart mainPart = wordDoc.MainDocumentPart;
                if (mainPart == null)
                    throw new InvalidOperationException("The document does not contain a main part.");

                var body = mainPart.Document.Body;
                if (body == null)
                    throw new InvalidOperationException("The document does not contain a body.");

                string placeholder = $"{{WheelValue({entryName})}}";

                foreach (var para in body.Descendants<Paragraph>())
                {
                    var runs = para.Elements<Run>().ToList();
                    if (!runs.Any()) continue;

                    string fullText = string.Join("", runs.Select(r => r.GetFirstChild<Text>()?.Text ?? ""));
                    int placeholderIndex = fullText.IndexOf(placeholder);

                    if (placeholderIndex != -1)
                    {
                        // Remove the paragraph containing the placeholder
                        para.Remove();

                        // Insert the table at the same position
                        Table table = CreateTable(column1Values, column2Values);
                        body.Append(table);

                        break; // Stop after first replacement
                    }
                }

                // Save changes
                mainPart.Document.Save();
            }
        }

        private static Table CreateTable(string[] column1Values, string[] column2Values)
        {
            Table table = new Table();

            // Define table properties (borders)
            TableProperties tblProperties = new TableProperties(
                new TableBorders(
                    new TopBorder() { Val = BorderValues.Single, Size = 12 },
                    new BottomBorder() { Val = BorderValues.Single, Size = 12 },
                    new LeftBorder() { Val = BorderValues.Single, Size = 12 },
                    new RightBorder() { Val = BorderValues.Single, Size = 12 },
                    new InsideHorizontalBorder() { Val = BorderValues.Single, Size = 12 },
                    new InsideVerticalBorder() { Val = BorderValues.Single, Size = 12 }
                )
            );
            table.AppendChild(tblProperties);

            // Add table rows
            for (int i = 0; i < column1Values.Length; i++)
            {
                TableRow row = new TableRow();

                // Create first cell
                TableCell cell1 = new TableCell(new Paragraph(new Run(new Text(column1Values[i]))));
                cell1.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Auto }));

                // Create second cell
                TableCell cell2 = new TableCell(new Paragraph(new Run(new Text(column2Values[i]))));
                cell2.Append(new TableCellProperties(new TableCellWidth() { Type = TableWidthUnitValues.Auto }));

                // Add cells to row
                row.Append(cell1, cell2);

                // Add row to table
                table.Append(row);
            }

            return table;
        }

        static void CombineDocx(string firstDocx, string secondDocx, string outputDocx)
        {
            if (!File.Exists(firstDocx) || !File.Exists(secondDocx))
            {
                throw new FileNotFoundException("One or both input files do not exist.");
            }

            // Copy secondDocx as base document
            File.Copy(secondDocx, outputDocx, true);

            using (WordprocessingDocument mainDoc = WordprocessingDocument.Open(outputDocx, true))
            {
                MainDocumentPart mainPart = mainDoc.MainDocumentPart;
                if (mainPart == null)
                {
                    throw new InvalidOperationException("The second document is not a valid .docx file.");
                }

                using (WordprocessingDocument docToAppend = WordprocessingDocument.Open(firstDocx, false))
                {
                    MainDocumentPart partToAppend = docToAppend.MainDocumentPart;
                    if (partToAppend == null)
                    {
                        throw new InvalidOperationException("The first document is not a valid .docx file.");
                    }

                    // Get the body of both documents
                    Body mainBody = mainPart.Document.Body;
                    Body appendBody = partToAppend.Document.Body;

                    if (appendBody != null)
                    {
                        // Add a page break before appending content
                        mainBody.AppendChild(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));

                        // Append the content
                        foreach (var element in appendBody.Elements())
                        {
                            mainBody.Append(element.CloneNode(true));
                        }

                        // Save changes
                        mainPart.Document.Save();
                    }
                }
            }
        }

        public static async Task MergeDocx(string firstDocx, string secondDocx)
        {
            if (!File.Exists(firstDocx) || !File.Exists(secondDocx))
            {
                MyUtils.CreateFolderIfDoesntExist(ProjectBase.TemplatesFolderPath);
                await MyUtils.CopyLocalTemplate(secondDocx);
                
            }

            using (WordprocessingDocument mainDoc = WordprocessingDocument.Open(firstDocx, true))
            {
                MainDocumentPart mainPart = mainDoc.MainDocumentPart;
                if (mainPart == null)
                {
                    throw new InvalidOperationException("The first document is not a valid .docx file.");
                }

                // try to open the socond page (if it fails it will copy the local file and trys angin)
                try
                {
                    using (WordprocessingDocument docToAppend = WordprocessingDocument.Open(secondDocx, false))
                    {
                        MainDocumentPart partToAppend = docToAppend.MainDocumentPart;
                        if (partToAppend == null)
                        {
                            throw new InvalidOperationException("The second document is not a valid .docx file.");
                        }

                        // Get the body of both documents
                        Body mainBody = mainPart.Document.Body;
                        Body appendBody = partToAppend.Document.Body;

                        if (appendBody != null)
                        {
                            // Add a page break before appending content
                            mainBody.AppendChild(new Paragraph(new Run(new Break() { Type = BreakValues.Page })));

                            // Append the content
                            foreach (var element in appendBody.Elements())
                            {
                                mainBody.Append(element.CloneNode(true));
                            }

                            // Save changes
                            mainPart.Document.Save();
                        }
                    }
                }
                catch
                {
                }

                
            }
        }

        public static void ParseDocx(string inputDocxPath, string outputPath, Aspose.Words.SaveFormat format)
        {
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(inputDocxPath);
                doc.Save(outputPath, format);
            }
            catch (Exception ex)
            {

            }
        }



        public static void InsertAPicture(string document, string fileName)
        {
            using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(document, true))
            {
                if (wordprocessingDocument.MainDocumentPart is null)
                {
                    throw new ArgumentNullException("MainDocumentPart is null.");
                }

                MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;

                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Svg);

                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    imagePart.FeedData(stream);
                }

                AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));
            }
        }

        static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
        {
            // Define the reference of the image.
            var element =
                 new Drawing(
                     new DW.Inline(
                         new DW.Extent() { Cx = 990000L, Cy = 792000L },
                         new DW.EffectExtent()
                         {
                             LeftEdge = 0L,
                             TopEdge = 0L,
                             RightEdge = 0L,
                             BottomEdge = 0L
                         },
                         new DW.DocProperties()
                         {
                             Id = (UInt32Value)1U,
                             Name = "Picture 1"
                         },
                         new DW.NonVisualGraphicFrameDrawingProperties(
                             new A.GraphicFrameLocks() { NoChangeAspect = true }),
                         new A.Graphic(
                             new A.GraphicData(
            new PIC.Picture(
                                     new PIC.NonVisualPictureProperties(
                                         new PIC.NonVisualDrawingProperties()
                                         {
                                             Id = (UInt32Value)0U,
                                             Name = "New Bitmap Image.svg"
                                         },
                                         new PIC.NonVisualPictureDrawingProperties()),
                                     new PIC.BlipFill(
                                         new A.Blip(
                                             new A.BlipExtensionList(
                                                 new A.BlipExtension()
                                                 {
                                                     Uri =
                                                        "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                 })
                                         )
                                         {
                                             Embed = relationshipId,
                                             CompressionState =
                                             A.BlipCompressionValues.Print
                                         },
                                         new A.Stretch(
            new A.FillRectangle())),
                                     new PIC.ShapeProperties(
                                         new A.Transform2D(
                                             new A.Offset() { X = 0L, Y = 0L },
                                             new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                         new A.PresetGeometry(
                                             new A.AdjustValueList()
                                         )
                                         { Preset = A.ShapeTypeValues.Rectangle }))
                             )
                             { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                     )
                     {
                         DistanceFromTop = (UInt32Value)0U,
                         DistanceFromBottom = (UInt32Value)0U,
                         DistanceFromLeft = (UInt32Value)0U,
                         DistanceFromRight = (UInt32Value)0U,
                         EditId = "50D07946"
                     });

            if (wordDoc.MainDocumentPart is null || wordDoc.MainDocumentPart.Document.Body is null)
            {
                throw new ArgumentNullException("MainDocumentPart and/or Body is null.");
            }

            // Append the reference to body, the element should be in a Run.
            wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
        }

        /// <summary>
        /// Converts a Word document (.docx) to PDF using Microsoft.Office.Interop.Word.
        /// </summary>
        /// <param name="docxPath">Path to the .docx file.</param>
        /// <param name="pdfPath">Output path for the .pdf file.</param>
        /// <returns>True if conversion is successful, otherwise false.</returns>
        public static void ConvertDocxToPdfWithSmallWatermark(string docxPath, string pdfPath)
        {
            // Load the .docx file
            Spire.Doc.Document document = new Spire.Doc.Document();
            document.LoadFromFile(docxPath);

            // Save the document as a .pdf file
            document.SaveToFile(pdfPath, Spire.Doc.FileFormat.PDF);

            // Dispose the document object
            document.Dispose();
        }


        // DOES NOT WORK
        public static void RemoveTextFromPdf(string inputPath, string outputPath, string textToRemove)
        {
            using (PdfDocument document = PdfReader.Open(inputPath, PdfDocumentOpenMode.Modify))
            {
                using (PdfDocument newDocument = new PdfDocument())
                using (PdfDocument originalDocument = PdfReader.Open(inputPath, PdfDocumentOpenMode.Import))
                using (PdfDocument copyDocument = new PdfDocument())
                {
                    // Iterate through all pages
                    for (int i = 0; i < document.PageCount; i++)
                    {
                        PdfPage originalPage = originalDocument.Pages[i];
                        PdfPage newPage = newDocument.AddPage(originalPage);
                        XGraphics gfx = XGraphics.FromPdfPage(newPage);

                        // Extract text using PdfPig
                        using (UglyToad.PdfPig.PdfDocument pdfPigDoc = UglyToad.PdfPig.PdfDocument.Open(inputPath))
                        {
                            UglyToad.PdfPig.Content.Page pdfPigPage = pdfPigDoc.GetPage(i + 1);
                            string extractedText = pdfPigPage.Text;

                            // Replace unwanted text
                            string modifiedText = extractedText.Replace(textToRemove, "");

                            // Draw modified text back
                            gfx.DrawString(modifiedText, new XFont("Arial", 12), XBrushes.Black, new XPoint(50, 50));
                        }
                    }

                    newDocument.Save(outputPath);
                }
            }
        }
    }
}
