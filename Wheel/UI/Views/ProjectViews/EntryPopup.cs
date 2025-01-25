using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.UI.Views.ProjectViews
{
    internal class EntryPopup : Popup
    {
        public EntryPopup()
        {
            // Entry field for input
            var entry = new Entry
            {
                Placeholder = "Enter your text",
                WidthRequest = 250,
                HorizontalOptions = LayoutOptions.Center
            };

            // Button to close the popup
            var closeButton = new Button
            {
                Text = "Close",
                HorizontalOptions = LayoutOptions.Center
            };
            closeButton.Clicked += (s, e) =>
            {
                // Close the popup and return the Entry text as a result
                Close(entry.Text);
            };

            // Layout for the popup content
            var stackLayout = new VerticalStackLayout
            {
                Spacing = 10,
                Padding = new Thickness(20),
                Children = { entry, closeButton }
            };

            Content = stackLayout; // Set the content of the popup
        }
    }
}
