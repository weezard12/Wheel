using System;
using System.Data.SqlTypes;
using System.Drawing.Imaging;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Miscellaneous;
using SvgLayerSample.Svg;
using Wheel.Logic.CodeParser.Base;

namespace Wheel.Logic.Graphs
{
    internal static class GraphsUtils
    {

        /// <summary>
        /// this method is for android studio project screens flow diagram
        /// </summary>
        /// <returns></returns>
        public static Diagram GetScreenDiagram(List<ClassFile> classFiles)
        {
            var drawingGraph = new Graph();
            foreach (var classFile in classFiles)
            {
                if (classFile.ExtentsFrom.Contains(new SourceClass("AppCompatActivity")))
                {
                    if (drawingGraph.FindNode(classFile.Name) == null)
                        drawingGraph.AddNode(new ComponentNode(classFile.Name,technology: "extends AppCompatActivity"));
                    
                    List<string> strings = GetIntentsFromClass(classFile.Content);
                    foreach (string s in strings)
                    {
                        if (drawingGraph.FindNode(s) == null)
                            drawingGraph.AddNode(new ComponentNode(s, technology: "extends AppCompatActivity"));

                        drawingGraph.AddEdge(classFile.Name, "", s);
                    }

                }
            }

            var doc = new Diagram(drawingGraph);
            doc.Run();
            return doc;
        }
        /// <summary>
        /// Extracts the class names of all intents from a Java class string.
        /// </summary>
        /// <param name="classContent">The content of the Java class as a string.</param>
        /// <returns>A list of intent target class names.</returns>
        public static List<string> GetIntentsFromClass(string classContent)
        {
            // Define the regular expression to match Intent class usage
            string pattern = @"new\s+Intent\([\w\.]+\s*,\s*([\w\.]+)\.class\)";

            // Create a list to hold the extracted class names
            List<string> intents = new List<string>();

            // Perform the regex matching
            MatchCollection matches = Regex.Matches(classContent, pattern);
            foreach (Match match in matches)
            {
                // Extract the class name from the first capturing group
                if (match.Groups.Count > 1)
                {
                    if (!intents.Contains(match.Groups[1].Value))
                        intents.Add(match.Groups[1].Value);
                }
            }

            return intents;
        }



        static string GetSvgAsString(Graph drawingGraph)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            var svgWriter = new SvgGraphWriter(writer.BaseStream, drawingGraph);
            svgWriter.Write();
            // get the string from MemoryStream
            ms.Position = 0;
            var sr = new StreamReader(ms);
            return sr.ReadToEnd();
        }
        public static void SaveSvgStringAsPng(string exportPath, Graph drawingGraph)
        {
            SaveSvgStringAsPng(exportPath, GetSvgAsString(drawingGraph));
        }
        public static void SaveSvgStringAsPng(string exportPath, string svgString)
        {
            string svgPath = Path.Combine(Path.GetDirectoryName(exportPath), "graph.svg");

            File.WriteAllText(svgPath, svgString);
/*            var svgDocument = SvgDocument.Open(svgPath);
            var bitmap = svgDocument.Draw();

            if (!Path.HasExtension(exportPath))
                exportPath += ".png";
            bitmap.Save(exportPath, ImageFormat.Png);*/
        }


        public static void MyAddNode(this Graph drawingGraph, string name)
        {
            drawingGraph.AddNode(name);
            var node = drawingGraph.FindNode(name);
            node.Label.Width = 10;
            node.Label.Height = 40;

        }
    }
}
