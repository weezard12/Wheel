namespace Wheel.UI.Pages.AndroidStudio;
using static Wheel.Logic.Projects.AndroidStudioProject;
using static Wheel.Logic.Docx.Jsons;
using Page = Wheel.Logic.Docx.Jsons.Page;
using Wheel.Logic.Docx;
using static Wheel.Logic.Docx.IDocxPage;
using Wheel.Logic.CodeParser.Base;

public partial class ClassesPage : ContentPage
{
    private ClassView _classView;

    public ClassesPage()
    {
        InitializeComponent();

        CurrentProject.OnProjectLoaded += (project) =>
        {
            LoadProjectFirst.IsVisible = false;

            foreach (ClassFile classFile in project.ClassFiles)
            {

                _classView = new ClassView(classFile);
                EntriesGrid.Add(_classView);

                TableValue variablesTable  = new TableValue() { Name = "properties_table", Row2 = classFile.Variables.Select(v => v.ToString()).ToList() };
                variablesTable.Row1.Insert(0,"תיאור התכונה");
                variablesTable.Row2.Insert(0,"שם התכונה");

                TableValue methodsTable = new TableValue() { Name = "methods_table", Row2 = classFile.Methods.Select(m => m.ToString()).ToList() };
                methodsTable.Row1.Insert(0, "תיאור הפעולה");
                methodsTable.Row2.Insert(0, "שם הפעולה");

                CurrentProject.Root.Pages.Add(new Page()
                {
                    ID = "class_" + classFile.Name,
                    Name = "Class Page",
                    Index = (int)PageType.ClassesPage,
                    Values = new List<ValueBase> {
                        new Value() { Name="class_name", CurrentValue = classFile.Name},
                        new Value() { Name="class_description", CurrentValue = classFile.Description, BaseValue = "No description generated"},


                        variablesTable,
                        methodsTable
                    }
                });
            }
            CurrentProject.SaveConfig();
        };
    }

}