using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.Logic.Docx
{
    internal class DocxParser
    {
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
        //public static List<string[]> GetAllEntriesOnPage()
    }
}
