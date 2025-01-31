using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Wheel.Logic;

namespace Wheel
{
    public static class MauiProgram
    {
        public static readonly string TempPath = Path.Combine(Path.GetTempPath(),"Wheel");
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit(options =>
            {
                options.SetShouldEnableSnackbarOnWindows(true); // Enable Snackbar for Windows
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
            MyUtils.CreateFolderIfDoesntExist(TempPath); // creates the wheel folder (and for now clears it)

            return builder.Build();
        }
    }
}
