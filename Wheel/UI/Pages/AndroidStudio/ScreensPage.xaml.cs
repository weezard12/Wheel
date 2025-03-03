using Microsoft.Msagl.Drawing;
using SvgLayerSample.Svg;
using Wheel.Logic;
using Wheel.Logic.CodeParser.Base;
using Wheel.Logic.Graphs;
using Wheel.UI.Views.ProjectViews;
using static Wheel.Logic.Projects.AndroidStudioProject;
using static Wheel.Logic.Docx.Jsons;
using Page = Wheel.Logic.Docx.Jsons.Page;
using Wheel.Logic.Docx;
using static Wheel.Logic.Docx.IDocxPage;
using Wheel.UI.Views.AIViews.AndroidStudio;

namespace Wheel.UI.Pages.AndroidStudio;

public partial class ScreensPage : ContentPage
{
    public FlowDiagramView ScreensFlowDiagram { get; private set; }
    public AITextViewsHolder TextViewsHolder { get; private set; }


    public ScreensPage()
	{
		InitializeComponent();

        CurrentProject.OnProjectLoaded += (project) =>
        {

            Graph graph = GraphsUtils.GetScreenGraph(project.ClassFiles);
            Diagram doc = GraphsUtils.GetScreenDiagram(project.ClassFiles);
            GraphsUtils.SaveSvg(Path.Combine(Path.GetTempPath(), "Wheel\\screens_diagram.svg"), doc.ToString());

            ScreensFlowDiagram = new FlowDiagramView("screens_diagram.svg", graph);
            EntriesGrid.Add(ScreensFlowDiagram);
            EntriesGrid.SetRow(ScreensFlowDiagram, 0);

            project.Root.Pages.Add(new Page()
            {
                Name = "Screens Diagram",
                Index = (int) PageType.ScreensDiagramPage,
                Values = new List<ValueBase>()
            {
                new Value()
                {
                    CurrentValue = "Path\\screens_diagram.svg"
                }
            }
            });


            TextViewsHolder = new AITextViewsHolder();
            EntriesGrid.Add(TextViewsHolder);
            EntriesGrid.SetRow(TextViewsHolder, 1);

            foreach (ClassFile screen in project.Screens)
            {
                TextViewsHolder.AddAITextView(new ScreenAITextView(screen));
                CurrentProject.Root.Pages.Add(new Page()
                {
                    ID="screen_" + screen.Name,
                    Name="Screen Page",
                    Index = (int) PageType.ScreensAboutPage,
                    Values = new List<ValueBase> {
                        new Value() { Name="screen_name", CurrentValue = screen.Name},
                        new Value() { Name="screen_description", CurrentValue = "No description generated"}
                    }
                });
            }
            CurrentProject.SaveConfig();

        };
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

    }
    
}