using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Wheel.Logic.AI;

namespace Wheel.UI;

public partial class AITextView : ContentView
{
    public virtual string Prompt { get; set; }
    public string AIResponse { get; set; }

    public string Title { get => TitleXml.Text; set => SetTitle(value); }

    protected Action<string> OnGeneratedValidResponse { get; set; }

	public AITextView()
	{
		InitializeComponent();
	}
    public AITextView(string title, string prompt = "") : this()
    {
        this.Title = title;
        this.Prompt = prompt;
    }

    private async void Copy_Clicked(object sender, EventArgs e)
    {
        await Clipboard.SetTextAsync(OutputText.Text);

        // Use Snackbar instead of Toast (Works on Windows)
        try
        {
            var snackbar = Snackbar.Make("Text copied to clipboard!", null, "OK", TimeSpan.FromSeconds(2));
            await snackbar.Show();
        }
        catch (Exception ex) { }

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
        OutputText.Text = String.IsNullOrEmpty(AIResponse) ? "Error when getting AI response." : AIResponse;
        OnGeneratedValidResponse.Invoke(OutputText.Text);
    }

    private void SetTitle(string value)
    {
        if (value.Equals(String.Empty))
        {
            TitleXml.Text = "";
            TitleXml.IsVisible = false;
            return;
        }
        TitleXml.Text = value;
        TitleXml.IsVisible = true;
        
    }
}