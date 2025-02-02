using CommunityToolkit.Maui.Storage;

namespace Wheel.UI.Views;

public partial class LoadFolderView : ContentView
{
    public string SelectedFolderPath
    {
        get;
        private set;
    }
    public string SelectedFolderName
    {
        get;
        private set;
    }
    public Action<Exception?> OnSelectedFileSet;
	public LoadFolderView()
	{
		InitializeComponent();
	}
    public LoadFolderView(string buttonText) : this()
    {
        UploadButton.Text = buttonText;
    }
    private async void UploadButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Pick a single file
            var result = await FolderPicker.Default.PickAsync();


            if (result != null)
            {
                if (result.IsSuccessful)
                {
                    SelectedFolderName = result.Folder.Name;
                    SelectedFolderPath = result.Folder.Path;
                }
                OnSelectedFileSet?.Invoke(result.Exception);
            }
        }
        catch (Exception ex)
        {
            OnSelectedFileSet.Invoke(ex);
        }
    }

}