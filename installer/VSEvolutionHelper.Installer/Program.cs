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

        try
        {
            string game = ArgValue(args, "--game") ?? FindGame();
            if (game == null)
            {
                Fail("Could not find Vampire Survivors.");
                Info("Pass the folder explicitly:");
                Info("    vsevolutionhelper-installer --game \"<path to Vampire Survivors>\"");
                return 2;
            }

            Ok("Game folder: " + game);
            if (!LooksLikeGameFolder(game))
            {
                Warn("That folder does not look like a Vampire Survivors install.");
                Warn("Expected VampireSurvivors.exe or VampireSurvivors.app inside it.");
                if (!Confirm(args, "Continue anyway?")) return 3;
            }

            if (IsGameRunning())
            {
                Fail("Vampire Survivors appears to be running.");
                Info("Close the game first — Windows keeps the mod DLL locked while it runs.");
                return 4;
            }

            DisableMelonLoader(game);

            string bepInExZip = ArgValue(args, "--bepinex") ?? FindPayload("BepInEx", ".zip");
            bool bepInExPresent = Directory.Exists(Path.Combine(game, "BepInEx", "core"));

            if (bepInExZip != null)
            {
                if (bepInExPresent && !Confirm(args, "BepInEx is already installed. Reinstall it?"))
                    Info("Keeping the existing BepInEx.");
                else
                {
                    Step("Installing BepInEx…");
                    ExtractInto(bepInExZip, game);
                    Ok("BepInEx installed from " + Path.GetFileName(bepInExZip));
                    bepInExPresent = true;
                }
            }
            else if (!bepInExPresent)
            {
                Fail("BepInEx is not installed and no BepInEx archive was bundled.");
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
            if (dll == null)
            {
                Fail("Could not find " + ModDll + " next to the installer.");
                return 6;
            }

            string target = Path.Combine(game, "BepInEx", "plugins", ModFolder);
            Directory.CreateDirectory(target);
            string dest = Path.Combine(target, ModDll);
            Step("Installing the mod…");
            File.Copy(dll, dest, true);
            Ok("Mod installed: " + dest);

            VerifyLoader(game);

            Console.WriteLine();
            Ok("Done.");
            Console.WriteLine();
            Info("Next: launch the game once and let it reach the main menu.");
            Info("The first launch after installing BepInEx is slow — it generates the");
            Info("IL2CPP interop assemblies. That is normal, not a hang.");
            Console.WriteLine();
            Info("To confirm, look in  BepInEx/LogOutput.log  for:");
            Info("    Loading [VS Evolution Helper …]");
            return 0;
        }
        catch (Exception ex)
        {
            Fail(ex.Message);
            return 1;
        }
    }

    // ── Steam discovery ──────────────────────────────────────────────────────

    /// <summary>
    /// Locate the game by asking Steam where its libraries are, then reading the app manifest.
    /// Guessing "Program Files (x86)" alone misses the common case of games on a second drive.
    /// </summary>
    private static string FindGame()
    {
        Step("Looking for Vampire Survivors…");
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
            Warn("MelonLoader found — disabled it by renaming version.dll to version.dll.melon.off");
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
            Warn("Expected winhttp.dll — the archive may have been the Mono build, or extracted one level too deep.");
        else
            Warn("Expected run_bepinex.sh — on macOS/Linux the game is launched through that script.");
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
        Console.Write(Paint("  ?  ", "35") + question + " [y/N] ");
        string answer = Console.ReadLine();
        return answer != null && answer.Trim().StartsWith("y", StringComparison.OrdinalIgnoreCase);
    }

    // ── Output ───────────────────────────────────────────────────────────────

    private static string Env(string name) => Environment.GetEnvironmentVariable(name);

    private static string Paint(string text, string code) => _color ? "\u001b[" + code + "m" + text + "\u001b[0m" : text;

    private static void Step(string s) => Console.WriteLine(Paint("  >  ", "36") + s);
    private static void Ok(string s) => Console.WriteLine(Paint("  +  ", "32") + s);
    private static void Warn(string s) => Console.WriteLine(Paint("  !  ", "33") + s);
    private static void Fail(string s) => Console.WriteLine(Paint("  x  ", "31") + s);
    private static void Info(string s) => Console.WriteLine("     " + s);

    private static void Banner()
    {
        string[] bat =
        {
            @"        __       _,-""~^""-.                        ",
            @"      _// )      _,'       `.                      ",
            @"      "" ( ^ ~^~ /             )                     ",
            @"       `.       (  )        ,'                      ",
            @"         `-._  _)  ) ___,-'                        ",
            @"             ``   ``                               ",
        };

        Console.WriteLine();
        foreach (string line in bat) Console.WriteLine(Paint(line, "31"));
        Console.WriteLine(Paint(@"   V S   E V O L U T I O N   H E L P E R", "1;37"));
        Console.WriteLine(Paint(@"   ~ it is a night of tooltips ~", "35"));
        Console.WriteLine();
    }
}
