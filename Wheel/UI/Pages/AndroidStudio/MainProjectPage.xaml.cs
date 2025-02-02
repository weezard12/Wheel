using Wheel.Logic.Projects;
using static Wheel.Logic.Projects.AndroidStudioProject;
using static Wheel.Logic.Docx.Jsons;
using Page = Wheel.Logic.Docx.Jsons.Page;
using Wheel.UI.Views.ProjectViews;

namespace Wheel.UI.Pages.AndroidStudio;

public partial class MainProjectPage : ContentPage
{
	public MainProjectPage()
	{
		InitializeComponent();
        UploadProjectXml.OnSelectedFileSet = new Action<Exception?>(OnProjectFolderUploaded);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        ProjectName.Text = $"Current Project: {CurrentProject.Name}";

        Page mainPage = CurrentProject.Root.GetDocxPage("Main Page");
        EntriesStackLayout.Children.Clear();
        EntriesStackLayout.Children.Add(new DataPageView(mainPage));
    }
    private void OnProjectFolderUploaded(Exception? ex)
    {
        if (ex != null)
        {
            DisplayAlert("Cant load project file",ex.Message,"OK");
            return;
        }

        DisplayAlert("Project Loading", "Enjoy the Auto Generated Project", "W! OK");

        CurrentProject.SetupAllProjectFiles(UploadProjectXml.SelectedFolderPath);
/*        // generate the svg
        string projectPath = UploadProjectXml.SelectedFolderPath;
        string sourceCodePath = GetSourceCodePath(projectPath);

        List<ClassFile> classNames = GetClassNames(sourceCodePath);
        SetExtensionList(classNames);

        Graph graph = GraphsUtils.GetScreenGraph(classNames);
        Diagram doc = GraphsUtils.GetScreenDiagram(classNames);
        GraphsUtils.SaveSvg(Path.Combine(Path.GetTempPath(),"Wheel\\screens_diagram.svg"), doc.ToString());

        Project.SetupProjectClasses(classNames);
        EntriesStackLayout.Children.Add(new FlowDiagramView("screens_diagram.svg", graph));*/
    }
}