# Wallpaper

Wallpaper is a simplistic "wallpaper manager" for GNOME Shell. Every day, it sets your background to a random
background from a random configured source. Optionally, you may choose to select a new random background
for that day, or reset it to what it was initially (before any re-rolls for that day).

Wallpaper will not interfere with manual background changes, and will only change your background upon every login to the random
background selected for that day. If you happen to be using your computer while the day changes, your background won't abruptly
change with it, and if you restart your computer, your background will stay the same.

Only GNOME Shell is officially supported, but other desktop environments that are based on it (such as Cinnamon) may work as well.

## How to use

The Wallpaper UI is not finished yet. The usage steps below guide you through writing source configurations manually.

To begin, start the program. It will create its config directory at `~/.config/wallpaper`,
including a `config.json` file and an empty `sources` directory.

Place a JSON file with any name and the following contents inside the `sources` directory:
```json
{
  "name": "Quite Possibly A Source",
  "backend": "directory",
  "arguments": {
    "path": "/path/to/your/backgrounds/directory/"
  }
}
```
You may place multiple JSON files inside the `sources` directory to add multiple directories
from which Wallpaper will pull its backgrounds.

Restart the program to reload its config, and then click the `Today's Background` button.
If all went well, your background should change (unless today's background is the same as one you've already set yourself).

At this point, you may click the `Random Background` button to replace today's background with a new one,
and `Today's Background` to reset it to today's first background (what it was before `Random Background` was clicked).

To get a new background every day, run `Shardion.Wallpaper -u` upon logging in.
(You may place this command in a `.desktop` file inside `~/.config/autostart`.)

## Running

No binary releases are provided yet.
To use Wallpaper, you must compile it from source code.

- Install the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) and [Git](https://git-scm.com/download/linux).
- Clone this repository with `git clone https://github.com/Shardion/Wallpaper`.
- Run `dotnet publish` inside of the newly-created `Wallpaper` directory.
- Run `Wallpaper/artifacts/publish/Shardion.Wallpaper/release/Shardion.Wallpaper`.

## Roadmap

- [ ] Finish the UI
- [ ] Automatically start with GNOME (enable/disable switch?)
- [ ] Don't use the same background twice in a row
- [ ] Support for background sources outside of local directories
- [ ] Flatpak support

## Contributing

Wallpaper is licensed under the GPLv3 (`GPL-3.0-or-later`).

There are many `TODO` comments scattered around the codebase, which are good places
to start when contributing to the project.

Wallpaper's UI is created with [Cambalache](https://gitlab.gnome.org/jpu/cambalache).
