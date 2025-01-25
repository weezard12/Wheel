namespace Wheel.UI;

using Microsoft.Msagl.Drawing;
using Wheel.UI.Views.VectorViews;

public partial class DiagramEdgeView : ContentView, IFlowDiagram
{
    private Edge _edge;

    public FlowDiagramView FlowDiagram { get; set; }
    public Graph DiagramGraph => FlowDiagram.Graph;

    public DiagramEdgeView(FlowDiagramView flowDiagram, Edge edge)
	{
		InitializeComponent();

        _edge = edge;
        FlowDiagram = flowDiagram;

		EdgeTo.Items.Clear();
		EdgeTo.Items.Add(edge.TargetNode.Label.Text);
		EdgeTo.SelectedIndex = 0;
	}

    private void RemoveEdge_Clicked(object sender, EventArgs e)
    {
        if (_edge == null || DiagramGraph == null)
            return;

        DiagramGraph.RemoveEdge(_edge);

        FlowDiagram.UpdateVectorDisplay();
        FlowDiagram.SetNodesBasedOnDiagram();
    }
}