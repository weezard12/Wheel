using Wheel.Logic.Projects;

namespace Wheel.UI;

public partial class CreateNewProjectView : ContentView
{
	public CreateNewProjectView()
	{
		InitializeComponent();
	}

    private void CreateProject_Clicked(object sender, EventArgs e)
    {
		if (ProjectType.SelectedIndex == 0)
		{
            AndroidStudioProject project = new AndroidStudioProject(NameEntry.Text);
            AndroidStudioTemplate.Project = project;
            Shell.Current.GoToAsync(nameof(AndroidStudioTemplate));
        }
    }
}