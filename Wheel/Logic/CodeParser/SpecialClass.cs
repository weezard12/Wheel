using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;

namespace Wheel.Logic.CodeParser
{
    public class SpecialClass
    {
        public string[] CustomClasses { get; set; }
        public Type ClassFile { get; set; }

        public SpecialClass(Type classFile, string customClassName) : this(classFile, new string[] { customClassName })
        {
        }

        public SpecialClass(Type classFile, string[] customClassName)
        {
            this.CustomClasses = customClassName;
            this.ClassFile = classFile;
        }
    }
}
