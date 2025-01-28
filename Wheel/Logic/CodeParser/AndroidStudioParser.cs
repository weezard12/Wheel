using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser.Base;

namespace Wheel.Logic.CodeParser
{
    public static class AndroidStudioParser
    {
        public static List<SpecialClass> SpecialClasses = new List<SpecialClass>() { new SpecialClass(typeof(AndroidScreen), "AppCompactActivity") };
        public static List<ClassFile> GetClassNames(string projectPath)
        {
            if (string.IsNullOrEmpty(projectPath) || !Directory.Exists(projectPath))
            {
                throw new ArgumentException("The provided project path is invalid or does not exist.");
            }

            List<ClassFile> classNames = new List<ClassFile>();
/*            string[] javaFiles = Directory.GetFiles(projectPath, "*.java", SearchOption.AllDirectories);

            Regex classRegex = new Regex(@"\bclass\s+(\w+)", RegexOptions.Compiled);

            foreach (string file in javaFiles)
            {
                string fileContent = File.ReadAllText(file);
                MatchCollection matches = classRegex.Matches(fileContent);

                foreach (Match match in matches)
                {
                    if (match.Groups.Count > 1)
                    {
                        classNames.Add(new ClassFile(match.Groups[1].Value, fileContent, file));
                    }
                }
            }
*/
            return classNames;
        }

        public static void SetExtensionList(List<ClassFile> classes)
        {
            // Regex pattern to extract the class that the current class extends
            var extendsPattern = @"class\s+\w+\s+extends\s+(\w+)";

            // Iterate through all ClassFile objects
            foreach (var classFile in classes)
            {
                // Initialize ExtentsFrom if not already done
                if (classFile.ExtentsFrom == null)
                {
                    classFile.ExtentsFrom = new List<ProjectFile>();
                }

                // Match the "extends" clause in the class content
                var match = Regex.Match(classFile.Content, extendsPattern);
                if (match.Success)
                {
                    // Extract the name of the extended class
                    string extenderName = match.Groups[1].Value;

                    // Find the extender class in the provided classes
                    ProjectFile extenderClass = classes.FirstOrDefault(c => c.Name == extenderName);

                    if (extenderClass == null)
                    {
                        // If extender class doesn't exist, create a SourceClass and add it to the list
                        extenderClass = new SourceClass(extenderName);
                    }

                    // Add the extender class to the ExtentsFrom property
                    classFile.ExtentsFrom.Add(extenderClass);
                }
            }
        }
        public static void SetSpecialClasses(List<ClassFile> classes)
        {
            for (int i = 0; i < classes.Count; i++)
            {
                /*                if(classes[i].ExtentsFrom.Contains(SpecialClasses.All(item => name)){

                                }*/
            }
        }
        public static string GetSourceCodePath(string projectPath)
        {
            return Path.Combine(projectPath, "app\\src\\main\\java");
        }
    }

}
