using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace VSEvolutionHelper.Installer;

/// <summary>
/// Installs BepInEx and VS Evolution Helper into a Steam copy of Vampire Survivors.
///
/// Everything the manual instructions get wrong is handled here: the loader has to sit next to
/// the executable rather than in a subfolder, a leftover MelonLoader has to be disabled, and the
/// mod DLL belongs under BepInEx/plugins rather than BepInEx itself.
/// </summary>
internal static class Program
{
    private const string AppId = "1794680";
    private const string ModFolder = "VSEvolutionHelper";
    private const string ModDll = "VSEvolutionHelper.dll";

    private static bool _color = true;

    private static int Main(string[] args)
    {
        if (Array.IndexOf(args, "--no-color") >= 0) _color = false;
        try { Console.OutputEncoding = System.Text.Encoding.UTF8; } catch { }

        Banner();

        // Nothing on the command line means it was double-clicked, so drive it with a menu
        // rather than printing usage at someone who never asked for flags.
        bool interactive = args.Length == 0 || (args.Length == 1 && args[0] == "--no-color");

        try
        {
            string game = ArgValue(args, "--game") ?? FindGame();

            if (interactive) return Menu(game, args);

            if (game == null)
            {
                Fail("Could not find Vampire Survivors.");
                Info("Pass the folder explicitly:");
                Info("    vsevolutionhelper-installer --game \"<path to Vampire Survivors>\"");
                return 2;
            }

            Ok("Game folder: " + game);

            if (Array.IndexOf(args, "--uninstall") >= 0)
                return Uninstall(game, args);

            return Install(game, args);
        }
        catch (Exception ex)
        {
            Fail(ex.Message);
            return 1;
        }
    }

    // ── Interactive menu ─────────────────────────────────────────────────────

