using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;

namespace Wheel.UI.Views.ProjectViews.AndroidStudio
{
    internal class ScreenAITextView : AITextView
    {
        public override string Prompt { get => GetPrompt(); }

        private ClassFile ClassFile { get; set; }

        public ScreenAITextView(ClassFile classFile) : base(classFile.Name)
        {
            this.ClassFile = classFile;
        }


        private string GetPrompt()
        {
            string instructions = 
@"You are about to receive a Java screen class (extends from AppCompatActivity) created in Android Studio.
You must describe the screen in a summery as an explanation for the user based on the instructions provided and nothing more.
Do not:
- Give explanation technical android studio logic.
- Do not explain the factuality of already existing classes in android studio.
- Do not start the message with something like ""Sure, here is a summary of the ""Screen Name"" class for the user:"", instead just start the summery.
For the screen, you must write:
• Title/name/role of the screen
• Description of the screen – what information is displayed, what can be done on it
• For each display element (button, text box, etc.), their role must be explained

Simple Example:
`TicTacToGameActivity
- The game activity is the main activity of the game where the most logic is.
- In the game activity you can play the Tic Tac To game.
- There are 9 buttons in the screen each for one tile of the Tic Tac To.
`
The screen class:";

            return instructions + ClassFile.Content;
        }
    }
}
