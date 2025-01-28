using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.CodeParser;
using Wheel.Logic.CodeParser.Base;
using Wheel.UI;

namespace Wheel.Logic.Projects
{
    public class AndroidStudioProject : ProjectBase
    {
        public static new AndroidStudioProject CurrentProject { get; private set; }

        public override List<ClassFile> ClassFiles { get => GetClassFiles(); set => base.ClassFiles = value; }

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
    }
}
