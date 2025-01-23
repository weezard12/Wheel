namespace Wheel.UI.Views.ProjectViews;

public partial class DataPageView : ContentView
{
	public string PageName { get; set; }

	List<ProjectValueEntryView> PageEntries { get; set; }

	public DataPageView()
	{
		InitializeComponent();
	}
    public DataPageView(string pageName)
    {
        InitializeComponent();
		this.PageName = pageName;
    }

    public void SetEntriesByDocx(string documentPath, int idx)
	{
		
	}
}