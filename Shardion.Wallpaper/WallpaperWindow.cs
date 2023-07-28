using System.Threading.Tasks;

namespace Shardion.Wallpaper
{
    public class WallpaperWindow : Adw.ApplicationWindow
    {
        [Gtk.Connect] private readonly Gtk.ListBoxRow _addSourceRow;
        [Gtk.Connect] private readonly Gtk.Button _randomBackgroundButton;
        [Gtk.Connect] private readonly Gtk.Button _resetBackgroundButton;
        private Task<BackgroundManager>? _bgTask;
        private BackgroundManager? _bg;

        private WallpaperWindow(Gtk.Builder builder, string name) : base(builder.GetPointer(name), false)
        {
            builder.Connect(this);
            _bgTask = BackgroundManager.LoadFromConfigDirectory();
            _addSourceRow.OnActivate += async (_, _) =>
            {
                if (_bg is null)
                {
                    _bg = await _bgTask;
                }
                await _bg.ReapplyDailyBackground();
            };
            _randomBackgroundButton.OnClicked += async (_, _) =>
            {
                if (_bg is null)
                {
                    _bg = await _bgTask;
                }
                await _bg.RandomizeDailyBackground();
            };
            _resetBackgroundButton.OnClicked += async (_, _) =>
            {
                if (_bg is null)
                {
                    _bg = await _bgTask;
                }
                await _bg.ResetDailyBackground();
            };
        }

        public WallpaperWindow() : this(new Gtk.Builder("Wallpaper.4.ui"), "_mainWindow")
        {
        }
    }
}