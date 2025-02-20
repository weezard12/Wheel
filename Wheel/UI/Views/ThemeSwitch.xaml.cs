namespace Wheel.UI;

public partial class ThemeSwitch : ContentView
{
	public ThemeSwitch()
	{
		InitializeComponent();
        LoadThemePreference();
	}
    private void OnThemeSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            Preferences.Set("AppTheme", "Dark"); // Save preference
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            Preferences.Set("AppTheme", "Light"); // Save preference
        }
    }

    private void LoadThemePreference()
    {
        string savedTheme = Preferences.Get("AppTheme", "Dark");
        if (savedTheme == "Dark")
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Light;
        }
    }
}
