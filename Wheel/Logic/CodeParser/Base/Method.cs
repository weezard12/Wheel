using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    public class Method : INameable
    {
        public string Name { get; set; }
        public List<Variable> Parameters { get; set; } = new List<Variable>();

        public override string ToString()
        {
            string parameterString = string.Join(", ", Parameters.Select(p => $"{p.Type} {p.Name}"));
            return $"{Name}({parameterString})";
        }
    }
}
