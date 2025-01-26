namespace Wheel.UI;

using Microsoft.Msagl.Drawing;
using Wheel.UI.Views.VectorViews;

public partial class DiagramEdgeView : ContentView, IFlowDiagram
{
    private Edge _edge;

    private bool _initialized;

    public FlowDiagramView FlowDiagram { get; set; }
    public Graph DiagramGraph => FlowDiagram.Graph;

    public DiagramEdgeView(FlowDiagramView flowDiagram, Edge edge)
	{
		InitializeComponent();

        _edge = edge;
        FlowDiagram = flowDiagram;

        SetupEdgesItems();

        _initialized = true;
    }

    private void SetupEdgesItems()
    {
        // Sets the current edge
        EdgeTo.Items.Clear();
        EdgeTo.Items.Add(_edge.TargetNode.Label.Text);
        EdgeTo.SelectedIndex = 0;

        // Adds other nodes as an option for an edge
        Node[] currentNodeEdges = _edge.SourceNode.Edges
        .Select(edge => edge.TargetNode)
        .Distinct()                      
        .ToArray();            
        
        foreach (var node in DiagramGraph.Nodes)
            if (!currentNodeEdges.Contains(node))
                EdgeTo.Items.Add(node.LabelText);
        
    }

    private void RemoveEdge_Clicked(object sender, EventArgs e)
    {
        if (_edge == null || DiagramGraph == null)
            return;

        DiagramGraph.RemoveEdge(_edge);

        FlowDiagram.UpdateVectorDisplay();
        FlowDiagram.SetNodesBasedOnDiagram();
    }

    private void EdgeTo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(!_initialized)
            return;

        DiagramGraph.AddEdge(_edge.Source, EdgeTo.Items[EdgeTo.SelectedIndex]);
        DiagramGraph.RemoveEdge(_edge);

        FlowDiagram.UpdateVectorDisplay();
        FlowDiagram.SetNodesBasedOnDiagram();
    }
}