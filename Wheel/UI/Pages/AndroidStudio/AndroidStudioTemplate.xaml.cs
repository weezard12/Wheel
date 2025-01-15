using Wheel.Logic.CodeParser.Base;
using Wheel.Logic.Graphs;
using Wheel.Logic.Projects;
using static Wheel.Logic.CodeParser.AndroidStudioParser;

namespace Wheel.UI;

public partial class AndroidStudioTemplate : ContentPage
{
    public static AndroidStudioProject Project { get; set; }

    public AndroidStudioTemplate()
    {
        InitializeComponent();
        UploadProjectXml.OnSelectedFileSet = new Action<Exception?>(OnProjectFolderUploaded);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Project.ProjectPage = this;
        Project.SetupUI();
    }

    private void OnProjectFolderUploaded(Exception? ex)
    {
        if (ex != null)
        {
            DisplayAlert("Cant load project file",ex.Message,"OK");
            return;
        }

        DisplayAlert("Project Loaded", "Flow diagram is being generated", "OK");

        // generate the svg
        string projectPath = UploadProjectXml.SelectedFolderPath;
        string sourceCodePath = GetSourceCodePath(projectPath);

        List<ClassFile> classNames = GetClassNames(sourceCodePath);
        SetExtensionList(classNames);

        SvgLayerSample.Svg.Diagram doc = GraphsUtils.GetScreenDiagram(classNames);
        //GraphsUtils.SaveSvgStringAsPng(Path.Combine(MauiProgram.TestingFolder, "graphImage"), doc.ToString());

    }

}