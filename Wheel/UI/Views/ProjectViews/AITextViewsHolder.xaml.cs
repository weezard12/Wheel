namespace Wheel.UI.Views.ProjectViews;

public partial class AITextViewsHolder : ContentView
{
	public  List<AITextView> TextViews { get; private set; } = new List<AITextView>();
	public AITextViewsHolder()
	{
		InitializeComponent();
	}

    private void GenerateAll_Clicked(object sender, EventArgs e)
    {
		foreach (AITextView textView in TextViews)
		{
			textView.Generate();
		}
    }
	public void AddAITextView(AITextView textView)
	{
		TextViews.Add(textView);
		EntriesLayout.Children.Add(textView);
	}
    public void RemoveAITextView(AITextView textView)
    {
        TextViews.Remove(textView);
        EntriesLayout.Children.Remove(textView);
    }
}