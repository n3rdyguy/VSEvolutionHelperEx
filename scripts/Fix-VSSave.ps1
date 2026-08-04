<#
.SYNOPSIS
  Fix Vampire Survivors save files (JSON repair + checksum + optional modern merge).

.DESCRIPTION
  1) Repairs common JSON corruption (double commas, trailing commas, BOM)
  2) Regenerates checksum (SHA-256 of compact JSON with "checksum":"")
  3) Optional -ModernTemplate: overlay progress from -Path onto a known-good
     current-version save so older full-unlock dumps get modern schema fields
  4) Optional -InstallSteamSlot: write into Steam userdata remote + remotecache

.EXAMPLE
  .\Fix-VSSave.ps1 -Path .\storage\SaveData3 -Force

.EXAMPLE
  # Fix an old full-unlock dump using your current slot as schema template
  .\Fix-VSSave.ps1 -Path .\storage\SaveData -ModernTemplate "C:\Program Files (x86)\Steam\userdata\<YourSteamId>\1794680\remote\SaveData" -InstallSteamSlot 3 -SteamId <YourSteamId> -Force
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(
  [Parameter(Mandatory = $true, Position = 0)]
  [string] $Path,

  [string] $OutPath,

  # Known-good save from THIS game version (schema template). Progress is taken from -Path.
  [string] $ModernTemplate,

  [string] $SaveName,

  [ValidateRange(1, 10)]
  [int] $InstallSteamSlot = 0,

  [string] $SteamId,

  [int] $AppId = 1794680,

  [string[]] $SteamUserdataRoots = @(
    "${env:ProgramFiles(x86)}\Steam\userdata",
    "$env:ProgramFiles\Steam\userdata",
    "D:\Steam\userdata",
    "C:\Steam\userdata"
  ),

  [switch] $NoChecksum,
  [switch] $Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-Sha256Hex([string] $Text) {
  $sha = [System.Security.Cryptography.SHA256]::Create()
  try {
    $bytes = [System.Text.Encoding]::UTF8.GetBytes($Text)
    $hash = $sha.ComputeHash($bytes)
    return -join ($hash | ForEach-Object { $_.ToString('x2') })
  }
  finally { $sha.Dispose() }
}

function Repair-SaveJsonText([string] $Raw) {
  if ([string]::IsNullOrWhiteSpace($Raw)) { throw "File is empty." }
  if ($Raw.Length -gt 0 -and [int][char]$Raw[0] -eq 0xFEFF) { $Raw = $Raw.Substring(1) }
  $Raw = $Raw.Trim()
  $Raw = $Raw -replace ',,+', ','
  $Raw = $Raw -replace ',\s*}', '}'
  $Raw = $Raw -replace ',\s*]', ']'
  # Unity JSON often rejects NaN/Infinity
  $Raw = $Raw -replace ':\s*NaN\b', ':0'
  $Raw = $Raw -replace ':\s*-Infinity\b', ':0'
  $Raw = $Raw -replace ':\s*Infinity\b', ':0'
  if (-not $Raw.StartsWith('{')) {
    throw "Does not look like a VS save (expected JSON object)."
  }
  try { $null = $Raw | ConvertFrom-Json -ErrorAction Stop }
  catch { throw "JSON still invalid after repairs: $($_.Exception.Message)" }
  return $Raw
}

function Update-SaveChecksum([string] $Raw) {
  if ($Raw -notmatch '"checksum"\s*:') {
    if (-not $Raw.EndsWith('}')) { throw "Cannot insert checksum." }
    $inner = $Raw.Substring(0, $Raw.Length - 1).TrimEnd().TrimEnd(',')
    $Raw = $inner + ',"checksum":""}'
  }
  $withEmpty = [regex]::Replace($Raw, '"checksum"\s*:\s*"[a-fA-F0-9]*"', '"checksum":""')
  $hash = Get-Sha256Hex $withEmpty
  $withHash = [regex]::Replace($withEmpty, '"checksum":""', "`"checksum`":`"$hash`"")
  $checkEmpty = [regex]::Replace($withHash, '"checksum"\s*:\s*"[a-fA-F0-9]{64}"', '"checksum":""')
  if ((Get-Sha256Hex $checkEmpty) -ne $hash) { throw "Internal checksum verification failed." }
  return $withHash
}

function Test-SaveChecksum([string] $Raw) {
  if ($Raw -notmatch '"checksum"\s*:\s*"([a-fA-F0-9]{64})"') { return $false }
  $expected = $Matches[1]
  $withEmpty = [regex]::Replace($Raw, '"checksum"\s*:\s*"[a-fA-F0-9]{64}"', '"checksum":""')
  return (Get-Sha256Hex $withEmpty) -eq $expected
}

function Merge-IntoModernTemplate {
  param([string]$ProgressJson, [string]$TemplateJson, [string]$Name)
  # Prefer Node for type-faithful merge
  $node = Get-Command node -ErrorAction SilentlyContinue
  if ($node) {
    $tmpProg = Join-Path $env:TEMP ("vs_prog_{0}.json" -f [guid]::NewGuid().ToString('N'))
    $tmpTmpl = Join-Path $env:TEMP ("vs_tmpl_{0}.json" -f [guid]::NewGuid().ToString('N'))
    $tmpOut  = Join-Path $env:TEMP ("vs_out_{0}.json" -f [guid]::NewGuid().ToString('N'))
    $tmpJs   = Join-Path $env:TEMP ("vs_merge_{0}.js" -f [guid]::NewGuid().ToString('N'))
    try {
      [System.IO.File]::WriteAllText($tmpProg, $ProgressJson)
      [System.IO.File]::WriteAllText($tmpTmpl, $TemplateJson)
      $js = @'
const fs = require("fs");
const crypto = require("crypto");
const prog = JSON.parse(fs.readFileSync(process.argv[2], "utf8"));
const modern = JSON.parse(fs.readFileSync(process.argv[3], "utf8"));
const saveName = process.argv[4] || prog.saveName || "Unlocked";
const out = JSON.parse(JSON.stringify(modern));
const keepModern = new Set([
  "SoundsEnabled","MusicEnabled","SoundsVolume","MusicVolume","Fullscreen","Language",
  "FlashingVFXEnabled","JoystickVisible","SelectedJoystickType","DamageNumbersEnabled",
  "StreamSafeEnabled","ScreenShakeEnabled","ControllerVibrationEnabled","AssignControllerToPlayer1",
  "ShowPlayerIndicators","PermanentCoopOutlines","TintUISelection","PlayerColours",
  "BorderType","PixelFont","ReducePhysics","ClassicMusic","VisuallyInvertStages",
  "HideProgress","HideCompletedAchievements","ShowPickups","ShowSmallMapIcons",
  "GlimmerCarouselEnabled","hideXPBar","HideAdsButtons","DisableMovingBackground","DisableBlood",
  "DisplayDefangedEnemies","StageLighting","SequentialChestMode","SelectedRandomEvents",
  "SelectedRandomLevels","SelectedBGMPlayback","PlayBGMOnlyDuringRun","AlwaysQuickTreasureAnim",
  "CollectionFilterMode","HideUnavailableAdventures","CharacterSelectSortMode","CharacterSelectSortOrder",
  "FavouriteCharacters","HiddenCharacterSelectGroups","SelectedSurvarots","UnlockedSurvarotSets",
  "ActiveSurvarotSets","Platform","SaveOriginalPlatform","SaveTouchedPlatforms","AcceptedEULA",
  "SaveSyncPlatformAchievements","saveDate"
]);
for (const [k,v] of Object.entries(prog)) {
  if (k === "checksum") continue;
  if (keepModern.has(k)) continue;
  out[k] = v;
}
out.saveName = saveName;
if (prog.saveIcon) out.saveIcon = prog.saveIcon;
out.Platform = "Steam";
out.SaveOriginalPlatform = out.SaveOriginalPlatform || "STEAM";
let s = JSON.stringify(out);
if (!/"checksum"\s*:/.test(s)) s = s.replace(/\}$/, ',"checksum":""}');
s = s.replace(/"checksum"\s*:\s*"[a-fA-F0-9]*"/, '"checksum":""');
const hash = crypto.createHash("sha256").update(s,"utf8").digest("hex");
s = s.replace(/"checksum":""/, `"checksum":"${hash}"`);
fs.writeFileSync(process.argv[5], s);
console.log(JSON.stringify({
  name: JSON.parse(s).saveName,
  chars: (JSON.parse(s).UnlockedCharacters||[]).length,
  stages: (JSON.parse(s).UnlockedStages||[]).length,
  coins: JSON.parse(s).Coins,
  len: s.length
}));
'@
      [System.IO.File]::WriteAllText($tmpJs, $js)
      $null = & node $tmpJs $tmpProg $tmpTmpl $Name $tmpOut
      return [System.IO.File]::ReadAllText($tmpOut)
    }
    finally {
      Remove-Item $tmpProg, $tmpTmpl, $tmpOut, $tmpJs -Force -ErrorAction SilentlyContinue
    }
  }

  # Fallback: PowerShell object merge (less type-faithful)
  $prog = $ProgressJson | ConvertFrom-Json
  $modern = $TemplateJson | ConvertFrom-Json
  $base = $modern | ConvertTo-Json -Depth 100 | ConvertFrom-Json
  foreach ($p in $prog.PSObject.Properties) {
    if ($p.Name -eq 'checksum') { continue }
    if ($p.Name -in @('saveDate','Platform','SaveOriginalPlatform','SaveTouchedPlatforms','SoundsEnabled','MusicEnabled')) { continue }
    $base | Add-Member -NotePropertyName $p.Name -NotePropertyValue $p.Value -Force
  }
  if ($Name) { $base.saveName = $Name }
  $compact = $base | ConvertTo-Json -Depth 100 -Compress
  return (Update-SaveChecksum $compact)
}

function Resolve-SteamUserdata([string] $SteamId, [string[]] $Roots) {
  $existing = @()
  foreach ($r in $Roots) {
    if (Test-Path $r) { $existing += (Resolve-Path $r).Path }
  }
  if ($existing.Count -eq 0) { throw "No Steam userdata folder found." }

  if ($SteamId) {
    foreach ($r in $existing) {
      $p = Join-Path $r $SteamId
      if (Test-Path $p) { return (Resolve-Path $p).Path }
    }
    throw "SteamId folder not found: $SteamId"
  }

  $hits = @()
  foreach ($r in $existing) {
    Get-ChildItem $r -Directory -ErrorAction SilentlyContinue | ForEach-Object {
      if (Test-Path (Join-Path $_.FullName "$AppId\remote")) { $hits += $_.FullName }
    }
  }
  $hits = $hits | Select-Object -Unique
  if ($hits.Count -eq 1) { return $hits[0] }
  if ($hits.Count -eq 0) { throw "Could not auto-detect Steam user. Pass -SteamId." }
  throw "Multiple Steam users. Pass -SteamId one of: $($hits | ForEach-Object { Split-Path $_ -Leaf })"
}

function Update-RemoteCache([string] $RemoteDir, [string] $CachePath) {
  $unix = [int][double]::Parse((Get-Date -UFormat %s))
  $sb = New-Object System.Text.StringBuilder
  [void]$sb.AppendLine("`"$AppId`"")
  [void]$sb.AppendLine('{')
  [void]$sb.AppendLine('"ChangeNumber""1"')
  [void]$sb.AppendLine('"ostype""0"')
  $files = Get-ChildItem $RemoteDir -File -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -match '^SaveData\d*$' } | Sort-Object Name
  foreach ($f in $files) {
    $size = $f.Length
    $sha1 = (Get-FileHash $f.FullName -Algorithm SHA1).Hash.ToLowerInvariant()
    [void]$sb.AppendLine("`"$($f.Name)`"")
    [void]$sb.AppendLine('{')
    [void]$sb.AppendLine('"root""0"')
    [void]$sb.AppendLine("`"size`"`"$size`"")
    [void]$sb.AppendLine("`"localtime`"`"$unix`"")
    [void]$sb.AppendLine("`"time`"`"$unix`"")
    [void]$sb.AppendLine("`"remotetime`"`"$unix`"")
    [void]$sb.AppendLine("`"sha`"`"$sha1`"")
    [void]$sb.AppendLine('"syncstate""1"')
    [void]$sb.AppendLine('"persiststate""0"')
    [void]$sb.AppendLine('"platformstosync2""-1"')
    [void]$sb.AppendLine('}')
  }
  [void]$sb.AppendLine('}')
  $utf8 = New-Object System.Text.UTF8Encoding $false
  if ($PSCmdlet.ShouldProcess($CachePath, "Write remotecache.vdf")) {
    [System.IO.File]::WriteAllText($CachePath, $sb.ToString(), $utf8)
  }
}

# ── main ──────────────────────────────────────────────────────────────
$Path = $PSCmdlet.SessionState.Path.GetUnresolvedProviderPathFromPSPath($Path)
if (-not (Test-Path -LiteralPath $Path)) { throw "File not found: $Path" }

Write-Host "Reading $Path"
$original = [System.IO.File]::ReadAllText($Path)
$wasValidChecksum = $false
try { $wasValidChecksum = Test-SaveChecksum $original } catch { }

$repaired = Repair-SaveJsonText $original

if ($ModernTemplate) {
  $ModernTemplate = $PSCmdlet.SessionState.Path.GetUnresolvedProviderPathFromPSPath($ModernTemplate)
  if (-not (Test-Path -LiteralPath $ModernTemplate)) { throw "ModernTemplate not found: $ModernTemplate" }
  Write-Host "Merging progress into modern template: $ModernTemplate"
  $tmpl = [System.IO.File]::ReadAllText($ModernTemplate)
  $tmpl = Repair-SaveJsonText $tmpl
  $name = if ($SaveName) { $SaveName } else { ($repaired | ConvertFrom-Json).saveName }
  if (-not $name) { $name = "Unlocked" }
  $repaired = Merge-IntoModernTemplate -ProgressJson $repaired -TemplateJson $tmpl -Name $name
}
elseif (-not $NoChecksum) {
  $repaired = Update-SaveChecksum $repaired
}

$obj = $repaired | ConvertFrom-Json
$okCs = Test-SaveChecksum $repaired
Write-Host "saveName=$($obj.saveName) coins=$($obj.Coins) unlockedChars=$(@($obj.UnlockedCharacters).Count) stages=$(@($obj.UnlockedStages).Count)"
Write-Host "checksumValid=$okCs (wasValidBefore=$wasValidChecksum)"

if (-not $OutPath) { $OutPath = $Path }
else { $OutPath = $PSCmdlet.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutPath) }

$utf8NoBom = New-Object System.Text.UTF8Encoding $false
if ($PSCmdlet.ShouldProcess($OutPath, "Write fixed save")) {
  if ((Test-Path -LiteralPath $OutPath) -and ($OutPath -eq $Path)) {
    $bak = "$Path.bak_$(Get-Date -Format 'yyyyMMdd_HHmmss')"
    Copy-Item -LiteralPath $Path -Destination $bak -Force
    Write-Host "Backup: $bak"
  }
  [System.IO.File]::WriteAllText($OutPath, $repaired, $utf8NoBom)
  Write-Host "Wrote $OutPath ($((Get-Item -LiteralPath $OutPath).Length) bytes)"
}

if ($InstallSteamSlot -gt 0) {
  $userRoot = Resolve-SteamUserdata -SteamId $SteamId -Roots $SteamUserdataRoots
  $remote = Join-Path $userRoot "$AppId\remote"
  if (-not (Test-Path $remote)) { New-Item -ItemType Directory -Path $remote -Force | Out-Null }
  $slotName = if ($InstallSteamSlot -eq 1) { 'SaveData' } else { "SaveData$InstallSteamSlot" }
  $dest = Join-Path $remote $slotName
  if ($PSCmdlet.ShouldProcess($dest, "Install fixed save as $slotName")) {
    if (Test-Path $dest) {
      Copy-Item $dest "$dest.bak_$(Get-Date -Format 'yyyyMMdd_HHmmss')" -Force
    }
    [System.IO.File]::WriteAllText($dest, $repaired, $utf8NoBom)
    Write-Host "Installed -> $dest"
    $cache = Join-Path $userRoot "$AppId\remotecache.vdf"
    Update-RemoteCache -RemoteDir $remote -CachePath $cache
    Write-Host "Updated $cache"
    Write-Host ""
    Write-Host "Fully quit Steam, then launch Vampire Survivors."
    Write-Host "If Steam asks about cloud conflict, choose LOCAL files."
  }
}

Write-Host "Done."
