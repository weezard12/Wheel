using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.UI.Views.ProjectViews
{
    internal class ValueSelectionPopup : Popup
    {
        protected VerticalStackLayout PopupLayout { get; set; } = new VerticalStackLayout();

        public ValueSelectionPopup()
        {
            
        }
        protected virtual void OnConfirmClicked(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Add your UI when overriting and then call the base method
        /// </summary>
        protected virtual void SetupUI()
        {
            // Button to confirm
            var confirmButton = new Button
            {
                Text = "Confirm",
                HorizontalOptions = LayoutOptions.Center
            };

            // Button to close the popup
            var closeButton = new Button
            {
                Text = "Close",
                HorizontalOptions = LayoutOptions.Center
            };

            closeButton.Clicked += (s, e) => Close();
            confirmButton.Clicked += OnConfirmClicked;

            PopupLayout.Add(closeButton);
            PopupLayout.Add(confirmButton);

            Content = PopupLayout; // Set the content of the popup
        }
    }
}
