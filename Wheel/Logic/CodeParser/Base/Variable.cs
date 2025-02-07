using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    public class Variable : INameable
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string BaseValue { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", Type, Name);
        }
    }
}
