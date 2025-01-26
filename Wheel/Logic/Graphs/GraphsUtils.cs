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
using Microsoft.Msagl.GraphmapsWithMesh;
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
            var doc = new Diagram(GetScreenGraph(classFiles));
            doc.Run();
            return doc;
        }
        public static Graph GetScreenGraph(List<ClassFile> classFiles)
        {
            var drawingGraph = new Graph();
            foreach (var classFile in classFiles)
            {
                if (classFile.ExtentsFrom.Contains(new SourceClass("AppCompatActivity")))
                {
                    if (drawingGraph.FindNode(classFile.Name) == null)
                        drawingGraph.AddNode(new ComponentNode(classFile.Name, technology: "extends AppCompatActivity"));

                    List<string> strings = GetIntentsFromClass(classFile.Content);
                    foreach (string s in strings)
                    {
                        if (drawingGraph.FindNode(s) == null)
                            drawingGraph.AddNode(new ComponentNode(s, technology: "extends AppCompatActivity"));

                        drawingGraph.AddEdge(classFile.Name, "", s);
                    }

                }
            }

            return drawingGraph;
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

        public static void SaveSvg(string exportPath, string svgString)
        {
            MyUtils.CreateFileIfDoesntExist(exportPath, svgString);
        }


        public static void ReplaceNodeWithNewLabel(Graph graph, Microsoft.Msagl.Drawing.Node oldNode, string newNodeId, string newLabel)
        {

            if (oldNode == null)
            {
                throw new ArgumentException($"Node with ID '{oldNode}' not found in the graph.");
            }

            // Create a new node with the updated label
            Microsoft.Msagl.Drawing.Node newNode = new Microsoft.Msagl.Drawing.Node(newNodeId)
            {
                LabelText = newLabel,
                Attr = oldNode.Attr.Clone(), // Copy the attributes (like color, shape, etc.)
                UserData = oldNode.UserData  // Copy user data, if any
            };

            // Add the new node to the graph
            graph.AddNode(newNode);

            // Copy all outgoing edges from the old node to the new node
            foreach (Microsoft.Msagl.Drawing.Edge edge in graph.Edges)
            {
                if (edge.Source == oldNode.Id)
                {
                    graph.AddEdge(newNodeId, edge.LabelText, edge.Target);
                }
            }

            // Copy all incoming edges to the new node
            foreach (Microsoft.Msagl.Drawing.Edge edge in graph.Edges)
            {
                if (edge.Target == oldNode.Id)
                {
                    graph.AddEdge(edge.Source, edge.LabelText, newNodeId);
                }
            }

            // Remove the old node
            graph.RemoveNode(oldNode);
        }

        public static string[] GetAllPossibleConnections(this Microsoft.Msagl.Drawing.Node node, Graph graph)
        {
            // Use a HashSet for fast lookups of currentNodeEdges
            var currentNodeEdgeSet = new HashSet<Microsoft.Msagl.Drawing.Node>(
                node.Edges.Select(e => e.TargetNode)
            );

            // Use LINQ to filter nodes not in the currentNodeEdgeSet
            var result = graph.Nodes
                .Where(node => !currentNodeEdgeSet.Contains(node))
                .Select(node => node.Id)
                .ToArray();

            return result;
        }
        public static string[] GetAllPossibleConnections(this Microsoft.Msagl.Drawing.Edge edge, Graph graph)
        {
            return GetAllPossibleConnections(edge.SourceNode, graph);
        }

        public static Microsoft.Msagl.Drawing.Edge? GetOtherSideEdge(this Microsoft.Msagl.Drawing.Edge edge, Graph graph)
        {
            foreach (Microsoft.Msagl.Drawing.Node node in graph.Nodes)
            {
                foreach (Microsoft.Msagl.Drawing.Edge otherEdge in node.Edges)
                {
                    if(otherEdge.Target.Equals(edge.Source) && otherEdge.Source.Equals(edge.Target))
                        return otherEdge;
                }
            }
            return null;
        }
    }
}
