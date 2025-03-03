using Microsoft.Maui;
using Wheel.UI.Views.AIViews;

namespace Wheel.UI;

public partial class AITableView : ContentView, IAIViewHolder
{
    public List<AITextView> AIViews { get; set; } = new List<AITextView>();
	public TableView GetTable() => Table;

    public AITableView()
	{
		InitializeComponent();
	}


	public void SetTitle(string title)
	{
		Root.Title = title;
	}

    public void AddSection()
    {
		AddSection(new TableSection());
    }
    public void AddSection(TableSection section)
	{
		Root.Add(section);
	}

	public void AddAIView(int section, AITextView AIView)
	{
		AIViews.Add(AIView);
        Root[section].Add(new ViewCell() { View = AIView});
		
	}

    private void ToggleTableViewClicked(object sender, EventArgs e)
    {
		Table.IsVisible = !Table.IsVisible;
    }
}