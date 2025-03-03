using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;
using static Wheel.Logic.Projects.AndroidStudioProject;
using static Wheel.Logic.Docx.Jsons;

namespace Wheel.UI.Views.AIViews.AndroidStudio
{
    class ClassDescriptionAIView : AITextView
    {
        public override string Prompt { get => GetPrompt(); }

        private ClassFile ClassFile { get; set; }

        private readonly string _nameInJson;

        public ClassDescriptionAIView(ClassFile classFile) : base("Class Description")
        {
            ClassFile = classFile;
            _nameInJson = "class_" + ClassFile.Name;
            OnGeneratedValidResponse += (responce) =>
            {
                if (CurrentProject.Root.GetDocxPageByID(_nameInJson) != null)
                {
                    CurrentProject.Root.GetDocxPageByID(_nameInJson).SetValueByName("class_description", AIResponse);
                    CurrentProject.SaveConfig();
                }

                classFile.Description = responce;
            };
        }


        private string GetPrompt()
        {
            string instructions =
@"Analyze the following Java class and provide a concise summary of its purpose and functionality.
Clearly define the class's role within the application, its primary responsibilities, and how it interacts with other components.
Avoid discussing specific method details or implementation logic. Begin the response immediately without introductory phrases or uncertain language.
Ensure that all statements are definitive and assertive, without using words like: ""it likely"" or ""probably"".

Class Content:";
            return instructions + ClassFile.Content;
        }

    }
}
