using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wheel.Logic
{
    internal static class MyUtils
    {
        #region Files Utils
        public static string FileFromTemp(string filename)
        {
            return Path.Combine(MauiProgram.TempPath, filename);
        }

        public static void CreateFolderIfDoesntExist(string path, bool clearDirectory = false)
        {
            //just in case
            if (path.Equals("Program Files") || Path.GetDirectoryName(path).Equals("C:\\") || path.Length < 15)
                return;

            try
            {
                //if the path is to a file it will call the method again but with the file folder as path
                if (Path.HasExtension(path))
                {
                    CreateFolderIfDoesntExist(Path.GetDirectoryName(path), clearDirectory);
                    return;
                }

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    return;
                }
                if (clearDirectory)
                {
                    Directory.Delete(path, true);
                    Directory.CreateDirectory(path);
                }
            }
            catch
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    File.Delete(path);
                    CreateFolderIfDoesntExist(path, clearDirectory);
                }
            }
        }
        public static void CreateFileIfDoesntExist(string path, string content = "")
        {
            try
            {
                // If the path contains a directory structure, ensure it exists
                string directoryPath = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directoryPath))
                {
                    CreateFolderIfDoesntExist(directoryPath);
                }

                File.WriteAllText(path, content);


            }
            catch (Exception ex)
            {
                throw; // Optionally rethrow the exception if needed
            }
        }

        public static void DebugLog(params string[] message)
        {
            string sout = String.Empty;
            foreach (string s in message)
            {
                sout += s +"\n";
            }
            CreateFileIfDoesntExist(FileFromTemp("log.txt"), sout);
        }

        #endregion

        #region Views Utils

        public static Page GetParentPage(this View contentView, bool recursive = false)
        {
            Element current = contentView;

            while (current != null)
            {
                if (current is Page page)
                    return page;

                if (recursive && current.Parent is ContentView parentContentView)
                {
                    // Use recursion to check for the parent of the parent
                    return parentContentView.GetParentPage(true);
                }

                current = current.Parent;
            }

            return null; // Parent Page not found
        }

        public static void DebugAlert(this View view, string message)
        {
            GetParentPage(view,true).DisplayAlert("Debug Message", message, "OK");
        }
        public static void DebugAlert(this Page page, string message)
        {
            page.DisplayAlert("Debug Message", message, "OK");
        }
        #endregion
    }
}
