using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Msagl.Drawing;
using Wheel.UI.Views.ProjectViews;
using Wheel.UI.Views.VectorViews;

namespace Wheel.UI;

public partial class DiagramNodeView : ContentView, IFlowDiagram
{
	private Node _node;

    public FlowDiagramView FlowDiagram { get; set; }
    public Graph DiagramGraph => FlowDiagram.Graph;

    public DiagramNodeView(FlowDiagramView view, Node node)
	{
        FlowDiagram = view;


		InitializeComponent();
		_node = node;

		NodeName.Text = _node.LabelText;

		foreach (Edge edge in _node.Edges)
		{
            Connections.Children.Add(new DiagramEdgeView(view, edge));
        }
        

    }

    private void RemoveNode_Clicked(object sender, EventArgs e)
    {
        if (_node == null || DiagramGraph == null)
            return;

        DiagramGraph.RemoveNode(_node);

        FlowDiagram.UpdateVectorDisplay();
        FlowDiagram.SetNodesBasedOnDiagram();
    }

    private void NodeName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (_node == null || DiagramGraph == null)
            return;

        _node.LabelText = NodeName.Text;

        FlowDiagram.UpdateVectorDisplay();
        FlowDiagram.SetNodesBasedOnDiagram();
    }

    private void NodeName_Clicked(object sender, EventArgs e)
    {
        var popup = new EntryPopup();
        
        var result = 
        
    }
}