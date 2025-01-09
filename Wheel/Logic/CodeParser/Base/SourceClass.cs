using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    public class SourceClass : INameable
    {
        public string Name { get; set; }
        public SourceClass(string name)
        {
            Name = name;
        }
        public override string ToString()
        {
            return String.Format("Source Class, Name: {0}",Name);
        }
        public override bool Equals(object? obj)
        {
            if (obj != null)
                if(obj is string)
                    return Name.Equals(obj);
            if(obj is SourceClass sourceClass)
                return sourceClass.Name.Equals(this.Name);
            
            return base.Equals(obj);
        }
    }
}
