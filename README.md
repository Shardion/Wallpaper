# Wallpaper

A 

## How to use

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
Your background won't change between different sessions on the same day - this is intentional, and the core feature of Wallpaper.

## Running

No binary releases are provided, yet.
To use Wallpaper, you must compile it from source code.

With the .NET CLI:

- Install the [.NET 7 SDK](https://learn.microsoft.com/en-us/dotnet/core/install/linux) and [Git](https://git-scm.com/download/linux).
- Clone this repository with `git clone https://github.com/Shardion/Wallpaper`.
- Run `dotnet build` inside of the newly-created `Wallpaper/Shardion.Wallpaper` directory.
- Run `Wallpaper/Shardion.Wallpaper/bin/Debug/net7.0/Shardion.Wallpaper`.

## Roadmap

- [ ] Finish the UI
- [ ] Automatically start with GNOME
- [ ] Don't use the same background twice in a row
- [ ] Support for background sources outside of local directories
- [ ] Flatpak support

## Contributing

Wallpaper is licensed under the GPLv3 (`GPL-3.0-or-later`).

There are many `TODO` comments scattered around the codebase, which are good places
to start when contributing to the project.

Wallpaper's UI is created with [Cambalache](https://gitlab.gnome.org/jpu/cambalache).
