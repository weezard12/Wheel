using System.ComponentModel;
using Wheel.Logic.Projects;
using Wheel.UI.Views;

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

    }

}