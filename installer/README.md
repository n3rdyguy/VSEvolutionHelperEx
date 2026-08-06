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
| `--bepinex <zip>` | BepInEx archive to install; defaults to one found beside the installer |
| `--mod <dll>` | `VSEvolutionHelper.dll`; defaults to one found beside the installer |
| `--yes` | Answer prompts with yes |
| `--no-color` | Plain output (binary only) |

PowerShell uses `-Game`, `-BepInEx`, `-Mod`, `-Yes`.

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

## BepInEx archive

Bundled releases include it. If you are running the installer on its own, download the
**Unity.IL2CPP** build for your platform from
[builds.bepinex.dev/projects/bepinex_be](https://builds.bepinex.dev/projects/bepinex_be) and
either drop the zip beside the installer or pass `--bepinex <zip>`.

The three things people get wrong: it must be **6.x bleeding-edge** (5.x has no IL2CPP support
for Unity 6), **Unity.IL2CPP** (the Mono package silently does nothing), and **64-bit**.

## Platform notes

The mod itself is a managed DLL and is platform-independent — what differs is the loader.

- **Windows** — verified. `winhttp.dll` sits next to `VampireSurvivors.exe`.
- **Linux / macOS** — the installer handles these, but **the mod has not been tested there.**
  BepInEx attaches through `run_bepinex.sh` rather than a DLL, so Steam needs launch options:

  ```
  "/path/to/Vampire Survivors/run_bepinex.sh" %command%
  ```

  The installer prints this with the real path once it has installed.
- **Proton** — the game is still the Windows build, so use the **win-x64** BepInEx even on Linux.
  The installer cannot detect this reliably; pass `--bepinex` with the Windows archive.

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
