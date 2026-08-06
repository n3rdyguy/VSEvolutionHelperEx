#!/usr/bin/env bash
#
# Installs BepInEx and VS Evolution Helper into a Steam copy of Vampire Survivors.
#
# Script equivalent of the bundled installer binary, for anyone who would rather read what they
# run than trust an executable. Same behaviour: find the game through Steam's own library index,
# disable a leftover MelonLoader, unpack BepInEx, drop the mod into BepInEx/plugins.
#
# Usage:
#   ./install.sh
#   ./install.sh --game "/path/to/Vampire Survivors"
#   ./install.sh --bepinex ./BepInEx-Unity.IL2CPP-linux-x64-6.0.0-be.785.zip
#
set -euo pipefail

APP_ID=1794680
GAME=""
BEPINEX=""
MOD=""
ASSUME_YES=0
HERE="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

while [ $# -gt 0 ]; do
  case "$1" in
    --game)    GAME="$2"; shift 2 ;;
    --bepinex) BEPINEX="$2"; shift 2 ;;
    --mod)     MOD="$2"; shift 2 ;;
    --yes|-y)  ASSUME_YES=1; shift ;;
    -h|--help) sed -n '2,14p' "$0"; exit 0 ;;
    *) echo "Unknown option: $1" >&2; exit 64 ;;
  esac
done

if [ -t 1 ]; then
  C_RED=$'\033[31m'; C_GRN=$'\033[32m'; C_YEL=$'\033[33m'
  C_CYN=$'\033[36m'; C_MAG=$'\033[35m'; C_BLD=$'\033[1;37m'; C_OFF=$'\033[0m'
else
  C_RED=""; C_GRN=""; C_YEL=""; C_CYN=""; C_MAG=""; C_BLD=""; C_OFF=""
fi

step() { printf '%s  >  %s%s\n' "$C_CYN" "$C_OFF" "$1"; }
ok()   { printf '%s  +  %s%s\n' "$C_GRN" "$C_OFF" "$1"; }
warn() { printf '%s  !  %s%s\n' "$C_YEL" "$C_OFF" "$1"; }
fail() { printf '%s  x  %s%s\n' "$C_RED" "$C_OFF" "$1"; }
info() { printf '     %s\n' "$1"; }

banner() {
  printf '\n'
  printf '%s        __       _,-"~^"-.%s\n'      "$C_RED" "$C_OFF"
  printf '%s      _// )      _,'"'"'       `.%s\n' "$C_RED" "$C_OFF"
  printf '%s      " ( ^ ~^~ /             )%s\n'  "$C_RED" "$C_OFF"
  printf '%s       `.       (  )        ,'"'"'%s\n' "$C_RED" "$C_OFF"
  printf '%s         `-._  _)  ) ___,-'"'"'%s\n'   "$C_RED" "$C_OFF"
  printf '%s             ``   ``%s\n'            "$C_RED" "$C_OFF"
  printf '%s   V S   E V O L U T I O N   H E L P E R%s\n' "$C_BLD" "$C_OFF"
  printf '%s   ~ it is a night of tooltips ~%s\n\n'       "$C_MAG" "$C_OFF"
}

confirm() {
  [ "$ASSUME_YES" -eq 1 ] && return 0
  printf '%s  ?  %s%s [y/N] ' "$C_MAG" "$C_OFF" "$1"
  read -r answer
  case "$answer" in [Yy]*) return 0 ;; *) return 1 ;; esac
}

steam_roots() {
  if [ "$(uname -s)" = "Darwin" ]; then
    echo "$HOME/Library/Application Support/Steam"
  else
    echo "$HOME/.steam/steam"
    echo "$HOME/.steam/root"
    echo "$HOME/.local/share/Steam"
    # Flatpak keeps its own copy of everything.
    echo "$HOME/.var/app/com.valvesoftware.Steam/data/Steam"
  fi
}

# Every library folder Steam knows about, including the root install itself.
libraries() {
  local root="$1"
  echo "$root"
  local vdf="$root/steamapps/libraryfolders.vdf"
  [ -f "$vdf" ] || return 0
  grep -o '"path"[[:space:]]*"[^"]*"' "$vdf" 2>/dev/null \
    | sed 's/.*"path"[[:space:]]*"//; s/"$//; s|\\\\|/|g' || true
}

find_game() {
  step "Looking for Vampire Survivors..."
  local root lib manifest installdir path
  while IFS= read -r root; do
    [ -d "$root/steamapps" ] || continue
    while IFS= read -r lib; do
      manifest="$lib/steamapps/appmanifest_${APP_ID}.acf"
      [ -f "$manifest" ] || continue
      installdir=$(grep -o '"installdir"[[:space:]]*"[^"]*"' "$manifest" \
        | sed 's/.*"installdir"[[:space:]]*"//; s/"$//' || true)
      [ -n "$installdir" ] || installdir="Vampire Survivors"
      path="$lib/steamapps/common/$installdir"
      if [ -d "$path" ]; then printf '%s\n' "$path"; return 0; fi
    done < <(libraries "$root")
  done < <(steam_roots)
  return 1
}

