using System.ComponentModel;
using Wheel.Logic.Projects;

namespace Wheel.UI;

public partial class AndroidStudioTemplate : ContentPage
{
    public static AndroidStudioProject Project { get; set; }
    public AndroidStudioTemplate()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Project.ProjectPage = this;
        Project.SetupUI();
    }
}