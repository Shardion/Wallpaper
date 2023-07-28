using System.Text.Json;
using System;
using Flurl;

namespace Shardion.Wallpaper
{
    public class GNOMEBackgroundSinkBackend : IBackgroundSinkBackend
    {
        public GNOMEBackgroundSinkBackend()
        {
            Gio.Module.Initialize();
        }

        public void SetBackgroundUri(string uri)
        {
            if (Url.Parse(uri).Scheme is not "file")
            {
                // TODO: not supported by gnome, make an attempt to download the file at the uri and store it somewhere
                throw new NotImplementedException("Non-local background images are not supported in the GNOME Desktop sink.");
            }

            using (Gio.Settings settings = Gio.Settings.New("org.gnome.desktop.background"))
            {
                settings.Delay();
                settings.SetString("picture-uri", uri);
                // TODO: backend argument to not set the dark style background, vice versa
                settings.SetString("picture-uri-dark", uri);
                settings.Apply();
            }
        }

        public string? GetBackgroundUri()
        {
            string? background = null;
            using (Gio.Settings settings = Gio.Settings.New("org.gnome.desktop.background"))
            {
                background = settings.GetString("picture-uri");
                if (background != settings.GetString("picture-uri-dark"))
                {
                    // TODO: handle this case better
                    Console.WriteLine("Dark style background URI differs from normal background URI, returned background URI may be inaccurate.");
                }
            }
            return background;
        }
    }
}