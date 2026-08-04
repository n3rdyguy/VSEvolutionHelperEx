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
        if (!LooksLikeLocKey(s))
            return s;

        // Direct I2 lookup
        string t = Translate(s);
        if (!string.IsNullOrEmpty(t))
            return t;

        try
        {
            // powerupLang/MERCHANT name  |  itemLang/{MERCHANT}description  |  powerupLang/MERCHANT name
            if (TryParseLangTerm(s, out string prefix, out string id, out string suffix))
            {
                t = TryLocKeyVariants(prefix, id, suffix);
                if (!string.IsNullOrEmpty(t))
                    return t;
                // Cross-table: characters often live under powerupLang in VS data
                foreach (string alt in new[] { "powerupLang/", "itemLang/", "charLang/", "weaponLang/" })
                {
                    if (string.Equals(alt, prefix, StringComparison.OrdinalIgnoreCase))
                        continue;
                    t = TryLocKeyVariants(alt, id, suffix);
                    if (!string.IsNullOrEmpty(t))
                        return t;
                }
                // Name keys with no translation → humanize the id (better than raw term)
                if (string.Equals(suffix, "name", StringComparison.OrdinalIgnoreCase)
                    || string.IsNullOrEmpty(suffix))
                    return HumanizeEnum(id);
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
                    t = TryLocKeyVariants(pfx, bid, sfx);
                    if (!string.IsNullOrEmpty(t))
                        return t;
                    if (string.Equals(sfx, "name", StringComparison.OrdinalIgnoreCase)
                        || string.IsNullOrEmpty(sfx))
                        return HumanizeEnum(bid);
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
                t = Translate(stripped);
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
        return HumanizeEnum(type.ToString());
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
