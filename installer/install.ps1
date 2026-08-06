<#
.SYNOPSIS
    Installs BepInEx and VS Evolution Helper into a Steam copy of Vampire Survivors.

.DESCRIPTION
    Script equivalent of the bundled installer binary, for anyone who would rather read what
    they run than trust an executable. Same behaviour: find the game through Steam's own
    library index, disable a leftover MelonLoader, unpack BepInEx, drop the mod into
    BepInEx/plugins.

.PARAMETER Game
    Game folder, if auto-detection fails.

.PARAMETER BepInEx
    Path to a BepInEx Unity.IL2CPP zip. Defaults to one found next to this script.

.PARAMETER Mod
    Path to VSEvolutionHelper.dll. Defaults to one found next to this script.

.EXAMPLE
    ./install.ps1
    ./install.ps1 -Game "D:\SteamLibrary\steamapps\common\Vampire Survivors"
#>
[CmdletBinding()]
param(
    [string]$Game,
    [string]$BepInEx,
    [string]$Mod,
    [switch]$Yes,
    [switch]$Latest,
    [switch]$NoDownload,
    [switch]$Uninstall,
    [switch]$All,
    [switch]$KeepConfig
)

$ErrorActionPreference = 'Stop'
$AppId = '1794680'

# The build this mod is developed and tested against. Pinned rather than always taking the
# newest: bleeding-edge means exactly that, and a broken loader is harder to diagnose than an
# out-of-date one. -Latest opts into the newest build instead.
$BuildsHost  = 'https://builds.bepinex.dev'
$BuildsPage  = "$BuildsHost/projects/bepinex_be"
$PinnedBuild = '785'
$PinnedHash  = '6abdba4'

function Write-Banner {
    $bat = @(
        '        __       _,-"~^"-.',
        '      _// )      _,''       `.',
        '      " ( ^ ~^~ /             )',
        '       `.       (  )        ,''',
        '         `-._  _)  ) ___,-''',
        '             ``   ``'
    )
    Write-Host ''
    foreach ($line in $bat) { Write-Host $line -ForegroundColor DarkRed }
    Write-Host '   V S   E V O L U T I O N   H E L P E R' -ForegroundColor White
    Write-Host '   ~ it is a night of tooltips ~' -ForegroundColor Magenta
    Write-Host ''
}

function Step($m) { Write-Host '  >  ' -ForegroundColor Cyan -NoNewline;   Write-Host $m }
function Ok($m)   { Write-Host '  +  ' -ForegroundColor Green -NoNewline;  Write-Host $m }
function Warn($m) { Write-Host '  !  ' -ForegroundColor Yellow -NoNewline; Write-Host $m }
function Fail($m) { Write-Host '  x  ' -ForegroundColor Red -NoNewline;    Write-Host $m }
function Info($m) { Write-Host "     $m" }

