using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System;
using Flurl;

namespace Shardion.Wallpaper
{
    // TODO: Refactor this entire class.
    // It was originally written as a wad of duct tape to tie together the rest
    // of the program, so it would at least work.
    // Its code is unsightly and convoluted.
    public class BackgroundManager
    {
        public IList<BackgroundSink> Sinks { get; set; }
        public IList<BackgroundSource> Sources { get; set; }
        public WallpaperConfig Config { get; set; }
        public string? ConfigPath { get; }

        private Random _random;

        public BackgroundManager(IList<BackgroundSink> sinks, IList<BackgroundSource> sources, WallpaperConfig config, string? configPath = null)
        {
            Sinks = sinks;
            Sources = sources;
            Config = config;
            ConfigPath = configPath;
            _random = new(DateOnly.FromDateTime(DateTime.Today).DayNumber);
        }

        public static async Task<BackgroundManager> LoadFromConfigDirectory(string? pathOverride = null)
        {
            string path;
            if (pathOverride is not null)
            {
                path = pathOverride;
            }
            else
            {
                path = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "wallpaper");
                Directory.CreateDirectory(Path.Join(path, "sources"));
            }

            if (!Directory.Exists(Path.Join(path, "sources")))
            {
                throw new InvalidOperationException(
                    """
                    Configuration directory does not exist.
                    If you have not set a custom configuration directory, please ensure that the filesystem is writable,
                    so the default configuration directory can be created.
                    """
                );
            }

            List<BackgroundSink> sinks = new();
            sinks.Add(new BackgroundSink());
            List<BackgroundSource> sources = new();

            // TODO: figure out parallel.foreach
            foreach (string file in Directory.EnumerateFiles(Path.Join(path, "sources")))
            {
                using (Stream fileStream = File.OpenRead(file))
                {
                    using (JsonDocument doc = await JsonDocument.ParseAsync(fileStream))
                    {
                        sources.Add(new BackgroundSource(doc));
                    }
                }
            }
            if (!File.Exists(Path.Join(path, "config.json")))
            {
                await File.WriteAllTextAsync(Path.Join(path, "config.json"),
                """
                {
                    "ApplicableDayNumberForExtraRuns": null,
                    "ExtraRuns": 0
                }
                """);
            }
            WallpaperConfig config = new();
            using (Stream fileStream = File.OpenRead(Path.Join(path, "config.json")))
            {
                using (JsonDocument doc = await JsonDocument.ParseAsync(fileStream))
                {
                    if (doc.Deserialize<WallpaperConfig>(WallpaperConfigJsonSerializerContext.Default.WallpaperConfig) is WallpaperConfig deserializedConfig)
                    {
                        config = deserializedConfig;
                    }
                }
            }
            return new BackgroundManager(sinks, sources, config, Path.Join(path, "config.json"));
        }

        public async Task RandomizeDailyBackground()
        {
            if (Config.ApplicableDayNumberForExtraRngRuns != DateOnly.FromDateTime(DateTime.Today).DayNumber)
            {
                Config.ExtraRngRuns = 0;
            }
            Config.ApplicableDayNumberForExtraRngRuns = DateOnly.FromDateTime(DateTime.Today).DayNumber;
            Config.ExtraRngRuns++;
            await ReapplyDailyBackground(false);
            if (ConfigPath != null)
            {
                _ = Task.Run(() =>
                {
                    _ = File.WriteAllBytesAsync(ConfigPath, JsonSerializer.SerializeToUtf8Bytes<WallpaperConfig>(Config, WallpaperConfigJsonSerializerContext.Default.WallpaperConfig));
                });
            }
        }

        public async Task ResetDailyBackground()
        {
            Config.ApplicableDayNumberForExtraRngRuns = null;
            Config.ExtraRngRuns = 0;
            await ReapplyDailyBackground(false);
            if (ConfigPath != null)
            {
                    _ = File.WriteAllBytesAsync(ConfigPath, JsonSerializer.SerializeToUtf8Bytes<WallpaperConfig>(Config, WallpaperConfigJsonSerializerContext.Default.WallpaperConfig));
            }
        }

        public async Task ReapplyDailyBackground(bool writeConfig = true)
        {
            if (Config.ApplicableDayNumberForExtraRngRuns != DateOnly.FromDateTime(DateTime.Today).DayNumber)
            {
                Config.ApplicableDayNumberForExtraRngRuns = null;
                Config.ExtraRngRuns = 0;
                if (ConfigPath != null)
                {
                    _ = File.WriteAllBytesAsync(ConfigPath, JsonSerializer.SerializeToUtf8Bytes<WallpaperConfig>(Config, WallpaperConfigJsonSerializerContext.Default.WallpaperConfig));
                }
            }
            // reset RNG
            _random = new(DateOnly.FromDateTime(DateTime.Today).DayNumber);

            // pre-run RNG (to support keeping the same background between runs if it was changed with the random background button)
            for (int run = 0; run < Config.ExtraRngRuns; run++)
            {
                _ = _random.Next();
            }

            // TODO: allow use of a different algorithm for choosing a random background
            List<string> backgroundUris = new();
            foreach (BackgroundSource source in Sources)
            {
                backgroundUris.AddRange(await source.Backend.GetBackgroundUris().ConfigureAwait(false));
            }
            int randomBackgroundUriIndex = _random.Next() % backgroundUris.Count;
            string randomBackgroundUri = backgroundUris[randomBackgroundUriIndex];
            if (Url.Parse(randomBackgroundUri).IsRelative)
            {
                Console.WriteLine("Randomly-picked URI is relative, this is a bug in the application!");
            }
            foreach (BackgroundSink sink in Sinks)
            {
                sink.Backend.SetBackgroundUri(randomBackgroundUri);
            }
        }
    }
}