using Wheel.Logic.Projects;
using static Wheel.Logic.Docx.Jsons;

namespace Wheel.UI;

public partial class ProjectValueEntryView : ContentView
{
    Value EntryValue { get; set; }

    public ProjectValueEntryView(Value value)
    {
        InitializeComponent();
        this.EntryValue = value;

        ValueNameXml.Text = value.DisplayName;

        ValueEntryXml.Text = value.CurrentValue;
        ValueEntryXml.Placeholder = value.BaseValue;
    }

    private void ValueEntryXml_TextChanged(object sender, TextChangedEventArgs e)
    {
        EntryValue.CurrentValue = ValueEntryXml.Text;
        AndroidStudioProject.CurrentProject.SaveConfig();
    }
}