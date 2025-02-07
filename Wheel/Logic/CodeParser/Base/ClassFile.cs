using System.Text.RegularExpressions;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    // load all the class names content and methods
    // setup the class extension list
    //
    public class ClassFile : ContentProjectFile
    {
        public List<Variable> Variables { get; set; } = new List<Variable>();
        public List<Method> Methods { get; set; } = new List<Method>();

        /// <summary>
        /// List of all the classes that this class extends from.
        /// </summary>
        public List<ProjectFile> ExtentsFrom { get; set; }

        public ClassFile(string fileName, string content) : base(fileName, content)
        {
            SetupVariablesFromContent();
            SetupMethodsFromContent();
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

            return classFile;
        }

        private void SetupVariablesFromContent()
        {
            Variables.Clear();

            // Remove all method bodies before processing
            string classContent = Regex.Replace(Content, @"\{[^{}]*\}", "{}");

            // Regular Expression to capture Java class-level variable declarations
            string pattern = @"(?:private|protected|public|static|\s)*\s*(\w+)\s+(\w+)\s*(?:=\s*([^;]+))?;";
            Regex regex = new Regex(pattern);

            MatchCollection matches = regex.Matches(classContent);
            foreach (Match match in matches)
            {
                if (match.Groups.Count >= 3)
                {
                    Variables.Add(new Variable
                    {
                        Type = match.Groups[1].Value,   // Data type (e.g., int, String)
                        Name = match.Groups[2].Value,   // Variable name
                        BaseValue = match.Groups[3].Success ? match.Groups[3].Value.Trim() : null // Initial value if exists
                    });
                }
            }
        }

        private void SetupMethodsFromContent()
        {
            Methods.Clear();

            // Regular Expression to match Java methods
            string pattern = @"(?:public|private|protected|static|\s)*\s+(\w+)\s+(\w+)\s*\(([^)]*)\)\s*\{?";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(Content);

            foreach (Match match in matches)
            {
                if (match.Groups.Count >= 3)
                {
                    List<Variable> parameters = new List<Variable>();
                    string parametersString = match.Groups[3].Value.Trim();

                    if (!string.IsNullOrEmpty(parametersString))
                    {
                        string[] paramPairs = parametersString.Split(',');

                        foreach (string param in paramPairs)
                        {
                            string[] parts = param.Trim().Split(' ');
                            if (parts.Length == 2)
                            {
                                parameters.Add(new Variable
                                {
                                    Type = parts[0],
                                    Name = parts[1]
                                });
                            }
                        }
                    }

                    Methods.Add(new Method
                    {
                        Name = match.Groups[2].Value, // Method name
                        Parameters = parameters
                    });
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Class Name: {0}, Path: {1}. Content: \n{2}", Name, Path, Content);
        }
    }
}
