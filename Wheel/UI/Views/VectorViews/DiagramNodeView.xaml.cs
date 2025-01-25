using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Msagl.Drawing;

namespace Wheel.UI;

public partial class DiagramNodeView : FlowDiagramContent
{
	private Node _node;
	private Graph _graph;

	public DiagramNodeView(FlowDiagramView view,Node node) : base(view)
	{
		InitializeComponent();
		_node = node;

		NodeName.Text = _node.LabelText;

		foreach (Edge edge in _node.Edges)
		{
            Connections.Children.Add(new DiagramEdgeView(edge));
        }
		
	}

}