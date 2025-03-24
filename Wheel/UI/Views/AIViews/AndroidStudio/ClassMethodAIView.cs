using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;
using static Wheel.Logic.Docx.Jsons;
using static Wheel.Logic.Projects.AndroidStudioProject;

namespace Wheel.UI.Views.AIViews.AndroidStudio
{
    class ClassMethodAIView : AITextView
    {
        public override string Prompt { get => GetPrompt(); }

        private ClassFile ClassFile { get; set; }

        private readonly string _pageNameInJson;
        private readonly string _methodName;

        public ClassMethodAIView(ClassFile classFile, string methodToString) : base(methodToString)
        {
            ClassFile = classFile;
            _methodName = methodToString;
            _pageNameInJson = "class_" + ClassFile.Name;
            OnGeneratedValidResponse += (responce) =>
            {
                if (CurrentProject.Root.GetDocxPageByID(_pageNameInJson) != null)
                {
                    CurrentProject.Root.GetDocxPageByID(_pageNameInJson).SetTableValueByName("methods_table", methodToString, responce);
                    CurrentProject.SaveConfig();
                }

                Method method = classFile.Methods.FirstOrDefault(n => n.ToString() == methodToString);
                if (method != null)
                    method.Description = responce;
            };
        }


        private string GetPrompt()
        {
            string instructions =
@"Analyze the following Java method and provide a concise summary of its purpose and role within the class.
Clearly define its type, intended usage, and significance within the application's logic.
Explain how it interacts with other components but avoid discussing specific operations or implementation details.
Begin the response immediately without introductory phrases or uncertain language.
Ensure that all statements are definitive and assertive, without using words like 'it likely' or 'probably'.
Method: ";
            return instructions + _methodName + "\nClass: " + ClassFile.Content;
        }


    }
}

