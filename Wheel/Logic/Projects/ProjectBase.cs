using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;
using Wheel.Logic.CodeParser.enums;
using Wheel.UI;
using Wheel.UI.Views.ProjectViews;

namespace Wheel.Logic.Projects
{
    public abstract partial class ProjectBase : INameable
    {
        public ProjectBase CurrentProject { get; protected set; }

        public string Name { get; set; }

        public bool IsProjectLoaded { get; protected set; }

        public List<ProjectFile> AllProjectFiles { get; set; } = new List<ProjectFile>();
        public virtual List<ClassFile> ClassFiles { get; set; } = new List<ClassFile>();
        public List<ContentProjectFile> ResourceFiles { get; set; } = new List<ContentProjectFile>();

        public ProjectBase(string ProjectName)
        {
            Name = ProjectName;
            CurrentProject = this;
        }
        public abstract void SetupAllProjectFiles(string folderPath);
    }
}
