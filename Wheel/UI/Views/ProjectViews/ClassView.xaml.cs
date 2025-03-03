using Microsoft.Msagl.Core.ProjectionSolver;
using Wheel.Logic.CodeParser.Base;
using Wheel.UI.Views.AIViews.AndroidStudio;

using Variable = Wheel.Logic.CodeParser.Base.Variable;

namespace Wheel.UI;

public partial class ClassView : ContentView
{
	ClassFile ClassFile { get; set; }

	public ClassView(ClassFile classFile)
	{
		InitializeComponent();

		this.ClassFile = classFile;
		UpdateClasView();
	}

	public void UpdateClasView()
	{
        ClassName.Text = ClassFile.Name;

        AIViewsHolder.AddAITextView(new ClassDescriptionAIView(ClassFile));

        AITableView tableView = new AITableView();
        tableView.GetTable().IsVisible = false;


        //TableView.SetTitle("Values");
        tableView.AddSection(new TableSection("Values"));
/*		if(ClassFile.Variables.Count > 0)
			TableView.AddAIView(0, new ClassPropertyAIView(ClassFile, ClassFile.Variables[0].Name));
        if (ClassFile.Variables.Count > 1)
            TableView.AddAIView(0, new ClassPropertyAIView(ClassFile, ClassFile.Variables[1].Name));*/

        foreach (Variable variable in ClassFile.Variables)
        {
            tableView.AddAIView(0, new ClassPropertyAIView(ClassFile, variable.ToString()));
		}
        AIViewsHolder.AddAITextView(tableView);
    }
}