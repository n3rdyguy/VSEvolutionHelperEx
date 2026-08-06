# VS Evolution Helper — installer

Installs **BepInEx** and the **mod** into a Steam copy of Vampire Survivors, finding the game
through Steam's own library index rather than guessing at `Program Files`.

Two forms, same behaviour — pick whichever you trust more:

| | |
|--|--|
| **Binary** | `vsevolutionhelper-installer` — no runtime to install, one file |
| **Script** | `install.ps1` (Windows) / `install.sh` (macOS, Linux) — readable before you run it |

## Usage

```bash
# Windows
vsevolutionhelper-installer.exe
powershell -ExecutionPolicy Bypass -File install.ps1

# macOS / Linux
chmod +x vsevolutionhelper-installer install.sh
./vsevolutionhelper-installer
./install.sh
```

Options (all optional; the binary and both scripts accept the same ones):

| Option | Purpose |
|--------|---------|
| `--game <path>` | Game folder, if auto-detection fails |
| `--bepinex <zip>` | Use a local BepInEx archive instead of downloading |
| `--mod <dll>` | `VSEvolutionHelper.dll`; defaults to one found beside the installer |
| `--latest` | Download the newest BepInEx BE build instead of the pinned one |
| `--no-download` | Never reach the network |
| `--platform <rid>` | `win-x64` or `linux-x64`; needed for Proton |
| `--yes` | Answer prompts with yes |
| `--no-color` | Plain output (binary only) |

PowerShell uses `-Game`, `-BepInEx`, `-Mod`, `-Latest`, `-NoDownload`, `-Yes`.

## BepInEx is downloaded for you

If BepInEx is not already installed and no archive is sitting beside the installer, it is
fetched over HTTPS from the official CI at
[builds.bepinex.dev](https://builds.bepinex.dev/projects/bepinex_be) (~33 MB) and checked for
being a real zip before anything is unpacked — a CI error page saved as `.zip` would otherwise
fail much later and far less clearly.

**A specific build is pinned by default** — `6.0.0-be.785`, the one this mod is developed and
tested against. Bleeding-edge means exactly that, and a loader that breaks on a fresh CI build
is much harder to diagnose than one that is merely out of date. Pass `--latest` to take the
newest instead; if the build list cannot be read it falls back to the pinned build rather than
failing.

The URL cannot be constructed from a version number alone — the filename embeds a commit hash
(`…-6.0.0-be.785+6abdba4.zip`), which is why `--latest` scrapes rather than guesses.

Use `--bepinex <zip>` or `--no-download` for an offline install.

## What it does

1. **Finds the game.** Reads Steam's registered path (registry on Windows, the usual locations
   on macOS/Linux including Flatpak), walks `libraryfolders.vdf` for every library, and reads
   `appmanifest_1794680.acf` for the real install directory. Games on a second drive are the
   normal case, not an edge case.
2. **Refuses to run while the game is open.** Windows keeps the DLL locked, so installing over a
   running game silently leaves the old build in place.
3. **Disables MelonLoader if present**, by renaming `version.dll` to `version.dll.melon.off`.
   Both loaders hooking the process is what crashes the game. Renaming is reversible; deleting
   another mod loader is not this installer's call.
4. **Unpacks BepInEx** into the game folder — lifting the contents out if the archive wraps
   everything in a single top-level folder, which is the "extracted one level too deep" mistake.
5. **Installs the mod** to `BepInEx/plugins/VSEvolutionHelper/`.
6. **Checks the loader landed**, and says which one it expected: `winhttp.dll` on Windows,
   `run_bepinex.sh` on macOS/Linux.

## Platform notes

The mod itself is a managed DLL and is platform-independent — what differs is the loader.

| Platform | Status |
|----------|--------|
| **Windows** | Verified end to end, including the download path. `winhttp.dll` lands next to `VampireSurvivors.exe`. |
| **Linux** | Implemented and downloadable (`linux-x64` IL2CPP exists), but **untested** by us. |
| **macOS** | **BepInEx publishes no IL2CPP build for macOS.** The installer runs and can install the mod, but there is no loader to fetch, so it cannot complete an install on its own. |

- **Linux** — BepInEx attaches through `run_bepinex.sh` rather than a DLL, so Steam needs launch
  options. The installer prints this with the real path once it has installed:

  ```
  "/path/to/Vampire Survivors/run_bepinex.sh" %command%
  ```

- **Proton** — the game is still the *Windows* build, so it needs the **win-x64** loader even
  though the host is Linux. That cannot be detected from inside the installer, so pass
  `--platform win-x64` explicitly.

## Building

```bash
cd VSEvolutionHelper.Installer
dotnet publish -c Release -r win-x64   -o ../dist/win-x64
dotnet publish -c Release -r linux-x64 -o ../dist/linux-x64
dotnet publish -c Release -r osx-x64   -o ../dist/osx-x64
dotnet publish -c Release -r osx-arm64 -o ../dist/osx-arm64
```

Self-contained, single-file and trimmed — about 11–13 MB per platform, no .NET runtime required
on the user's machine.