find_payload() {
  local pattern="$1" dir hit
  for dir in "$HERE" "$HERE/payload" "$PWD"; do
    [ -d "$dir" ] || continue
    hit=$(find "$dir" -maxdepth 1 -name "$pattern" -type f 2>/dev/null | head -n1 || true)
    [ -n "$hit" ] && { printf '%s\n' "$hit"; return 0; }
  done
  return 1
}

banner

if [ -z "$GAME" ]; then GAME=$(find_game || true); fi
if [ -z "$GAME" ]; then
  fail "Could not find Vampire Survivors."
  info 'Pass the folder explicitly:  ./install.sh --game "<path>"'
  exit 2
fi
ok "Game folder: $GAME"

if [ ! -e "$GAME/VampireSurvivors.exe" ] && [ ! -e "$GAME/GameAssembly.dll" ] \
   && [ ! -e "$GAME/VampireSurvivors.app" ]; then
  warn "That folder does not look like a Vampire Survivors install."
  confirm "Continue anyway?" || exit 3
fi

if pgrep -i "VampireSurvivors" >/dev/null 2>&1; then
  fail "Vampire Survivors is running. Close it first."
  exit 4
fi

# MelonLoader and BepInEx both hook the process; together they crash the game. Rename rather
# than delete - reversible, and not this script's place to throw away another loader.
if [ -f "$GAME/version.dll" ]; then
  rm -f "$GAME/version.dll.melon.off"
  mv "$GAME/version.dll" "$GAME/version.dll.melon.off"
  warn "MelonLoader found - renamed version.dll to version.dll.melon.off"
  warn "Rename it back to undo. Running both loaders crashes the game."
fi

[ -n "$BEPINEX" ] || BEPINEX=$(find_payload 'BepInEx*.zip' || true)
BEP_PRESENT=0
[ -d "$GAME/BepInEx/core" ] && BEP_PRESENT=1

if [ -n "$BEPINEX" ]; then
  DO_IT=1
  if [ "$BEP_PRESENT" -eq 1 ]; then
    confirm "BepInEx is already installed. Reinstall it?" || DO_IT=0
  fi
  if [ "$DO_IT" -eq 1 ]; then
    step "Installing BepInEx..."
    command -v unzip >/dev/null 2>&1 || { fail "unzip is required but not installed."; exit 7; }
    unzip -oq "$BEPINEX" -d "$GAME"
    # Some archives wrap everything in one folder; lift it out if so.
    wrapper=$(find "$GAME" -maxdepth 1 -type d -name 'BepInEx-*' | head -n1 || true)
    if [ -n "$wrapper" ]; then
      (shopt -s dotglob; mv "$wrapper"/* "$GAME"/)
      rmdir "$wrapper" 2>/dev/null || true
    fi
    # The launcher script is what starts the game with the loader attached.
    [ -f "$GAME/run_bepinex.sh" ] && chmod +x "$GAME/run_bepinex.sh"
    ok "BepInEx installed from $(basename "$BEPINEX")"
    BEP_PRESENT=1
  else
    info "Keeping the existing BepInEx."
  fi
elif [ "$BEP_PRESENT" -eq 0 ]; then
  fail "BepInEx is not installed and no BepInEx archive was found next to this script."
  info "Download the Unity.IL2CPP build from https://builds.bepinex.dev/projects/bepinex_be"
  info 'then re-run with:  ./install.sh --bepinex "<path to zip>"'
  exit 5
else
  ok "BepInEx already installed."
fi

[ -n "$MOD" ] || MOD=$(find_payload 'VSEvolutionHelper.dll' || true)
if [ -z "$MOD" ]; then
  fail "Could not find VSEvolutionHelper.dll next to this script."
  exit 6
fi

TARGET="$GAME/BepInEx/plugins/VSEvolutionHelper"
mkdir -p "$TARGET"
step "Installing the mod..."
cp -f "$MOD" "$TARGET/VSEvolutionHelper.dll"
ok "Mod installed: $TARGET/VSEvolutionHelper.dll"

if [ ! -f "$GAME/run_bepinex.sh" ] && [ ! -f "$GAME/winhttp.dll" ]; then
  warn "No BepInEx loader found next to the game."
  warn "On macOS/Linux the game must be launched through run_bepinex.sh."
fi

printf '\n'
ok "Done."
printf '\n'
if [ -f "$GAME/run_bepinex.sh" ]; then
  info "On macOS/Linux the loader only attaches when the game is started through"
  info "run_bepinex.sh. To make Steam do that, set its launch options to:"
  info "    \"$GAME/run_bepinex.sh\" %command%"
  printf '\n'
fi
info "Launch the game once and let it reach the main menu. The first launch after"
info "installing BepInEx is slow - it generates the IL2CPP interop assemblies."
printf '\n'
info "To confirm, look in  BepInEx/LogOutput.log  for:"
info "    Loading [VS Evolution Helper ...]"
