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
USE_LATEST=0
NO_DOWNLOAD=0
UNINSTALL=0
REMOVE_ALL=0
KEEP_CONFIG=0
PLATFORM=""
HERE="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

# The build this mod is developed and tested against. Pinned rather than always taking the
# newest: bleeding-edge means exactly that, and a broken loader is harder to diagnose than an
# out-of-date one. --latest opts into the newest build instead.
BUILDS_HOST="https://builds.bepinex.dev"
BUILDS_PAGE="$BUILDS_HOST/projects/bepinex_be"
PINNED_BUILD="785"
PINNED_HASH="6abdba4"

while [ $# -gt 0 ]; do
  case "$1" in
    --game)        GAME="$2"; shift 2 ;;
    --bepinex)     BEPINEX="$2"; shift 2 ;;
    --mod)         MOD="$2"; shift 2 ;;
    --platform)    PLATFORM="$2"; shift 2 ;;
    --latest)      USE_LATEST=1; shift ;;
    --no-download) NO_DOWNLOAD=1; shift ;;
    --uninstall)   UNINSTALL=1; shift ;;
    --all)         REMOVE_ALL=1; shift ;;
    --keep-config) KEEP_CONFIG=1; shift ;;
    --yes|-y)      ASSUME_YES=1; shift ;;
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

fetch() {
  # curl and wget are both common enough that requiring one specifically is unhelpful.
  if command -v curl >/dev/null 2>&1; then curl -fsSL "$1" -o "$2"
  elif command -v wget >/dev/null 2>&1; then wget -qO "$2" "$1"
  else return 127
  fi
}

fetch_stdout() {
  if command -v curl >/dev/null 2>&1; then curl -fsSL "$1"
  elif command -v wget >/dev/null 2>&1; then wget -qO- "$1"
  else return 127
  fi
}

download_bepinex() {
  # Only win-x64 and linux-x64 IL2CPP artifacts are published; there is no macOS build.
  local plat="$PLATFORM"
  if [ -z "$plat" ]; then
    if [ "$(uname -s)" = "Darwin" ]; then
      warn "No BepInEx IL2CPP build is published for macOS." >&2
      info "Only win-x64 and linux-x64 IL2CPP artifacts exist on builds.bepinex.dev." >&2
      return 1
    fi
    plat="linux-x64"
  fi

  local url=""
  if [ "$USE_LATEST" -eq 1 ]; then
    url=$(fetch_stdout "$BUILDS_PAGE" 2>/dev/null \
      | grep -o "/projects/bepinex_be/[0-9]*/BepInEx-Unity\.IL2CPP-${plat}-[^\"'<> ]*\.zip" \
      | head -n1 || true)
    [ -n "$url" ] && url="$BUILDS_HOST$url"
    [ -z "$url" ] && warn "Could not read the build list; falling back to the pinned build." >&2
  fi
  if [ -z "$url" ]; then
    url="$BUILDS_HOST/projects/bepinex_be/$PINNED_BUILD/BepInEx-Unity.IL2CPP-${plat}-6.0.0-be.${PINNED_BUILD}%2B${PINNED_HASH}.zip"
  fi

  step "Downloading BepInEx ($plat)..." >&2
  info "$url" >&2
  local temp
  temp="$(mktemp -t vseh-bepinex-XXXXXX).zip"
  if ! fetch "$url" "$temp"; then
    fail "Download failed (curl or wget required)." >&2
    info "Download it manually from $BUILDS_PAGE then re-run with --bepinex '<path>'" >&2
    rm -f "$temp"
    return 1
  fi
  # A CI error page saved as .zip would fail much later and much less clearly.
  if command -v unzip >/dev/null 2>&1 && ! unzip -tq "$temp" >/dev/null 2>&1; then
    fail "The downloaded file is not a valid zip archive." >&2
    rm -f "$temp"
    return 1
  fi
  ok "Downloaded $(( $(wc -c < "$temp") / 1048576 )) MB" >&2
  printf '%s\n' "$temp"
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

if [ "$UNINSTALL" -eq 1 ]; then
  if pgrep -i "VampireSurvivors" >/dev/null 2>&1; then
    fail "Vampire Survivors is running. Close it first."
    exit 4
  fi

  TARGETS=()
  [ -d "$GAME/BepInEx/plugins/VSEvolutionHelper" ] && TARGETS+=("$GAME/BepInEx/plugins/VSEvolutionHelper")
  if [ "$KEEP_CONFIG" -eq 0 ] && [ -f "$GAME/BepInEx/config/com.nihil.vsevolutionhelper.cfg" ]; then
    TARGETS+=("$GAME/BepInEx/config/com.nihil.vsevolutionhelper.cfg")
  fi

  if [ "$REMOVE_ALL" -eq 1 ]; then
    for n in BepInEx dotnet; do
      [ -d "$GAME/$n" ] && TARGETS+=("$GAME/$n")
    done
    for n in winhttp.dll doorstop_config.ini .doorstop_version run_bepinex.sh libdoorstop.so libdoorstop.dylib; do
      [ -f "$GAME/$n" ] && TARGETS+=("$GAME/$n")
    done
    # changelog.txt is left alone: BepInEx ships one, but so might the game.
  fi

  if [ ${#TARGETS[@]} -eq 0 ]; then
    ok "Nothing to remove - no VS Evolution Helper install found here."
    exit 0
  fi

  if [ "$REMOVE_ALL" -eq 1 ] && [ -d "$GAME/BepInEx/plugins" ]; then
    # BepInEx/plugins is shared; removing the loader takes other mods with it.
    others=$(find "$GAME/BepInEx/plugins" -maxdepth 1 -mindepth 1 ! -name 'VSEvolutionHelper' 2>/dev/null || true)
    if [ -n "$others" ]; then
      warn "Removing BepInEx will also remove these other plugins:"
      while IFS= read -r o; do info "  - $(basename "$o")"; done <<< "$others"
    fi
  fi

  step "About to remove:"
  for t in "${TARGETS[@]}"; do info "  $t"; done
  printf '\n'
  if [ "$REMOVE_ALL" -eq 1 ]; then
    confirm "Remove the mod AND BepInEx?" || { info "Cancelled."; exit 0; }
  else
    confirm "Remove the mod?" || { info "Cancelled."; exit 0; }
  fi

  FAILURES=0
  for t in "${TARGETS[@]}"; do
    if rm -rf "$t"; then ok "Removed $t"; else fail "Could not remove $t"; FAILURES=$((FAILURES+1)); fi
  done

  # If this script disabled MelonLoader on the way in, put it back on the way out.
  if [ "$REMOVE_ALL" -eq 1 ] && [ -f "$GAME/version.dll.melon.off" ] && [ ! -f "$GAME/version.dll" ]; then
    mv "$GAME/version.dll.melon.off" "$GAME/version.dll" && ok "Restored MelonLoader (version.dll)"
  fi

  printf '\n'
  if [ "$FAILURES" -gt 0 ]; then fail "$FAILURES item(s) could not be removed."; exit 8; fi
  if [ "$REMOVE_ALL" -eq 1 ]; then
    ok "BepInEx and the mod removed."
  else
    ok "Mod removed. BepInEx is still installed."
    info "Pass --all to remove BepInEx as well."
  fi
  exit 0
fi

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

if [ -z "$BEPINEX" ] && [ "$BEP_PRESENT" -eq 0 ] && [ "$NO_DOWNLOAD" -eq 0 ]; then
  BEPINEX=$(download_bepinex || true)
fi

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
  fail "BepInEx is not installed and could not be obtained."
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
