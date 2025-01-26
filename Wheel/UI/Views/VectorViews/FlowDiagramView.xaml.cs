using Microsoft.Msagl.Drawing;
using SvgLayerSample.Svg;
using Wheel.Logic.Graphs;
using static Wheel.Logic.MyUtils;

namespace Wheel.UI;

public partial class FlowDiagramView : ContentView
{
	private string _graphName;
	public Graph Graph { get; private set; }

	// list of all of the nodes in the graph.
	List<DiagramNodeView> ConnectionsNodes;
	public FlowDiagramView(string graphName, Graph graph)
	{
		InitializeComponent();
		
		_graphName = graphName;
		Graph = graph;

		VectorDisplay.Source = FileFromTemp(graphName);

		SetNodesBasedOnDiagram();
    }
	public void SetNodesBasedOnDiagram()
	{
        ConnectionsNodes = new List<DiagramNodeView>();
        ConnectionNodesXml.Children.Clear();
        foreach (Node node in Graph.Nodes)
        {
            DiagramNodeView diagramNode = new DiagramNodeView(this, node);
            ConnectionsNodes.Add(diagramNode);

			ConnectionNodesXml.Children.Add(diagramNode);
        }
    }

	public void UpdateVectorDisplay()
	{
		Diagram doc = new Diagram(Graph);
        doc.Run();
        GraphsUtils.SaveSvg(Path.Combine(Path.GetTempPath(), Path.Combine("Wheel", _graphName)), doc.ToString());

        VectorDisplay.Source = FileFromTemp(_graphName);
        VectorDisplay.Reload();
		
    }
    private void OnWebViewNavigated(object sender, WebNavigatedEventArgs e)
    {

    }
}