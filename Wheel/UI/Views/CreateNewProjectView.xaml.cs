using Wheel.Logic.Projects;
using static Wheel.Logic.MyUtils;

namespace Wheel.UI;

public partial class CreateNewProjectView : ContentView
{
	public CreateNewProjectView()
	{
		InitializeComponent();
        ProjectType.SelectedIndex = 0;
	}

    private async void CreateProject_Clicked(object sender, EventArgs e)
    {
		if (ProjectType.SelectedIndex == 0)
		{
            AndroidStudioProject project = new AndroidStudioProject(NameEntry.Text);
            await CopyLocalFileAsync("Templates\\android studio.json", AndroidStudioProject.ProjectConfig);
            await Shell.Current.GoToAsync(nameof(AndroidStudioTemplate));
        }
    }
}