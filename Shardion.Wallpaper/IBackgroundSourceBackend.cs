using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace Shardion.Wallpaper
{
    public interface IBackgroundSourceBackend
    {
        public Task<IReadOnlyList<string>> GetBackgroundUris();

        public void AcceptArguments(JsonElement arguments);
    }
}