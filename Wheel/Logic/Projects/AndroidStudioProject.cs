using Aspose.Words.LowCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser;
using Wheel.Logic.CodeParser.Base;
using Wheel.UI;
using static Wheel.Logic.Docx.Jsons;
using static Wheel.Logic.MyUtils;

namespace Wheel.Logic.Projects
{
    public class AndroidStudioProject : ProjectBase
    {
        public static new AndroidStudioProject CurrentProject { get; private set; }

        public const string FinalFileName = "Final Prodect.docx";
        public static string FinalFilePath => FileFromTemp(FinalFileName);
        public static string ProjectConfig => FileFromTemp("android studio.json");
        public static string ProjectConfigString => File.ReadAllText(ProjectConfig);

        private DocxRoot _root;
        public DocxRoot Root { get => GetDocxRoot(); private set => _root = value; }

        public override List<ClassFile> ClassFiles { get => GetClassFiles(); set => base.ClassFiles = value; }
        public List<ClassFile> Screens { get => GetScreens(); }

        public string Path { get; private set; }
        public string SourceCodePath { get; private set; }

        public event Action <AndroidStudioProject> OnProjectLoaded;


        public AndroidStudioProject(string ProjectName) : base(ProjectName)
        {
            CurrentProject = this;
        }

        public override void SetupAllProjectFiles(string folderPath)
        {
            this.Path = folderPath;
            this.SourceCodePath = AndroidStudioParser.GetSourceCodePath(folderPath);

            string[] files = Directory.GetFiles(SourceCodePath, "*.*", SearchOption.AllDirectories);
            
            AllProjectFiles.Clear();
            foreach (var file in files)
            {
                ContentProjectFile projectFile;
                switch (System.IO.Path.GetExtension(file))
                {
                    case ".java":
                        projectFile = ClassFile.GetClassFromText(File.ReadAllText(file));
                        break;

                    default:
                        projectFile = new ContentProjectFile(file);
                        break;
                }
                AllProjectFiles.Add(projectFile);
            }
            AndroidStudioParser.SetExtensionList(ClassFiles);
            MyUtils.DebugLog(ClassFiles.Select(a => a.Name).ToArray());
            IsProjectLoaded = true;
            OnProjectLoaded.Invoke(this);
        }

        private List<ClassFile> GetClassFiles()
        {
            List<ClassFile> classes = new List<ClassFile>();
            foreach (var file in AllProjectFiles)
            {
                if(file is ClassFile)
                {
                    classes.Add((ClassFile)file);
                }
            }
            return classes;
        }

        private List<ClassFile> GetScreens() {
            List<ClassFile> screens = new List<ClassFile>();
            foreach (ClassFile classFile in ClassFiles)
                if(classFile.ExtentsFrom.Contains(new SourceClass("AppCompatActivity")))
                    screens.Add(classFile);

            return screens;
        }

        public void SaveConfig()
        {
            var options = new JsonSerializerOptions()
            {
                Converters = { new ValueBaseConverter() }
            };
            string json = JsonSerializer.Serialize<DocxRoot>(Root, options);
            File.WriteAllText(ProjectConfig, json);
        }


        public DocxRoot GetDocxRoot()
        {
            if (_root == null)
            {

                var options = new JsonSerializerOptions()
                {
                    Converters = { new ValueBaseConverter() }
                };
                _root = JsonSerializer.Deserialize<DocxRoot>(ProjectConfigString, options);

            }

            return _root;    
        }
        public void UpdateDocxRoot()
        {
            var options = new JsonSerializerOptions()
            {
                Converters = { new ValueBaseConverter() }
            };
            _root = JsonSerializer.Deserialize<DocxRoot>(ProjectConfigString, options);
        }

    }
}
