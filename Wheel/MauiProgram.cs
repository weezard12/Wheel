using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

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
            .UseMauiCommunityToolkit() // Initialize the .NET MAUI Community Toolkit
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
