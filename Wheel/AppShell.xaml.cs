using Wheel.UI;

namespace Wheel
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AndroidStudioTemplate), typeof(AndroidStudioTemplate));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }
    }
}
