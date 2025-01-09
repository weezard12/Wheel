using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    internal class Method : INameable
    {
        public string Name { get; set; }
        public List<Variable> Parameters { get; set; }

    }
}
