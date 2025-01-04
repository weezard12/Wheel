using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.UI;

namespace Wheel.Logic.Projects
{
    public abstract partial class ProjectBase : ObservableObject
    {
        [ObservableProperty]
        string name;

        private List<ProjectValueEntryView> entries = new List<ProjectValueEntryView>();

        public ContentPage ProjectPage { get; set; }
        public ProjectBase()
        {

        }
        public ProjectBase(string ProjectName)
        {
            this.name = ProjectName;
        }
        public abstract void SetupUI();
        protected void AddEntry(ProjectValueEntryView entryView)
        {
            entries.Add(entryView);
            entryView.Margin = 5;
            ProjectPage.FindByName<VerticalStackLayout>("EntriesStackLayout").Add(entryView);
        }
    }
}
