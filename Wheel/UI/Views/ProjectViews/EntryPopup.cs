using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.UI.Views.ProjectViews
{
    internal class EntryPopup : ValueSelectionPopup
    {
        private INameable _value;
        private Entry _entry;

        public EntryPopup()
        {
            SetupUI();
        }

        public EntryPopup(INameable value)
        {
            _value = value;
            SetupUI();
        }

        protected override void SetupUI()
        {
            // Entry field for input
            _entry = new Entry
            {
                Placeholder = "Enter your text",
                WidthRequest = 250,
                HorizontalOptions = LayoutOptions.Center
            };

            PopupLayout.Add(_entry);
            base.SetupUI();
        }
        protected override void OnConfirmClicked(object sender, EventArgs e)
        {
            if(_value != null)
                _value.Name = _entry.Text; // Set the value to the entry text
            Close(_entry.Text);
        }
    }
}