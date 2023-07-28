using System.Text.Json;

namespace Shardion.Wallpaper
{
    public sealed class BackgroundSource
    {
        public string Name { get; }
        public IBackgroundSourceBackend Backend { get; }

        public BackgroundSource(JsonDocument source)
        {
            if (!source.RootElement.TryGetProperty("name", out JsonElement name))
            {
                throw new JsonException("Background source definitions are required to have a name property.");
            }
            if (name.ValueKind is not JsonValueKind.String)
            {
                throw new JsonException("Background source names are required to be strings.");
            }
            if (!source.RootElement.TryGetProperty("arguments", out JsonElement arguments))
            {
                throw new JsonException("Background source definitions are required to have an arguments property.");
            }

            Name = name.GetString() ?? throw new JsonException("Background source names are required to be strings.");
            Backend = new DirectoryBackgroundSourceBackend();
            Backend.AcceptArguments(arguments.Clone());
        }
    }
}