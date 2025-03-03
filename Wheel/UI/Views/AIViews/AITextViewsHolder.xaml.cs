using Wheel.UI.Views.AIViews;
using Wheel.UI.Views.ProjectViews;

namespace Wheel.UI;

public partial class AITextViewsHolder : ContentView, IAIViewHolder
{
	public List<AITextView> AIViews { get; set; } = new List<AITextView>();
    public List<IAIViewHolder> Holders { get; private set; } = new List<IAIViewHolder>();

	public AITextViewsHolder()
	{
		InitializeComponent();
	}

    public List<AITextView> GetAllAIViews()
    {
        var uniqueViews = new HashSet<AITextView>(AIViews); // Add AIViews from this holder

        if (Holders != null)
        {
            foreach (var holder in Holders)
            {
                if (holder?.AIViews != null)
                {
                    foreach (var view in holder.AIViews)
                    {
                        uniqueViews.Add(view); // Add views from Holders while preventing duplicates
                    }
                }
            }
        }

        return uniqueViews.ToList();
    }

    private void GenerateAll_Clicked(object sender, EventArgs e)
    {
		foreach (AITextView textView in GetAllAIViews())
		{
			textView.Generate();
		}
    }
    public void AddAITextView(IAIViewHolder holder)
    {
        Holders.Add(holder);
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
}