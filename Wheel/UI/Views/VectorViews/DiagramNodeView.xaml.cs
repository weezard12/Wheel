using CommunityToolkit.Maui.Views;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Msagl.Drawing;
using Wheel.Logic.CodeParser.enums;
using Wheel.UI.Views.ProjectViews;
using Wheel.UI.Views.VectorViews;
using Page = Microsoft.Maui.Controls.Page;

namespace Wheel.UI;

public partial class DiagramNodeView : ContentView, IFlowDiagram, INameable
{
	private Node _node;

    public FlowDiagramView FlowDiagram { get; set; }
    public Graph DiagramGraph => FlowDiagram.Graph;

    public string Name { get => _node.LabelText; set => RenameNode(value); }

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

    private void RenameNode(string name)
    {
        if (_node == null || DiagramGraph == null)
            return;
        _node.LabelText = name;
        _node.Label.Text = name;
        _node.Id = name;
        
        
        GetParentPage().DisplayAlert("debug",DiagramGraph.FindNode(_node.Id).Id,"a");
        FlowDiagram.UpdateVectorDisplay();
        FlowDiagram.SetNodesBasedOnDiagram();
    }

    private void NodeName_Clicked(object sender, EventArgs e)
    {
        var popup = new EntryPopup(this);
        
        // Get the parent Page (e.g., ContentPage)
        Page parentPage = this.GetParentPage();

        // Show the popup
        parentPage?.ShowPopup(popup);
    }
    private Page GetParentPage()
    {
        Element current = this;
        while (current != null)
        {
            if (current is Page page)
                return page;
            current = current.Parent;
        }
        return null; // Parent Page not found
    }
}