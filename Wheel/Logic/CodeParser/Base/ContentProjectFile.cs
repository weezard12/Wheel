using System;
using System.IO;
using Wheel.Logic.CodeParser.enums;

namespace Wheel.Logic.CodeParser.Base
{
    public class ContentProjectFile : ProjectFile, IContent, IDescription
    {
        public string Content { get; set; }
        public string Path { get; private set; }
        public string InFolder { get; private set; }
        public string Description { get; set; }

        public bool Included { get; set; } = true;

        public ContentProjectFile(string path) : base(path)
        {
            Path = path;
            SetInFolder();

            SetContentBasedOnPath(path);
        }

        public ContentProjectFile(string path, string content = "") : base(path)
        {
            Path = path;
            SetInFolder();

            Content = content;
        }

        private void SetContentBasedOnPath(string path)
        {
            try
            {
                Content = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                Content = ex.Message;
            }
        }

        private void SetInFolder()
        {
            InFolder = System.IO.Path.GetDirectoryName(Path) ?? string.Empty;
        }

        public bool Open()
        {
            try
            {
                File.Open(Path, FileMode.Open);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