    private static int Menu(string game, string[] args)
    {
        while (true)
        {
            Console.WriteLine();
            if (game == null) Warn("Vampire Survivors not found - choose [5] to set the folder.");
            else Ok("Game: " + game);

            bool installed = game != null
                && File.Exists(Path.Combine(game, "BepInEx", "plugins", ModFolder, ModDll));
            bool loader = game != null && Directory.Exists(Path.Combine(game, "BepInEx", "core"));
            Info("BepInEx: " + (loader ? "installed" : "not installed")
                + "    Mod: " + (installed ? "installed" : "not installed"));

            Console.WriteLine();
            MenuLine("[1]", "Install            BepInEx + the mod");
            MenuLine("[2]", "Update mod only    keep BepInEx and settings");
            MenuLine("[3]", "Remove mod         keep BepInEx");
            MenuLine("[4]", "Remove everything  mod + BepInEx");
            MenuLine("[5]", "Change game folder");
            MenuLine("[Q]", "Quit");
            Console.WriteLine();
            Paint("   Press a key: ", ConsoleColor.Magenta);

            char key;
            try { key = char.ToUpperInvariant(Console.ReadKey(true).KeyChar); }
            catch { return 0; } // no console to read from
            Console.WriteLine(key);
            Console.WriteLine();

            switch (key)
            {
                case '1':
                case '2':
                    if (game == null) { Fail("Set the game folder first."); break; }
                    var installArgs = new List<string>(args);
                    if (key == '2') installArgs.Add("--no-bepinex");
                    Run(() => Install(game, installArgs.ToArray()));
                    break;

                case '3':
                    if (game == null) { Fail("Set the game folder first."); break; }
                    Run(() => Uninstall(game, args));
                    break;

                case '4':
                    if (game == null) { Fail("Set the game folder first."); break; }
                    var removeArgs = new List<string>(args) { "--all" };
                    Run(() => Uninstall(game, removeArgs.ToArray()));
                    break;

                case '5':
                    Console.Write("   Path to the Vampire Survivors folder: ");
                    string entered = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(entered))
                    {
                        entered = entered.Trim().Trim('"');
                        if (Directory.Exists(entered)) { game = entered; Ok("Game folder set."); }
                        else Fail("That folder does not exist.");
                    }
                    break;

                case 'Q':
                case (char)27:
                    return 0;

                default:
                    Warn("Unknown choice.");
                    break;
            }
        }
    }

    /// <summary>Run an action and keep the menu alive even if it throws.</summary>
    private static void Run(Func<int> action)
    {
        try { action(); }
        catch (Exception ex) { Fail(ex.Message); }
        Console.WriteLine();
        Info("Press any key to return to the menu...");
        try { Console.ReadKey(true); } catch { }
    }

    // ── Install ──────────────────────────────────────────────────────────────

    private static int Install(string game, string[] args)
    {
        try
        {
            if (!LooksLikeGameFolder(game))
            {
                Warn("That folder does not look like a Vampire Survivors install.");
                Warn("Expected VampireSurvivors.exe or VampireSurvivors.app inside it.");
                if (!Confirm(args, "Continue anyway?")) return 3;
            }

            if (IsGameRunning())
            {
                Fail("Vampire Survivors appears to be running.");
                Info("Close the game first - Windows keeps the mod DLL locked while it runs.");
                return 4;
            }

            DisableMelonLoader(game);

            bool skipBepInEx = Array.IndexOf(args, "--no-bepinex") >= 0;
            string bepInExZip = skipBepInEx ? null : (ArgValue(args, "--bepinex") ?? FindPayload("BepInEx", ".zip"));
            bool bepInExPresent = Directory.Exists(Path.Combine(game, "BepInEx", "core"));

            if (bepInExZip == null && !bepInExPresent && !skipBepInEx
                && Array.IndexOf(args, "--no-download") < 0)
                bepInExZip = DownloadBepInEx(args);

            if (bepInExZip != null)
            {
                if (bepInExPresent && !Confirm(args, "BepInEx is already installed. Reinstall it?"))
                    Info("Keeping the existing BepInEx.");
                else
                {
                    Step("Installing BepInEx...");
                    ExtractInto(bepInExZip, game);
                    Ok("BepInEx installed from " + Path.GetFileName(bepInExZip));
                    bepInExPresent = true;
                }
            }
            else if (!bepInExPresent)
            {
                Fail("BepInEx is not installed and could not be obtained.");
                Info("Download the Unity.IL2CPP build for your platform from:");
                Info("    https://builds.bepinex.dev/projects/bepinex_be");
                Info("then re-run with:  --bepinex \"<path to that zip>\"");
                return 5;
            }
            else
            {
                Ok("BepInEx already installed.");
            }

            string dll = ArgValue(args, "--mod") ?? FindPayload(ModDll, null);
            if (dll == null && Array.IndexOf(args, "--no-download") < 0)
                dll = DownloadMod(args);
            if (dll == null)
            {
                Fail("Could not find or download " + ModDll + ".");
                Info("Download the release zip from:");
                Info("    " + ReleasesPage);
                Info("then re-run with:  --mod \"<path to " + ModDll + ">\"");
                return 6;
            }

            string target = Path.Combine(game, "BepInEx", "plugins", ModFolder);
            Directory.CreateDirectory(target);
            string dest = Path.Combine(target, ModDll);
            Step("Installing the mod...");
            File.Copy(dll, dest, true);
            Ok("Mod installed: " + dest);

            VerifyLoader(game);

            Console.WriteLine();
            Ok("Done.");
            Console.WriteLine();
            Info("Next: launch the game once and let it reach the main menu.");
            Info("The first launch after installing BepInEx is slow - it generates the");
            Info("IL2CPP interop assemblies. That is normal, not a hang.");
            Console.WriteLine();
            Info("To confirm, look in  BepInEx/LogOutput.log  for:");
            Info("    Loading [VS Evolution Helper ...]");
            return 0;
        }
        catch (Exception ex)
        {
            Fail(ex.Message);
            return 1;
        }
    }

    // ── Uninstall ────────────────────────────────────────────────────────────

    /// <summary>
    /// Remove the mod, and with <c>--all</c> the loader too.
    ///
    /// Everything to be deleted is listed and confirmed first, because BepInEx/plugins is
    /// shared: removing the loader takes any other mods installed alongside this one with it.
    /// </summary>
    private static int Uninstall(string game, string[] args)
    {
        bool all = Array.IndexOf(args, "--all") >= 0;

        if (IsGameRunning())
        {
            Fail("Vampire Survivors appears to be running. Close it first.");
            return 4;
        }

        var targets = new List<string>();
        string modDir = Path.Combine(game, "BepInEx", "plugins", ModFolder);
        if (Directory.Exists(modDir)) targets.Add(modDir);

        string config = Path.Combine(game, "BepInEx", "config", "com.nihil.vsevolutionhelper.cfg");
        bool keepConfig = Array.IndexOf(args, "--keep-config") >= 0;
        if (!keepConfig && File.Exists(config)) targets.Add(config);

        if (all)
        {
            foreach (string name in new[] { "BepInEx", "dotnet" })
            {
                string d = Path.Combine(game, name);
                if (Directory.Exists(d)) targets.Add(d);
            }
            foreach (string name in new[]
            {
                "winhttp.dll", "doorstop_config.ini", ".doorstop_version",
                "run_bepinex.sh", "libdoorstop.so", "libdoorstop.dylib",
            })
            {
                string f = Path.Combine(game, name);
                if (File.Exists(f)) targets.Add(f);
            }
            // changelog.txt is deliberately left alone: BepInEx ships one, but so might the
            // game, and deleting a game file to tidy up would be a poor trade.
        }

        if (targets.Count == 0)
        {
            Ok("Nothing to remove - no VS Evolution Helper install found here.");
            return 0;
        }

        if (all)
        {
            var others = OtherPlugins(game);
            if (others.Count > 0)
            {
                Warn("Removing BepInEx will also remove these other plugins:");
                foreach (string o in others) Info("  - " + o);
            }
        }

        Step("About to remove:");
        foreach (string t in targets) Info("  " + t);
        Console.WriteLine();
        if (!Confirm(args, all ? "Remove the mod AND BepInEx?" : "Remove the mod?"))
        {
            Info("Cancelled.");
            return 0;
        }

        int failures = 0;
        foreach (string t in targets)
        {
            try
            {
                if (Directory.Exists(t)) Directory.Delete(t, true);
                else File.Delete(t);
                Ok("Removed " + t);
            }
            catch (Exception ex)
            {
                Fail("Could not remove " + t + " (" + ex.Message + ")");
                failures++;
            }
        }

        // If this installer disabled MelonLoader on the way in, put it back on the way out.
        string melonOff = Path.Combine(game, "version.dll.melon.off");
        if (all && File.Exists(melonOff))
        {
            string melon = Path.Combine(game, "version.dll");
            if (!File.Exists(melon))
            {
                try
                {
                    File.Move(melonOff, melon);
                    Ok("Restored MelonLoader (version.dll)");
                }
                catch (Exception ex) { Warn("Could not restore MelonLoader: " + ex.Message); }
            }
        }

        Console.WriteLine();
        if (failures > 0) { Fail($"{failures} item(s) could not be removed."); return 8; }
        Ok(all ? "BepInEx and the mod removed." : "Mod removed. BepInEx is still installed.");
        if (!all) Info("Pass --all to remove BepInEx as well.");
        return 0;
    }

    /// <summary>Plugin folders and DLLs that are not ours.</summary>
    private static List<string> OtherPlugins(string game)
    {
        var others = new List<string>();
        string plugins = Path.Combine(game, "BepInEx", "plugins");
        if (!Directory.Exists(plugins)) return others;
        try
        {
            foreach (string entry in Directory.GetFileSystemEntries(plugins))
            {
                string name = Path.GetFileName(entry);
                if (string.Equals(name, ModFolder, StringComparison.OrdinalIgnoreCase)) continue;
                others.Add(name);
            }
        }
        catch { }
        return others;
    }

    // ── Steam discovery ──────────────────────────────────────────────────────

    /// <summary>
    /// Locate the game by asking Steam where its libraries are, then reading the app manifest.
    /// Guessing "Program Files (x86)" alone misses the common case of games on a second drive.
    /// </summary>
    private static string FindGame()
    {
        Step("Looking for Vampire Survivors...");
        foreach (string steam in SteamRoots())
        {
            foreach (string library in Libraries(steam))
            {
                string manifest = Path.Combine(library, "steamapps", "appmanifest_" + AppId + ".acf");
                if (!File.Exists(manifest)) continue;

                string installDir = ReadVdfValue(File.ReadAllText(manifest), "installdir") ?? "Vampire Survivors";
                string path = Path.Combine(library, "steamapps", "common", installDir);
                if (Directory.Exists(path)) return path;
            }
        }
        return null;
    }

    private static IEnumerable<string> SteamRoots()
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        string home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        foreach (string candidate in Candidates())
        {
            if (string.IsNullOrWhiteSpace(candidate)) continue;
            string full;
            try { full = Path.GetFullPath(candidate); } catch { continue; }
            if (!seen.Add(full)) continue;
            if (Directory.Exists(Path.Combine(full, "steamapps"))) yield return full;
        }

        IEnumerable<string> Candidates()
        {
            if (OperatingSystem.IsWindows())
            {
                // Steam records its own location; reading it via reg avoids a Windows-only
                // package reference in a build that also targets macOS and Linux.
                string reg = RegistryQuery(@"HKCU\Software\Valve\Steam", "SteamPath");
                if (reg != null) yield return reg.Replace('/', '\\');
                yield return Path.Combine(Env("ProgramFiles(x86)") ?? @"C:\Program Files (x86)", "Steam");
                yield return Path.Combine(Env("ProgramFiles") ?? @"C:\Program Files", "Steam");
                foreach (var d in DriveInfo.GetDrives())
                {
                    yield return Path.Combine(d.Name, "Steam");
                    yield return Path.Combine(d.Name, "SteamLibrary");
                }
            }
            else if (OperatingSystem.IsMacOS())
            {
                yield return Path.Combine(home, "Library", "Application Support", "Steam");
            }
            else
            {
                yield return Path.Combine(home, ".steam", "steam");
                yield return Path.Combine(home, ".steam", "root");
                yield return Path.Combine(home, ".local", "share", "Steam");
                // Flatpak keeps its own copy of everything.
                yield return Path.Combine(home, ".var", "app", "com.valvesoftware.Steam", "data", "Steam");
            }
        }
    }

    /// <summary>Every library folder Steam knows about, including the root install itself.</summary>
    private static IEnumerable<string> Libraries(string steamRoot)
    {
        yield return steamRoot;

        string vdf = Path.Combine(steamRoot, "steamapps", "libraryfolders.vdf");
        if (!File.Exists(vdf)) yield break;

        string text;
        try { text = File.ReadAllText(vdf); } catch { yield break; }

        foreach (Match m in Regex.Matches(text, "\"path\"\\s*\"([^\"]+)\"", RegexOptions.IgnoreCase))
        {
            string p = m.Groups[1].Value.Replace("\\\\", "\\");
            if (Directory.Exists(p)) yield return p;
        }
    }

    private static string ReadVdfValue(string text, string key)
    {
        var m = Regex.Match(text, "\"" + Regex.Escape(key) + "\"\\s*\"([^\"]+)\"", RegexOptions.IgnoreCase);
        return m.Success ? m.Groups[1].Value.Replace("\\\\", "\\") : null;
    }

    private static string RegistryQuery(string key, string value)
    {
        try
        {
            var psi = new ProcessStartInfo("reg", $"query \"{key}\" /v {value}")
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using var p = Process.Start(psi);
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit(5000);
            var m = Regex.Match(output, value + @"\s+REG_SZ\s+(.+)", RegexOptions.IgnoreCase);
            return m.Success ? m.Groups[1].Value.Trim() : null;
        }
        catch { return null; }
    }

    // ── Install steps ────────────────────────────────────────────────────────

    private static bool LooksLikeGameFolder(string game)
    {
        return File.Exists(Path.Combine(game, "VampireSurvivors.exe"))
            || Directory.Exists(Path.Combine(game, "VampireSurvivors.app"))
            || File.Exists(Path.Combine(game, "VampireSurvivors"))
            || File.Exists(Path.Combine(game, "GameAssembly.dll"));
    }

    private static bool IsGameRunning()
    {
        try
        {
            foreach (var p in Process.GetProcesses())
            {
                if (p.ProcessName.IndexOf("VampireSurvivors", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
        }
        catch { }
        return false;
    }

    /// <summary>
    /// MelonLoader and BepInEx both hook the process and crash the game together. Renaming is
    /// used rather than deleting: it is reversible, and it is not this installer's place to
    /// throw away another mod loader.
    /// </summary>
    private static void DisableMelonLoader(string game)
    {
        string version = Path.Combine(game, "version.dll");
        if (!File.Exists(version)) return;

        string off = version + ".melon.off";
        try
        {
            if (File.Exists(off)) File.Delete(off);
            File.Move(version, off);
            Warn("MelonLoader found - disabled it by renaming version.dll to version.dll.melon.off");
            Warn("Rename it back to undo. Running both loaders crashes the game.");
        }
        catch (Exception ex)
        {
            Warn("Could not disable MelonLoader (" + ex.Message + ").");
            Warn("Remove or rename version.dll yourself, or the game may crash.");
        }
    }

    /// <summary>
    /// Extract without the "one level too deep" mistake: if the archive wraps everything in a
    /// single top folder, its contents are lifted out rather than nested inside the game folder.
    /// </summary>
    private static void ExtractInto(string zipPath, string game)
    {
        using var archive = ZipFile.OpenRead(zipPath);

        string prefix = CommonRoot(archive);
        foreach (var entry in archive.Entries)
        {
            if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\")) continue;

            string relative = entry.FullName;
            if (prefix != null && relative.StartsWith(prefix, StringComparison.Ordinal))
                relative = relative.Substring(prefix.Length);
            if (relative.Length == 0) continue;

            string destination = Path.GetFullPath(Path.Combine(game, relative.Replace('/', Path.DirectorySeparatorChar)));
            // Refuse entries that would escape the game folder.
            if (!destination.StartsWith(Path.GetFullPath(game), StringComparison.OrdinalIgnoreCase)) continue;

            Directory.CreateDirectory(Path.GetDirectoryName(destination));
            entry.ExtractToFile(destination, true);
        }
    }

    private static string CommonRoot(ZipArchive archive)
    {
        string root = null;
        foreach (var entry in archive.Entries)
        {
            int slash = entry.FullName.IndexOf('/');
            if (slash <= 0) return null;
            string top = entry.FullName.Substring(0, slash + 1);
            if (root == null) root = top;
            else if (!string.Equals(root, top, StringComparison.Ordinal)) return null;
        }
        return root;
    }

    private static void VerifyLoader(string game)
    {
        bool loader = File.Exists(Path.Combine(game, "winhttp.dll"))
            || File.Exists(Path.Combine(game, "run_bepinex.sh"))
            || File.Exists(Path.Combine(game, "libdoorstop.so"))
            || File.Exists(Path.Combine(game, "libdoorstop.dylib"));

        if (loader) return;

        Warn("No BepInEx loader found next to the game executable.");
        if (OperatingSystem.IsWindows())
            Warn("Expected winhttp.dll - the archive may have been the Mono build, or extracted one level too deep.");
        else
            Warn("Expected run_bepinex.sh - on macOS/Linux the game is launched through that script.");
    }

    // ── BepInEx download ─────────────────────────────────────────────────────

    private const string BuildsHost = "https://builds.bepinex.dev";
    private const string BuildsPage = BuildsHost + "/projects/bepinex_be";

    /// <summary>
    /// The build this mod is developed and tested against. Pinned by default rather than always
    /// taking the newest: bleeding-edge means exactly that, and a broken loader is much harder
    /// for someone to diagnose than an out-of-date one. <c>--latest</c> opts into the newest.
    /// </summary>
    private const string PinnedBuild = "785";
    private const string PinnedCommit = "6abdba4";

    /// <summary>
    /// Fetch BepInEx from the official CI.
    ///
    /// Only win-x64 and linux-x64 IL2CPP artifacts are published - there is no macOS IL2CPP
    /// build, so macOS cannot be served this way at all.
    /// </summary>
    private static string DownloadBepInEx(string[] args)
    {
        string platform = ArgValue(args, "--platform") ?? DefaultPlatform();
        if (platform == null)
        {
            Warn("No BepInEx IL2CPP build is published for macOS.");
            Info("Only win-x64 and linux-x64 IL2CPP artifacts exist on builds.bepinex.dev.");
            return null;
        }

        string url = null;
        if (Array.IndexOf(args, "--latest") >= 0)
        {
            url = ResolveLatest(platform);
            if (url == null) Warn("Could not read the build list; falling back to the pinned build.");
        }
        url ??= $"{BuildsHost}/projects/bepinex_be/{PinnedBuild}"
              + $"/BepInEx-Unity.IL2CPP-{platform}-6.0.0-be.{PinnedBuild}%2B{PinnedCommit}.zip";

        Step($"Downloading BepInEx ({platform})...");
        Info(url);

        try
        {
            string temp = Path.Combine(Path.GetTempPath(), "vseh-bepinex-" + Guid.NewGuid().ToString("N") + ".zip");
            using (var http = new System.Net.Http.HttpClient())
            {
                http.Timeout = TimeSpan.FromMinutes(5);
                http.DefaultRequestHeaders.Add("User-Agent", "VSEvolutionHelper-Installer");
                using var response = http.GetAsync(url).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                using var stream = response.Content.ReadAsStream();
                using var file = File.Create(temp);
                stream.CopyTo(file);
            }

            // A CI error page saved as .zip would otherwise fail much later and much less
            // clearly, so the archive is opened before it is trusted.
            try { using var probe = ZipFile.OpenRead(temp); }
            catch
            {
                File.Delete(temp);
                Fail("The downloaded file is not a valid zip archive.");
                return null;
            }

            Ok($"Downloaded {new FileInfo(temp).Length / 1024 / 1024} MB");
            return temp;
        }
        catch (Exception ex)
        {
            Fail("Download failed: " + ex.Message);
            Info("Download it manually from " + BuildsPage);
            Info("then re-run with:  --bepinex \"<path to that zip>\"");
            return null;
        }
    }

    private static string DefaultPlatform()
    {
        if (OperatingSystem.IsWindows()) return "win-x64";
        // Under Proton the game is the Windows build and needs the Windows loader, which cannot
        // be detected from here - hence --platform.
        if (OperatingSystem.IsLinux()) return "linux-x64";
        return null;
    }

    /// <summary>
    /// Scrape the newest artifact link. The filename carries a commit hash, so the URL cannot
    /// be constructed from a build number alone.
    /// </summary>
    private static string ResolveLatest(string platform)
    {
        try
        {
            using var http = new System.Net.Http.HttpClient();
            http.Timeout = TimeSpan.FromSeconds(30);
            http.DefaultRequestHeaders.Add("User-Agent", "VSEvolutionHelper-Installer");
            string html = http.GetStringAsync(BuildsPage).GetAwaiter().GetResult();

            var m = Regex.Match(html,
                "/projects/bepinex_be/\\d+/BepInEx-Unity\\.IL2CPP-" + Regex.Escape(platform) + "-[^\"'<>\\s]+\\.zip",
                RegexOptions.IgnoreCase);
            return m.Success ? BuildsHost + m.Value : null;
        }
        catch { return null; }
    }

    // ── Mod download ─────────────────────────────────────────────────────────

    private const string Repo = "n3rdyguy/VSEvolutionHelperEx";
    private const string ReleasesPage = "https://github.com/" + Repo + "/releases";

    /// <summary>
    /// Fetch the mod from its GitHub release, so the installer works on its own with nothing
    /// beside it. <c>--version vX.Y.Z</c> pins a specific tag; otherwise the latest is used.
    /// </summary>
    private static string DownloadMod(string[] args)
    {
        string tag = ArgValue(args, "--version");
        string api = tag == null
            ? $"https://api.github.com/repos/{Repo}/releases/latest"
            : $"https://api.github.com/repos/{Repo}/releases/tags/{tag}";

        Step("Downloading VS Evolution Helper" + (tag == null ? " (latest)" : " " + tag) + "...");

        try
        {
            using var http = new System.Net.Http.HttpClient();
            http.Timeout = TimeSpan.FromMinutes(3);
            // GitHub rejects requests without a User-Agent.
            http.DefaultRequestHeaders.Add("User-Agent", "VSEvolutionHelper-Installer");

            string json = http.GetStringAsync(api).GetAwaiter().GetResult();

            var asset = Regex.Match(json,
                "\"browser_download_url\"\\s*:\\s*\"([^\"]+VSEvolutionHelper[^\"]*\\.zip)\"",
                RegexOptions.IgnoreCase);
            if (!asset.Success)
            {
                Fail("No release asset found" + (tag == null ? "." : " for " + tag + "."));
                return null;
            }

            string version = Regex.Match(json, "\"tag_name\"\\s*:\\s*\"([^\"]+)\"").Groups[1].Value;
            string url = asset.Groups[1].Value;
            Info(url);

            string temp = Path.Combine(Path.GetTempPath(), "vseh-mod-" + Guid.NewGuid().ToString("N") + ".zip");
            using (var response = http.GetAsync(url).GetAwaiter().GetResult())
            {
                response.EnsureSuccessStatusCode();
                using var stream = response.Content.ReadAsStream();
                using var file = File.Create(temp);
                stream.CopyTo(file);
            }

            // The release zip mirrors the install layout, so pull the DLL out of it rather than
            // unpacking the whole thing over the game folder.
            string extracted = Path.Combine(Path.GetTempPath(), "vseh-mod-" + Guid.NewGuid().ToString("N") + ".dll");
            using (var archive = ZipFile.OpenRead(temp))
            {
                ZipArchiveEntry entry = null;
                foreach (var e in archive.Entries)
                {
                    if (e.Name.Equals(ModDll, StringComparison.OrdinalIgnoreCase)) { entry = e; break; }
                }
                if (entry == null)
                {
                    Fail("The release zip did not contain " + ModDll + ".");
                    File.Delete(temp);
                    return null;
                }
                entry.ExtractToFile(extracted, true);
            }
            File.Delete(temp);

            Ok("Downloaded " + (string.IsNullOrEmpty(version) ? "mod" : version));
            return extracted;
        }
        catch (Exception ex)
        {
            Fail("Download failed: " + ex.Message);
            return null;
        }
    }

    // ── Payload discovery ────────────────────────────────────────────────────

    /// <summary>Find a bundled file next to the installer, or one folder down.</summary>
    private static string FindPayload(string contains, string extension)
    {
        string baseDir = AppContext.BaseDirectory;
        foreach (string dir in new[] { baseDir, Path.Combine(baseDir, "payload"), Directory.GetCurrentDirectory() })
        {
            if (!Directory.Exists(dir)) continue;
            foreach (string file in Directory.GetFiles(dir))
            {
                string name = Path.GetFileName(file);
                if (name.IndexOf(contains, StringComparison.OrdinalIgnoreCase) < 0) continue;
                if (extension != null && !name.EndsWith(extension, StringComparison.OrdinalIgnoreCase)) continue;
                if (extension == null && !name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)) continue;
                return file;
            }
        }
        return null;
    }

    private static string ArgValue(string[] args, string name)
    {
        for (int i = 0; i < args.Length - 1; i++)
            if (string.Equals(args[i], name, StringComparison.OrdinalIgnoreCase)) return args[i + 1];
        return null;
    }

    private static bool Confirm(string[] args, string question)
    {
        if (Array.IndexOf(args, "--yes") >= 0) return true;
        Paint("  ?  ", ConsoleColor.Magenta); Console.Write(question + " [y/N] ");
        string answer = Console.ReadLine();
        return answer != null && answer.Trim().StartsWith("y", StringComparison.OrdinalIgnoreCase);
    }

    // ── Output ───────────────────────────────────────────────────────────────

    private static string Env(string name) => Environment.GetEnvironmentVariable(name);

    /// <summary>
    /// Colour through the console API rather than raw ANSI escapes.
    ///
    /// Classic conhost does not enable virtual terminal processing by default, so escape codes
    /// printed there appear literally as "ESC[31m" instead of colouring anything.
    /// Console.ForegroundColor works on conhost, Windows Terminal and Unix alike, and does
    /// nothing harmful when output is redirected to a file or a pipe.
    /// </summary>
    private static void Paint(string text, ConsoleColor color)
    {
        if (!_color) { Console.Write(text); return; }
        ConsoleColor previous;
        try { previous = Console.ForegroundColor; }
        catch { Console.Write(text); return; }
        try
        {
            Console.ForegroundColor = color;
            Console.Write(text);
        }
        finally
        {
            try { Console.ForegroundColor = previous; } catch { }
        }
    }

    private static void PaintLine(string text, ConsoleColor color)
    {
        Paint(text, color);
        Console.WriteLine();
    }

    private static void Tagged(string tag, ConsoleColor color, string message)
    {
        Paint(tag, color);
        Console.WriteLine(message);
    }

    private static void MenuLine(string key, string label) => Tagged("   " + key, ConsoleColor.White, "  " + label);

    private static void Step(string s) => Tagged("  >  ", ConsoleColor.Cyan, s);
    private static void Ok(string s) => Tagged("  +  ", ConsoleColor.Green, s);
    private static void Warn(string s) => Tagged("  !  ", ConsoleColor.Yellow, s);
    private static void Fail(string s) => Tagged("  x  ", ConsoleColor.Red, s);
    private static void Info(string s) => Console.WriteLine("     " + s);

    private static void Banner()
    {
        string[] bat =
        {
            @"        __       _,-""~^""-.",
            @"      _// )      _,'       `.",
            @"      "" ( ^ ~^~ /             )",
            @"       `.       (  )        ,'",
            @"         `-._  _)  ) ___,-'",
            @"             ``   ``",
        };

        Console.WriteLine();
        foreach (string line in bat) PaintLine(line, ConsoleColor.DarkRed);
        PaintLine(@"   V S   E V O L U T I O N   H E L P E R", ConsoleColor.White);
        PaintLine(@"   ~ it is a night of tooltips ~", ConsoleColor.Magenta);
        Console.WriteLine();
    }
}
