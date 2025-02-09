namespace Wheel.UI;

public partial class WheelIcon : ContentView
{
	public WheelIcon()
	{
		InitializeComponent();
        CogImage.Loaded += (a, b) => StartCogAnimation();
        BackCogImage.Loaded += (a, b) => StartBackCogAnimation();
	}
    private async void StartCogAnimation()
    {
        while (CogImage.IsVisible) // Infinite loop for continuous rotation
        {
            await CogImage.RotateTo(360, 6000, Easing.SinInOut); // 1 second per full rotation
            CogImage.Rotation = 0; // Reset to prevent overflow issues
        }
    }
    private async void StartBackCogAnimation()
    {
        while (CogImage.IsVisible) // Infinite loop for continuous rotation
        {
            await BackCogImage.RotateTo(-360, 8000, Easing.SinInOut); // 1 second per full rotation
            BackCogImage.Rotation = 0; // Reset to prevent overflow issues
        }
    }
    private async void OnImageTapped(object sender, EventArgs e)
    {
        string url = "https://ko-fi.com/weezard12";
        await Launcher.OpenAsync(url);
    }


}