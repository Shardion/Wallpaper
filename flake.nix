{
  description = "Nix build support for Floating";
  inputs.nixpkgs.url = "nixpkgs/nixos-unstable";
  outputs = { self, nixpkgs }:
    let
      lastModifiedDate = self.lastModifiedDate or self.lastModified or "19700101";
      version = builtins.substring 0 8 lastModifiedDate;
      supportedSystems = [ "x86_64-linux" ]; # Untested on other platforms
      forAllSystems = nixpkgs.lib.genAttrs supportedSystems;
      nixpkgsFor = forAllSystems (system: import nixpkgs { inherit system; overlays = [ self.overlay ]; });
    in

    {
      overlay = final: prev: {
        wallpaper = with final; buildDotnetModule rec {
          name = "${pname}-${version}";
          pname = "wallpaper";
          version = "1.0.0";

          src = ./Shardion.Wallpaper;
          projectFile = "Shardion.Wallpaper.csproj";

          nativeBuildInputs = [ wrapGAppsHook4 ];
          runtimeDeps = [ gtk4 libadwaita glib dbus ];

          dotnet-sdk = dotnetCorePackages.sdk_7_0;
          dotnet-runtime = dotnetCorePackages.runtime_7_0;
          nugetDeps = ./deps.nix;

          postFixup = ''
          mv $out/bin/Shardion.Wallpaper $out/bin/wallpaper
          '';

          executables = [ "Shardion.Wallpaper" ];
          meta.mainProgram = "wallpaper";
        };
      };

      devShells = forAllSystems (system:
        {
          default = let
            dotnet_devenv_sdk = (with nixpkgsFor.${system}.dotnetCorePackages; combinePackages [
            sdk_8_0
            sdk_6_0
          ]);
          in nixpkgsFor.${system}.mkShell rec {
            name = "default";
            packages = with nixpkgsFor.${system}; [
              omnisharp-roslyn
              netcoredbg
              dotnet_devenv_sdk
            ];
            shellHook = ''
              # Microsoft.Build.Locate assumes `dotnet` is never a symlink, so
              # we comply and place the original `dotnet` binary on the PATH
              # before the dotnet_sdk/bin symlink that Nix adds.
              # We also set DOTNET_ROOT, because the PATH changing trick
              # seems to only work when this is properly set.
              export DOTNET_ROOT="${dotnet_devenv_sdk}"
              export DOTNET_ROOT_X64="${dotnet_devenv_sdk}"
              export PATH=${dotnet_devenv_sdk}:$PATH
            '';
          };
        }
      );

      packages = forAllSystems (system:
        {
          inherit (nixpkgsFor.${system}) wallpaper;
        });
      defaultPackage = forAllSystems (system: self.packages.${system}.wallpaper);
    };
}
