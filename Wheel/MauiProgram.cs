using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Wheel.Logic;

namespace Wheel
{
    public static class MauiProgram
    {
        public static readonly string TempPath = Path.Combine(Path.GetTempPath(),"Wheel");


        public static readonly Type wordType = Type.GetTypeFromProgID("Word.Application");
        public static bool IsWordInstalled => wordType != null;

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit(
            options =>
            {
                // Enable Snackbar for Windows (only works on MSIX with an Administrator!) else it will crush
                //options.SetShouldEnableSnackbarOnWindows(true); 
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // My Logic
            MyUtils.CreateFolderIfDoesntExist(TempPath, true); // creates the wheel folder (and for now clears it)

            return builder.Build();
        }
    }
}