function Get-SteamRoots {
    $roots = New-Object System.Collections.Generic.List[string]
    try {
        $reg = (Get-ItemProperty 'HKCU:\Software\Valve\Steam' -ErrorAction SilentlyContinue).SteamPath
        if ($reg) { $roots.Add(($reg -replace '/', '\')) }
    } catch { }
    $roots.Add("${env:ProgramFiles(x86)}\Steam")
    $roots.Add("$env:ProgramFiles\Steam")
    foreach ($d in [System.IO.DriveInfo]::GetDrives()) {
        $roots.Add((Join-Path $d.Name 'Steam'))
        $roots.Add((Join-Path $d.Name 'SteamLibrary'))
    }
    $roots | Where-Object { $_ -and (Test-Path (Join-Path $_ 'steamapps')) } | Select-Object -Unique
}

function Get-Libraries($steamRoot) {
    $libs = @($steamRoot)
    $vdf = Join-Path $steamRoot 'steamapps\libraryfolders.vdf'
    if (Test-Path $vdf) {
        foreach ($m in [regex]::Matches((Get-Content $vdf -Raw), '"path"\s*"([^"]+)"')) {
            $p = $m.Groups[1].Value -replace '\\\\', '\'
            if (Test-Path $p) { $libs += $p }
        }
    }
    $libs | Select-Object -Unique
}

function Find-Game {
    Step 'Looking for Vampire Survivors...'
    foreach ($root in Get-SteamRoots) {
        foreach ($lib in Get-Libraries $root) {
            $manifest = Join-Path $lib "steamapps\appmanifest_$AppId.acf"
            if (-not (Test-Path $manifest)) { continue }
            $m = [regex]::Match((Get-Content $manifest -Raw), '"installdir"\s*"([^"]+)"')
            $dir = if ($m.Success) { $m.Groups[1].Value } else { 'Vampire Survivors' }
            $path = Join-Path $lib "steamapps\common\$dir"
            if (Test-Path $path) { return $path }
        }
    }
    return $null
}

function Get-BepInEx {
    # Only win-x64 and linux-x64 IL2CPP artifacts are published; there is no macOS build.
    $url = $null
    if ($Latest) {
        try {
            $html = (Invoke-WebRequest -Uri $BuildsPage -UseBasicParsing).Content
            $m = [regex]::Match($html, '/projects/bepinex_be/\d+/BepInEx-Unity\.IL2CPP-win-x64-[^"''<>\s]+\.zip')
            if ($m.Success) { $url = "$BuildsHost$($m.Value)" }
        } catch { }
        if (-not $url) { Warn 'Could not read the build list; falling back to the pinned build.' }
    }
    if (-not $url) {
        $url = "$BuildsHost/projects/bepinex_be/$PinnedBuild/BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.$PinnedBuild%2B$PinnedHash.zip"
    }

    Step 'Downloading BepInEx (win-x64)...'
    Info $url
    $temp = Join-Path ([System.IO.Path]::GetTempPath()) ("vseh-bepinex-" + [guid]::NewGuid().ToString('N') + '.zip')
    try {
        Invoke-WebRequest -Uri $url -OutFile $temp -UseBasicParsing
    } catch {
        Fail "Download failed: $($_.Exception.Message)"
        Info "Download it manually from $BuildsPage then re-run with -BepInEx '<path to zip>'"
        return $null
    }
    # A CI error page saved as .zip would fail much later and much less clearly.
    try {
        Add-Type -AssemblyName System.IO.Compression.FileSystem
        $probe = [System.IO.Compression.ZipFile]::OpenRead($temp); $probe.Dispose()
    } catch {
        Remove-Item $temp -Force -ErrorAction SilentlyContinue
        Fail 'The downloaded file is not a valid zip archive.'
        return $null
    }
    Ok ("Downloaded {0} MB" -f [int]((Get-Item $temp).Length / 1MB))
    return $temp
}

function Find-Payload($pattern) {
    foreach ($dir in @($PSScriptRoot, (Join-Path $PSScriptRoot 'payload'), (Get-Location).Path)) {
        if (-not $dir -or -not (Test-Path $dir)) { continue }
        $hit = Get-ChildItem -Path $dir -Filter $pattern -File -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($hit) { return $hit.FullName }
    }
    return $null
}

Write-Banner

if (-not $Game) { $Game = Find-Game }
if (-not $Game) {
    Fail 'Could not find Vampire Survivors.'
    Info 'Pass the folder explicitly:  ./install.ps1 -Game "<path>"'
    exit 2
}
Ok "Game folder: $Game"

if ($Uninstall) {
    if (Get-Process -Name 'VampireSurvivors' -ErrorAction SilentlyContinue) {
        Fail 'Vampire Survivors is running. Close it first.'
        exit 4
    }

    $targets = @()
    $modDir = Join-Path $Game 'BepInEx\plugins\VSEvolutionHelper'
    if (Test-Path $modDir) { $targets += $modDir }
    $cfg = Join-Path $Game 'BepInEx\config\com.nihil.vsevolutionhelper.cfg'
    if (-not $KeepConfig -and (Test-Path $cfg)) { $targets += $cfg }

    if ($All) {
        foreach ($n in 'BepInEx', 'dotnet') {
            $p = Join-Path $Game $n
            if (Test-Path $p) { $targets += $p }
        }
        foreach ($n in 'winhttp.dll', 'doorstop_config.ini', '.doorstop_version') {
            $p = Join-Path $Game $n
            if (Test-Path $p) { $targets += $p }
        }
        # changelog.txt is left alone: BepInEx ships one, but so might the game.
    }

    if ($targets.Count -eq 0) {
        Ok 'Nothing to remove - no VS Evolution Helper install found here.'
        exit 0
    }

    if ($All) {
        # BepInEx/plugins is shared; removing the loader takes other mods with it.
        $plugins = Join-Path $Game 'BepInEx\plugins'
        if (Test-Path $plugins) {
            $others = Get-ChildItem $plugins -Force | Where-Object { $_.Name -ne 'VSEvolutionHelper' }
            if ($others) {
                Warn 'Removing BepInEx will also remove these other plugins:'
                foreach ($o in $others) { Info "  - $($o.Name)" }
            }
        }
    }

    Step 'About to remove:'
    foreach ($t in $targets) { Info "  $t" }
    Write-Host ''
    if (-not $Yes) {
        $q = if ($All) { 'Remove the mod AND BepInEx?' } else { 'Remove the mod?' }
        $answer = Read-Host "  ?  $q [y/N]"
        if ($answer -notmatch '^[Yy]') { Info 'Cancelled.'; exit 0 }
    }

    $failures = 0
    foreach ($t in $targets) {
        try { Remove-Item $t -Recurse -Force -ErrorAction Stop; Ok "Removed $t" }
        catch { Fail "Could not remove $t ($($_.Exception.Message))"; $failures++ }
    }

    # If this script disabled MelonLoader on the way in, put it back on the way out.
    $melonOff = Join-Path $Game 'version.dll.melon.off'
    if ($All -and (Test-Path $melonOff) -and -not (Test-Path (Join-Path $Game 'version.dll'))) {
        try { Move-Item $melonOff (Join-Path $Game 'version.dll'); Ok 'Restored MelonLoader (version.dll)' }
        catch { Warn "Could not restore MelonLoader: $($_.Exception.Message)" }
    }

    Write-Host ''
    if ($failures -gt 0) { Fail "$failures item(s) could not be removed."; exit 8 }
    if ($All) { Ok 'BepInEx and the mod removed.' }
    else { Ok 'Mod removed. BepInEx is still installed.'; Info 'Pass -All to remove BepInEx as well.' }
    exit 0
}

if (-not (Test-Path (Join-Path $Game 'VampireSurvivors.exe')) -and
    -not (Test-Path (Join-Path $Game 'GameAssembly.dll'))) {
    Warn 'That folder does not look like a Vampire Survivors install.'
    if (-not $Yes) {
        $answer = Read-Host '  ?  Continue anyway? [y/N]'
        if ($answer -notmatch '^[Yy]') { exit 3 }
    }
}

if (Get-Process -Name 'VampireSurvivors' -ErrorAction SilentlyContinue) {
    Fail 'Vampire Survivors is running.'
    Info 'Close it first - Windows keeps the mod DLL locked while the game runs.'
    exit 4
}

# MelonLoader and BepInEx both hook the process; together they crash the game. Rename rather
# than delete - reversible, and not this script's place to throw away another loader.
$melon = Join-Path $Game 'version.dll'
if (Test-Path $melon) {
    $off = "$melon.melon.off"
    if (Test-Path $off) { Remove-Item $off -Force }
    Move-Item $melon $off
    Warn 'MelonLoader found - renamed version.dll to version.dll.melon.off'
    Warn 'Rename it back to undo. Running both loaders crashes the game.'
}

if (-not $BepInEx) { $BepInEx = Find-Payload 'BepInEx*.zip' }
$bepPresent = Test-Path (Join-Path $Game 'BepInEx\core')
if (-not $BepInEx -and -not $bepPresent -and -not $NoDownload) { $BepInEx = Get-BepInEx }

if ($BepInEx) {
    $doIt = $true
    if ($bepPresent -and -not $Yes) {
        $answer = Read-Host '  ?  BepInEx is already installed. Reinstall it? [y/N]'
        $doIt = $answer -match '^[Yy]'
    }
    if ($doIt) {
        Step 'Installing BepInEx...'
        Expand-Archive -Path $BepInEx -DestinationPath $Game -Force
        # Some archives wrap everything in one folder; lift it out if so.
        $wrapper = Get-ChildItem -Path $Game -Directory -Filter 'BepInEx-*' -ErrorAction SilentlyContinue | Select-Object -First 1
        if ($wrapper) {
            Get-ChildItem -Path $wrapper.FullName -Force | Move-Item -Destination $Game -Force
            Remove-Item $wrapper.FullName -Recurse -Force
        }
        Ok "BepInEx installed from $(Split-Path $BepInEx -Leaf)"
        $bepPresent = $true
    } else {
        Info 'Keeping the existing BepInEx.'
    }
} elseif (-not $bepPresent) {
    Fail 'BepInEx is not installed and could not be obtained.'
    Info 'Download the Unity.IL2CPP build from https://builds.bepinex.dev/projects/bepinex_be'
    Info 'then re-run with:  ./install.ps1 -BepInEx "<path to zip>"'
    exit 5
} else {
    Ok 'BepInEx already installed.'
}

if (-not $Mod) { $Mod = Find-Payload 'VSEvolutionHelper.dll' }
if (-not $Mod) {
    Fail 'Could not find VSEvolutionHelper.dll next to this script.'
    exit 6
}

$target = Join-Path $Game 'BepInEx\plugins\VSEvolutionHelper'
New-Item -ItemType Directory -Path $target -Force | Out-Null
Step 'Installing the mod...'
Copy-Item $Mod (Join-Path $target 'VSEvolutionHelper.dll') -Force
Ok "Mod installed: $target\VSEvolutionHelper.dll"

if (-not (Test-Path (Join-Path $Game 'winhttp.dll'))) {
    Warn 'winhttp.dll is missing next to the game executable.'
    Warn 'The archive may have been the Mono build, or was extracted one level too deep.'
}

Write-Host ''
Ok 'Done.'
Write-Host ''
Info 'Next: launch the game once and let it reach the main menu.'
Info 'The first launch after installing BepInEx is slow - it generates the'
Info 'IL2CPP interop assemblies. That is normal, not a hang.'
Write-Host ''
Info 'To confirm, look in  BepInEx/LogOutput.log  for:'
Info '    Loading [VS Evolution Helper ...]'
