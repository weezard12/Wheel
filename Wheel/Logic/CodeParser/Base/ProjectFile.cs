using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    public class ProjectFile : INameable
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public ProjectFile(string fileName)
        {
            string fileNameWithoutPath = Path.GetFileName(fileName);

            Extension = Path.GetExtension(fileNameWithoutPath)?.TrimStart('.') ?? string.Empty; // Remove dot from extension
            Name = Path.GetFileNameWithoutExtension(fileNameWithoutPath) ?? fileNameWithoutPath;
        }
        public override string ToString()
        {
            return String.Format("Source Class, Name: {0}", Name);
        }
        public override bool Equals(object? obj)
        {
            if (obj != null)
                if (obj is string)
                    return Name.Equals(obj);
            if (obj is SourceClass sourceClass)
                return sourceClass.Name.Equals(this.Name);

            return base.Equals(obj);
        }
    }
}
