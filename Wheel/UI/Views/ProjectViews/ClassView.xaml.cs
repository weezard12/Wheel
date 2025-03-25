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

        // Variables Table
        AITableView variablesTableView = new AITableView("Toggle Variables Table");
        variablesTableView.GetTable().IsVisible = false;
        variablesTableView.OverritePrompt = String.Format(
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

        variablesTableView.UseOverritePrompt = true;

        variablesTableView.AddSection(new TableSection("Values"));

        foreach (Variable variable in ClassFile.Variables)
        {
            variablesTableView.AddAIView(0, new ClassPropertyAIView(ClassFile, variable.ToString()));
		}
        AIViewsHolder.AddAITextView(variablesTableView);
        variablesTableView.Padding = new Thickness(0, 0, 0, 10);

        // Methods Table
        AITableView methodsTableView = new AITableView("Toggle Methods Table");
        methodsTableView.GetTable().IsVisible = false;
        methodsTableView.OverritePrompt = String.Format(
@"Analyze the following Java class and its methods. For each method, provide a concise summary of its purpose and role within the class and replace its description.

Clearly define its return type, parameters, intended functionality, and significance within the application's logic.
Explain how it interacts with other components but avoid discussing specific operations or implementation details.
Begin the response immediately without introductory phrases or uncertain language.
Ensure that all statements are definitive and assertive, without using words like 'it likely' or 'probably'.
The response must be a valid JSON object, formatted as follows:

{{
  ""methods"": [
    {{
      ""name"": ""method name"",
      ""description"": ""A detailed and assertive explanation of the method’s purpose, functionality, and role in the application.""
    }}
  ]
}}

Java Class:
{0}

List of Methods:
{1}

Example Input for two Methods:
{{
  ""methods"": [
    {{
      ""name"": ""calculateScore"",
      ""returnType"": ""int"",
      ""parameters"": [
        {{
          ""name"": ""points"",
          ""type"": ""int""
        }},
        {{
          ""name"": ""multiplier"",
          ""type"": ""double""
        }}
      ],
      ""description"": """"
    }},
    {{
      ""name"": ""resetGame"",
      ""returnType"": ""void"",
      ""parameters"": [],
      ""description"": """"
    }}
  ]
}}

Expected JSON Response:

{{
  ""methods"": [
    {{
      ""name"": ""calculateScore"",
      ""description"": ""`calculateScore` is a method that computes the final score based on the given points and multiplier. It takes an integer `points`, representing base points, and a double `multiplier`, which scales the points based on performance. The method returns an integer representing the calculated score. It plays a key role in determining the player's final score after an action or game session.""
    }},
    {{
      ""name"": ""resetGame"",
      ""description"": ""`resetGame` is a method that resets all game parameters to their initial state. It clears the current game state and prepares the system for a new session. This method is crucial for restarting a game without needing to reload the application.""
    }}
  ]
}}"
, ClassFile.GetClassWithoutImports(), ClassFile.GetMethodsAsJson());

        methodsTableView.UseOverritePrompt = true;

        methodsTableView.AddSection(new TableSection("Methods"));

        foreach (Method method in ClassFile.Methods)
        {
            methodsTableView.AddAIView(0, new ClassMethodAIView(ClassFile, method.ToString()));
        }
        AIViewsHolder.AddAITextView(methodsTableView);
        methodsTableView.Padding = new Thickness(0, 0, 0, 10);
    }
}