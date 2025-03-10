using Microsoft.Maui;
using System.Text.Json;
using Wheel.Logic;
using Wheel.Logic.AI;
using Wheel.Logic.CodeParser.Base;
using Wheel.Logic.Docx;
using Wheel.UI.Views.AIViews;

namespace Wheel.UI;

public partial class AITableView : ContentView, IAIViewHolder
{
    public List<IAIViewHolder> AIViews { get; set; } = new List<IAIViewHolder>();
	public TableView GetTable() => Table;
	public string OverritePrompt { get; set; }
	public bool UseOverritePrompt { get; set; }

    public AITableView()
	{
		InitializeComponent();
	}


	public void SetTitle(string title)
	{
		Root.Title = title;
	}

    public void AddSection()
    {
		AddSection(new TableSection());
    }
    public void AddSection(TableSection section)
	{
		Root.Add(section);
	}

	public void AddAIView(int section, AITextView AIView)
	{
		AIViews.Add(AIView);
        Root[section].Add(new ViewCell() { View = AIView});
		
	}

    private void ToggleTableViewClicked(object sender, EventArgs e)
    {
		Table.IsVisible = !Table.IsVisible;
    }

    public async void Generate_Clicked(object sender, EventArgs e)
    {
		if(!String.IsNullOrEmpty(OverritePrompt))
		{
			if (UseOverritePrompt)
			{
                string jsonResponce = await GeminiAPI.GetGeminiResponse(OverritePrompt);
                string responce = GeminiAPI.GetFullTextFromResponse(jsonResponce);
				if (responce.StartsWith("```json"))
				{
                    responce = responce.Substring(8, responce.Length-11);
                }

				bool triedFixingJson = false;
				parseJson:
				try
				{
					ClassFile classFile = JsonSerializer.Deserialize<ClassFile>(responce);
                    MyUtils.DebugLog(responce);
                    foreach (IAIViewHolder viewHolder in AIViews)
					{
						if (viewHolder is AITextView textView)
						{
							foreach (Variable variable in classFile.Variables)
							{
								if (textView.Title.Equals(variable.ToString()))
								{
									textView.SetOutputText(variable.Description);
									textView.OnGeneratedValidResponse?.Invoke(variable.Description);
									break;
								}

							}

						}
					}


				}
				catch (Exception ex)
				{
					
					if (triedFixingJson)
						return;
					triedFixingJson = true;
					responce = Jsons.EscapeJsonString(responce);
					goto parseJson;
				}
                    
                return;
			}
		}

        foreach (IAIViewHolder viewHolder in AIViews)
            viewHolder.Generate_Clicked(sender, e);

    }
}