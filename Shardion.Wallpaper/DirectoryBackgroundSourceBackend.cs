using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System;
using Flurl;

namespace Shardion.Wallpaper
{
    public class DirectoryBackgroundSourceBackend : IBackgroundSourceBackend
    {
        public string? DirectoryPath { get; set; }

        public Task<IReadOnlyList<string>> GetBackgroundUris()
        {
            List<string> files = new();
            if (DirectoryPath is null)
            {
                throw new InvalidOperationException("Background source backends must accept arguments before any operation can be performed on them.");
            }
            foreach (string file in Directory.EnumerateFiles(DirectoryPath))
            {
                Url fileUri = Url.EncodeIllegalCharacters(Path.GetFullPath(file));
                fileUri.Scheme = "file";
                fileUri.Path = "///" + fileUri.Path.TrimStart('/'); // Flurl file URI handling workaround
                files.Add(fileUri.ToString());
            }
            return Task.FromResult<IReadOnlyList<string>>(files.AsReadOnly());
        }

        public void AcceptArguments(JsonElement arguments)
        {
            if (arguments.ValueKind is not JsonValueKind.Object)
            {
                throw new JsonException("Directory background source backend arguments are required to be a JSON object.");
            }
            if (!arguments.TryGetProperty("path", out JsonElement path) || path.ValueKind is not JsonValueKind.String)
            {
                throw new JsonException("Directory background source backend arguments must include a path string property.");
            }
            DirectoryPath = Path.GetFullPath(path.GetString() ?? throw new JsonException("Directory background source backend arguments must include a path string property."));
        }
    }
}