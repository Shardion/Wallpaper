using System.Text.Json;

namespace Shardion.Wallpaper
{
    // TODO: Delete this class, and instead automatically load every sink backend,
    // using the most appropriate one for the current environment.
    // This means that sink backends won't be able to take arguments,
    // but it's a price worth paying for higher-quality code and better support for
    // non-GNOME environments.
    public sealed class BackgroundSink
    {
        public IBackgroundSinkBackend Backend { get; }

        public BackgroundSink()
        {
            Backend = new GNOMEBackgroundSinkBackend();
        }
    }
}