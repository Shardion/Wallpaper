using System.Text.Json.Serialization;

namespace Shardion.Wallpaper
{
    // TODO: Entirely replace this class with GSettings.
    // Maybe keep JSON config support around for better Nix support.
    public class WallpaperConfig
    {
        public int? ApplicableDayNumberForExtraRngRuns { get; set; } = null;
        public int ExtraRngRuns { get; set; } = 0;
    }

    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata, WriteIndented = true)]
    [JsonSerializable(typeof(WallpaperConfig))]
    internal partial class WallpaperConfigJsonSerializerContext : JsonSerializerContext
    {
        
    }
}