using System.Threading.Tasks;
using System;

namespace Shardion.Wallpaper
{
    public class WallpaperWindow : Adw.ApplicationWindow
    {
        [Gtk.Connect] private readonly Gtk.ListBoxRow _addSourceRow;
        [Gtk.Connect] private readonly Gtk.Button _randomBackgroundButton;
        [Gtk.Connect] private readonly Gtk.Button _resetBackgroundButton;
        private Task<BackgroundManager> _bgTask;
        private BackgroundManager? _bg;

        private WallpaperWindow(Gtk.Builder builder, string name, Adw.Application application) : base(builder.GetPointer(name), false)
        {
            builder.Connect(this);
            _bgTask = BackgroundManager.LoadFromConfigDirectory();
            _randomBackgroundButton.OnClicked += UseRandomBackground;
            _resetBackgroundButton.OnClicked += UseTodaysBackground;

            Gio.SimpleAction shortcutsAction = Gio.SimpleAction.New("keyboardShortcuts", null);
            Gio.SimpleAction aboutAction = Gio.SimpleAction.New("about", null);
            Gio.SimpleAction quitAction = Gio.SimpleAction.New("quit", null);
            Gio.SimpleAction randomBackgroundAction = Gio.SimpleAction.New("randomBackground", null);
            Gio.SimpleAction todaysBackgroundAction = Gio.SimpleAction.New("todaysBackground", null);
            shortcutsAction.OnActivate += OpenShortcutsWindow;
            aboutAction.OnActivate += OpenAboutWindow;
            quitAction.OnActivate += Quit;
            randomBackgroundAction.OnActivate += UseRandomBackground;
            todaysBackgroundAction.OnActivate += UseTodaysBackground;
            application.SetAccelsForAction("win.keyboardShortcuts", new string[] { "<Ctrl>question" });
            application.SetAccelsForAction("win.about", new string[] { "F1" });
            application.SetAccelsForAction("win.quit", new string[] { "<Ctrl>q" });
            application.SetAccelsForAction("win.randomBackground", new string[] { "<Ctrl>r" });
            application.SetAccelsForAction("win.todaysBackground", new string[] { "<Ctrl>t" });
            AddAction(shortcutsAction);
            AddAction(aboutAction);
            AddAction(quitAction);
            AddAction(randomBackgroundAction);
            AddAction(todaysBackgroundAction);
        }

        public WallpaperWindow(Adw.Application application) : this(new Gtk.Builder("Wallpaper.4.ui"), "_mainWindow", application)
        {
        }

        private async Task UseRandomBackground()
        {
            if (_bg is null)
            {
                _bg = await _bgTask;
            }
            await _bg.RandomizeDailyBackground();
        }

        private async Task UseTodaysBackground()
        {
            if (_bg is null)
            {
                _bg = await _bgTask;
            }
            await _bg.ResetDailyBackground();
        }

        // Makes the relevant event handlers happy
        private async void UseTodaysBackground(Gio.SimpleAction sender, EventArgs e) { await UseTodaysBackground(); }
        private async void UseTodaysBackground(Gtk.Button sender, EventArgs e) { await UseTodaysBackground(); }
        private async void UseRandomBackground(Gio.SimpleAction sender, EventArgs e) { await UseRandomBackground(); }
        private async void UseRandomBackground(Gtk.Button sender, EventArgs e) { await UseRandomBackground(); }

        private void OpenAboutWindow(Gio.SimpleAction sender, EventArgs e)
        {
            Gtk.Builder aboutBuilder = new Gtk.Builder("Wallpaper.4.ui");
            if (aboutBuilder.GetObject("_aboutWindow") is Adw.AboutWindow aboutWindow)
            {
                aboutWindow.SetTransientFor(this);
                aboutWindow.Show();
            }
        }
        private void OpenShortcutsWindow(Gio.SimpleAction sender, EventArgs e)
        {
            Gtk.Builder aboutBuilder = new Gtk.Builder("Wallpaper.4.ui");
            if (aboutBuilder.GetObject("_shortcutsWindow") is Gtk.ShortcutsWindow shortcutsWindow)
            {
                shortcutsWindow.SetTransientFor(this);
                shortcutsWindow.Show();
            }
        }
        private void Quit(Gio.SimpleAction sender, EventArgs e)
        {

        }
    }
}