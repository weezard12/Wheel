using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;
using static Wheel.Logic.Projects.AndroidStudioProject;
using static Wheel.Logic.Docx.Jsons;

namespace Wheel.UI
{
    class ClassDescriptionAIView : AITextView
    {
        public override string Prompt { get => GetPrompt(); }

        private ClassFile ClassFile { get; set; }

        private readonly string _nameInJson;

        public ClassDescriptionAIView(ClassFile classFile) : base("Class Description")
        {
            this.ClassFile = classFile;
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
Focus on the general role of the class within the application, its primary responsibilities, and how it interacts with other components.
Do not include specific method details or implementation logic. The response should start immediately without any introductory phrases.

Class Content:";
            return instructions + ClassFile.Content;
        }

    }
}
