using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel.Logic.Projects;


namespace Wheel.Logic
{
    internal static class MyUtils
    {
        #region Files Utils
        public static string FileFromTemp(string filename)
        {
            return Path.Combine(MauiProgram.TempPath, filename);
        }
        public static string FileFromTemplates(string filename)
        {
            return Path.Combine(ProjectBase.TemplatesFolderPath, filename);
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
            string logDirectory = FileFromTemp("Logs");
            CreateFolderIfDoesntExist(logDirectory);

            // Generate timestamped log filename
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string logFilePath = Path.Combine(logDirectory, $"log_{timestamp}.txt");

            // Combine messages into a single string
            string sout = string.Join("\n", message);

            // Write the log message to a new file
            File.WriteAllText(logFilePath, sout);

            // Cleanup old logs (keep max 20)
            CleanupOldLogs(logDirectory);
        }

        private static void CleanupOldLogs(string logDirectory)
        {
            try
            {
                var logFiles = Directory.GetFiles(logDirectory, "log_*.txt")
                                        .OrderBy(File.GetCreationTime)
                                        .ToList();

                if (logFiles.Count > 20)
                {
                    foreach (var file in logFiles.Take(logFiles.Count - 20))
                    {
                        File.Delete(file);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error cleaning up logs: {ex.Message}");
            }
        }
        public static void DebugError(params string[] message)
        {
            List<string> errorMessage = message.ToList();
            errorMessage.Insert(0, $"{GetCurrentTimeAsString()} Error: ");
            DebugLog(errorMessage.ToArray());
        }
        public static void DebugError(Exception ex)
        {
            DebugError($"{ex} \n {ex.Message}");
        }

        public static async Task<bool> CopyLocalFileAsync(string localFileName, string destinationPath)
        {
            try
            {
                if (File.Exists(destinationPath))
                    File.Delete(destinationPath);

                // Open the .docx file as a read-only stream from the MAUI resources
                using Stream inputStream = await FileSystem.OpenAppPackageFileAsync(localFileName);
                
                // Create a write stream for the destination file
                using FileStream outputStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.Read);

                // Copy the file ensuring full integrity
                await inputStream.CopyToAsync(outputStream);
                await outputStream.FlushAsync(); // Ensure all data is written before closing

                return true;
            }
            catch (Exception ex)
            {
                MyUtils.DebugLog($"Error copying file: {ex}");
                return false;
            }
        }
        public static async Task CopyLocalTemplate(string templateName)
        {
            if (!Path.HasExtension(templateName))
                templateName += ".docx";
            await CopyLocalFileAsync(Path.Combine("Templates\\Docx", Path.GetFileName(templateName)), Path.Combine(ProjectBase.TemplatesFolderPath, Path.GetFileName(templateName)));
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
        public static void DebugAlert(this ContentPage page, string message)
        {
            page.DisplayAlert("Debug Message", message, "OK");
        }
        #endregion

        #region Other Utils
        public static string GetCurrentTimeAsString(string format = "hh:mm tt")
        {
            // Get the current date and time
            DateTime currentTime = DateTime.Now;

            // Format the time according to the specified format
            string timeString = currentTime.ToString(format);

            return timeString;
        }
        #endregion
    }
}
