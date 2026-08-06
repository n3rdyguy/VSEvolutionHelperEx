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
    [switch]$Yes
)

$ErrorActionPreference = 'Stop'
$AppId = '1794680'

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
    Fail 'BepInEx is not installed and no BepInEx archive was found next to this script.'
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
