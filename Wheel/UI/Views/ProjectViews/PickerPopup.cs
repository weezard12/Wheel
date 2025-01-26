using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.UI.Views.ProjectViews
{
    internal class PickerPopup : ValueSelectionPopup
    {
        private Picker _picker;

        public string[] PickerValues { get; set; }

        public PickerPopup(string[] values) : base()
        {
            PickerValues = values;
        }

        protected override void SetupUI()
        {
            // Entry field for input
            _picker = new Picker
            {
                SelectedIndex = 0,
                WidthRequest = 250,
                HorizontalOptions = LayoutOptions.Center
            };

            if (PickerValues == null)
            {
                Close();
                return;
            }
                
            foreach (var value in PickerValues)
                _picker.Items.Add(value);
            
            PopupLayout.Add(_picker);

            base.SetupUI();
        }

        protected override void OnConfirmClicked(object sender, EventArgs e)
        {
            Close(_picker.Items[_picker.SelectedIndex]);
        }
    }
}
