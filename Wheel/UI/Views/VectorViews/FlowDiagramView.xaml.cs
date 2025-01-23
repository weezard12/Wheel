using Microsoft.Msagl.Drawing;
using SvgLayerSample.Svg;
using static Wheel.Logic.MyUtils;

namespace Wheel.UI;

public partial class FlowDiagramView : ContentView
{
	private string _graphName;
	private Graph _graph;

	// list of all of the nodes in the graph.
	List<DiagramConnectionView> ConnectionsNodes;
	public FlowDiagramView(string graphName, Graph graph)
	{
		InitializeComponent();

		_graphName = graphName;
		_graph = graph;

		VectorDisplay.Source = FileFromTemp(graphName);

        ConnectionsNodes = new List<DiagramConnectionView>();
		
		foreach (Node node in _graph.Nodes)
		{
			ConnectionsNodes.Add(new DiagramConnectionView(node));

        }

    }
}