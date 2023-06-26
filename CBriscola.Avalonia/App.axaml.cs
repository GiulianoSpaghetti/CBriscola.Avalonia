using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;

namespace CBriscola.Avalonia
{
    public partial class App : Application
    {
        public static string separator;
        public static OperatingSystemType t;
        public static string SistemaOperativo;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            t = AvaloniaLocator.Current.GetService<IRuntimePlatform>().GetRuntimeInfo().OperatingSystem;
            if (t == OperatingSystemType.WinNT)
            {
                separator = "\\";
                SistemaOperativo = "Windows";
            }
            else if (t == OperatingSystemType.Linux)
            {
                separator = "/";
                SistemaOperativo = "Linux";
            }
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
            }

            base.OnFrameworkInitializationCompleted();

        }
    }
}
