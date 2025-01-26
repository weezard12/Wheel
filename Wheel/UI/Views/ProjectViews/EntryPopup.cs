using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.UI.Views.ProjectViews
{
    internal class EntryPopup : Popup
    {
        private INameable _value;
        private Entry _entry;

        public EntryPopup(INameable value)
        {
            _value = value;

            // Entry field for input
            _entry = new Entry
            {
                Placeholder = "Enter your text",
                WidthRequest = 250,
                HorizontalOptions = LayoutOptions.Center
            };

            // Button to confirm
            var confirmButton = new Button
            {
                Text = "Confirm",
                HorizontalOptions = LayoutOptions.Center
            };
            confirmButton.Clicked += OnConfirmClicked;

            // Button to close the popup
            var closeButton = new Button
            {
                Text = "Close",
                HorizontalOptions = LayoutOptions.Center
            };
            closeButton.Clicked += (s, e) => Close();

            // Layout for the popup content
            var stackLayout = new VerticalStackLayout
            {
                Spacing = 10,
                Padding = new Thickness(20),
                Children = { _entry, confirmButton, closeButton }
            };

            Content = stackLayout; // Set the content of the popup
        }

        private void OnConfirmClicked(object sender, EventArgs e)
        {
            _value.Name = _entry.Text; // Set the value to the entry text
            Close();
        }
    }
}