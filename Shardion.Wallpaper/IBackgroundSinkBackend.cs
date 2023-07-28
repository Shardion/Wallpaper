namespace Shardion.Wallpaper
{
    public interface IBackgroundSinkBackend
    {
        public string? GetBackgroundUri();
        public void SetBackgroundUri(string uri);
    }
}