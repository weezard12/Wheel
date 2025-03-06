using Wheel.UI.Views.AIViews;
using Wheel.UI.Views.ProjectViews;

namespace Wheel.UI;

public partial class AITextViewsHolder : ContentView, IAIViewHolder
{
	public List<IAIViewHolder> AIViews { get; set; } = new List<IAIViewHolder>();


	public AITextViewsHolder()
	{
		InitializeComponent();
	}

    private void GenerateAll_Clicked(object sender, EventArgs e)
    {
        Generate_Clicked(sender, e);
    }
    public void AddAITextView(IAIViewHolder holder)
    {
        AIViews.Add(holder);
        if(holder is View view)
            EntriesLayout.Children.Add(view);
    }
    public void AddAITextView(AITextView textView)
	{
		AIViews.Add(textView);
		EntriesLayout.Children.Add(textView);
	}
    public void RemoveAITextView(AITextView textView)
    {
        AIViews.Remove(textView);
        EntriesLayout.Children.Remove(textView);
    }

    public void RemoveAllAITextView()
    {
		AIViews.Clear();
		EntriesLayout.Children.Clear();
    }

    public void Generate_Clicked(object sender, EventArgs e)
    {
        foreach (IAIViewHolder viewHolder in AIViews)
        {
            viewHolder.Generate_Clicked(sender, e);
        }
    }
}