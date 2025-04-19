using Microsoft.Extensions.Logging;
using BCIMockApp.Services;
using BCIMockApp.ViewModels;

namespace BCIMockApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<MockBCIService>();

            // Register view models
            builder.Services.AddSingleton<MainViewModel>();

            // Register pages
            builder.Services.AddSingleton<MainPage>();

            // Register shell
            builder.Services.AddSingleton<AppShell>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
