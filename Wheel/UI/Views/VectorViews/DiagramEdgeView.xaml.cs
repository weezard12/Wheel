namespace Wheel.UI;

using Microsoft.Msagl.Drawing;

public partial class DiagramEdgeView : ContentView
{
    private Edge _edge;
    private Graph _graph;

	public DiagramEdgeView(Edge edge)
	{
		InitializeComponent();

		EdgeTo.Items.Clear();
		EdgeTo.Items.Add(edge.TargetNode.Label.Text);
		EdgeTo.SelectedIndex = 0;
	}

    private void RemoveEdge_Clicked(object sender, EventArgs e)
    {
/*        if (_currentEdge == null || _graph == null)
        {
            DisplayAlert("Error", "Edge or graph is null.", "OK");
            return;
        }

        // Remove the edge from the graph
        string sourceNodeId = _currentEdge.Source;
        string targetNodeId = _currentEdge.Target;

        if (_graph.Edges.FirstOrDefault(e => e.Source == sourceNodeId && e.Target == targetNodeId) != null)
        {
            _graph.RemoveEdge(sourceNodeId, targetNodeId);

            // Optionally update the UI to reflect changes
            DisplayAlert("Success", $"Edge from {sourceNodeId} to {targetNodeId} removed.", "OK");
        }
        else
        {
            DisplayAlert("Error", "Edge not found in the graph.", "OK");
        }*/
    }
}