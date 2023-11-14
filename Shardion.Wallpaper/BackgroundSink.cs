using System.Text.Json;

namespace Shardion.Wallpaper
{
    // TODO: Delete this class. I don't plan on supporting any sink other
    // than GNOME Shell.
    public sealed class BackgroundSink
    {
        public IBackgroundSinkBackend Backend { get; }

        public BackgroundSink()
        {
            Backend = new GNOMEBackgroundSinkBackend();
        }
    }
}
