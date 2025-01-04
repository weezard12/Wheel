namespace Wheel.UI;

public partial class ProjectValueEntryView : ContentView
{
    public string ValueName { get; private set; }
    public string Value { get; private set; }

    public string EntryPreview { get; private set; }
    public int EntryMinRows { get; private set; }
    public string HelpMessage { get; private set; }

    public ProjectValueEntryView(string valueName, string value = "", string entryPreview = "", int entryMinRows = 1, string helpMessage = "")
    {
        InitializeComponent();
        ValueName = valueName;
        Value = value;
        EntryPreview = entryPreview;
        EntryMinRows = entryMinRows;
        HelpMessage = helpMessage;

        ValueNameXml.Text = valueName + ":";

        ValueEntryXml.Text = value;
        ValueEntryXml.Placeholder = entryPreview;
        ValueEntryXml.Placeholder = entryPreview;
        if(entryMinRows > 1)
        {
            Grid.SetColumn(ValueEntryXml,0);
            Grid.SetRow(ValueEntryXml,1);
        }
    }
}