# Wallpaper

*A background manager for GNOME*

## 🚧️ Warning 🚧️

Wallpaper is beta software. While we're pretty sure it won't eat your data,
it might crash without a visible reason, or do something strange, on top of
generally being incomplete and lacking features.

Incomplete features are marked with 🚧️.

## Features

- 🚧️ Set a random background every day
- 🚧️ Get backgrounds from multiple sources
- Manually set a random background for a day

## 🚧️ How to use 🚧️

As Wallpaper is beta software, using it can be inconvenient.

To begin, start the program. It will create its config directory at `~/.config/wallpaper`,
including a `config.json` file and an empty `sources` directory.

Place a JSON file with any name and the following contents inside the `sources` directory:
```json
{
  "name": "<your name here>",
  "backend": "directory",
  "arguments": {
    "path": "/path/to/your/backgrounds/directory/"
  }
}
```
You may place multiple JSON files inside the `sources` directory to add multiple directories
from which Wallpaper will pull its backgrounds.

Restart the program to reload its config, and then click the `Today's background` button.
If all went well, your background should change (unless today's background is the same as the one you've already set yourself).

At this point, you may click the `Random background` button to replace today's background with a new one,
and `Today's background` to reset it to today's first background - what it was before `Random background` was clicked.

To get a new background every day, run `wallpaper -u` upon logging in.
Your background won't change between different sessions on the same day - this is intentional and the core feature of Wallpaper.

## Building

- Install the [.NET 7 SDK](https://learn.microsoft.com/en-us/dotnet/core/install/linux) and [Git](https://git-scm.com/download/linux).
- Clone this repository with `git clone https://github.com/Shardion/Wallpaper`.
- Run `dotnet build` inside of the newly-created `Wallpaper/Shardion.Wallpaper` directory.

## Roadmap

- [ ] Finish the UI
- [ ] Automatically start with GNOME
- [ ] Don't use the same background twice in a row
- [ ] Support for `wlroots`-based Wayland compositors with `swaybg`
- [ ] Support for background sources outside of local directories
- [ ] Flatpak support
- [ ] Home Manager support