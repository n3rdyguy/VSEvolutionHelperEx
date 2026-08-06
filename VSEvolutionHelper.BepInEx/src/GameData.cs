using System;
using System.Collections.Generic;
using BepInEx.Logging;
using I2.Loc;
using Il2CppSystem.Collections.Generic;
using UnityEngine;
using VampireSurvivors.Data;
using VampireSurvivors.Data.PowerUp;
using VampireSurvivors.Data.Weapons;
using VampireSurvivors.Graphics;
using VampireSurvivors.UI;
using Object = UnityEngine.Object;

// Alias Il2Cpp collections so we don't clash with System.Collections.Generic
using VampireSurvivors.Data.Items;
using Il2CppDictWeapons = Il2CppSystem.Collections.Generic.Dictionary<VampireSurvivors.Data.WeaponType, Il2CppSystem.Collections.Generic.List<VampireSurvivors.Data.Weapons.WeaponData>>;
using Il2CppDictPowerUps = Il2CppSystem.Collections.Generic.Dictionary<VampireSurvivors.Data.PowerUpType, Il2CppSystem.Collections.Generic.List<VampireSurvivors.Data.PowerUp.PowerUpData>>;
using Il2CppDictArcanas = Il2CppSystem.Collections.Generic.Dictionary<VampireSurvivors.Data.ArcanaType, VampireSurvivors.Data.ArcanaData>;
using Il2CppDictItems = Il2CppSystem.Collections.Generic.Dictionary<VampireSurvivors.Data.ItemType, VampireSurvivors.Data.Items.ItemData>;
using Il2CppListWeapons = Il2CppSystem.Collections.Generic.List<VampireSurvivors.Data.Weapons.WeaponData>;
using Il2CppListWeaponType = Il2CppSystem.Collections.Generic.List<VampireSurvivors.Data.WeaponType>;
using Il2CppListObject = Il2CppSystem.Collections.Generic.List<Il2CppSystem.Object>;

namespace VSItemTooltips;

/// <summary>
/// Typed access to Vampire Survivors 1.15 data (from BepInEx Il2Cpp interop decompilation).
/// Replaces reflection-heavy MelonLoader-era dictionary walks that break under Il2Cpp lists.
/// </summary>
public static class GameData
{
    private static ManualLogSource Log => Plugin.Log;

    private static DataManager _dataManager;
    private static Il2CppDictWeapons _weapons;
    private static Il2CppDictPowerUps _powerUps;
    private static Il2CppDictArcanas _arcanas;
    private static Il2CppDictItems _items;
    private static bool _built;
    private static bool _loggedBuild;
    private static bool _arcanaBuilt;
    private static bool _loggedArcana;
    private static bool _itemsBuilt;
    private static bool _loggedItems;

    // frameName (lowercase) -> weapon / powerup / item
    public static readonly System.Collections.Generic.Dictionary<string, WeaponType> SpriteToWeapon =
        new System.Collections.Generic.Dictionary<string, WeaponType>(StringComparer.OrdinalIgnoreCase);
    public static readonly System.Collections.Generic.Dictionary<string, PowerUpType> SpriteToPowerUp =
        new System.Collections.Generic.Dictionary<string, PowerUpType>(StringComparer.OrdinalIgnoreCase);
    public static readonly System.Collections.Generic.Dictionary<string, ItemType> SpriteToItem =
        new System.Collections.Generic.Dictionary<string, ItemType>(StringComparer.OrdinalIgnoreCase);

    // Pretty names (localized when possible)
    private static readonly System.Collections.Generic.Dictionary<WeaponType, string> WeaponNames =
        new System.Collections.Generic.Dictionary<WeaponType, string>();
    private static readonly System.Collections.Generic.Dictionary<PowerUpType, string> PowerUpNames =
        new System.Collections.Generic.Dictionary<PowerUpType, string>();
    private static readonly System.Collections.Generic.Dictionary<ItemType, string> ItemNames =
        new System.Collections.Generic.Dictionary<ItemType, string>();
    private static readonly System.Collections.Generic.Dictionary<ItemType, string> ItemDescriptions =
        new System.Collections.Generic.Dictionary<ItemType, string>();
    private static readonly System.Collections.Generic.Dictionary<ArcanaType, string> ArcanaNames =
        new System.Collections.Generic.Dictionary<ArcanaType, string>();
    private static readonly System.Collections.Generic.Dictionary<ArcanaType, string> ArcanaDescriptions =
        new System.Collections.Generic.Dictionary<ArcanaType, string>();

    // Reverse index: weapon/item -> arcanas that list them
    private static readonly System.Collections.Generic.Dictionary<WeaponType, System.Collections.Generic.List<ArcanaType>> WeaponToArcanas =
        new System.Collections.Generic.Dictionary<WeaponType, System.Collections.Generic.List<ArcanaType>>();
    private static readonly System.Collections.Generic.Dictionary<ItemType, System.Collections.Generic.List<ArcanaType>> ItemToArcanas =
        new System.Collections.Generic.Dictionary<ItemType, System.Collections.Generic.List<ArcanaType>>();

    public static bool IsReady => _built && _weapons != null;
    public static DataManager DataManager => _dataManager;
    public static Il2CppDictWeapons WeaponsDict => _weapons;
    public static Il2CppDictPowerUps PowerUpsDict => _powerUps;
    public static Il2CppDictArcanas ArcanasDict => _arcanas;
    public static Il2CppDictItems ItemsDict => _items;

    public static void Reset()
    {
        _dataManager = null;
        _weapons = null;
        _powerUps = null;
        _arcanas = null;
        _items = null;
        _built = false;
        _loggedBuild = false;
        _arcanaBuilt = false;
        _loggedArcana = false;
        _itemsBuilt = false;
        _loggedItems = false;
        SpriteToWeapon.Clear();
        SpriteToPowerUp.Clear();
        SpriteToItem.Clear();
        WeaponNames.Clear();
        PowerUpNames.Clear();
        ItemNames.Clear();
        ItemDescriptions.Clear();
        ArcanaNames.Clear();
        ArcanaDescriptions.Clear();
        WeaponToArcanas.Clear();
        ItemToArcanas.Clear();
    }

