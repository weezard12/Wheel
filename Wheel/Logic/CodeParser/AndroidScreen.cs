using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser
{
    internal class AndroidScreen : ClassFile
    {
        public string[] ClassesName { get; set; } = { "AppCompatActivity" };

        public List<AndroidScreen> ConectedScreens { get; set; }
        

        public AndroidScreen(string fileName, string content, string path) : base(fileName, content, path)
        {
        }
    }
}
