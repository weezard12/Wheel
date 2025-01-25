using Wheel.UI;

namespace Wheel
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

            //for testing
            Shell.Current.GoToAsync(nameof(HomePage));
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {

            SemanticScreenReader.Announce(CounterBtn.Text);
            Shell.Current.GoToAsync(nameof(HomePage));
        }
    }

}
