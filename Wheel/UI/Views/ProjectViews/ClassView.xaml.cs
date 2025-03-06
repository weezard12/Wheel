using Microsoft.Maui.Controls;
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
        tableView.OverritePrompt = String.Format(
@"Analyze the following Java class and its variables. For each variable, provide a concise summary of its purpose and role within the class and replace its description.

Clearly define its type, intended usage, and significance within the application's logic.
Explain how it interacts with other components but avoid discussing specific operations or implementation details.
Begin the response immediately without introductory phrases or uncertain language.
Ensure that all statements are definitive and assertive, without using words like 'it likely' or 'probably'.
The response must be a valid JSON object, formatted as follows:

{{
  ""variables"": [
    {{
      ""name"": ""variable name"",
      ""type"": ""variable type"",
      ""description"": ""A detailed and assertive explanation of the variable’s type, purpose, and role in the application.""
    }}
  ]
}}

Java Class:
{0}

List of Variables:
{1}

Example Input for a two Variables:
{{
  ""variables"": [
    {{
      ""name"": ""goal"",
      ""type"": ""int"",
      ""description"": """"
    }},
    {{
        ""name"": ""difficulty"",
        ""type"": ""String"",
        ""description"": """"
    }}
  ]
}}

Expected JSON Response:

{{
  ""variables"": [
    {{
      ""name"": ""goal"",
      ""type"": ""int"",
      ""description"": ""`goal` is an integer that represents the target value required to achieve a specific achievement. It defines the numeric milestone which the application tracks. The `goal` interacts with other parts of the application by providing a threshold against which progress or performance is measured, ultimately determining when an achievement is unlocked.""
    }},
    {{
        ""name"": ""difficulty"",
        ""type"": ""String"",
        ""description"": ""`difficulty` is a String that represents the current game difficulty level (e.g., \""easy\"", \""medium\"", \""hard\""). It influences game parameters such as board size and number of mines and affects leaderboard placement.""
    }}
  ]
}}", ClassFile.GetClassWithoutImports(), ClassFile.GetVariablesAsJson());

        tableView.UseOverritePrompt = true;

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