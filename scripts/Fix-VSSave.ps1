<#
.SYNOPSIS
  Fix Vampire Survivors save files (JSON repair + checksum).

.DESCRIPTION
  - Removes accidental double commas (,,) that invalidate JSON
  - Parses and re-validates as JSON
  - Regenerates "checksum" the way the current game expects:
      SHA-256(UTF-8) of the compact JSON with "checksum":"" then write the hash back
  - Optional: install into Steam cloud remote slot folder
  - Optional: refresh remotecache.vdf for that app

.EXAMPLE
  .\Fix-VSSave.ps1 -Path .\SaveData3

.EXAMPLE
  .\Fix-VSSave.ps1 -Path .\SaveData -InstallSteamSlot 3 -SteamId <YourSteamId>

.EXAMPLE
  .\Fix-VSSave.ps1 -Path .\SaveData3 -OutPath .\SaveData3.fixed -WhatIf
#>
[CmdletBinding(SupportsShouldProcess = $true)]
param(
  [Parameter(Mandatory = $true, Position = 0)]
  [string] $Path,

  [string] $OutPath,

  # 1 = SaveData, 2 = SaveData2, ... (Steam remote file name)
  [ValidateRange(1, 10)]
  [int] $InstallSteamSlot = 0,

  # Steam user id folder under userdata (auto-detect if only one)
  [string] $SteamId,

  # Vampire Survivors Steam AppID
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
  if ([string]::IsNullOrWhiteSpace($Raw)) {
    throw "File is empty."
  }

  # BOM
  if ($Raw.Length -gt 0 -and [int][char]$Raw[0] -eq 0xFEFF) {
    $Raw = $Raw.Substring(1)
  }

  $Raw = $Raw.Trim()

  # Common corruptions
  $Raw = $Raw -replace ',,+', ','          # double/multi commas
  $Raw = $Raw -replace ',\s*}', '}'      # trailing commas before }
  $Raw = $Raw -replace ',\s*]', ']'      # trailing commas before ]

  # Must be an object
  if (-not $Raw.StartsWith('{')) {
    throw "Does not look like a VS save (expected JSON object starting with '{')."
  }

  # Validate parse
  try {
    $null = $Raw | ConvertFrom-Json -ErrorAction Stop
  }
  catch {
    throw "JSON still invalid after basic repairs: $($_.Exception.Message)"
  }

  return $Raw
}

function Update-SaveChecksum([string] $Raw) {
  # Ensure checksum property exists
  if ($Raw -notmatch '"checksum"\s*:') {
    if (-not $Raw.EndsWith('}')) {
      throw "Cannot insert checksum: unexpected file ending."
    }
    $inner = $Raw.Substring(0, $Raw.Length - 1).TrimEnd().TrimEnd(',')
    $Raw = $inner + ',"checksum":""}'
  }

  # Empty the checksum value in-place (keep compact layout / key order)
  $withEmpty = [regex]::Replace(
    $Raw,
    '"checksum"\s*:\s*"[a-fA-F0-9]*"',
    '"checksum":""'
  )

  $hash = Get-Sha256Hex $withEmpty
  $withHash = [regex]::Replace(
    $withEmpty,
    '"checksum":""',
    "`"checksum`":`"$hash`""
  )

  # Verify
  $checkEmpty = [regex]::Replace($withHash, '"checksum"\s*:\s*"[a-fA-F0-9]{64}"', '"checksum":""')
  $verify = Get-Sha256Hex $checkEmpty
  if ($verify -ne $hash) {
    throw "Internal checksum verification failed."
  }

  return $withHash
}

function Test-SaveChecksum([string] $Raw) {
  if ($Raw -notmatch '"checksum"\s*:\s*"([a-fA-F0-9]{64})"') {
    return $false
  }
  $expected = $Matches[1]
  $withEmpty = [regex]::Replace($Raw, '"checksum"\s*:\s*"[a-fA-F0-9]{64}"', '"checksum":""')
  return (Get-Sha256Hex $withEmpty) -eq $expected
}

function Resolve-SteamUserdata([string] $SteamId, [string[]] $Roots) {
  $existing = @()
  foreach ($r in $Roots) {
    if (Test-Path $r) { $existing += (Resolve-Path $r).Path }
  }
  if ($existing.Count -eq 0) {
    throw "No Steam userdata folder found. Pass -SteamId and ensure Steam is installed."
  }

  if ($SteamId) {
    foreach ($r in $existing) {
      $p = Join-Path $r $SteamId
      if (Test-Path $p) { return (Resolve-Path $p).Path }
    }
    throw "SteamId folder not found: $SteamId under $($existing -join ', ')"
  }

  # Auto: pick userdata/*/AppId if unique
  $hits = @()
  foreach ($r in $existing) {
    Get-ChildItem $r -Directory -ErrorAction SilentlyContinue | ForEach-Object {
      $app = Join-Path $_.FullName "$AppId\remote"
      if (Test-Path $app) { $hits += $_.FullName }
    }
  }
  $hits = $hits | Select-Object -Unique
  if ($hits.Count -eq 1) { return $hits[0] }
  if ($hits.Count -eq 0) {
    throw "Could not auto-detect Steam user with app $AppId. Pass -SteamId."
  }
  throw "Multiple Steam users have app $AppId. Pass -SteamId one of: $($hits | ForEach-Object { Split-Path $_ -Leaf })"
}

function Update-RemoteCache([string] $RemoteDir, [string] $CachePath) {
  $unix = [int][double]::Parse((Get-Date -UFormat %s))
  $sb = New-Object System.Text.StringBuilder
  [void]$sb.AppendLine("`"$AppId`"")
  [void]$sb.AppendLine('{')
  [void]$sb.AppendLine('"ChangeNumber""1"')
  [void]$sb.AppendLine('"ostype""0"')

  $files = Get-ChildItem $RemoteDir -File -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -match '^SaveData\d*$' } |
    Sort-Object Name

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
if (-not (Test-Path -LiteralPath $Path)) {
  throw "File not found: $Path"
}

Write-Host "Reading $Path"
$original = [System.IO.File]::ReadAllText($Path)
$wasValidChecksum = $false
try { $wasValidChecksum = Test-SaveChecksum $original } catch { $wasValidChecksum = $false }

$repaired = Repair-SaveJsonText $original
if (-not $NoChecksum) {
  $repaired = Update-SaveChecksum $repaired
}

# Summarize
$obj = $repaired | ConvertFrom-Json
$name = $obj.saveName
$chars = @($obj.UnlockedCharacters).Count
$stages = @($obj.UnlockedStages).Count
$coins = $obj.Coins
$okCs = Test-SaveChecksum $repaired

Write-Host "saveName=$name coins=$coins unlockedChars=$chars stages=$stages"
Write-Host "checksumValid=$okCs (wasValidBefore=$wasValidChecksum)"

if (-not $OutPath) {
  $OutPath = $Path
}
else {
  $OutPath = $PSCmdlet.SessionState.Path.GetUnresolvedProviderPathFromPSPath($OutPath)
}

$utf8NoBom = New-Object System.Text.UTF8Encoding $false
if ($PSCmdlet.ShouldProcess($OutPath, "Write fixed save")) {
  if ((Test-Path -LiteralPath $OutPath) -and -not $Force -and ($OutPath -eq $Path)) {
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
  if (-not (Test-Path $remote)) {
    New-Item -ItemType Directory -Path $remote -Force | Out-Null
  }
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
    Write-Host "Quit Steam completely, then launch Vampire Survivors."
    Write-Host "If Steam asks about cloud conflict, choose local files."
  }
}

Write-Host "Done."
