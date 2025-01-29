using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Wheel.Logic.AI;

namespace Wheel.UI.Views.ProjectViews;

public partial class AITextView : ContentView
{
    public virtual string Prompt { get; set; }
    public string AIResponse { get; set; }
	public AITextView()
	{
		InitializeComponent();
	}

    private async void Copy_Clicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(OutputText.Text);

        // Use Snackbar instead of Toast (Works on Windows)
        var snackbar = Snackbar.Make("Text copied to clipboard!", null, "OK", TimeSpan.FromSeconds(2));
        await snackbar.Show();
    }

    private void Generate_Clicked(object sender, EventArgs e)
    {
        Generate();
        if(Prompt != null)
            GenerateButton.Text = "Regenerate";
    }
    public virtual async void Generate()
    {
        if (Prompt == null || Prompt.Equals(String.Empty))
        {
            return;
        }
        AIResponse = GeminiAPI.GetFullTextFromResponse(await GeminiAPI.GetGeminiResponse(Prompt));
        OutputText.Text = AIResponse;
    }
}