namespace Wheel.UI.Views.ProjectViews;

using Page = Wheel.Logic.Docx.Jsons.Page;
public partial class DataPageView : ContentView
{
    Page DataPage { get; set; }

    List<ProjectValueEntryView> PageEntries { get; set; } = new List<ProjectValueEntryView>();

    public DataPageView(Page dataPage)
    {
        InitializeComponent();
		
        this.DataPage = dataPage;
        PageNameXml.Text = dataPage.Name;

        UpdateValueEntries();
    }
    private void UpdateValueEntries()
    {
        EntriesLayout.Children.Clear();
        foreach (var value in DataPage.Values)
        {
            ProjectValueEntryView projectValueEntry = new ProjectValueEntryView(value);
            PageEntries.Add(projectValueEntry);
            EntriesLayout.Children.Add(projectValueEntry);
        }
    }

}