using CommunityToolkit.Maui.Views;
using Microsoft.Msagl.Drawing;
using SvgLayerSample.Svg;
using Wheel.Logic;
using Wheel.Logic.Graphs;
using Wheel.UI.Views.ProjectViews;
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

            if (SearchEntry == null || SearchEntry.Text == null)
            {
                DiagramNodeView diagramNode = new DiagramNodeView(this, node);
                ConnectionsNodes.Add(diagramNode);

                ConnectionNodesXml.Children.Add(diagramNode);
            }
            else 
            {
                if (node.Id.ToLower().Contains(SearchEntry.Text.ToLower()))
                {
                    DiagramNodeView diagramNode = new DiagramNodeView(this, node);
                    ConnectionsNodes.Add(diagramNode);

                    ConnectionNodesXml.Children.Add(diagramNode);
                }
            }


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

    private async void NewNode_Clicked(object sender, EventArgs e)
    {
        EntryPopup entryPopup = new EntryPopup();

        try
        {
            string newNodeName = (string)await this.GetParentPage().ShowPopupAsync(entryPopup);

            Graph.AddNode(new ComponentNode(newNodeName));
            
            UpdateVectorDisplay();
            SetNodesBasedOnDiagram();
        }
        catch (Exception ex)
        {
            this.DebugAlert(ex.Message);
        }

    }

    private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        SetNodesBasedOnDiagram();
    }
}