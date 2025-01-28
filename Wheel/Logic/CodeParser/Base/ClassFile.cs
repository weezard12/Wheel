using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    // load all the class names content and methods
    // setup the class extension list
    //
    public class ClassFile : ContentProjectFile
    {
        List<Variable> Variables { get; set; } = new List<Variable>();
        List<Method> Methods { get; set; } = new List<Method>();

        /// <summary>
        /// List of all the classes that this class extends from.
        /// </summary>
        public List<ProjectFile> ExtentsFrom { get; set; }

        public ClassFile(string fileName, string content) : base(fileName, content)
        {
            
        }

        public static ClassFile GetClassFromText(string fileContent)
        {
            // Regex patterns for parsing
            var classNamePattern = @"class\s+(\w+)";
            var variablePattern = @"(?:public|private|protected)?\s+(?:final|static)?\s*\w+\s+(\w+)\s*;";
            var methodPattern = @"(?:public|private|protected)?\s+(?:final|static)?\s*\w+\s+(\w+)\s*\(([^)]*)\)";

            // Class Name
            var classNameMatch = System.Text.RegularExpressions.Regex.Match(fileContent, classNamePattern);
            string className = classNameMatch.Success ? classNameMatch.Groups[1].Value : "UnknownClass";

            // Create ClassFile instance
            var classFile = new ClassFile(className, fileContent);

            // Variables
            var variableMatches = System.Text.RegularExpressions.Regex.Matches(fileContent, variablePattern);
            foreach (System.Text.RegularExpressions.Match match in variableMatches)
            {
                if (match.Success)
                {
                    var variable = new Variable
                    {
                        Name = match.Groups[1].Value
                    };
                    classFile.Variables.Add(variable);
                }
            }

            // Methods
            var methodMatches = System.Text.RegularExpressions.Regex.Matches(fileContent, methodPattern);
            foreach (System.Text.RegularExpressions.Match match in methodMatches)
            {
                if (match.Success)
                {
                    var method = new Method
                    {
                        Name = match.Groups[1].Value,
                        Parameters = match.Groups[2].Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                        .Select(p => new Variable { Name = p.Trim().Split(' ').Last() })
                                        .ToList()
                    };
                    classFile.Methods.Add(method);
                }
            }

            return classFile;
        }

        public override string ToString()
        {
            return string.Format("Class Name: {0}, Path: {1}. Content: \n{2}", Name, Path, Content);
        }
    }
}