    /// <summary>Try to locate DataManager in the scene / UI and build caches.</summary>
    public static bool EnsureLoaded()
    {
        if (_built && _weapons != null)
        {
            return true;
        }

        // DataManager is not a UnityEngine.Object — resolve via UI pages / GameManager injection.
        if (_dataManager == null)
        {
            try
            {
                var pages = Object.FindObjectsOfType<BaseUIPage>();
                if (pages != null)
                {
                    int n = pages.Length;
                    for (int i = 0; i < n; i++)
                    {
                        var page = pages[i];
                        if (page != null && page.Data != null)
                        {
                            _dataManager = page.Data;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogWarning("[GameData] BaseUIPage.Data: " + ex.Message);
            }
        }

        if (_dataManager == null)
        {
            try
            {
                // Scan MonoBehaviours for a "Data" property of type DataManager
                var behaviours = Object.FindObjectsOfType<MonoBehaviour>();
                if (behaviours != null)
                {
                    int n = Math.Min(behaviours.Length, 200);
                    for (int i = 0; i < n; i++)
                    {
                        var b = behaviours[i];
                        if (b == null) continue;
                        var prop = b.GetType().GetProperty("Data", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                        if (prop != null && typeof(DataManager).IsAssignableFrom(prop.PropertyType))
                        {
                            var val = prop.GetValue(b) as DataManager;
                            if (val != null)
                            {
                                _dataManager = val;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogWarning("[GameData] MonoBehaviour Data scan: " + ex.Message);
            }
        }

        if (_dataManager == null)
        {
            return false;
        }

        try
        {
            _weapons = _dataManager.GetConvertedWeapons();
            _powerUps = _dataManager.GetConvertedPowerUpData();

            // Merge DLC weapons
            try
            {
                foreach (DlcType dlc in Enum.GetValues(typeof(DlcType)))
                {
                    try
                    {
                        var dlcWeapons = _dataManager.GetConvertedDlcWeaponData(dlc);
                        MergeWeaponDict(_weapons, dlcWeapons);
                    }
                    catch
                    {
                        // DLC not owned / empty
                    }

                    try
                    {
                        var dlcPu = _dataManager.GetConvertedDlcPowerUpData(dlc);
                        MergePowerUpDict(_powerUps, dlcPu);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogWarning("[GameData] DLC merge: " + ex.Message);
            }

            BuildCaches();
            BuildArcanaCaches();
            BuildItemCaches();
            _built = SpriteToWeapon.Count > 0 || WeaponNames.Count > 0;
            if (_built && !_loggedBuild)
            {
                _loggedBuild = true;
                Log.LogInfo($"[GameData] Ready: {WeaponNames.Count} weapon names, {SpriteToWeapon.Count} weapon sprites, {PowerUpNames.Count} powerups, {ItemNames.Count} items, {ArcanaNames.Count} arcanas (typed Il2Cpp API)");
            }
            return _built;
        }
        catch (Exception ex)
        {
            Log.LogError("[GameData] EnsureLoaded failed: " + ex);
            return false;
        }
    }

    public static void CacheFrom(DataManager dm)
    {
        if (dm == null)
        {
            return;
        }
        _dataManager = dm;
        _built = false;
        EnsureLoaded();
    }

    private static void BuildCaches()
    {
        SpriteToWeapon.Clear();
        SpriteToPowerUp.Clear();
        WeaponNames.Clear();
        PowerUpNames.Clear();

        if (_weapons != null)
        {
            foreach (var kvp in _weapons)
            {
                WeaponType type = kvp.Key;
                Il2CppListWeapons list = kvp.Value;
                if (list == null || list.Count == 0)
                {
                    continue;
                }

                WeaponData head = list[0];
                if (head == null)
                {
                    continue;
                }

                string frame = head.frameName;
                if (!string.IsNullOrEmpty(frame) && !SpriteToWeapon.ContainsKey(frame))
                {
                    SpriteToWeapon[frame] = type;
                }

                string pretty = ResolveWeaponName(head, type);
                WeaponNames[type] = pretty;

                // Also index every level entry frame
                for (int i = 0; i < list.Count; i++)
                {
                    var wd = list[i];
                    if (wd == null || string.IsNullOrEmpty(wd.frameName))
                    {
                        continue;
                    }
                    if (!SpriteToWeapon.ContainsKey(wd.frameName))
                    {
                        SpriteToWeapon[wd.frameName] = type;
                    }
                }
            }
        }

        // Strip noise frames (same as original mod)
        SpriteToWeapon.Remove("goldenegg");
        SpriteToWeapon.Remove("Antidote");

        if (_powerUps != null)
        {
            foreach (var kvp in _powerUps)
            {
                PowerUpType type = kvp.Key;
                var list = kvp.Value;
                if (list == null || list.Count == 0)
                {
                    continue;
                }
                PowerUpData head = list[0];
                if (head == null)
                {
                    continue;
                }
                if (!string.IsNullOrEmpty(head.frameName))
                {
                    SpriteToPowerUp[head.frameName] = type;
                }
                PowerUpNames[type] = ResolvePowerUpName(head, type);
            }
        }
    }

    private static void MergeWeaponDict(Il2CppDictWeapons target, Il2CppDictWeapons source)
    {
        if (target == null || source == null)
        {
            return;
        }
        foreach (var kvp in source)
        {
            if (!target.ContainsKey(kvp.Key))
            {
                target[kvp.Key] = kvp.Value;
            }
        }
    }

    private static void MergePowerUpDict(Il2CppDictPowerUps target, Il2CppDictPowerUps source)
    {
        if (target == null || source == null)
        {
            return;
        }
        foreach (var kvp in source)
        {
            if (!target.ContainsKey(kvp.Key))
            {
                target[kvp.Key] = kvp.Value;
            }
        }
    }

    public static WeaponData GetWeaponData(WeaponType type)
    {
        EnsureLoaded();
        if (_weapons == null || !_weapons.ContainsKey(type))
        {
            return null;
        }
        var list = _weapons[type];
        if (list == null || list.Count == 0)
        {
            return null;
        }
        return list[0];
    }

    public static Il2CppListWeapons GetWeaponDataList(WeaponType type)
    {
        EnsureLoaded();
        if (_weapons == null || !_weapons.ContainsKey(type))
        {
            return null;
        }
        return _weapons[type];
    }

    public static PowerUpData GetPowerUpData(PowerUpType type)
    {
        EnsureLoaded();
        if (_powerUps == null || !_powerUps.ContainsKey(type))
        {
            return null;
        }
        var list = _powerUps[type];
        if (list == null || list.Count == 0)
        {
            return null;
        }
        return list[0];
    }

    public static string GetWeaponName(WeaponType type)
    {
        EnsureLoaded();
        // VOID is not a real weapon — callers that care should use IsRealWeaponType first
        if (!IsRealWeaponType(type))
            return "";
        if (WeaponNames.TryGetValue(type, out string cached) && !string.IsNullOrEmpty(cached))
        {
            return cached;
        }
        var data = GetWeaponData(type);
        string name = ResolveWeaponName(data, type);
        if (string.Equals(name, "VOID", StringComparison.OrdinalIgnoreCase)
            || string.Equals(name, "Void", StringComparison.OrdinalIgnoreCase))
            return "";
        WeaponNames[type] = name;
        return name;
    }

    /// <summary>VOID (0) is the enum default / "no weapon", not a usable starter.</summary>
    public static bool IsRealWeaponType(WeaponType type)
    {
        try
        {
            int v = Convert.ToInt32(type);
            if (v == 0) return false;
        }
        catch
        {
            string s = type.ToString();
            if (string.IsNullOrEmpty(s) || s.Equals("VOID", StringComparison.OrdinalIgnoreCase) || s == "0")
                return false;
        }
        return true;
    }

    /// <summary>
    /// Map a UI sprite (e.g. CharacterItemUI._WeaponIcon) back to a WeaponType.
    /// Matches frameName, sprite.name, and instance/rect equality against known weapon sprites.
    /// </summary>
    public static bool TryIdentifyWeaponFromSprite(Sprite sprite, out WeaponType type)
    {
        type = default;
        if ((Object)(object)sprite == (Object)null) return false;
        EnsureLoaded();

        string sn = null;
        try { sn = sprite.name; } catch { }
        if (!string.IsNullOrEmpty(sn))
        {
            // Unity often appends " (Instance)" or clone suffixes
            string bare = sn;
            int paren = bare.IndexOf(" (", StringComparison.Ordinal);
            if (paren > 0) bare = bare.Substring(0, paren);
            bare = bare.Trim();

            if (SpriteToWeapon.TryGetValue(bare, out type) && IsRealWeaponType(type))
                return true;
            if (SpriteToWeapon.TryGetValue(sn, out type) && IsRealWeaponType(type))
                return true;

            // case-insensitive frame lookup
            foreach (var kv in SpriteToWeapon)
            {
                if (!IsRealWeaponType(kv.Value)) continue;
                if (string.Equals(kv.Key, bare, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(kv.Key, sn, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(System.IO.Path.GetFileNameWithoutExtension(kv.Key), bare, StringComparison.OrdinalIgnoreCase))
                {
                    type = kv.Value;
                    return true;
                }
            }

            // enum / slug parse from sprite name (e.g. "whip", "Weapon_Whip")
            if (TryParseWeaponType(bare, out type) && IsRealWeaponType(type))
                return true;
            string compact = System.Text.RegularExpressions.Regex.Replace(bare, "^Weapon_?", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (TryParseWeaponType(compact, out type) && IsRealWeaponType(type))
                return true;
        }

        // Pixel/identity match against loaded weapon sprites (slower, reliable for set icons)
        try
        {
            if (_weapons != null)
            {
                foreach (var kvp in _weapons)
                {
                    WeaponType wt = kvp.Key;
                    if (!IsRealWeaponType(wt)) continue;
                    Sprite known = GetSprite(wt);
                    if ((Object)(object)known == (Object)null) continue;
                    if ((Object)(object)known == (Object)(object)sprite)
                    {
                        type = wt;
                        return true;
                    }
                    try
                    {
                        if ((Object)(object)known.texture == (Object)(object)sprite.texture
                            && known.rect.Equals(sprite.rect)
                            && known.pivot == sprite.pivot)
                        {
                            type = wt;
                            return true;
                        }
                    }
                    catch { }
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] TryIdentifyWeaponFromSprite: " + ex.Message);
        }

        return false;
    }

    public static string GetWeaponDescription(WeaponType type)
    {
        EnsureLoaded();
        var data = GetWeaponData(type);
        if (data == null)
        {
            return "";
        }
        try
        {
            string term = data.GetLocalizedDescriptionTerm(type);
            string t = LocalizeDisplayText(term);
            if (!string.IsNullOrEmpty(t))
                return t;
        }
        catch
        {
        }
        string raw = null;
        try { raw = data.description; } catch { }
        if (string.IsNullOrEmpty(raw))
        {
            try { raw = data.tips; } catch { }
        }
        return LocalizeDisplayText(raw) ?? "";
    }

    public static string GetPowerUpName(PowerUpType type)
    {
        EnsureLoaded();
        if (PowerUpNames.TryGetValue(type, out string cached) && !string.IsNullOrEmpty(cached))
        {
            return cached;
        }
        var data = GetPowerUpData(type);
        string name = ResolvePowerUpName(data, type);
        PowerUpNames[type] = name;
        return name;
    }

    private static string ResolveWeaponName(WeaponData data, WeaponType type)
    {
        if (data != null)
        {
            try
            {
                string t = LocalizeDisplayText(data.GetLocalizedNameTerm(type));
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch
            {
            }

            try
            {
                string t = LocalizeDisplayText(data.name);
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch
            {
            }
        }
        return HumanizeEnum(type.ToString());
    }

    private static string ResolvePowerUpName(PowerUpData data, PowerUpType type)
    {
        if (data != null)
        {
            try
            {
                string t = LocalizeDisplayText(data.GetLocalizedName(type));
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch
            {
            }
            try
            {
                string t = LocalizeDisplayText(data.name);
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch
            {
            }
        }
        return HumanizeEnum(type.ToString());
    }

    public static string Translate(string term)
    {
        if (string.IsNullOrEmpty(term))
        {
            return null;
        }
        // Already human text (spaces, no term-path markers)
        if (!LooksLikeLocKey(term))
        {
            return term;
        }
        try
        {
            string result = TryI2(term, applyParameters: false);
            if (IsGoodTranslation(result, term))
                return result;
            // Terms with {TYPE} placeholders often need parameter application
            if (term.IndexOf('{') >= 0)
            {
                result = TryI2(term, applyParameters: true);
                if (IsGoodTranslation(result, term))
                    return result;
            }
            return null;
        }
        catch (Exception ex)
        {
            Log.LogWarning("[GameData.I2] " + ex.Message);
            return null;
        }
    }

    /// <summary>
    /// Resolve player-facing text from a raw game string or I2 term.
    /// Never returns raw keys like <c>itemLang/{MERCHANT}description</c>
    /// or <c>powerupLang/MERCHANT name</c>.
    /// </summary>
    public static string LocalizeDisplayText(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return null;
        string s = raw.Trim();
        // Collapse internal newlines in single-field terms (UI sometimes wraps keys)
        if (s.IndexOf('\n') >= 0 || s.IndexOf('\r') >= 0)
        {
            // Only collapse if every line still looks like part of a key (no prose)
            string flat = s.Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ');
            flat = System.Text.RegularExpressions.Regex.Replace(flat, @"\s+", " ").Trim();
            if (LooksLikeLocKey(flat))
                s = flat;
            else if (!LooksLikeLocKey(s))
                return s;
        }
        // "MERCHANT name" / "MERCHANT description" — a key that already lost its xxxLang/
        // prefix. It has no slash or braces, so LooksLikeLocKey says "prose" and the raw key
        // was shown verbatim on merchant map icons.
        if (TryParseBareLangKey(s, out string bareId, out string bareSuffix))
        {
            string bt = LocalizeTypedDescription(bareId, bareSuffix);
            if (!string.IsNullOrEmpty(bt) && !TryParseBareLangKey(bt, out _, out _))
                return bt;
            return string.Equals(bareSuffix, "name", StringComparison.OrdinalIgnoreCase)
                ? HumanizeId(bareId)
                : null;
        }

        if (!LooksLikeLocKey(s))
            return s;

        // Direct I2 lookup. I2 can "succeed" and hand back a placeholder that is itself a
        // stripped key ("MERCHANT name"), so the result is validated, not just the input —
        // returning it unchecked is what put raw keys on the merchant map tooltip.
        string t = Usable(Translate(s));
        if (!string.IsNullOrEmpty(t))
            return t;

        try
        {
            // powerupLang/MERCHANT name  |  itemLang/{MERCHANT}description  |  powerupLang/MERCHANT name
            if (TryParseLangTerm(s, out string prefix, out string id, out string suffix))
            {
                t = Usable(TryLocKeyVariants(prefix, id, suffix));
                if (!string.IsNullOrEmpty(t))
                    return t;
                // Cross-table: characters often live under powerupLang in VS data
                foreach (string alt in new[] { "powerupLang/", "itemLang/", "charLang/", "weaponLang/" })
                {
                    if (string.Equals(alt, prefix, StringComparison.OrdinalIgnoreCase))
                        continue;
                    t = Usable(TryLocKeyVariants(alt, id, suffix));
                    if (!string.IsNullOrEmpty(t))
                        return t;
                }
                // Name keys with no translation → humanize the id (better than raw term)
                if (string.Equals(suffix, "name", StringComparison.OrdinalIgnoreCase)
                    || string.IsNullOrEmpty(suffix))
                    return HumanizeId(id);
                // Description missing — omit rather than show key
                return null;
            }

            // Capture: prefix/{ID}suffix  e.g. itemLang/{MERCHANT}description
            int braceL = s.IndexOf('{');
            int braceR = s.IndexOf('}');
            if (braceL >= 0 && braceR > braceL)
            {
                string pfx = s.Substring(0, braceL); // "itemLang/"
                string bid = s.Substring(braceL + 1, braceR - braceL - 1); // "MERCHANT"
                string sfx = braceR + 1 < s.Length ? s.Substring(braceR + 1).Trim() : "";
                if (!string.IsNullOrEmpty(bid))
                {
                    t = Usable(TryLocKeyVariants(pfx, bid, sfx));
                    if (!string.IsNullOrEmpty(t))
                        return t;
                    if (string.Equals(sfx, "name", StringComparison.OrdinalIgnoreCase)
                        || string.IsNullOrEmpty(sfx))
                        return HumanizeId(bid);
                }
            }

            // Strip braces and re-space glued "NAMEdescription"
            string stripped = s.Replace("{", "").Replace("}", "");
            stripped = System.Text.RegularExpressions.Regex.Replace(
                stripped,
                @"(\w+Lang/\w+)(description|name|tips|desc)$",
                "$1 $2",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (!string.Equals(stripped, s, StringComparison.Ordinal))
            {
                t = Usable(Translate(stripped));
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] LocalizeDisplayText: " + ex.Message);
        }

        return null;
    }

    /// <summary>
    /// Parse <c>powerupLang/MERCHANT name</c>, <c>itemLang/{ANTONIO}description</c>, etc.
    /// </summary>
    private static bool TryParseLangTerm(string s, out string prefix, out string id, out string suffix)
    {
        prefix = null;
        id = null;
        suffix = null;
        if (string.IsNullOrEmpty(s)) return false;
        // (xxxLang/)({?)(ID)(}?)(space?)(kind?)
        var m = System.Text.RegularExpressions.Regex.Match(
            s.Trim(),
            @"^(?<prefix>\w+Lang/)(?:\{)?(?<id>[A-Za-z0-9_]+)(?:\})?(?:\s+(?<suffix>\w+)|(?<suffix>description|name|tips|desc))?$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);
        if (!m.Success) return false;
        prefix = m.Groups["prefix"].Value;
        id = m.Groups["id"].Value;
        suffix = m.Groups["suffix"].Success ? m.Groups["suffix"].Value : "";
        // glued form: powerupLang/MERCHANTdescription
        if (string.IsNullOrEmpty(suffix) && id.Length > 4)
        {
            foreach (string kind in new[] { "description", "name", "tips", "desc" })
            {
                if (id.EndsWith(kind, StringComparison.OrdinalIgnoreCase) && id.Length > kind.Length)
                {
                    suffix = kind;
                    id = id.Substring(0, id.Length - kind.Length);
                    break;
                }
            }
        }
        return !string.IsNullOrEmpty(id);
    }

    /// <summary>One line of a tooltip list: an optional icon plus its label.</summary>
    public sealed class IconRow
    {
        public Sprite Sprite;
        public string Label;
        /// <summary>Render as a section heading rather than an icon row.</summary>
        public bool IsHeader;
        public IconRow(Sprite sprite, string label) { Sprite = sprite; Label = label; }
        public static IconRow Header(string label) { return new IconRow(null, label) { IsHeader = true }; }
    }

    /// <summary>
    /// The full record for a secret, from <c>DataManager.AllSecrets</c>.
    ///
    /// The SecretData handed to SecretItemUI is sparse — every reward field arrives null (or
    /// VOID), so reading rewards off the UI's copy yields nothing. The catalog keyed by
    /// SecretType holds the populated record. Same shape as the custom-merchant catalog.
    /// </summary>
    public static VampireSurvivors.Data.SecretData GetSecretData(SecretType type)
    {
        if (_dataManager == null) return null;
        try
        {
            var all = _dataManager.AllSecrets;
            if (all == null) return null;
            foreach (var kv in all)
            {
                if (kv.Key == type) return kv.Value;
            }
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] GetSecretData: " + ex.Message);
        }
        return null;
    }

    // ── Secrets: rewards live in the raw JSON, not in the parsed record ──────
    //
    // Every reward field on SecretData reads back as the enum's VOID member for mystery
    // secrets, in both the row's copy and the DataManager.AllSecrets catalog — but the raw
    // JSON DataManager parsed those records from still carries them as plain strings
    // ("characterToUnlock": "NEO"). So the shipped data does hold the answers; only the
    // deserialized view is blank. Reading the JSON is therefore the only reliable source.

    private sealed class SecretRewardJson
    {
        public string Character;
        public string Weapon;
        public System.Collections.Generic.List<string> WeaponList;
        public string Stage;
        public string Hyper;
        public string Relic;
        public string Arcana;
        public string PowerUp;
        public System.Collections.Generic.List<string> Skins;
        public int Gold;
        public string Special;
        public string CustomText;
        public string CustomFrame;
        public string CustomTexture;
    }

    private static System.Collections.Generic.Dictionary<string, SecretRewardJson> _secretRewards;
    private static bool _secretRewardsParsed;

    private static void EnsureSecretRewards()
    {
        // Not "tried" until DataManager exists — otherwise an early call would poison the
        // cache with an empty result for the rest of the session.
        if (_secretRewardsParsed || _dataManager == null) return;
        _secretRewardsParsed = true;
        try
        {
            var json = _dataManager._allSecretsJson;
            if (json == null)
            {
                Plugin.Dbg("[GameData] _allSecretsJson is null");
                return;
            }
            string raw = null;
            try { raw = json.ToString(); } catch { }
            if (string.IsNullOrEmpty(raw))
            {
                Plugin.Dbg("[GameData] secrets JSON not stringifiable");
                return;
            }

            var map = new System.Collections.Generic.Dictionary<string, SecretRewardJson>(
                StringComparer.OrdinalIgnoreCase);
            using (var doc = System.Text.Json.JsonDocument.Parse(raw))
            {
                if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Object)
                {
                    Plugin.Dbg("[GameData] secrets JSON root is " + doc.RootElement.ValueKind);
                    return;
                }
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    SecretRewardJson entry = ReadSecretEntry(prop.Value);
                    if (entry != null) map[prop.Name] = entry;
                }
            }
            _secretRewards = map;
            Plugin.Dbg($"[GameData] secrets JSON parsed: {map.Count} entries");
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] secrets JSON: " + ex.Message);
        }
    }

    private static SecretRewardJson ReadSecretEntry(System.Text.Json.JsonElement el)
    {
        // Some VS catalogs wrap each record in a single-element array (the per-level shape).
        if (el.ValueKind == System.Text.Json.JsonValueKind.Array)
        {
            foreach (var first in el.EnumerateArray()) return ReadSecretEntry(first);
            return null;
        }
        if (el.ValueKind != System.Text.Json.JsonValueKind.Object) return null;

        return new SecretRewardJson
        {
            Character = JsonStr(el, "characterToUnlock"),
            Weapon = JsonStr(el, "weaponToUnlock"),
            WeaponList = JsonStrList(el, "weaponListToUnlock"),
            Stage = JsonStr(el, "stageToUnlock"),
            Hyper = JsonStr(el, "hyperToUnlock"),
            Relic = JsonStr(el, "relicToUnlock"),
            Arcana = JsonStr(el, "arcanaToUnlock"),
            PowerUp = JsonStr(el, "powerUpToUnlock"),
            Skins = JsonStrList(el, "skinsToUnlock"),
            Gold = JsonInt(el, "goldPrize"),
            Special = JsonStr(el, "special"),
            CustomText = JsonStr(el, "customUnlockText"),
            CustomFrame = JsonStr(el, "customFrame"),
            CustomTexture = JsonStr(el, "customTexture"),
        };
    }

    private static bool TryJsonProp(System.Text.Json.JsonElement obj, string name,
        out System.Text.Json.JsonElement val)
    {
        if (obj.TryGetProperty(name, out val)) return true;
        foreach (var p in obj.EnumerateObject())
        {
            if (string.Equals(p.Name, name, StringComparison.OrdinalIgnoreCase))
            {
                val = p.Value;
                return true;
            }
        }
        val = default;
        return false;
    }

    private static string JsonStr(System.Text.Json.JsonElement obj, string name)
    {
        if (!TryJsonProp(obj, name, out var v)) return null;
        if (v.ValueKind != System.Text.Json.JsonValueKind.String) return null;
        string s = v.GetString();
        return IsVoidValue(s) ? null : s;
    }

    private static int JsonInt(System.Text.Json.JsonElement obj, string name)
    {
        if (!TryJsonProp(obj, name, out var v)) return 0;
        if (v.ValueKind == System.Text.Json.JsonValueKind.Number && v.TryGetInt32(out int i)) return i;
        if (v.ValueKind == System.Text.Json.JsonValueKind.String
            && int.TryParse(v.GetString(), out int j)) return j;
        return 0;
    }

    /// <summary>
    /// A list of ids. Skin entries are objects rather than bare strings, so an object element
    /// contributes its most identifying string field instead of being skipped.
    /// </summary>
    private static System.Collections.Generic.List<string> JsonStrList(
        System.Text.Json.JsonElement obj, string name)
    {
        if (!TryJsonProp(obj, name, out var v)) return null;
        if (v.ValueKind != System.Text.Json.JsonValueKind.Array) return null;
        var list = new System.Collections.Generic.List<string>();
        foreach (var el in v.EnumerateArray())
        {
            if (el.ValueKind == System.Text.Json.JsonValueKind.String)
            {
                string s = el.GetString();
                if (!IsVoidValue(s)) list.Add(s);
            }
            else if (el.ValueKind == System.Text.Json.JsonValueKind.Object)
            {
                string s = JsonStr(el, "charType") ?? JsonStr(el, "characterType")
                    ?? JsonStr(el, "skin") ?? JsonStr(el, "skinName") ?? JsonStr(el, "name");
                if (s == null)
                {
                    foreach (var p in el.EnumerateObject())
                    {
                        if (p.Value.ValueKind != System.Text.Json.JsonValueKind.String) continue;
                        string cand = p.Value.GetString();
                        if (!IsVoidValue(cand)) { s = cand; break; }
                    }
                }
                if (s != null) list.Add(s);
            }
        }
        return list.Count > 0 ? list : null;
    }

    /// <summary>
    /// What some piece of content awards, as plain ids. Secrets fill this from the raw JSON;
    /// achievements can fill it straight from their typed record, whose reward fields are
    /// already strings.
    /// </summary>
    public sealed class RewardIds
    {
        public string Character;
        public System.Collections.Generic.List<string> CharacterList;
        public string Weapon;
        public System.Collections.Generic.List<string> WeaponList;
        public string Relic;
        public string Arcana;
        public string PowerUp;
        public System.Collections.Generic.List<string> Skins;
        public string Stage;
        public string Hyper;
        public int Gold;
        public string CustomText;
        public string CustomFrame;
        public string CustomTexture;
        public string Special;
    }

    /// <summary>
    /// Resolve reward ids to displayable rows. Ids that no longer parse to a live enum member
    /// — DLC content that is not installed — still render as a humanized label rather than
    /// silently vanishing, so the list never looks shorter than it really is.
    /// </summary>
    public static System.Collections.Generic.List<IconRow> BuildRewardRows(RewardIds e)
    {
        var rows = new System.Collections.Generic.List<IconRow>();
        if (e == null) return rows;

        void addCharacter(string id)
        {
            if (IsVoidValue(id)) return;
            rows.Add(new IconRow(GetCharacterPortrait(id), DescribeRewardCharacter(id)));
        }

        void addWeapon(string id)
        {
            if (IsVoidValue(id)) return;
            if (Enum.TryParse<WeaponType>(id, true, out WeaponType w) && !IsVoidValue(w.ToString()))
            {
                string n = GetWeaponName(w);
                rows.Add(new IconRow(GetSprite(w), string.IsNullOrEmpty(n) ? HumanizeId(id) : n));
            }
            else rows.Add(new IconRow(null, HumanizeId(id)));
        }

        addCharacter(e.Character);
        if (e.CharacterList != null) foreach (string id in e.CharacterList) addCharacter(id);

        addWeapon(e.Weapon);
        if (e.WeaponList != null) foreach (string id in e.WeaponList) addWeapon(id);

        if (!IsVoidValue(e.Relic))
        {
            if (Enum.TryParse<ItemType>(e.Relic, true, out ItemType it) && !IsVoidValue(it.ToString()))
                rows.Add(new IconRow(GetItemSprite(it), GetItemName(it)));
            else rows.Add(new IconRow(null, HumanizeId(e.Relic)));
        }
        if (!IsVoidValue(e.Arcana))
        {
            if (Enum.TryParse<ArcanaType>(e.Arcana, true, out ArcanaType at) && !IsVoidValue(at.ToString()))
                rows.Add(new IconRow(GetArcanaSprite(at), GetArcanaName(at)));
            else rows.Add(new IconRow(null, HumanizeId(e.Arcana)));
        }
        if (!IsVoidValue(e.PowerUp))
        {
            if (Enum.TryParse<PowerUpType>(e.PowerUp, true, out PowerUpType pt) && !IsVoidValue(pt.ToString()))
                rows.Add(new IconRow(GetSprite(pt), GetPowerUpName(pt)));
            else rows.Add(new IconRow(null, HumanizeId(e.PowerUp)));
        }

        if (e.Skins != null)
            foreach (string id in e.Skins)
                rows.Add(new IconRow(null, DescribeRewardCharacter(id) + " (skin)"));

        if (!IsVoidValue(e.Stage)) rows.Add(new IconRow(null, DescribeStage(e.Stage) + " (stage)"));
        if (!IsVoidValue(e.Hyper)) rows.Add(new IconRow(null, DescribeStage(e.Hyper) + " (Hyper)"));
        if (e.Gold > 0) rows.Add(new IconRow(null, $"Gold: {e.Gold}"));

        if (!string.IsNullOrEmpty(e.CustomText))
        {
            string t = LocalizeDisplayText(e.CustomText) ?? e.CustomText;
            if (!string.IsNullOrEmpty(t) && !LooksLikeLocKey(t))
                rows.Add(new IconRow(LoadSprite(e.CustomFrame, e.CustomTexture), t));
        }
        if (!string.IsNullOrEmpty(e.Special))
        {
            string t = LocalizeDisplayText(e.Special) ?? e.Special;
            if (!string.IsNullOrEmpty(t) && !LooksLikeLocKey(t)) rows.Add(new IconRow(null, t));
        }

        return rows;
    }

    /// <summary>
    /// Everything a secret awards, resolved from the raw JSON by secret key (the
    /// <c>SecretType</c> name, e.g. <c>KissMe</c>).
    /// </summary>
    public static System.Collections.Generic.List<IconRow> GetSecretRewards(string key)
    {
        EnsureSecretRewards();
        if (_secretRewards == null || string.IsNullOrEmpty(key))
            return new System.Collections.Generic.List<IconRow>();
        if (!_secretRewards.TryGetValue(key, out SecretRewardJson e) || e == null)
            return new System.Collections.Generic.List<IconRow>();

        return BuildRewardRows(new RewardIds
        {
            Character = e.Character,
            Weapon = e.Weapon,
            WeaponList = e.WeaponList,
            Relic = e.Relic,
            Arcana = e.Arcana,
            PowerUp = e.PowerUp,
            Skins = e.Skins,
            Stage = e.Stage,
            Hyper = e.Hyper,
            Gold = e.Gold,
            CustomText = e.CustomText,
            CustomFrame = e.CustomFrame,
            CustomTexture = e.CustomTexture,
            Special = e.Special,
        });
    }

    // ── Bestiary ─────────────────────────────────────────────────────────────

    /// <summary>Enemy stats, traits and stage list as one labelled row list.</summary>
    public sealed class EnemyInfo
    {
        public System.Collections.Generic.List<IconRow> Rows;
    }

    /// <summary>
    /// maxHp is stored at a tenth of the value the game reports — an enemy the Bestiary calls
    /// 5 HP is 0.5 in the data. Scaled here so the tooltip agrees with the game.
    /// </summary>
    private const float EnemyHpScale = 10f;

    /// <summary>
    /// Tidy a humanized enum id for display: HumanizeId turns HpXLevel into "Hp X Level", but
    /// the stat is HP and the multiplier reads as a lowercase x.
    /// </summary>
    private static string PrettyTrait(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        s = System.Text.RegularExpressions.Regex.Replace(s, @"\bHp\b", "HP");
        s = System.Text.RegularExpressions.Regex.Replace(s, @"\bX\b", "x");
        s = System.Text.RegularExpressions.Regex.Replace(s, @"\bXp\b", "XP");
        return s;
    }

    // Stat icons: the passive that governs each stat, so the rows read like the game's own
    // item vocabulary (Hollow Heart, Wings, Spinach) rather than bare numbers.
    private static Sprite HpIcon() { try { return GetSprite(WeaponType.MAXHEALTH); } catch { return null; } }
    private static Sprite SpeedIcon() { try { return GetSprite(WeaponType.MOVESPEED); } catch { return null; } }
    private static Sprite PowerIcon() { try { return GetSprite(WeaponType.POWER); } catch { return null; } }
    /// <summary>The XP gem, same icon stage selection uses for its XP Bonus stat.</summary>
    private static Sprite XpIcon() { try { return GetItemSprite(ItemType.GEM); } catch { return null; } }

    // "Found in" is captured from the game's own Bestiary info panel rather than derived.
    // Deriving it from the stage JSON disagreed with the game — Boss Rash for an enemy the
    // game lists under Cappella Magna — and missed stages entirely, and a wrong stage list is
    // indistinguishable from a right one to whoever reads it.
    private static readonly System.Collections.Generic.Dictionary<string, string> _enemyFoundIn =
        new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// What the game's own Bestiary info panel printed. Keyed by the displayed title, which is
    /// what a hovered row can be matched on.
    /// </summary>
    public sealed class EnemyPanel
    {
        public string Hp, Power, Speed, Resistances, Skills, FoundIn;
    }

    private static readonly System.Collections.Generic.Dictionary<string, EnemyPanel> _enemyPanels =
        new System.Collections.Generic.Dictionary<string, EnemyPanel>(StringComparer.OrdinalIgnoreCase);

    public static void SetEnemyPanel(string title, EnemyPanel panel)
    {
        if (string.IsNullOrWhiteSpace(title) || panel == null) return;
        _enemyPanels[title.Trim()] = panel;
    }

    public static EnemyPanel GetEnemyPanel(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return null;
        return _enemyPanels.TryGetValue(title.Trim(), out EnemyPanel p) ? p : null;
    }

    private static bool _dumpedEnemyJson;

    /// <summary>
    /// One-time dump of an enemy's raw JSON. The parse reports one record per enemy while the
    /// Bestiary shows stat ranges, so either the ranges are computed from fields on a single
    /// record or the array is shaped differently than assumed — this tells us which.
    /// </summary>
    public static void DumpEnemyJsonOnce(string enemyId)
    {
        if (_dumpedEnemyJson || _dataManager == null || !Plugin.DebugVerbose || string.IsNullOrEmpty(enemyId)) return;
        try
        {
            var json = _dataManager._allEnemiesJson;
            if (json == null) { _dumpedEnemyJson = true; return; }
            string raw = json.ToString();
            if (string.IsNullOrEmpty(raw)) { _dumpedEnemyJson = true; return; }
            int at = raw.IndexOf("\"" + enemyId + "\"", StringComparison.OrdinalIgnoreCase);
            if (at < 0) return;
            _dumpedEnemyJson = true;
            int len = Math.Min(900, raw.Length - at);
            Plugin.Dbg($"[GameData] enemy JSON '{enemyId}': " + raw.Substring(at, len).Replace('\n', ' '));
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] enemy JSON dump: " + ex.Message);
        }
    }

    /// <summary>Record what the game printed in its own Found In label for an enemy.</summary>
    public static void SetEnemyFoundIn(string enemyId, string text)
    {
        if (string.IsNullOrEmpty(enemyId) || string.IsNullOrWhiteSpace(text)) return;
        string t = text.Trim();
        if (LooksLikeLocKey(t)) return;
        _enemyFoundIn[enemyId] = t;
    }

    public static string GetEnemyFoundIn(string enemyId)
    {
        if (string.IsNullOrEmpty(enemyId)) return null;
        return _enemyFoundIn.TryGetValue(enemyId, out string t) ? t : null;
    }


    /// <summary>Format without a locale decimal comma, so it matches the game's English UI.</summary>
    private static string Num(float v)
    {
        if (Math.Abs(v - Mathf.Round(v)) < 0.005f)
            return Mathf.RoundToInt(v).ToString(System.Globalization.CultureInfo.InvariantCulture);
        return v.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Everything the Bestiary knows about an enemy but never shows. The resistance fields are
    /// the point of this: nothing in game explains why an enemy shrugs off freeze or Rosary.
    ///
    /// Values are reported as the data stores them rather than converted to percentages — the
    /// scale is not documented anywhere and inventing one would be worse than showing none.
    /// </summary>
    /// <summary>Min/max of a stat across an enemy's level variants.</summary>
    private struct Range
    {
        public float Min, Max;
        public bool Any;
        public void Add(float v)
        {
            if (!Any) { Min = Max = v; Any = true; return; }
            if (v < Min) Min = v;
            if (v > Max) Max = v;
        }
        public string Text(float scale = 1f)
        {
            float a = Min * scale, b = Max * scale;
            return Math.Abs(a - b) < 0.005f ? Num(a) : $"{Num(a)}–{Num(b)}";
        }
    }

    /// <summary>One enemy's record from the enemies JSON, including its Bestiary metadata.</summary>
    private sealed class EnemyRec
    {
        public bool HasHp, HasPower, HasSpeed, HasXp, HasKnock;
        public float Hp, Power, SpeedMin, SpeedMax, Xp, Knock;
        public System.Collections.Generic.List<string> Traits = new System.Collections.Generic.List<string>();
        /// <summary>Sibling ids the Bestiary groups into one entry — the source of stat ranges.</summary>
        public System.Collections.Generic.List<string> Variants;
        /// <summary>Stage ids the Bestiary lists under "Found in".</summary>
        public System.Collections.Generic.List<string> Places;
        public string Name, Desc;
    }

    private static System.Collections.Generic.Dictionary<string, EnemyRec> _enemyRecs;
    private static bool _enemyRecsParsed;

    /// <summary>
    /// Parse the enemies JSON, which carries the Bestiary's own metadata alongside the stats:
    /// <c>bVariants</c> (the sibling ids one Bestiary entry covers), <c>bPlaces</c> (its stage
    /// list), <c>bName</c> and <c>bDesc</c>.
    ///
    /// The ranges the Bestiary prints come from aggregating over <c>bVariants</c> — BAT1's
    /// entry spans BAT1/BAT2/BAT3, which is why one record read alone showed a single value.
    /// </summary>
    private static void EnsureEnemyRecs()
    {
        if (_enemyRecsParsed || _dataManager == null) return;
        _enemyRecsParsed = true;

        var map = new System.Collections.Generic.Dictionary<string, EnemyRec>(StringComparer.OrdinalIgnoreCase);
        try
        {
            var json = _dataManager._allEnemiesJson;
            if (json == null) { Plugin.Dbg("[GameData] _allEnemiesJson is null"); return; }
            string raw = null;
            try { raw = json.ToString(); } catch { }
            if (string.IsNullOrEmpty(raw)) return;

            using (var doc = System.Text.Json.JsonDocument.Parse(raw))
            {
                if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Object) return;
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    System.Text.Json.JsonElement rec = prop.Value;
                    if (rec.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        bool any = false;
                        foreach (var f in rec.EnumerateArray()) { rec = f; any = true; break; }
                        if (!any) continue;
                    }
                    if (rec.ValueKind != System.Text.Json.JsonValueKind.Object) continue;
                    map[prop.Name] = ReadEnemyRec(rec);
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] enemies JSON: " + ex.Message);
        }

        _enemyRecs = map;
        int withVariants = 0, withPlaces = 0;
        foreach (var kv in map)
        {
            if (kv.Value.Variants != null && kv.Value.Variants.Count > 1) withVariants++;
            if (kv.Value.Places != null && kv.Value.Places.Count > 0) withPlaces++;
        }
        Plugin.Dbg($"[GameData] enemy records: {map.Count}, {withVariants} with variants, {withPlaces} with places");
    }

    private static EnemyRec ReadEnemyRec(System.Text.Json.JsonElement rec)
    {
        var r = new EnemyRec();
        r.HasHp = TryJsonFloat(rec, "maxHp", out r.Hp);
        r.HasPower = TryJsonFloat(rec, "power", out r.Power) && r.Power > 0f;
        r.HasSpeed = TryJsonFloat(rec, "speed", out r.SpeedMin) && r.SpeedMin > 0f;
        if (!TryJsonFloat(rec, "maxSpeed", out r.SpeedMax) || r.SpeedMax <= 0f) r.SpeedMax = r.SpeedMin;
        r.HasXp = TryJsonFloat(rec, "xp", out r.Xp) && r.Xp > 0f;
        r.HasKnock = TryJsonFloat(rec, "knockback", out r.Knock) && r.Knock > 0f;

        void trait(string s)
        {
            if (!string.IsNullOrEmpty(s) && !r.Traits.Contains(s)) r.Traits.Add(s);
        }
        void resist(string label, string field)
        {
            if (TryJsonFloat(rec, field, out float v)) trait($"{label}: {Num(v)}");
        }
        resist("Freeze resist", "res_Freeze");
        resist("Rosary resist", "res_Rosary");
        resist("Debuff resist", "res_Debuffs");
        resist("Knockback resist", "res_Knockback");
        resist("Corridor resist", "res_Corridor");
        resist("Defang resist", "res_Defang");
        resist("Fire weakness", "weak_Fire");
        if (TryJsonProp(rec, "passThroughWalls", out var ptw)
            && ptw.ValueKind == System.Text.Json.JsonValueKind.True)
            trait("Passes through walls");
        if (TryJsonFloat(rec, "shieldDuration", out float sd) && sd > 0f) trait("Shield: " + Num(sd) + "s");
        if (TryJsonFloat(rec, "lives", out float lv) && lv > 1f) trait("Lives: " + Num(lv));
        var skills = JsonStrList(rec, "skills");
        if (skills != null)
            foreach (string s in skills)
                if (!IsVoidValue(s)) trait(PrettyTrait(HumanizeId(s)));

        r.Variants = JsonStrList(rec, "bVariants");
        r.Places = JsonStrList(rec, "bPlaces");
        r.Name = JsonStr(rec, "bName");
        r.Desc = JsonStr(rec, "bDesc");
        return r;
    }

    /// <summary>The Bestiary name for an enemy, as shipped in its record.</summary>
    public static string GetEnemyName(string enemyId)
    {
        EnsureEnemyRecs();
        if (_enemyRecs == null || string.IsNullOrEmpty(enemyId)) return null;
        if (!_enemyRecs.TryGetValue(enemyId, out EnemyRec r) || r == null) return null;
        if (string.IsNullOrWhiteSpace(r.Name)) return null;
        string t = LocalizeDisplayText(r.Name) ?? r.Name;
        return LooksLikeLocKey(t) ? null : t.Trim();
    }

    private static System.Collections.Generic.Dictionary<string, string> _stageNames;
    private static bool _stageNamesParsed;

    /// <summary>Stage id → display name, for turning <c>bPlaces</c> into readable stages.</summary>
    private static string StageDisplayName(string stageId)
    {
        if (string.IsNullOrEmpty(stageId)) return null;
        if (!_stageNamesParsed && _dataManager != null)
        {
            _stageNamesParsed = true;
            var map = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var json = _dataManager._allStagesJson;
                string raw = json != null ? json.ToString() : null;
                if (!string.IsNullOrEmpty(raw))
                {
                    using (var doc = System.Text.Json.JsonDocument.Parse(raw))
                    {
                        if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object)
                        {
                            foreach (var prop in doc.RootElement.EnumerateObject())
                            {
                                System.Text.Json.JsonElement rec = prop.Value;
                                if (rec.ValueKind == System.Text.Json.JsonValueKind.Array)
                                {
                                    bool any = false;
                                    foreach (var f in rec.EnumerateArray()) { rec = f; any = true; break; }
                                    if (!any) continue;
                                }
                                if (rec.ValueKind != System.Text.Json.JsonValueKind.Object) continue;
                                string n = JsonStr(rec, "stageName");
                                if (string.IsNullOrWhiteSpace(n)) continue;
                                string loc = LocalizeDisplayText(n) ?? n;
                                if (!string.IsNullOrWhiteSpace(loc) && !LooksLikeLocKey(loc))
                                    map[prop.Name] = loc.Trim();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Plugin.Dbg("[GameData] stage names: " + ex.Message); }
            _stageNames = map;
            Plugin.Dbg($"[GameData] stage names: {map.Count}");
        }
        if (_stageNames != null && _stageNames.TryGetValue(stageId, out string name)) return name;
        return DescribeStage(stageId);
    }

    private static bool TryJsonFloat(System.Text.Json.JsonElement obj, string name, out float value)
    {
        value = 0f;
        if (!TryJsonProp(obj, name, out var v)) return false;
        if (v.ValueKind == System.Text.Json.JsonValueKind.Number && v.TryGetDouble(out double d))
        {
            value = (float)d;
            return true;
        }
        if (v.ValueKind == System.Text.Json.JsonValueKind.String
            && float.TryParse(v.GetString(), System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out float f))
        {
            value = f;
            return true;
        }
        return false;
    }

    public static EnemyInfo GetEnemyInfo(VampireSurvivors.Data.Enemies.EnemyData data, string enemyId = null,
        VampireSurvivors.Data.EnemyType? type = null, string title = null)
    {
        var rows = new System.Collections.Generic.List<IconRow>();

        EnsureEnemyRecs();
        EnemyRec self = null;
        if (_enemyRecs != null && !string.IsNullOrEmpty(enemyId)) _enemyRecs.TryGetValue(enemyId, out self);

        if (self == null)
        {
            if (data == null) return null;
            try { rows.Add(new IconRow(HpIcon(), "HP: " + Num(data.maxHp * EnemyHpScale))); } catch { }
            try { if (data.power > 0f) rows.Add(new IconRow(PowerIcon(), "Damage: " + Num(data.power))); } catch { }
            try { if (data.speed > 0f) rows.Add(new IconRow(SpeedIcon(), "Speed: " + Num(data.speed))); } catch { }
            return rows.Count > 0 ? new EnemyInfo { Rows = rows } : null;
        }

        // One Bestiary entry covers a family of ids; its printed stats span all of them.
        var family = new System.Collections.Generic.List<EnemyRec> { self };
        if (self.Variants != null)
        {
            foreach (string vid in self.Variants)
            {
                if (string.Equals(vid, enemyId, StringComparison.OrdinalIgnoreCase)) continue;
                if (_enemyRecs.TryGetValue(vid, out EnemyRec v) && v != null) family.Add(v);
            }
        }

        Range hp = default, power = default, speed = default, xp = default, knock = default;
        var traits = new System.Collections.Generic.List<string>();
        foreach (var r in family)
        {
            if (r.HasHp) hp.Add(r.Hp);
            if (r.HasPower) power.Add(r.Power);
            if (r.HasSpeed) { speed.Add(r.SpeedMin); speed.Add(r.SpeedMax); }
            if (r.HasXp) xp.Add(r.Xp);
            if (r.HasKnock) knock.Add(r.Knock);
            foreach (string t in r.Traits) if (!traits.Contains(t)) traits.Add(t);
        }

        if (hp.Any) rows.Add(new IconRow(HpIcon(), "HP: " + hp.Text(EnemyHpScale)));
        if (power.Any) rows.Add(new IconRow(PowerIcon(), "Damage: " + power.Text()));
        if (speed.Any) rows.Add(new IconRow(SpeedIcon(), "Speed: " + speed.Text()));
        if (xp.Any) rows.Add(new IconRow(XpIcon(), "XP: " + xp.Text()));
        if (knock.Any) rows.Add(new IconRow(null, "Knockback: " + knock.Text()));

        if (traits.Count > 0)
        {
            rows.Add(IconRow.Header("Traits:"));
            foreach (string t in traits) rows.Add(new IconRow(null, t));
        }

        if (self.Places != null && self.Places.Count > 0)
        {
            rows.Add(IconRow.Header("Found in:"));
            foreach (string pid in self.Places)
            {
                string n = StageDisplayName(pid);
                if (!string.IsNullOrEmpty(n)) rows.Add(new IconRow(null, n));
            }
        }

        return rows.Count > 0 ? new EnemyInfo { Rows = rows } : null;
    }

    // ── Character records (for secret reward rows) ───────────────────────────
    //
    // Only the portrait frame/texture *names* are cached, not resolved sprites: SpriteManager
    // atlases are torn down between scenes, so a cached Sprite would go stale.
    //
    // The name parts matter as much as the portrait. Several characters were renamed after
    // their enum id was fixed — GRAZIELLA ships as "Minnah Mannarah" — so a single
    // `<id> name` term returns the retired name while the game builds its own from
    // charName/surname. Composing the same way is what makes the tooltip agree with the
    // in-game unlock banner.

    private sealed class CharInfo
    {
        public string Frame;
        public string Texture;
        public string Prefix;
        public string CharName;
        public string Surname;
    }

    private static System.Collections.Generic.Dictionary<string, CharInfo> _charInfo;
    private static bool _charInfoParsed;

    private static void EnsureCharacterInfo()
    {
        if (_charInfoParsed || _dataManager == null) return;
        _charInfoParsed = true;
        try
        {
            var json = _dataManager._allCharactersJson;
            if (json == null) return;
            string raw = null;
            try { raw = json.ToString(); } catch { }
            if (string.IsNullOrEmpty(raw)) return;

            var map = new System.Collections.Generic.Dictionary<string, CharInfo>(
                StringComparer.OrdinalIgnoreCase);
            using (var doc = System.Text.Json.JsonDocument.Parse(raw))
            {
                if (doc.RootElement.ValueKind != System.Text.Json.JsonValueKind.Object) return;
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    // Characters are stored as an array of records (base outfit first).
                    System.Text.Json.JsonElement rec = prop.Value;
                    if (rec.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        bool any = false;
                        foreach (var first in rec.EnumerateArray()) { rec = first; any = true; break; }
                        if (!any) continue;
                    }
                    if (rec.ValueKind != System.Text.Json.JsonValueKind.Object) continue;

                    map[prop.Name] = new CharInfo
                    {
                        Frame = JsonStr(rec, "portraitName") ?? JsonStr(rec, "charSelFrame")
                            ?? JsonStr(rec, "spriteName"),
                        Texture = JsonStr(rec, "charSelTexture") ?? JsonStr(rec, "textureName"),
                        Prefix = JsonStr(rec, "prefix"),
                        CharName = JsonStr(rec, "charName"),
                        Surname = JsonStr(rec, "surname"),
                    };
                }
            }
            _charInfo = map;
            Plugin.Dbg($"[GameData] character records parsed: {map.Count}");
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] characters JSON: " + ex.Message);
        }
    }

    public static Sprite GetCharacterPortrait(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        EnsureCharacterInfo();
        if (_charInfo == null) return null;
        if (!_charInfo.TryGetValue(id, out CharInfo info) || info == null) return null;
        if (string.IsNullOrEmpty(info.Frame)) return null;
        try { return LoadSprite(info.Frame, info.Texture); } catch { return null; }
    }

    private static string Piece(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return null;
        string t = LocalizeDisplayText(s) ?? s;
        if (string.IsNullOrWhiteSpace(t) || LooksLikeLocKey(t)) return null;
        return t.Trim();
    }

    /// <summary>
    /// The character's name as the game itself renders it, built from the same charName /
    /// surname parts rather than from a single localization term.
    /// </summary>
    private static string ComposeCharacterName(string id)
    {
        EnsureCharacterInfo();
        if (_charInfo == null) return null;
        if (!_charInfo.TryGetValue(id, out CharInfo info) || info == null) return null;

        var parts = new System.Collections.Generic.List<string>();
        string p = Piece(info.Prefix); if (p != null) parts.Add(p);
        string c = Piece(info.CharName); if (c != null) parts.Add(c);
        string s = Piece(info.Surname); if (s != null) parts.Add(s);
        if (parts.Count == 0) return null;
        return string.Join(" ", parts);
    }

    /// <summary>
    /// A character reward label. When the composed name and the id's own localization term
    /// disagree — a renamed character — both are shown, because either may be the name the
    /// player recognizes depending on how long they have been playing.
    /// </summary>
    public static string DescribeRewardCharacter(string id)
    {
        string composed = ComposeCharacterName(id);
        string termed = LocalizeTypedDescription(id, "name");
        if (!string.IsNullOrWhiteSpace(termed) && LooksLikeLocKey(termed)) termed = null;

        if (string.IsNullOrWhiteSpace(composed)) return DescribeCharacter(id);
        if (string.IsNullOrWhiteSpace(termed)) return composed;

        bool same = string.Equals(composed.Trim(), termed.Trim(), StringComparison.OrdinalIgnoreCase)
            || composed.IndexOf(termed.Trim(), StringComparison.OrdinalIgnoreCase) >= 0;
        return same ? composed : $"{composed} ({termed.Trim()})";
    }

    /// <summary>
    /// Everything a secret awards, as displayable rows.
    ///
    /// SecretData carries each reward kind in its own nullable field, so this is a flat sweep
    /// of all of them rather than one "reward" lookup. Rows with no resolvable sprite still
    /// render (label only) — characters and stages often have no icon we can reach.
    /// </summary>
    public static System.Collections.Generic.List<IconRow> GetSecretRewards(VampireSurvivors.Data.SecretData data)
    {
        var rows = new System.Collections.Generic.List<IconRow>();
        if (data == null) return rows;

        void addWeapon(WeaponType w)
        {
            if (IsVoidValue(w.ToString())) return;
            string n = GetWeaponName(w);
            if (!string.IsNullOrEmpty(n)) rows.Add(new IconRow(GetSprite(w), n));
        }

        try { if (data.weaponToUnlock != null && data.weaponToUnlock.HasValue) addWeapon(data.weaponToUnlock.Value); } catch { }
        try
        {
            var list = data.weaponListToUnlock;
            if (list != null) for (int i = 0; i < list.Count; i++) addWeapon(list[i]);
        }
        catch { }
        try
        {
            if (data.relicToUnlock != null && data.relicToUnlock.HasValue)
            {
                ItemType it = data.relicToUnlock.Value;
                if (!IsVoidValue(it.ToString()))
                rows.Add(new IconRow(GetItemSprite(it), GetItemName(it)));
            }
        }
        catch { }
        try
        {
            if (data.arcanaToUnlock != null && data.arcanaToUnlock.HasValue)
            {
                ArcanaType at = data.arcanaToUnlock.Value;
                if (!IsVoidValue(at.ToString()))
                rows.Add(new IconRow(GetArcanaSprite(at), GetArcanaName(at)));
            }
        }
        catch { }
        try
        {
            if (data.powerUpToUnlock != null && data.powerUpToUnlock.HasValue)
            {
                PowerUpType pt = data.powerUpToUnlock.Value;
                if (!IsVoidValue(pt.ToString()))
                rows.Add(new IconRow(GetSprite(pt), GetPowerUpName(pt)));
            }
        }
        catch { }
        try
        {
            if (data.characterToUnlock != null && data.characterToUnlock.HasValue)
            {
                string cid = data.characterToUnlock.Value.ToString();
                if (!IsVoidValue(cid)) rows.Add(new IconRow(null, DescribeCharacter(cid)));
            }
        }
        catch { }
        try
        {
            var skins = data.skinsToUnlock;
            if (skins != null)
            {
                for (int i = 0; i < skins.Count; i++)
                {
                    string label = null;
                    try { label = DescribeCharacter(skins[i].ToString()); } catch { }
                    if (!string.IsNullOrEmpty(label)) rows.Add(new IconRow(null, label + " (skin)"));
                }
            }
        }
        catch { }
        try
        {
            if (data.stageToUnlock != null && data.stageToUnlock.HasValue)
            {
                string sid = data.stageToUnlock.Value.ToString();
                if (!IsVoidValue(sid)) rows.Add(new IconRow(null, DescribeStage(sid)));
            }
        }
        catch { }
        try
        {
            if (data.hyperToUnlock != null && data.hyperToUnlock.HasValue)
            {
                string hid = data.hyperToUnlock.Value.ToString();
                if (!IsVoidValue(hid)) rows.Add(new IconRow(null, DescribeStage(hid) + " (Hyper)"));
            }
        }
        catch { }
        try
        {
            if (data.goldPrize != null && data.goldPrize.HasValue && data.goldPrize.Value > 0)
                rows.Add(new IconRow(null, $"Gold: {data.goldPrize.Value}"));
        }
        catch { }
        try
        {
            string special = data.special;
            if (!string.IsNullOrEmpty(special))
            {
                string t = LocalizeDisplayText(special) ?? special;
                if (!string.IsNullOrEmpty(t) && !LooksLikeLocKey(t)) rows.Add(new IconRow(null, t));
            }
        }
        catch { }

        return rows;
    }

    /// <summary>
    /// The game stores "no reward of this kind" as the enum's VOID member (value 0), not as a
    /// null nullable — so HasValue is true for fields that award nothing. Without this every
    /// secret listed one reward called "Void".
    /// </summary>
    private static bool IsVoidValue(string id)
    {
        return string.IsNullOrEmpty(id)
            || string.Equals(id, "VOID", StringComparison.OrdinalIgnoreCase)
            || string.Equals(id, "NONE", StringComparison.OrdinalIgnoreCase);
    }

    private static string DescribeCharacter(string id)
    {
        string t = LocalizeTypedDescription(id, "name");
        return string.IsNullOrEmpty(t) ? HumanizeId(id) : t;
    }

    private static string DescribeStage(string id)
    {
        string t = LocalizeDisplayText("stageLang/" + id + " name");
        return string.IsNullOrEmpty(t) ? HumanizeId(id) : t;
    }

    /// <summary>What a custom merchant (Xanthia, adventure merchants) has for sale.</summary>
    public sealed class MerchantWares
    {
        public readonly System.Collections.Generic.List<WeaponType> Weapons =
            new System.Collections.Generic.List<WeaponType>();
        public readonly System.Collections.Generic.List<ItemType> Items =
            new System.Collections.Generic.List<ItemType>();
        public bool Any => Weapons.Count > 0 || Items.Count > 0;
    }

    /// <summary>
    /// Wares for a custom merchant, looked up by a loose id (map sprite name such as
    /// "Mercxanthia", or a type name).
    ///
    /// Read from <c>DataManager.AllCustomMerchantsData</c> / <c>AllAdventureMerchantsData</c>,
    /// keyed by CharacterType, so the list stays correct across patches instead of being
    /// transcribed from the wiki. This also handles DLC implicitly: content for a DLC that is
    /// not installed does not load, so its wares resolve to no sprite and are dropped.
    /// </summary>
    private static readonly System.Collections.Generic.Dictionary<string, MerchantWares> MerchantWaresCache =
        new System.Collections.Generic.Dictionary<string, MerchantWares>(StringComparer.OrdinalIgnoreCase);

    private static bool _loggedMerchantKeys;

    public static MerchantWares GetMerchantWares(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;
        string want = NormalizeMerchantId(id);
        if (want.Length < 5)
            return null;

        // Hover fires this repeatedly; the dictionary walk and sprite resolution are not free.
        if (MerchantWaresCache.TryGetValue(want, out MerchantWares cached))
            return cached;

        MerchantWares result = null;
        try
        {
            if (_dataManager == null)
            {
                Plugin.Dbg("[GameData] GetMerchantWares: DataManager not resolved yet");
                return null; // not cached — retry once the manager exists
            }
            LogMerchantKeysOnce();
            result = LookupMerchantWares(_dataManager.AllCustomMerchantsData, want)
                  ?? LookupMerchantWares(_dataManager.AllAdventureMerchantsData, want)
                  // The catalog dictionaries only describe merchants the base game knows about.
                  // A DLC merchant is built at spawn time, so the live pickup in the scene is
                  // the authoritative source for it.
                  ?? LookupMerchantWaresInScene(want);
            Plugin.Dbg($"[GameData] GetMerchantWares({id} -> {want}): " +
                $"{(result == null ? "no match" : result.Weapons.Count + " weapons, " + result.Items.Count + " items")}");
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] GetMerchantWares: " + ex.Message);
        }
        MerchantWaresCache[want] = result;
        return result;
    }

    /// <summary>One-time dump of the merchant dictionaries so a miss can be diagnosed.</summary>
    private static void LogMerchantKeysOnce()
    {
        if (_loggedMerchantKeys || !Plugin.DebugVerbose) return;
        _loggedMerchantKeys = true;
        DumpMerchantKeys("AllCustomMerchantsData", _dataManager.AllCustomMerchantsData);
        DumpMerchantKeys("AllAdventureMerchantsData", _dataManager.AllAdventureMerchantsData);
    }

    private static void DumpMerchantKeys(string label,
        Il2CppSystem.Collections.Generic.Dictionary<CharacterType, VampireSurvivors.App.Data.CustomMerchantData> dict)
    {
        try
        {
            if (dict == null)
            {
                Plugin.Dbg($"[GameData] {label}: <null>");
                return;
            }
            var names = new System.Collections.Generic.List<string>();
            foreach (var kv in dict)
            {
                int weapons = 0, items = 0;
                try { weapons = kv.Value?.MerchantInventory?.Count ?? 0; } catch { }
                try { items = kv.Value?.MerchantInventoryItems?.Count ?? 0; } catch { }
                names.Add($"{kv.Key}[{SafeSprite(kv.Value, 0)}|{SafeSprite(kv.Value, 1)}]({weapons}w/{items}i)");
            }
            Plugin.Dbg($"[GameData] {label}: {names.Count} -> {string.Join(", ", names)}");
        }
        catch (Exception ex)
        {
            Plugin.Dbg($"[GameData] {label}: " + ex.Message);
        }
    }

    private static MerchantWares LookupMerchantWares(
        Il2CppSystem.Collections.Generic.Dictionary<CharacterType, VampireSurvivors.App.Data.CustomMerchantData> dict, string want)
    {
        if (dict == null) return null;
        try
        {
            foreach (var kv in dict)
            {
                var data = kv.Value;
                if (data == null) continue;

                // CharacterType only names the base merchants (MERCHANT, ADVENTURE_MERCHANT,
                // CUSTOM_MERCHANT, TP_MERCHANT_LIBRARIAN) — DLC merchants such as Xanthia have
                // no named enum member, so the key alone cannot identify them. Their sprite
                // fields do: the map icon sprite is "mercXanthia".
                if (!MerchantMatches(data, kv.Key, want)) continue;
                return BuildWares(data);
            }
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] LookupMerchantWares: " + ex.Message);
        }
        return null;
    }

    /// <summary>
    /// Read wares off a live PickupCustomMerchant in the scene. Works for DLC merchants that
    /// never appear in the catalog dictionaries.
    ///
    /// This is a full-scene search, so it runs only for merchant map icons, only when the
    /// catalog lookup already failed, and its result is cached — a per-frame
    /// FindObjectsOfType scan is what crashed the Collections tab in 1.10.12.
    /// </summary>
    private static MerchantWares LookupMerchantWaresInScene(string want)
    {
        try
        {
            var pickups = UnityEngine.Object.FindObjectsOfType<VampireSurvivors.Objects.Items.PickupCustomMerchant>();
            if (pickups == null || pickups.Length == 0)
            {
                Plugin.Dbg("[GameData] No PickupCustomMerchant in scene");
                return null;
            }

            int seen = 0;
            for (int i = 0; i < pickups.Length; i++)
            {
                var p = pickups[i];
                if ((UnityEngine.Object)(object)p == (UnityEngine.Object)null) continue;
                VampireSurvivors.App.Data.CustomMerchantData data = null;
                try { data = p.CustomMerchantData; } catch { }
                if (data == null) continue;
                seen++;
                Plugin.Dbg($"[GameData] Scene merchant: sprite={SafeSprite(data, 0)} portrait={SafeSprite(data, 1)}");
                if (MerchantMatches(data, CharacterType.VOID, want))
                    return BuildWares(data);
            }

            // Deliberately no "only one merchant in the scene, so it must be this one"
            // fallback. The base Merchant legitimately matches nothing, and that guess
            // attributed the custom merchant's stock to him — wrong wares look identical to
            // right ones. A miss stays a miss.
            if (seen > 0)
                Plugin.Dbg($"[GameData] {seen} scene merchant(s), none matching '{want}'");
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] LookupMerchantWaresInScene: " + ex.Message);
        }
        return null;
    }

    /// <summary>
    /// Wares from one merchant record. A ware with no resolvable sprite belongs to a DLC that
    /// is not installed, so it is dropped rather than drawn as an empty slot.
    /// </summary>
    private static MerchantWares BuildWares(VampireSurvivors.App.Data.CustomMerchantData data)
    {
        if (data == null) return null;
        var wares = new MerchantWares();
        try
        {
            var weapons = data.MerchantInventory;
            if (weapons != null)
            {
                for (int i = 0; i < weapons.Count; i++)
                {
                    WeaponType w = weapons[i];
                    if ((UnityEngine.Object)(object)GetSprite(w) == (UnityEngine.Object)null) continue;
                    if (!wares.Weapons.Contains(w)) wares.Weapons.Add(w);
                }
            }
        }
        catch { }
        try
        {
            var items = data.MerchantInventoryItems;
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    ItemType it = items[i];
                    if ((UnityEngine.Object)(object)GetItemSprite(it) == (UnityEngine.Object)null) continue;
                    if (!wares.Items.Contains(it)) wares.Items.Add(it);
                }
            }
        }
        catch { }
        return wares.Any ? wares : null;
    }

    /// <summary>
    /// Does this merchant correspond to <paramref name="want"/> (a normalized sprite name or
    /// type name)? Sprite fields are checked before the enum key because DLC merchants have no
    /// named CharacterType member.
    /// </summary>
    private static bool MerchantMatches(VampireSurvivors.App.Data.CustomMerchantData data, CharacterType key, string want)
    {
        foreach (string candidate in new[] { SafeSprite(data, 0), SafeSprite(data, 1), SafeSprite(data, 2), SafeSprite(data, 3) })
        {
            string c = NormalizeMerchantId(candidate);
            if (c.Length >= 5 && (c.Contains(want) || want.Contains(c)))
                return true;
        }
        string k = NormalizeMerchantId(key.ToString());
        return k.Length >= 5 && (k.Contains(want) || want.Contains(k));
    }

    private static string SafeSprite(VampireSurvivors.App.Data.CustomMerchantData data, int which)
    {
        try
        {
            switch (which)
            {
                case 0: return data.StaticSprite;
                case 1: return data.PortraitSprite;
                case 2: return data.StaticSpriteTexture;
                case 3: return data.PortraitSpriteTexture;
            }
        }
        catch { }
        return null;
    }

    /// <summary>Upper-case, letters/digits only, so "Mercxanthia" matches MERCXANTHIA.</summary>
    private static string NormalizeMerchantId(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        var sb = new System.Text.StringBuilder(s.Length);
        foreach (char c in s)
        {
            if (char.IsLetterOrDigit(c)) sb.Append(char.ToUpperInvariant(c));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Display names for ids where the humanized enum id is wrong or unhelpful.
    /// <c>MERCXANTHIA</c> humanizes to "Mercxanthia"; the character is called Xanthia.
    /// </summary>
    private static readonly System.Collections.Generic.Dictionary<string, string> DisplayNameOverrides =
        new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "MERCHANT", "Merchant" },
            { "MERCXANTHIA", "Xanthia" },
        };

    /// <summary>
    /// Humanize an enum id, preferring a curated display name when the mechanical
    /// humanization would be wrong.
    /// </summary>
    public static string HumanizeId(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return HumanizeEnum(id);
        if (DisplayNameOverrides.TryGetValue(id.Trim(), out string name))
            return name;
        return HumanizeEnum(id);
    }

    /// <summary>
    /// A loc key that has already lost its <c>xxxLang/</c> prefix, e.g. <c>MERCHANT name</c>
    /// or <c>MERCHANT description</c>. These reach us as plain strings with a space, so the
    /// normal key detection passed them straight through to the tooltip as display text.
    /// </summary>
    /// <summary>
    /// Null out text that is really a stripped loc key ("MERCHANT name"), so a "successful"
    /// lookup that returned a placeholder is treated as a miss.
    /// </summary>
    private static string Usable(string s)
    {
        if (string.IsNullOrEmpty(s)) return null;
        return TryParseBareLangKey(s, out _, out _) ? null : s;
    }

    private static bool TryParseBareLangKey(string s, out string id, out string suffix)
    {
        id = null;
        suffix = null;
        if (string.IsNullOrWhiteSpace(s)) return false;
        var m = System.Text.RegularExpressions.Regex.Match(
            s.Trim(),
            @"^(?<id>[A-Z0-9_]{3,})\s+(?<suffix>name|description|tips|desc)$");
        if (!m.Success) return false;
        id = m.Groups["id"].Value;
        suffix = m.Groups["suffix"].Value;
        return true;
    }

    /// <summary>
    /// Build and translate common VS I2 term shapes for a character/item/weapon id.
    /// Used when the game hands us a templated key or when we synthesize from CharacterType.
    /// </summary>
    public static string LocalizeTypedDescription(string id, string kind = "description")
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;
        id = id.Trim();
        // Try brace form first (matches what GetDescription sometimes returns)
        string t = LocalizeDisplayText("itemLang/{" + id + "}" + kind);
        if (!string.IsNullOrEmpty(t)) return t;
        t = LocalizeDisplayText("powerupLang/" + id + " " + kind);
        if (!string.IsNullOrEmpty(t)) return t;
        t = TryLocKeyVariants("powerupLang/", id, kind);
        if (!string.IsNullOrEmpty(t)) return t;
        t = TryLocKeyVariants("itemLang/", id, kind);
        if (!string.IsNullOrEmpty(t)) return t;
        t = TryLocKeyVariants("charLang/", id, kind);
        if (!string.IsNullOrEmpty(t)) return t;
        t = TryLocKeyVariants("weaponLang/", id, kind);
        if (!string.IsNullOrEmpty(t)) return t;
        if (string.Equals(kind, "name", StringComparison.OrdinalIgnoreCase))
            return HumanizeEnum(id);
        return null;
    }

    private static string TryLocKeyVariants(string prefix, string id, string suffix)
    {
        if (string.IsNullOrEmpty(id)) return null;
        suffix = suffix ?? "";
        string[] candidates =
        {
            prefix + id + " " + suffix,
            prefix + id + suffix,
            prefix + id + "_" + suffix,
            prefix + id + "/" + suffix,
            prefix + "{" + id + "}" + suffix,
            prefix + "{" + id + "} " + suffix,
            "powerupLang/" + id + " " + suffix,
            "powerupLang/" + id + " description",
            "powerupLang/" + id + " name",
            "itemLang/" + id + " " + suffix,
            "itemLang/" + id + " description",
            "itemLang/" + id + " name",
            "itemLang/" + id + " tips",
            "charLang/" + id + " " + suffix,
            "weaponLang/" + id + " " + suffix,
            "Characters/" + id + "/" + suffix,
            "Characters/" + id + "/description",
            "Characters/" + id + "/name",
            id + " " + suffix,
        };
        foreach (string c in candidates)
        {
            if (string.IsNullOrWhiteSpace(c)) continue;
            string t = Translate(c.Trim());
            if (!string.IsNullOrEmpty(t))
                return t;
        }
        return null;
    }

    public static bool LooksLikeLocKey(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return true;
        // Multi-line with real prose — not a single term (handled by LocalizeMultiline)
        if ((s.IndexOf('\n') >= 0 || s.IndexOf('\r') >= 0)
            && s.IndexOf("Lang/", StringComparison.OrdinalIgnoreCase) < 0
            && s.IndexOf('{') < 0)
            return false;
        // Any I2 category: powerupLang/, itemLang/, weaponLang/, stageLang/, …
        if (s.IndexOf("Lang/", StringComparison.OrdinalIgnoreCase) >= 0)
            return true;
        if (s.IndexOf('{') >= 0 && s.IndexOf('}') >= 0)
            return true;
        // path-like term with no spaces
        if (s.IndexOf('/') >= 0 && s.IndexOf(' ') < 0)
            return true;
        return false;
    }

    /// <summary>
    /// Localize each line of a multi-line body independently so a bad first-line key
    /// does not wipe the rest of the tooltip text.
    /// </summary>
    public static string LocalizeMultilineDisplayText(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return null;
        if (raw.IndexOf('\n') < 0 && raw.IndexOf('\r') < 0)
            return LocalizeDisplayText(raw);

        var lines = raw.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                // keep blank separators
                if (sb.Length > 0) sb.AppendLine();
                continue;
            }
            if (LooksLikeLocKey(line))
            {
                string t = LocalizeDisplayText(line.Trim());
                if (string.IsNullOrEmpty(t))
                    continue; // drop untranslated keys
                sb.AppendLine(t);
            }
            else
            {
                sb.AppendLine(line);
            }
        }
        string result = sb.ToString().TrimEnd();
        return string.IsNullOrEmpty(result) ? null : result;
    }

    private static bool IsGoodTranslation(string result, string term)
    {
        if (string.IsNullOrEmpty(result) || result == term)
            return false;
        if (LooksLikeLocKey(result))
            return false;
        return true;
    }

    private static string TryI2(string term, bool applyParameters)
    {
        try
        {
            return LocalizationManager.GetTranslation(
                term,
                FixForRTL: true,
                maxLineLengthForRTL: 0,
                ignoreRTLnumbers: true,
                applyParameters: applyParameters,
                localParametersRoot: null,
                overrideLanguage: null,
                allowLocalizedParameters: true);
        }
        catch
        {
            return null;
        }
    }

    public static string HumanizeEnum(string raw)
    {
        if (string.IsNullOrEmpty(raw))
        {
            return "Unknown";
        }
        // WHIP -> Whip, MAGIC_MISSILE -> Magic Missile
        string[] parts = raw.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < parts.Length; i++)
        {
            string p = parts[i];
            if (p.Length == 0)
            {
                continue;
            }
            parts[i] = char.ToUpperInvariant(p[0]) + (p.Length > 1 ? p.Substring(1).ToLowerInvariant() : "");
        }
        return string.Join(" ", parts);
    }

    /// <summary>
    /// Parse weapon type from game strings. evoInto is often a lowercase slug ("vampirica"), not the enum name.
    /// </summary>
    public static bool TryParseWeaponType(string raw, out WeaponType type)
    {
        type = default;
        if (string.IsNullOrEmpty(raw))
        {
            return false;
        }
        // Case-insensitive enum parse (WHIP / whip / Whip)
        if (Enum.TryParse(raw, ignoreCase: true, out type) && Enum.IsDefined(typeof(WeaponType), type))
        {
            return true;
        }
        // Slug forms: "max-health" / "maxhealth" / "tp-alchemywhip1"
        string normalized = raw.Replace("-", "_").Replace(" ", "_");
        if (Enum.TryParse(normalized, ignoreCase: true, out type) && Enum.IsDefined(typeof(WeaponType), type))
        {
            return true;
        }
        // Match by frameName / name cache
        EnsureLoaded();
        foreach (var kv in SpriteToWeapon)
        {
            if (string.Equals(kv.Key, raw, StringComparison.OrdinalIgnoreCase)
                || string.Equals(System.IO.Path.GetFileNameWithoutExtension(kv.Key), raw, StringComparison.OrdinalIgnoreCase))
            {
                type = kv.Value;
                return true;
            }
        }
        foreach (var kv in WeaponNames)
        {
            if (string.Equals(kv.Value, raw, StringComparison.OrdinalIgnoreCase)
                || string.Equals(kv.Key.ToString(), raw, StringComparison.OrdinalIgnoreCase))
            {
                type = kv.Key;
                return true;
            }
        }
        // Match evoInto slug to enum by stripping non-alphanumerics
        string compact = System.Text.RegularExpressions.Regex.Replace(raw, "[^A-Za-z0-9]", "").ToUpperInvariant();
        foreach (WeaponType wt in Enum.GetValues(typeof(WeaponType)))
        {
            string en = System.Text.RegularExpressions.Regex.Replace(wt.ToString(), "[^A-Za-z0-9]", "").ToUpperInvariant();
            if (en == compact)
            {
                type = wt;
                return true;
            }
        }
        return false;
    }

    /// <summary>Load weapon/passive icon via typed SpriteManager.</summary>
    public static Sprite GetSprite(WeaponType type)
    {
        EnsureLoaded();
        WeaponData data = GetWeaponData(type);
        if (data == null)
        {
            return null;
        }
        return LoadSprite(data.frameName, data.texture);
    }

    public static Sprite GetSprite(PowerUpType type)
    {
        EnsureLoaded();
        PowerUpData data = GetPowerUpData(type);
        if (data == null)
        {
            return null;
        }
        return LoadSprite(data.frameName, data.texture);
    }

    public static Sprite LoadSprite(string frameName, string textureName)
    {
        if (string.IsNullOrEmpty(frameName))
        {
            return null;
        }
        try
        {
            Sprite s = null;
            if (!string.IsNullOrEmpty(textureName))
            {
                s = SpriteManager.GetSpriteFast(frameName, textureName);
                if (s != null) return s;
                s = SpriteManager.GetSprite(frameName, textureName, ignoreExtension: true);
                if (s != null) return s;
            }
            // frame-only lookups
            s = SpriteManager.GetSprite(frameName, ignoreExtension: true);
            if (s != null) return s;

            // Try without extension
            if (frameName.Contains("."))
            {
                string bare = frameName.Substring(0, frameName.LastIndexOf('.'));
                if (!string.IsNullOrEmpty(textureName))
                {
                    s = SpriteManager.GetSpriteFast(bare, textureName);
                    if (s != null) return s;
                }
                s = SpriteManager.GetSprite(bare, ignoreExtension: true);
                if (s != null) return s;
            }

            // Common atlases as fallback
            string[] atlases = { "weapons", "items", "ui", "characters", "UI", "Weapons", "Items" };
            foreach (string atlas in atlases)
            {
                s = SpriteManager.GetSpriteFast(frameName, atlas);
                if (s != null) return s;
                s = SpriteManager.GetSprite(frameName, atlas, true);
                if (s != null) return s;
            }
        }
        catch (Exception ex)
        {
            Log.LogWarning("[GameData.LoadSprite] " + frameName + "/" + textureName + ": " + ex.Message);
        }
        if (Plugin.DebugVerbose)
            Plugin.Dbg($"LoadSprite miss frame={frameName} tex={textureName}");
        return null;
    }

    /// <summary>
    /// Build evolution display rows for a base/passive weapon using typed requires/evolvesFrom.
    /// </summary>
    public static System.Collections.Generic.List<EvoDisplayRow> BuildEvoRowsFor(WeaponType weapon)
    {
        EnsureLoaded();
        var rows = new System.Collections.Generic.List<EvoDisplayRow>();
        if (_weapons == null)
        {
            return rows;
        }

        // Path A: this weapon has evoInto → simple base + passives → evolved
        WeaponData self = GetWeaponData(weapon);
        if (self != null && !string.IsNullOrEmpty(self.evoInto)
            && TryParseWeaponType(self.evoInto, out WeaponType into))
        {
            var row = BuildRowFromEvolved(into, weapon);
            if (row != null)
            {
                rows.Add(row);
            }
        }

        // Path B: scan all evolutions that list this weapon in evolvesFrom or requires
        foreach (var recipe in FindEvolutionsFor(weapon))
        {
            bool already = false;
            foreach (var r in rows)
            {
                if (r.Evolved == recipe.Evolved)
                {
                    already = true;
                    break;
                }
            }
            if (already) continue;

            var row = new EvoDisplayRow
            {
                Evolved = recipe.Evolved,
                EvolvedName = recipe.EvolvedName,
                EvolvedSprite = GetSprite(recipe.Evolved),
                Passives = new System.Collections.Generic.List<EvoPassive>()
            };
            // Passives = requires ∪ evolvesFrom minus the hovered weapon
            var seen = new System.Collections.Generic.HashSet<WeaponType>();
            void addPassive(WeaponType wt, bool max)
            {
                if (wt == weapon || seen.Contains(wt)) return;
                seen.Add(wt);
                row.Passives.Add(new EvoPassive
                {
                    Type = wt,
                    Name = GetWeaponName(wt),
                    Sprite = GetSprite(wt),
                    RequiresMax = max
                });
            }
            foreach (var b in recipe.Bases)
            {
                addPassive(b, false);
            }
            foreach (var r in recipe.Requires)
            {
                addPassive(r, recipe.RequiresMax.Contains(r));
            }
            foreach (var m in recipe.RequiresMax)
            {
                addPassive(m, true);
            }
            rows.Add(row);
        }

        if (Plugin.DebugVerbose)
        {
            Plugin.Dbg($"BuildEvoRowsFor({weapon}): {rows.Count} row(s)");
            foreach (var r in rows)
            {
                string pass = string.Join("+", System.Linq.Enumerable.Select(r.Passives, p => p.Type.ToString() + (p.Sprite != null ? "*" : "!")));
                Plugin.Dbg($"  -> {r.Evolved} sprite={(r.EvolvedSprite != null ? "ok" : "NULL")} passives=[{pass}]");
            }
        }
        return rows;
    }

    private static EvoDisplayRow BuildRowFromEvolved(WeaponType evolved, WeaponType baseWeapon)
    {
        WeaponData evoData = GetWeaponData(evolved);
        if (evoData == null)
        {
            return null;
        }
        var row = new EvoDisplayRow
        {
            Evolved = evolved,
            EvolvedName = GetWeaponName(evolved),
            EvolvedSprite = GetSprite(evolved),
            Passives = new System.Collections.Generic.List<EvoPassive>()
        };
        var seen = new System.Collections.Generic.HashSet<WeaponType>();
        var maxSet = new System.Collections.Generic.HashSet<WeaponType>();
        if (evoData.requiresMax != null)
        {
            for (int i = 0; i < evoData.requiresMax.Count; i++)
            {
                maxSet.Add(evoData.requiresMax[i]);
            }
        }
        void add(WeaponType wt)
        {
            if (wt == baseWeapon || seen.Contains(wt)) return;
            seen.Add(wt);
            row.Passives.Add(new EvoPassive
            {
                Type = wt,
                Name = GetWeaponName(wt),
                Sprite = GetSprite(wt),
                RequiresMax = maxSet.Contains(wt)
            });
        }
        if (evoData.evolvesFrom != null)
        {
            for (int i = 0; i < evoData.evolvesFrom.Count; i++)
            {
                add(evoData.evolvesFrom[i]);
            }
        }
        if (evoData.requires != null)
        {
            for (int i = 0; i < evoData.requires.Count; i++)
            {
                add(evoData.requires[i]);
            }
        }
        // evoSynergy on the base weapon (sometimes lists passives)
        WeaponData baseData = GetWeaponData(baseWeapon);
        if (baseData?.evoSynergy != null)
        {
            foreach (WeaponType wt in baseData.evoSynergy)
            {
                add(wt);
            }
        }
        return row;
    }

    /// <summary>
    /// Walk all evolution recipes involving the given weapon (as base or passive requirement).
    /// </summary>
    public static System.Collections.Generic.List<EvolutionRecipe> FindEvolutionsFor(WeaponType weapon)
    {
        EnsureLoaded();
        var results = new System.Collections.Generic.List<EvolutionRecipe>();
        if (_weapons == null)
        {
            return results;
        }

        foreach (var kvp in _weapons)
        {
            WeaponType evolvedType = kvp.Key;
            Il2CppListWeapons list = kvp.Value;
            if (list == null)
            {
                continue;
            }
            for (int i = 0; i < list.Count; i++)
            {
                WeaponData data = list[i];
                if (data == null || !data.isEvolution)
                {
                    continue;
                }

                bool involves = false;
                Il2CppListWeaponType from = data.evolvesFrom;
                if (from != null)
                {
                    for (int j = 0; j < from.Count; j++)
                    {
                        if (from[j] == weapon)
                        {
                            involves = true;
                            break;
                        }
                    }
                }
                Il2CppListWeaponType req = data.requires;
                if (!involves && req != null)
                {
                    for (int j = 0; j < req.Count; j++)
                    {
                        if (req[j] == weapon)
                        {
                            involves = true;
                            break;
                        }
                    }
                }
                Il2CppListWeaponType reqMax = data.requiresMax;
                if (!involves && reqMax != null)
                {
                    for (int j = 0; j < reqMax.Count; j++)
                    {
                        if (reqMax[j] == weapon)
                        {
                            involves = true;
                            break;
                        }
                    }
                }

                if (!involves)
                {
                    continue;
                }

                var recipe = new EvolutionRecipe
                {
                    Evolved = evolvedType,
                    EvolvedName = GetWeaponName(evolvedType),
                    Bases = new System.Collections.Generic.List<WeaponType>(),
                    Requires = new System.Collections.Generic.List<WeaponType>(),
                    RequiresMax = new System.Collections.Generic.List<WeaponType>()
                };
                if (from != null)
                {
                    for (int j = 0; j < from.Count; j++)
                    {
                        recipe.Bases.Add(from[j]);
                    }
                }
                if (req != null)
                {
                    for (int j = 0; j < req.Count; j++)
                    {
                        recipe.Requires.Add(req[j]);
                    }
                }
                if (reqMax != null)
                {
                    for (int j = 0; j < reqMax.Count; j++)
                    {
                        recipe.RequiresMax.Add(reqMax[j]);
                    }
                }
                results.Add(recipe);
            }
        }
        return results;
    }

    // ── Items / relics / floor pickups (AllItems) ─────────────────────────

    private static void BuildItemCaches()
    {
        ItemNames.Clear();
        ItemDescriptions.Clear();
        SpriteToItem.Clear();
        _itemsBuilt = false;
        _items = null;
        if (_dataManager == null) return;
        try
        {
            _items = _dataManager.AllItems;
        }
        catch (Exception ex)
        {
            Log.LogWarning("[GameData] AllItems: " + ex.Message);
            return;
        }
        if (_items == null) return;

        int n = 0;
        foreach (var kvp in _items)
        {
            ItemType type = kvp.Key;
            ItemData data = kvp.Value;
            if (data == null) continue;
            ItemNames[type] = ResolveItemName(data, type);
            ItemDescriptions[type] = ResolveItemDescription(data, type);
            void indexFrame(string frame)
            {
                if (string.IsNullOrEmpty(frame)) return;
                if (!SpriteToItem.ContainsKey(frame))
                    SpriteToItem[frame] = type;
                string bare = frame;
                if (bare.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    bare = bare.Substring(0, bare.Length - 4);
                if (!SpriteToItem.ContainsKey(bare))
                    SpriteToItem[bare] = type;
            }
            try { indexFrame(data.frameName); } catch { }
            try { indexFrame(data.collectionFrame); } catch { }
            n++;
        }
        _itemsBuilt = n > 0;
        if (_itemsBuilt && !_loggedItems)
        {
            _loggedItems = true;
            Log.LogInfo($"[GameData] Items: {n} entries, {SpriteToItem.Count} sprite keys");
        }
    }

    public static ItemData GetItemData(ItemType type)
    {
        EnsureLoaded();
        if (_items == null || !_items.ContainsKey(type)) return null;
        return _items[type];
    }

    public static string GetItemName(ItemType type)
    {
        EnsureLoaded();
        if (ItemNames.TryGetValue(type, out string cached) && !string.IsNullOrEmpty(cached))
            return cached;
        var data = GetItemData(type);
        string name = ResolveItemName(data, type);
        ItemNames[type] = name;
        return name;
    }

    public static string GetItemDescription(ItemType type)
    {
        EnsureLoaded();
        if (ItemDescriptions.TryGetValue(type, out string cached) && !string.IsNullOrEmpty(cached))
            return cached;
        var data = GetItemData(type);
        string desc = ResolveItemDescription(data, type);
        ItemDescriptions[type] = desc;
        return desc;
    }

    /// <summary>
    /// How to unlock this item/relic (achievement tip). Used for locked Collection cells.
    /// </summary>
    public static string GetItemUnlockHint(ItemType type)
    {
        EnsureLoaded();
        var data = GetItemData(type);
        if (data == null) return null;
        try
        {
            string t = LocalizeDisplayText(data.GetLocalizedAchievementTips(type));
            if (!string.IsNullOrEmpty(t)) return t;
        }
        catch { }
        try
        {
            string t = LocalizeDisplayText(data.achievementTips);
            if (!string.IsNullOrEmpty(t)) return t;
        }
        catch { }
        return null;
    }

    public static Sprite GetItemSprite(ItemType type)
    {
        EnsureLoaded();
        ItemData data = GetItemData(type);
        if (data == null) return null;
        Sprite s = LoadSprite(data.frameName, data.texture);
        if (s != null) return s;
        if (!string.IsNullOrEmpty(data.collectionFrame))
            s = LoadSprite(data.collectionFrame, data.texture);
        return s;
    }

    private static string ResolveItemName(ItemData data, ItemType type)
    {
        if (data != null)
        {
            try
            {
                string t = LocalizeDisplayText(data.GetLocalizedName(type));
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch { }
            try
            {
                string t = LocalizeDisplayText(data.name);
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch { }
        }
        return HumanizeId(type.ToString());
    }

    private static string ResolveItemDescription(ItemData data, ItemType type)
    {
        if (data != null)
        {
            try
            {
                string t = LocalizeDisplayText(data.GetLocalizedDescription(type));
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch { }
            try
            {
                string t = LocalizeDisplayText(data.GetLocalizedTips(type));
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch { }
            try
            {
                string t = LocalizeDisplayText(data.description);
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch { }
            try
            {
                string t = LocalizeDisplayText(data.tips);
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch { }
        }
        return "";
    }

    public static bool TryParseItemType(string raw, out ItemType type)
    {
        type = default;
        if (string.IsNullOrEmpty(raw)) return false;
        if (Enum.TryParse(raw, ignoreCase: true, out type) && Enum.IsDefined(typeof(ItemType), type))
            return true;
        string normalized = raw.Replace("-", "_").Replace(" ", "_");
        if (Enum.TryParse(normalized, ignoreCase: true, out type) && Enum.IsDefined(typeof(ItemType), type))
            return true;
        EnsureLoaded();
        if (SpriteToItem.TryGetValue(raw, out type)) return true;
        string bare = raw;
        if (bare.Contains(".")) bare = System.IO.Path.GetFileNameWithoutExtension(bare);
        if (SpriteToItem.TryGetValue(bare, out type)) return true;
        string compact = System.Text.RegularExpressions.Regex.Replace(raw, "[^A-Za-z0-9]", "").ToUpperInvariant();
        foreach (ItemType it in Enum.GetValues(typeof(ItemType)))
        {
            string en = System.Text.RegularExpressions.Regex.Replace(it.ToString(), "[^A-Za-z0-9]", "").ToUpperInvariant();
            if (en == compact) { type = it; return true; }
        }
        return false;
    }

    /// <summary>Resolve a map/icon sprite name to ItemType or WeaponType for tooltips.</summary>
    public static bool TryResolveSprite(string spriteName, out ItemType? item, out WeaponType? weapon)
    {
        item = null;
        weapon = null;
        if (string.IsNullOrEmpty(spriteName)) return false;
        EnsureLoaded();
        string bare = spriteName;
        if (bare.Contains(".")) bare = System.IO.Path.GetFileNameWithoutExtension(bare);

        if (SpriteToItem.TryGetValue(spriteName, out ItemType it) || SpriteToItem.TryGetValue(bare, out it)
            || TryParseItemType(bare, out it))
        {
            item = it;
            return true;
        }
        if (SpriteToWeapon.TryGetValue(spriteName, out WeaponType wt) || SpriteToWeapon.TryGetValue(bare, out wt)
            || TryParseWeaponType(bare, out wt))
        {
            weapon = wt;
            return true;
        }
        // Powerups are also WeaponType entries for many passives; already covered by SpriteToWeapon
        if (SpriteToPowerUp.TryGetValue(spriteName, out PowerUpType pu) || SpriteToPowerUp.TryGetValue(bare, out pu))
        {
            // PowerUpType and ItemType share some numeric values but different enums — try name match as item first failed
            if (TryParseWeaponType(pu.ToString(), out wt))
            {
                weapon = wt;
                return true;
            }
        }
        return false;
    }

    // ── Arcana (typed AllArcanas) ─────────────────────────────────────────

    private static void BuildArcanaCaches()
    {
        ArcanaNames.Clear();
        ArcanaDescriptions.Clear();
        WeaponToArcanas.Clear();
        ItemToArcanas.Clear();
        _arcanaBuilt = false;
        _arcanas = null;

        if (_dataManager == null)
        {
            return;
        }

        try
        {
            _arcanas = _dataManager.AllArcanas;
        }
        catch (Exception ex)
        {
            Log.LogWarning("[GameData] AllArcanas: " + ex.Message);
            return;
        }

        if (_arcanas == null)
        {
            return;
        }

        int indexed = 0;
        foreach (var kvp in _arcanas)
        {
            ArcanaType type = kvp.Key;
            ArcanaData data = kvp.Value;
            if (data == null)
            {
                continue;
            }

            // Skip void / always-hidden noise
            if (type == ArcanaType.VOID)
            {
                continue;
            }

            try
            {
                if (data.alwaysHidden)
                {
                    continue;
                }
            }
            catch
            {
            }

            ArcanaNames[type] = ResolveArcanaName(data, type);
            ArcanaDescriptions[type] = ResolveArcanaDescription(data, type);

            // weapons list -> reverse index
            try
            {
                Il2CppListObject weapons = data.weapons;
                if (weapons != null)
                {
                    for (int i = 0; i < weapons.Count; i++)
                    {
                        Il2CppSystem.Object obj = weapons[i];
                        if (TryParseObjectAsWeapon(obj, out WeaponType wt))
                        {
                            if (!WeaponToArcanas.TryGetValue(wt, out var list))
                            {
                                list = new System.Collections.Generic.List<ArcanaType>();
                                WeaponToArcanas[wt] = list;
                            }
                            if (!list.Contains(type))
                            {
                                list.Add(type);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Plugin.DebugVerbose)
                    Plugin.Dbg($"Arcana weapons parse {type}: {ex.Message}");
            }

            // items list -> reverse index (ItemType)
            try
            {
                Il2CppListObject items = data.items;
                if (items != null)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        Il2CppSystem.Object obj = items[i];
                        if (TryParseObjectAsItem(obj, out ItemType it))
                        {
                            if (!ItemToArcanas.TryGetValue(it, out var list))
                            {
                                list = new System.Collections.Generic.List<ArcanaType>();
                                ItemToArcanas[it] = list;
                            }
                            if (!list.Contains(type))
                            {
                                list.Add(type);
                            }
                        }
                        // Some arcanas put passives in items as weapon-type names
                        else if (TryParseObjectAsWeapon(obj, out WeaponType wt2))
                        {
                            if (!WeaponToArcanas.TryGetValue(wt2, out var list2))
                            {
                                list2 = new System.Collections.Generic.List<ArcanaType>();
                                WeaponToArcanas[wt2] = list2;
                            }
                            if (!list2.Contains(type))
                            {
                                list2.Add(type);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Plugin.DebugVerbose)
                    Plugin.Dbg($"Arcana items parse {type}: {ex.Message}");
            }

            indexed++;
        }

        _arcanaBuilt = indexed > 0;
        if (_arcanaBuilt && !_loggedArcana)
        {
            _loggedArcana = true;
            Log.LogInfo($"[GameData] Arcanas: {indexed} entries, {WeaponToArcanas.Count} weapon links, {ItemToArcanas.Count} item links");
        }
    }

    public static ArcanaData GetArcanaData(ArcanaType type)
    {
        EnsureLoaded();
        if (_arcanas == null || !_arcanas.ContainsKey(type))
        {
            return null;
        }
        return _arcanas[type];
    }

    public static string GetArcanaName(ArcanaType type)
    {
        EnsureLoaded();
        if (ArcanaNames.TryGetValue(type, out string cached) && !string.IsNullOrEmpty(cached))
        {
            return cached;
        }
        var data = GetArcanaData(type);
        string name = ResolveArcanaName(data, type);
        ArcanaNames[type] = name;
        return name;
    }

    public static string GetArcanaDescription(ArcanaType type)
    {
        EnsureLoaded();
        if (ArcanaDescriptions.TryGetValue(type, out string cached) && !string.IsNullOrEmpty(cached))
        {
            return cached;
        }
        var data = GetArcanaData(type);
        string desc = ResolveArcanaDescription(data, type);
        ArcanaDescriptions[type] = desc;
        return desc;
    }

    public static Sprite GetArcanaSprite(ArcanaType type)
    {
        EnsureLoaded();
        ArcanaData data = GetArcanaData(type);
        if (data == null)
        {
            return null;
        }
        string frame = data.frameName;
        string tex = data.texture;
        // Strip .png if present
        if (!string.IsNullOrEmpty(frame) && frame.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        {
            frame = frame.Substring(0, frame.Length - 4);
        }
        Sprite s = LoadSprite(frame, tex);
        if (s != null) return s;
        // Common arcana atlases
        string[] atlases = { "arcanas", "cards", "randomazzo", "ui", "items" };
        foreach (string atlas in atlases)
        {
            s = LoadSprite(frame, atlas);
            if (s != null) return s;
            if (!string.IsNullOrEmpty(data.frameName))
            {
                s = LoadSprite(data.frameName, atlas);
                if (s != null) return s;
            }
        }
        return null;
    }

    private static string ResolveArcanaName(ArcanaData data, ArcanaType type)
    {
        if (data != null)
        {
            try
            {
                string t = LocalizeDisplayText(data.GetLocalizedNameTerm(type));
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch
            {
            }
            try
            {
                string t = LocalizeDisplayText(data.name);
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch
            {
            }
        }
        // T21_BLOODY -> Bloody, D01_SAPPHIRE_MIST -> Sapphire Mist
        string raw = type.ToString();
        if (raw.Length > 4 && raw[0] == 'T' && char.IsDigit(raw[1]))
        {
            int us = raw.IndexOf('_');
            if (us > 0) raw = raw.Substring(us + 1);
        }
        else if (raw.Length > 4 && raw[0] == 'D' && char.IsDigit(raw[1]))
        {
            int us = raw.IndexOf('_');
            if (us > 0) raw = raw.Substring(us + 1);
        }
        return HumanizeEnum(raw);
    }

    private static string ResolveArcanaDescription(ArcanaData data, ArcanaType type)
    {
        if (data != null)
        {
            try
            {
                string t = LocalizeDisplayText(data.GetLocalizedDescriptionTerm(type));
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch
            {
            }
            try
            {
                string t = LocalizeDisplayText(data.description);
                if (!string.IsNullOrEmpty(t))
                    return t;
            }
            catch
            {
            }
        }
        return "";
    }

    /// <summary>All arcanas whose weapons/items list includes this weapon.</summary>
    public static System.Collections.Generic.List<ArcanaDisplayInfo> GetArcanasAffectingWeapon(WeaponType weapon)
    {
        EnsureLoaded();
        var result = new System.Collections.Generic.List<ArcanaDisplayInfo>();
        if (!WeaponToArcanas.TryGetValue(weapon, out var types) || types == null)
        {
            if (Plugin.DebugVerbose)
                Plugin.Dbg($"GetArcanasAffectingWeapon({weapon}): 0");
            return result;
        }
        foreach (ArcanaType at in types)
        {
            result.Add(ToArcanaDisplay(at));
        }
        if (Plugin.DebugVerbose)
            Plugin.Dbg($"GetArcanasAffectingWeapon({weapon}): {result.Count} [{string.Join(",", System.Linq.Enumerable.Select(result, a => a.Type.ToString()))}]");
        return result;
    }

    /// <summary>All arcanas whose items list includes this item.</summary>
    public static System.Collections.Generic.List<ArcanaDisplayInfo> GetArcanasAffectingItem(ItemType item)
    {
        EnsureLoaded();
        var result = new System.Collections.Generic.List<ArcanaDisplayInfo>();
        if (!ItemToArcanas.TryGetValue(item, out var types) || types == null)
        {
            return result;
        }
        foreach (ArcanaType at in types)
        {
            result.Add(ToArcanaDisplay(at));
        }
        return result;
    }

    public static System.Collections.Generic.List<WeaponType> GetWeaponsAffectedByArcana(ArcanaType type)
    {
        EnsureLoaded();
        var result = new System.Collections.Generic.List<WeaponType>();
        ArcanaData data = GetArcanaData(type);
        if (data?.weapons == null)
        {
            return result;
        }
        var seen = new System.Collections.Generic.HashSet<WeaponType>();
        try
        {
            Il2CppListObject weapons = data.weapons;
            for (int i = 0; i < weapons.Count; i++)
            {
                if (TryParseObjectAsWeapon(weapons[i], out WeaponType wt) && seen.Add(wt))
                {
                    result.Add(wt);
                }
            }
            // also items that parse as weapons (passives)
            if (data.items != null)
            {
                Il2CppListObject items = data.items;
                for (int i = 0; i < items.Count; i++)
                {
                    if (TryParseObjectAsWeapon(items[i], out WeaponType wt) && seen.Add(wt))
                    {
                        result.Add(wt);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Log.LogWarning("[GameData] GetWeaponsAffectedByArcana: " + ex.Message);
        }
        return result;
    }

    public static System.Collections.Generic.List<ItemType> GetItemsAffectedByArcana(ArcanaType type)
    {
        EnsureLoaded();
        var result = new System.Collections.Generic.List<ItemType>();
        ArcanaData data = GetArcanaData(type);
        if (data?.items == null)
        {
            return result;
        }
        var seen = new System.Collections.Generic.HashSet<ItemType>();
        try
        {
            Il2CppListObject items = data.items;
            for (int i = 0; i < items.Count; i++)
            {
                if (TryParseObjectAsItem(items[i], out ItemType it) && seen.Add(it))
                {
                    result.Add(it);
                }
            }
        }
        catch (Exception ex)
        {
            Log.LogWarning("[GameData] GetItemsAffectedByArcana: " + ex.Message);
        }
        return result;
    }

    private static ArcanaDisplayInfo ToArcanaDisplay(ArcanaType type)
    {
        return new ArcanaDisplayInfo
        {
            Type = type,
            Name = GetArcanaName(type),
            Description = GetArcanaDescription(type),
            Sprite = GetArcanaSprite(type),
            Data = GetArcanaData(type)
        };
    }

    /// <summary>Parse Il2Cpp Object from ArcanaData.weapons list (string / enum box / int).</summary>
    public static bool TryParseObjectAsWeapon(Il2CppSystem.Object obj, out WeaponType type)
    {
        type = default;
        if (obj == null)
        {
            return false;
        }
        try
        {
            string s = obj.ToString();
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            // "VampireSurvivors.Data.WeaponType.WHIP" or "WHIP" or "whip"
            int lastDot = s.LastIndexOf('.');
            if (lastDot >= 0 && lastDot < s.Length - 1)
            {
                s = s.Substring(lastDot + 1);
            }
            if (TryParseWeaponType(s, out type))
            {
                return true;
            }
            if (int.TryParse(s, out int n) && Enum.IsDefined(typeof(WeaponType), n))
            {
                type = (WeaponType)n;
                return true;
            }
        }
        catch
        {
        }
        return false;
    }

    public static bool TryParseObjectAsItem(Il2CppSystem.Object obj, out ItemType type)
    {
        type = default;
        if (obj == null)
        {
            return false;
        }
        try
        {
            string s = obj.ToString();
            if (string.IsNullOrEmpty(s))
            {
                return false;
            }
            int lastDot = s.LastIndexOf('.');
            if (lastDot >= 0 && lastDot < s.Length - 1)
            {
                s = s.Substring(lastDot + 1);
            }
            if (Enum.TryParse(s, ignoreCase: true, out type) && Enum.IsDefined(typeof(ItemType), type))
            {
                return true;
            }
            if (int.TryParse(s, out int n) && Enum.IsDefined(typeof(ItemType), n))
            {
                type = (ItemType)n;
                return true;
            }
        }
        catch
        {
        }
        return false;
    }
}

public class ArcanaDisplayInfo
{
    public ArcanaType Type;
    public string Name;
    public string Description;
    public Sprite Sprite;
    public ArcanaData Data;
}

public class EvoDisplayRow
{
    public WeaponType Evolved;
    public string EvolvedName;
    public Sprite EvolvedSprite;
    public System.Collections.Generic.List<EvoPassive> Passives;
}

public class EvoPassive
{
    public WeaponType Type;
    public string Name;
    public Sprite Sprite;
    public bool RequiresMax;
}

public class EvolutionRecipe
{
    public WeaponType Evolved;
    public string EvolvedName;
    public System.Collections.Generic.List<WeaponType> Bases;
    public System.Collections.Generic.List<WeaponType> Requires;
    public System.Collections.Generic.List<WeaponType> RequiresMax;
}
