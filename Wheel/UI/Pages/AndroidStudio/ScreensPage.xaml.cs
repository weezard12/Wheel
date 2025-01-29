using Microsoft.Msagl.Drawing;
using SvgLayerSample.Svg;
using Wheel.Logic;
using Wheel.Logic.CodeParser.Base;
using Wheel.Logic.Graphs;
using Wheel.UI.Views.ProjectViews;
using Wheel.UI.Views.ProjectViews.AndroidStudio;
using static Wheel.Logic.Projects.AndroidStudioProject;

namespace Wheel.UI.Pages.AndroidStudio;

public partial class ScreensPage : ContentPage
{
	public ScreensPage()
	{
		InitializeComponent();

        CurrentProject.OnProjectLoaded += (project) =>
        {

            Graph graph = GraphsUtils.GetScreenGraph(project.ClassFiles);
            Diagram doc = GraphsUtils.GetScreenDiagram(project.ClassFiles);
            GraphsUtils.SaveSvg(Path.Combine(Path.GetTempPath(), "Wheel\\screens_diagram.svg"), doc.ToString());

            EntriesStackLayout.Children.Add(new FlowDiagramView("screens_diagram.svg", graph));

            foreach (ClassFile screen in project.Screens)
            {
                EntriesStackLayout.Children.Add(new ScreenAITextView(screen));
            }
            
        };
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

    }
    
}