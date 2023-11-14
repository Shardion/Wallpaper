using System.Threading.Tasks;
using System;

namespace Shardion.Wallpaper
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "--update-background":
                    case "-u":
                        BackgroundManager manager = await BackgroundManager.LoadFromConfigDirectory();
                        await manager.ReapplyDailyBackground();
                        return 0;
                    case "--help":
                    case "-h":
                        Console.WriteLine(
                            """
                            Usage: wallpaper [OPTIONS]

                            Options:
                                --update-background, -u
                                    Instead of opening the UI, update the desktop background to today's current background and exit.
                                --help, -h
                                    Instead of opening the UI, show this help text and exit.
                            """
                        );
                        return 0;
                }
            }

            Adw.Application application = Adw.Application.New("org.gir.core", Gio.ApplicationFlags.FlagsNone);
            application.OnActivate += (sender, args) =>
            {
                WallpaperWindow window = new((Adw.Application)sender);
                window.Application = (Adw.Application)sender;
                window.Show();
            };
            return application.RunWithSynchronizationContext();
        }
    }
}
