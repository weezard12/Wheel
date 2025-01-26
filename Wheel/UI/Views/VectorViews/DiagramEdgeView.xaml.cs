namespace Wheel.UI;

using Microsoft.Msagl.Drawing;
using Wheel.Logic.Graphs;
using Wheel.UI.Views.VectorViews;

public partial class DiagramEdgeView : ContentView, IFlowDiagram
{
    private Edge _edge;

    private Edge _doubleSideEdge;

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
        foreach (var nodeId in _edge.GetAllPossibleConnections(DiagramGraph))
            EdgeTo.Items.Add(nodeId);


        UpdateOtherSideNode();
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

    private void DoubleSideEdge_Clicked(object sender, EventArgs e)
    {
        if (_doubleSideEdge == null)
        {
            DiagramGraph.AddEdge(_edge.Target, _edge.Source);
        }
        else
        {
            DiagramGraph.RemoveEdge(_doubleSideEdge);
        }
        FlowDiagram.UpdateVectorDisplay();
        FlowDiagram.SetNodesBasedOnDiagram();
    }

    private void UpdateOtherSideNode()
    {
        _doubleSideEdge = _edge.GetOtherSideEdge(DiagramGraph);
        if (_doubleSideEdge == null)
            DoubleSideEdge.Text = "-->";
        else
            DoubleSideEdge.Text = "<-->";
    }
}