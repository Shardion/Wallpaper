# Wallpaper

*A background manager for GNOME*

## 🚧️ Warning 🚧️

Wallpaper is beta software. While we're pretty sure it won't eat your data,
it might crash without a visible reason, or do something strange, on top of
generally being incomplete and lacking features.

Incomplete features are marked with 🚧️.

## Features

- 🚧️ Set a random background every day
- 🚧️ Get backgrounds from multiple different sources
- Manually set a random background for a day

## 🚧️ How to use 🚧️

As Wallpaper is beta software, using it can be inconvenient.

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
If all went well, your background should change (unless today's background is the same as the one you've already set yourself).

At this point, you may click the `Random Background` button to replace today's background with a new one,
and `Today's Background` to reset it to today's first background - what it was before `Random Background` was clicked.

To get a new background every day, run `wallpaper -u` upon logging in.
Your background won't change between different sessions on the same day - this is intentional and the core feature of Wallpaper.

## 🚧️ Running 🚧️

As Wallpaper is beta software, there are no binary releases yet.
To use Wallpaper, you must compile it from source code.

With the .NET CLI:

- Install the [.NET 7 SDK](https://learn.microsoft.com/en-us/dotnet/core/install/linux) and [Git](https://git-scm.com/download/linux).
- Clone this repository with `git clone https://github.com/Shardion/Wallpaper`.
- Run `dotnet build` inside of the newly-created `Wallpaper/Shardion.Wallpaper` directory.
- Run `Wallpaper/Shardion.Wallpaper/bin/Debug/net7.0/Shardion.Wallpaper`.

Or with Nix:

- Install [Nix](https://nixos.org/download.html) and [Git](https://git-scm.com/download/linux).
- Enable [Flakes](https://nixos.org/manual/nix/stable/contributing/experimental-features.html#xp-feature-flakes)
  and the [`nix` command](https://nixos.org/manual/nix/stable/contributing/experimental-features.html#xp-feature-nix-command).
- Run `nix run github:shardion/wallpaper#wallpaper`.
  - To only build the software, instead run `nix build github:shardion/wallpaper#wallpaper`.
  - To install the software system-wide, instead run `nix profile install github:shardion/wallpaper#wallpaper`.

## Roadmap

- [ ] Finish the UI
- [ ] Automatically start with GNOME
- [ ] Don't use the same background twice in a row
- [ ] Support for `wlroots`-based Wayland compositors with `swaybg`
- [ ] Support for background sources outside of local directories
- [ ] Clean up Nix support for more general use
- [ ] Home Manager support
- [ ] Flatpak support

## Contributing

Wallpaper is licensed under the GPLv3 (`GPL-3.0-or-later`).

There are many `TODO` comments scattered around the codebase, which are good places
to start when contributing to the project.

Wallpaper's UI is created with [Cambalache](https://gitlab.gnome.org/jpu/cambalache),
and using Cambalache is helpful when working on it.