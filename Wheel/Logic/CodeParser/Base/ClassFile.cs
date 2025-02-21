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

            // Remove method bodies to exclude local variables
            string classContent = Regex.Replace(Content, @"\b(public|private|protected|static|\s)*\s+\w+\s+\w+\s*\([^)]*\)\s*\{[^{}]*\}", "");

            // Regular Expression to capture Java class-level variable declarations
            string pattern = @"(?:private|protected|public|static|final|\s)+\s+(\w+)\s+(\w+)\s*(?:=\s*([^;]+))?\s*;";

            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(classContent);

            foreach (Match match in matches)
            {
                if (match.Groups.Count >= 3)
                {
                    string type = match.Groups[1].Value;
                    string name = match.Groups[2].Value;

                    // Ignore return statements
                    if (name.Equals("return", StringComparison.OrdinalIgnoreCase) || type.Equals("return", StringComparison.OrdinalIgnoreCase))
                        continue;

                    Variables.Add(new Variable
                    {
                        Type = type,   // Data type (e.g., int, String)
                        Name = name,   // Variable name
                        BaseValue = match.Groups[3].Success ? match.Groups[3].Value.Trim() : null // Initial value if exists
                    });
                }
            }
        }


        private void SetupMethodsFromContent()
        {
            Methods.Clear();

            // Regular Expression to match Java methods with opening '{'
            string pattern = @"(?:public|private|protected|static|\s)*\s+(\w+)\s+(\w+)\s*\(([^)]*)\)\s*\{";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(Content);

            foreach (Match match in matches)
            {
                if (match.Groups.Count >= 3)
                {
                    string returnType = match.Groups[1].Value;
                    string methodName = match.Groups[2].Value;
                    string parametersString = match.Groups[3].Value.Trim();

                    // **Skip Control Flow Statements**
                    if (Regex.IsMatch(methodName, @"^(if|for|while|switch|catch|try|do|else)$", RegexOptions.IgnoreCase))
                        continue;

                    // **Extract the Full Method Body**
                    string methodBody = ExtractMethodBody(Content, match.Index);

                    // **Detect Simple Getters and Setters**
                    if (IsSimpleGetter(methodName, methodBody) || IsSimpleSetter(methodName, methodBody))
                        continue;

                    // Parse method parameters
                    List<Variable> parameters = new List<Variable>();
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
                        Name = methodName,
                        Parameters = parameters,
                        Content = methodBody // Store the full method body
                    });
                }
            }
        }

        /// <summary>
        /// Extracts the full method body, handling nested brackets.
        /// </summary>
        private string ExtractMethodBody(string content, int startIndex)
        {
            int openBraces = 0;
            int endIndex = -1;

            for (int i = startIndex; i < content.Length; i++)
            {
                if (content[i] == '{') openBraces++;
                if (content[i] == '}') openBraces--;

                if (openBraces == 0 && endIndex == -1 && i > startIndex)
                {
                    endIndex = i;
                    break;
                }
            }

            if (endIndex > startIndex)
            {
                return content.Substring(startIndex, endIndex - startIndex + 1).Trim();
            }

            return string.Empty;
        }


        /// <summary>
        /// Checks if a method is a simple getter (e.g., `getX() { return x; }`).
        /// </summary>
        private bool IsSimpleGetter(string methodName, string methodBody)
        {
            if (!methodName.StartsWith("get")) return false;

            // Match: return <some_variable>;
            return Regex.IsMatch(methodBody, @"^\s*return\s+\w+;\s*$");
        }

        /// <summary>
        /// Checks if a method is a simple setter (e.g., `setX(value) { this.x = value; }`).
        /// </summary>
        private bool IsSimpleSetter(string methodName, string methodBody)
        {
            if (!methodName.StartsWith("set")) return false;

            // Match: this.<some_variable> = parameter;
            return Regex.IsMatch(methodBody, @"^\s*this\.\w+\s*=\s*\w+;\s*$");
        }

        public override string ToString()
        {
            return string.Format("Class Name: {0}, Path: {1}. Content: \n{2}", Name, Path, Content);
        }
    }
}
