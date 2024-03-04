using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using System;

namespace CBriscola.Avalonia
{
    public partial class App : Application
    {
        public static string SistemaOperativo;
        public static string path;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            {
                if (OperatingSystem.IsWindows())
                {
                    path = "C:\\Program Files\\wxBriscola";
                    SistemaOperativo = Environment.OSVersion.ToString();
                }
                else if (OperatingSystem.IsLinux())
                {
                    SistemaOperativo = "GNU/Linux";
                    path = "/usr/share/wxBriscola";
                }

                if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();

        }
    }
}
