namespace Wheel.UI.Pages.AndroidStudio;
using static Wheel.Logic.Projects.AndroidStudioProject;
using static Wheel.Logic.Docx.Jsons;
using Page = Wheel.Logic.Docx.Jsons.Page;
using Wheel.Logic.Docx;
using static Wheel.Logic.Docx.IDocxPage;
using Wheel.UI.Views.ProjectViews.AndroidStudio;
using Wheel.Logic.CodeParser.Base;

public partial class ClassesPage : ContentPage
{
    public ClassesPage()
    {
        InitializeComponent();

        CurrentProject.OnProjectLoaded += (project) =>
        {
            foreach (ClassFile classFile in project.ClassFiles)
            {
                TableValue variablesTable  = new TableValue() { Name = "properties_table", Row2 = classFile.Variables.Select(v => v.ToString()).ToList() };
                variablesTable.Row1.Insert(0,"תיאור התכונה");
                variablesTable.Row2.Insert(0,"שם התכונה");

                CurrentProject.Root.Pages.Add(new Page()
                {
                    ID = "class_" + classFile.Name,
                    Name = "Class Page",
                    Index = (int)PageType.ClassPage,
                    Values = new List<ValueBase> {
                        new Value() { Name="class_name", CurrentValue = classFile.Name},
                        new Value() { Name="class_description", CurrentValue = classFile.Name},


                        variablesTable
                    }
                });
            }
            CurrentProject.SaveConfig();
        };
    }

}