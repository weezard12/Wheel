using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

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


    }
}
