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
using Il2CppListPowerUps = Il2CppSystem.Collections.Generic.List<VampireSurvivors.Data.PowerUp.PowerUpData>;
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

        // DataManager is not a UnityEngine.Object - resolve via UI pages / GameManager injection.
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

    /// <summary>
    /// What a Power Up costs from here on, as displayable rows.
    ///
    /// Prices are computed, not stored: a level costs its own number times the power up's base
    /// price, plus a surcharge that grows with the total number of levels bought across every
    /// power up. So the record's <c>price</c> is a base, the same upgrade costs more later than
    /// it does now, and the page's single "next level" figure answers almost nothing about what
    /// finishing it will cost.
    ///
    /// <paramref name="nextPrice"/> is the game's own answer for the next level, taken from
    /// PlayerStats. Everything further ahead is projected from it rather than from a formula of
    /// our own: the surcharge is measured by subtracting the base part from that live number,
    /// then grown per purchase. If the two disagree about the next level - the one value we can
    /// check - no projection is shown at all, because a confidently wrong total is worse than
    /// no total.
    /// </summary>
    public static System.Collections.Generic.List<IconRow> GetPowerUpRows(
        PowerUpType type, int currentLevel, int maxLevel, float nextPrice, out string description)
    {
        description = null;
        EnsureLoaded();
        var rows = new System.Collections.Generic.List<IconRow>();
        if (_powerUps == null || !_powerUps.ContainsKey(type)) return rows;
        var list = _powerUps[type];
        if (list == null || list.Count == 0) return rows;

        try
        {
            description = LocalizeMultilineDisplayText(list[0].description);
            if (string.IsNullOrWhiteSpace(description)) description = null;
        }
        catch { }

        int basePrice = 0;
        try { basePrice = list[0].price; } catch { }

        int max = maxLevel > 0 ? maxLevel : list.Count;
        if (currentLevel < 0) currentLevel = 0;
        if (currentLevel > max) currentLevel = max;

        rows.Add(IconRow.Header("Levels:"));
        rows.Add(new IconRow(null, $"Owned: {currentLevel} / {max}"));

        if (currentLevel >= max)
        {
            rows.Add(new IconRow(null, "Maxed out"));
            AddRankRow(rows, list);
            return rows;
        }

        if (nextPrice > 0) rows.Add(new IconRow(null, $"Next level: {Gold(nextPrice)}"));

        // The surcharge riding on this purchase, measured rather than assumed.
        double surcharge = nextPrice - (double)(currentLevel + 1) * basePrice;
        if (nextPrice > 0 && basePrice > 0 && surcharge >= 0)
        {
            double running = surcharge;
            long remaining = 0;
            var ladder = new System.Text.StringBuilder();
            for (int lvl = currentLevel + 1; lvl <= max; lvl++)
            {
                long price = (long)((double)lvl * basePrice + Math.Floor(running));
                remaining += price;
                if (ladder.Length > 0) ladder.Append(", ");
                ladder.Append(Gold(price));
                // Each purchase anywhere pushes the surcharge up; buying these is what does it.
                running = running > 0 ? running * PowerUpSurchargeGrowth : running;
            }

            rows.Add(new IconRow(null, $"Remaining to max: {Gold(remaining)}"));
            if (ladder.Length > 0)
            {
                rows.Add(IconRow.Header("Projected per level:"));
                rows.Add(new IconRow(null, ladder.ToString()));
                rows.Add(new IconRow(null, "Assumes you buy these next; other purchases raise them."));
            }
        }
        else if (Plugin.DebugVerbose)
        {
            Plugin.Dbg($"[GameData] {type}: no projection, next={nextPrice} base={basePrice} "
                + $"level={currentLevel} surcharge={surcharge:0.##}");
        }

        AddRankRow(rows, list);
        return rows;
    }

    /// <summary>
    /// How fast the global surcharge grows per level bought. Community calculators put it at
    /// <c>floor(20 * 1.1^n)</c>, and only the growth is taken from that here - the amount itself
    /// is measured from the game's own price, so a changed constant shifts nothing.
    /// </summary>
    private const double PowerUpSurchargeGrowth = 1.1;

    private static void AddRankRow(System.Collections.Generic.List<IconRow> rows, Il2CppListPowerUps list)
    {
        try
        {
            int rank = list[0].unlockedRank;
            if (rank > 0)
            {
                rows.Add(IconRow.Header("Requires:"));
                rows.Add(new IconRow(null, $"Rank {rank}"));
            }
        }
        catch { }
    }

    /// <summary>Gold amounts read better grouped; the page writes them plain.</summary>
    private static string Gold(double amount)
    {
        return amount.ToString("N0", System.Globalization.CultureInfo.InvariantCulture);
    }

    public static string GetWeaponName(WeaponType type)
    {
        EnsureLoaded();
        // VOID is not a real weapon - callers that care should use IsRealWeaponType first
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
        // "MERCHANT name" / "MERCHANT description" - a key that already lost its xxxLang/
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
        // stripped key ("MERCHANT name"), so the result is validated, not just the input -
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
                // Description missing - omit rather than show key
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
    /// The SecretData handed to SecretItemUI is sparse - every reward field arrives null (or
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
    // secrets, in both the row's copy and the DataManager.AllSecrets catalog - but the raw
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
        // Not "tried" until DataManager exists - otherwise an early call would poison the
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
    /// - DLC content that is not installed - still render as a humanized label rather than
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
    /// maxHp is stored at a tenth of the value the game reports - an enemy the Bestiary calls
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
    // Deriving it from the stage JSON disagreed with the game - Boss Rash for an enemy the
    // game lists under Cappella Magna - and missed stages entirely, and a wrong stage list is
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
    /// record or the array is shaped differently than assumed - this tells us which.
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
    /// Values are reported as the data stores them rather than converted to percentages - the
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
            return Math.Abs(a - b) < 0.005f ? Num(a) : $"{Num(a)}-{Num(b)}";
        }
    }

    /// <summary>One enemy's record from the enemies JSON, including its Bestiary metadata.</summary>
    private sealed class EnemyRec
    {
        public bool HasHp, HasPower, HasSpeed, HasXp, HasKnock;
        public float Hp, Power, SpeedMin, SpeedMax, Xp, Knock;
        public System.Collections.Generic.List<string> Traits = new System.Collections.Generic.List<string>();
        /// <summary>Sibling ids the Bestiary groups into one entry - the source of stat ranges.</summary>
        public System.Collections.Generic.List<string> Variants;
        /// <summary>Stage ids the Bestiary lists under "Found in".</summary>
        public System.Collections.Generic.List<string> Places;
        public string Name, Desc;
        /// <summary>Sprite frame/texture as shipped, for enemies whose typed record has none.</summary>
        public string Frame, Texture;
    }

    private static System.Collections.Generic.Dictionary<string, EnemyRec> _enemyRecs;
    private static bool _enemyRecsParsed;

    /// <summary>
    /// Parse the enemies JSON, which carries the Bestiary's own metadata alongside the stats:
    /// <c>bVariants</c> (the sibling ids one Bestiary entry covers), <c>bPlaces</c> (its stage
    /// list), <c>bName</c> and <c>bDesc</c>.
    ///
    /// The ranges the Bestiary prints come from aggregating over <c>bVariants</c> - BAT1's
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
        r.Texture = JsonStr(rec, "textureName");
        var frames = JsonStrList(rec, "frameNames");
        if (frames != null && frames.Count > 0) r.Frame = frames[0];
        return r;
    }

    /// <summary>
    /// An enemy's sprite, from its own record and then from its Bestiary siblings.
    ///
    /// A row whose own frame does not resolve is often a variant of one that does - the entry
    /// covers a family - so the family is tried before giving up. The failure is logged with
    /// the frame and texture names, because a whole block of enemies failing together points at
    /// an atlas that is not loaded rather than at bad data.
    /// </summary>
    public static Sprite GetEnemySprite(string enemyId)
    {
        EnsureEnemyRecs();
        if (_enemyRecs == null || string.IsNullOrEmpty(enemyId)) return null;
        if (!_enemyRecs.TryGetValue(enemyId, out EnemyRec self) || self == null) return null;

        Sprite s = TryFrame(self);
        if ((Object)(object)s != (Object)null) return s;

        if (self.Variants != null)
        {
            foreach (string vid in self.Variants)
            {
                if (string.Equals(vid, enemyId, StringComparison.OrdinalIgnoreCase)) continue;
                if (!_enemyRecs.TryGetValue(vid, out EnemyRec v) || v == null) continue;
                s = TryFrame(v);
                if ((Object)(object)s != (Object)null) return s;
            }
        }

        if (Plugin.DebugVerbose)
            Plugin.Dbg($"[GameData] no sprite for {enemyId}: frame='{self.Frame}' texture='{self.Texture}'");
        return null;
    }

    private static Sprite TryFrame(EnemyRec r)
    {
        if (r == null || string.IsNullOrEmpty(r.Frame)) return null;
        try
        {
            Sprite s = LoadSprite(r.Frame, r.Texture);
            if ((Object)(object)s != (Object)null) return s;
            // Frame-only, in case the record names an atlas that is not the one holding it.
            return LoadSprite(r.Frame, null);
        }
        catch { return null; }
    }

    /// <summary>
    /// An enemy's Bestiary icon, by the naming the info panel uses.
    ///
    /// The panel draws portraits called "<c>kappa_i01</c>" and "<c>ugul_i01</c>" while the
    /// records name animation frames, "<c>kappa_0.png</c>". Those are two different sprites, and
    /// on some pages only the icon's atlas is loaded - which is why an enemy can be drawn on
    /// screen while its frame name resolves to nothing.
    ///
    /// The base is taken from the frame rather than the enemy id, because the two disagree
    /// often enough to matter: FS_ROTGHOUL's icon is "ugul_i01". Variants are tried second on
    /// the same reasoning as <see cref="GetEnemySprite"/>.
    /// </summary>
    public static Sprite GetEnemyIconSprite(string enemyId)
    {
        EnsureEnemyRecs();
        if (_enemyRecs == null || string.IsNullOrEmpty(enemyId)) return null;
        if (!_enemyRecs.TryGetValue(enemyId, out EnemyRec self) || self == null) return null;

        Sprite s = TryIcon(self);
        if ((Object)(object)s != (Object)null) return s;

        if (self.Variants != null)
        {
            foreach (string vid in self.Variants)
            {
                if (string.Equals(vid, enemyId, StringComparison.OrdinalIgnoreCase)) continue;
                if (!_enemyRecs.TryGetValue(vid, out EnemyRec v) || v == null) continue;
                s = TryIcon(v);
                if ((Object)(object)s != (Object)null) return s;
            }
        }
        return null;
    }

    private static Sprite TryIcon(EnemyRec r)
    {
        if (r == null || string.IsNullOrEmpty(r.Frame)) return null;
        try
        {
            string b = IconBase(r.Frame);
            if (string.IsNullOrEmpty(b)) return null;
            foreach (string cand in new[] { b + "_i01", b.ToLowerInvariant() + "_i01" })
            {
                Sprite s = LoadSprite(cand, r.Texture);
                if ((Object)(object)s != (Object)null) return s;
                s = LoadSprite(cand, null);
                if ((Object)(object)s != (Object)null) return s;
            }
        }
        catch { }
        return null;
    }

    /// <summary>"kappa_0.png" -> "kappa": drop the extension, then the frame number.</summary>
    private static string IconBase(string frame)
    {
        string b = frame;
        int dot = b.LastIndexOf('.');
        if (dot > 0) b = b.Substring(0, dot);
        int us = b.LastIndexOf('_');
        if (us > 0)
        {
            bool digits = us + 1 < b.Length;
            for (int i = us + 1; i < b.Length && digits; i++)
                if (b[i] < '0' || b[i] > '9') digits = false;
            if (digits) b = b.Substring(0, us);
        }
        return b;
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

    // ── Achievements ─────────────────────────────────────────────────────────

    /// <summary>An achievement's rewards and requirements, from the raw achievements JSON.</summary>
    private sealed class AchievementRec
    {
        public RewardIds Rewards;
        public string Description;
        public System.Collections.Generic.List<string> Requires;
    }

    private static System.Collections.Generic.Dictionary<string, AchievementRec> _achievements;
    private static bool _achievementsParsed;

    /// <summary>
    /// Read achievements from JSON rather than the typed catalog.
    ///
    /// AchievementData's reward fields are plain strings, so the VOID problem that broke the
    /// secrets does not apply - but its list fields are IL2CPP lists of enums, and iterating
    /// those returned the same value for every element when the Bestiary tried it. The JSON
    /// sidesteps both and covers every row without needing one selected first.
    /// </summary>
    private static void EnsureAchievements()
    {
        if (_achievementsParsed || _dataManager == null) return;
        _achievementsParsed = true;

        var map = new System.Collections.Generic.Dictionary<string, AchievementRec>(StringComparer.OrdinalIgnoreCase);
        try
        {
            var json = _dataManager._allAchievementsJson;
            if (json == null) { Plugin.Dbg("[GameData] _allAchievementsJson is null"); return; }
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
                    map[prop.Name] = ReadAchievementRec(rec);
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] achievements JSON: " + ex.Message);
        }

        _achievements = map;
        int withRewards = 0;
        foreach (var kv in map)
            if (kv.Value.Rewards != null && HasAnyReward(kv.Value.Rewards)) withRewards++;
        Plugin.Dbg($"[GameData] achievements: {map.Count} entries, {withRewards} with rewards");
    }

    private static bool HasAnyReward(RewardIds r)
    {
        return !IsVoidValue(r.Character) || !IsVoidValue(r.Weapon) || !IsVoidValue(r.Relic)
            || !IsVoidValue(r.Arcana) || !IsVoidValue(r.PowerUp) || !IsVoidValue(r.Stage)
            || !IsVoidValue(r.Hyper) || r.Gold > 0
            || (r.CharacterList != null && r.CharacterList.Count > 0)
            || (r.WeaponList != null && r.WeaponList.Count > 0)
            || (r.Skins != null && r.Skins.Count > 0);
    }

    private static AchievementRec ReadAchievementRec(System.Text.Json.JsonElement rec)
    {
        var r = new AchievementRec
        {
            Rewards = new RewardIds
            {
                Character = JsonStr(rec, "characterToUnlock"),
                CharacterList = JsonStrList(rec, "charactersToUnlock"),
                Weapon = JsonStr(rec, "weaponToUnlock"),
                WeaponList = JsonStrList(rec, "weaponListToUnlock"),
                Relic = JsonStr(rec, "relicToUnlock"),
                Arcana = JsonStr(rec, "arcanaToUnlock"),
                PowerUp = JsonStr(rec, "powerUpToUnlock") ?? JsonStr(rec, "unlock"),
                Skins = JsonStrList(rec, "skinsToUnlock"),
                Stage = JsonStr(rec, "stageToUnlock"),
                Hyper = JsonStr(rec, "hyperToUnlock"),
                Gold = JsonInt(rec, "goldPrize"),
                CustomText = JsonStr(rec, "forcedUnlockTips"),
                CustomFrame = JsonStr(rec, "forcedFrameName"),
                CustomTexture = JsonStr(rec, "forcedTexture"),
            },
            Description = JsonStr(rec, "description"),
            Requires = new System.Collections.Generic.List<string>(),
        };

        void requires(string label, string field)
        {
            string v = JsonStr(rec, field);
            if (IsVoidValue(v)) return;
            r.Requires.Add(label + ": " + HumanizeId(v));
        }
        requires("Character", "requiresChar");
        requires("Item", "requiresItem");
        requires("Stage", "requiresStage");
        requires("Weapon", "requiresWeapon");

        return r;
    }

    private static bool _dumpedAchievementJson;

    /// <summary>
    /// One-time dump of an achievement's raw JSON, to confirm the field names rather than
    /// assume they match the typed record's. Assuming that on the Bestiary cost several builds.
    /// </summary>
    public static void DumpAchievementJsonOnce(string key)
    {
        if (_dumpedAchievementJson || _dataManager == null || !Plugin.DebugVerbose
            || string.IsNullOrEmpty(key)) return;
        try
        {
            var json = _dataManager._allAchievementsJson;
            if (json == null) { _dumpedAchievementJson = true; return; }
            string raw = json.ToString();
            if (string.IsNullOrEmpty(raw)) { _dumpedAchievementJson = true; return; }
            int at = raw.IndexOf("\"" + key + "\"", StringComparison.OrdinalIgnoreCase);
            if (at < 0) return;
            _dumpedAchievementJson = true;
            Plugin.Dbg($"[GameData] achievement JSON '{key}': "
                + raw.Substring(at, Math.Min(700, raw.Length - at)).Replace('\n', ' '));
        }
        catch (Exception ex) { Plugin.Dbg("[GameData] achievement dump: " + ex.Message); }
    }

    /// <summary>Rows describing what an achievement grants, and what it needs.</summary>
    public static System.Collections.Generic.List<IconRow> GetAchievementRows(string id, out string description)
    {
        description = null;
        var rows = new System.Collections.Generic.List<IconRow>();
        EnsureAchievements();
        if (_achievements == null || string.IsNullOrEmpty(id)) return rows;
        if (!_achievements.TryGetValue(id, out AchievementRec rec) || rec == null) return rows;

        if (!string.IsNullOrWhiteSpace(rec.Description))
        {
            string t = LocalizeDisplayText(rec.Description) ?? rec.Description;
            if (!string.IsNullOrWhiteSpace(t) && !LooksLikeLocKey(t)) description = t.Trim();
        }

        var rewards = BuildRewardRows(rec.Rewards);
        if (rewards.Count > 0)
        {
            rows.Add(IconRow.Header("Unlocks:"));
            rows.AddRange(rewards);
        }

        if (rec.Requires != null && rec.Requires.Count > 0)
        {
            rows.Add(IconRow.Header("Requires:"));
            foreach (string s in rec.Requires) rows.Add(new IconRow(null, s));
        }

        return rows;
    }

    // ── Character records (for secret reward rows) ───────────────────────────
    //
    // Only the portrait frame/texture *names* are cached, not resolved sprites: SpriteManager
    // atlases are torn down between scenes, so a cached Sprite would go stale.
    //
    // The name parts matter as much as the portrait. Several characters were renamed after
    // their enum id was fixed - GRAZIELLA ships as "Minnah Mannarah" - so a single
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
    /// disagree - a renamed character - both are shown, because either may be the name the
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
    /// render (label only) - characters and stages often have no icon we can reach.
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
    /// null nullable - so HasValue is true for fields that award nothing. Without this every
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
                return null; // not cached - retry once the manager exists
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
                // CUSTOM_MERCHANT, TP_MERCHANT_LIBRARIAN) - DLC merchants such as Xanthia have
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
    /// catalog lookup already failed, and its result is cached - a per-frame
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
            // attributed the custom merchant's stock to him - wrong wares look identical to
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
        // Multi-line with real prose - not a single term (handled by LocalizeMultiline)
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

            // Anything already in memory, whatever atlas holds it. SpriteManager answers for
            // the atlases it knows the names of; DLC content is demonstrably not among them -
            // the Bestiary draws "kappa_i01" on screen while every lookup above returns null.
            s = FindLoadedSprite(frameName);
            if (s != null) return s;
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
    /// Bumped whenever the set of loaded sprites changes. Callers cache their misses against
    /// it, so "there is no icon for this" stays cheap without becoming permanent.
    /// </summary>
    public static int SpriteGeneration { get; private set; }

    public static void BumpSpriteGeneration()
    {
        SpriteGeneration++;
    }

    /// <summary>
    /// Atlases we have asked the game to load, so each is only requested once.
    ///
    /// DLC art is loaded on demand - selecting a Bestiary enemy is what pulls its atlas in - so
    /// an enemy that has only ever been hovered has no sprite anywhere in memory to find. This
    /// asks for that one atlas rather than preloading everything the page could possibly show.
    /// </summary>
    private static readonly System.Collections.Generic.HashSet<string> _texturesRequested =
        new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Ask for the atlas holding an enemy's art. The load is asynchronous and nothing waits on
    /// it: the caller has already failed to find a sprite this time round, and the point is for
    /// the next hover to succeed.
    /// </summary>
    public static void RequestEnemyTexture(string enemyId)
    {
        EnsureEnemyRecs();
        if (_enemyRecs == null || string.IsNullOrEmpty(enemyId)) return;
        if (!_enemyRecs.TryGetValue(enemyId, out EnemyRec r) || r == null) return;
        if (string.IsNullOrEmpty(r.Texture)) return;
        if (!_texturesRequested.Add(r.Texture)) return;

        try
        {
            var dlc = DlcFor(enemyId);
            string tex = r.Texture;
            // A real delegate rather than null: an unguarded callback inside the loader would
            // fault in IL2CPP, where a catch here cannot help.
            var done = Il2CppInterop.Runtime.DelegateSupport.ConvertDelegate<Il2CppSystem.Action<bool>>(
                (Action<bool>)(ok =>
                {
                    // The index predates the atlas, so it has to be rebuilt before the next look.
                    _spriteIndexBuiltAt = -999f;
                    BumpSpriteGeneration();
                    Plugin.Dbg($"[GameData] atlas '{tex}' load complete: {ok}");
                }));
            VampireSurvivors.Framework.Loading.SpriteLoader.LoadTextureAsync(
                tex, SpriteCacheGroup, dlc, done);
            Plugin.Dbg($"[GameData] requested atlas '{tex}' for {enemyId}"
                + (dlc.HasValue ? $" (dlc {dlc.Value})" : ""));
        }
        catch (Exception ex)
        {
            Log.LogWarning($"[GameData.RequestEnemyTexture] {r.Texture}: " + ex.Message);
        }
    }

    /// <summary>
    /// Our own cache group, not one of the game's. Loads land in a bucket the game does not
    /// manage, so its own unloading cannot be surprised by an atlas it never asked for.
    /// </summary>
    private const string SpriteCacheGroup = "VSEvolutionHelper";

    /// <summary>
    /// Which DLC an enemy belongs to, by the prefix on its id. The loader needs this to find
    /// the atlas at all; base game enemies pass no value.
    /// </summary>
    private static Il2CppSystem.Nullable<DlcType> DlcFor(string enemyId)
    {
        var none = new Il2CppSystem.Nullable<DlcType>();
        if (string.IsNullOrEmpty(enemyId)) return none;
        string id = enemyId.ToUpperInvariant();

        DlcType? t = null;
        if (id.StartsWith("MS_") || id.Contains("_MS_")) t = DlcType.Moonspell;
        else if (id.StartsWith("FS_") || id.Contains("_FS_")) t = DlcType.Foscari;
        else if (id.StartsWith("CHAL_") || id.Contains("_CHAL_")) t = DlcType.Chalcedony;
        else if (id.StartsWith("FB_") || id.Contains("_FB_")) t = DlcType.FirstBlood;
        else if (id.StartsWith("EME_") || id.Contains("_EME_")) t = DlcType.Emeralds;
        else if (id.StartsWith("TP_") || id.Contains("_TP_")) t = DlcType.ThosePeople;
        else if (id.StartsWith("LEM_") || id.Contains("_LEM_")) t = DlcType.Lemon;

        return t.HasValue ? new Il2CppSystem.Nullable<DlcType>(t.Value) : none;
    }

    /// <summary>
    /// Every sprite currently in memory, by name. Case-insensitive, because the same art ships
    /// under both "AGaea_i01" and "agaea_i01".
    /// </summary>
    private static System.Collections.Generic.Dictionary<string, Sprite> _spriteIndex;
    private static float _spriteIndexBuiltAt = -999f;

    /// <summary>
    /// A loaded sprite by name, regardless of which atlas holds it.
    ///
    /// Atlases load as pages need them, so a miss is not final: the index is rebuilt on a miss,
    /// rate limited, because the sprite may simply not have existed when it was last built.
    /// </summary>
    public static Sprite FindLoadedSprite(string name)
    {
        if (string.IsNullOrEmpty(name)) return null;
        string bare = name;
        int dot = bare.LastIndexOf('.');
        if (dot > 0) bare = bare.Substring(0, dot);

        Sprite hit = LookupIndexed(name) ?? LookupIndexed(bare);
        if ((Object)(object)hit != (Object)null) return hit;

        if (Time.realtimeSinceStartup - _spriteIndexBuiltAt < 10f) return null;
        BuildSpriteIndex();
        return LookupIndexed(name) ?? LookupIndexed(bare);
    }

    private static Sprite LookupIndexed(string key)
    {
        if (_spriteIndex == null || string.IsNullOrEmpty(key)) return null;
        if (!_spriteIndex.TryGetValue(key, out Sprite s)) return null;
        // Destroyed sprites linger as keys; the null check is the Unity one, not a reference one.
        return (Object)(object)s != (Object)null ? s : null;
    }

    private static void BuildSpriteIndex()
    {
        _spriteIndexBuiltAt = Time.realtimeSinceStartup;
        try
        {
            var all = Resources.FindObjectsOfTypeAll<Sprite>();
            if (all == null) return;
            var map = new System.Collections.Generic.Dictionary<string, Sprite>(
                StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < all.Count; i++)
            {
                var s = all[i];
                if ((Object)(object)s == (Object)null) continue;
                string n = ((Object)s).name;
                if (string.IsNullOrEmpty(n) || map.ContainsKey(n)) continue;
                map[n] = s;
            }
            _spriteIndex = map;
            Plugin.Dbg($"[GameData] sprite index: {map.Count} names from {all.Count} sprites");
        }
        catch (Exception ex)
        {
            Log.LogWarning("[GameData.BuildSpriteIndex] " + ex.Message);
        }
    }

    /// <summary>
    /// A weapon's evolutions and unions as icon rows, for the shared docked popup.
    ///
    /// <see cref="BuildEvoRowsFor"/> already answers both questions - Path A follows the weapon's
    /// own <c>evoInto</c>, Path B scans every recipe listing it in <c>evolvesFrom</c> or
    /// <c>requires</c>, which is what a union is - so this only reshapes the answer. Each result
    /// reads as the evolved weapon followed by what it needs alongside this one:
    ///
    /// <code>
    /// [icon] Bloody Tear
    ///   + Hollow Heart (max level)
    /// </code>
    ///
    /// Deliberately no arcana rows. This tooltip is shown from inside the arcana screen, where
    /// which arcana is in play is the one thing already on screen.
    /// </summary>
    public static System.Collections.Generic.List<IconRow> GetWeaponEvoIconRows(WeaponType weapon)
    {
        var rows = new System.Collections.Generic.List<IconRow>();
        try
        {
            var evos = BuildEvoRowsFor(weapon);
            if (evos == null) return rows;
            foreach (var e in evos)
            {
                if (e == null) continue;
                string name = string.IsNullOrEmpty(e.EvolvedName)
                    ? HumanizeEnum(e.Evolved.ToString())
                    : e.EvolvedName;
                rows.Add(new IconRow(e.EvolvedSprite, name));
                if (e.Passives == null) continue;
                foreach (var p in e.Passives)
                {
                    if (p == null) continue;
                    string pn = string.IsNullOrEmpty(p.Name) ? HumanizeEnum(p.Type.ToString()) : p.Name;
                    rows.Add(new IconRow(p.Sprite, "+ " + pn + (p.RequiresMax ? " (max level)" : "")));
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] GetWeaponEvoIconRows: " + ex.Message);
        }
        return rows;
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
            // PowerUpType and ItemType share some numeric values but different enums - try name match as item first failed
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

            // alwaysHidden is not the noise flag it was taken for.
            //
            // It is set on the Darkanas the player has not unlocked, and skipping them dropped
            // exactly those nine from the name table, the description table and the reverse weapon
            // index alike - which is why they hovered with a humanized enum for a title and nothing
            // else, while every arcana beside them resolved fully.
            //
            // Whether their records actually carry weapons and text is what this reports; the entry
            // is indexed either way, so a record that holds something now shows it.
            try
            {
                if (data.alwaysHidden && Plugin.DebugVerbose)
                {
                    int nWeapons = 0, nItems = 0;
                    try { if (data.weapons != null) nWeapons = data.weapons.Count; } catch { }
                    try { if (data.items != null) nItems = data.items.Count; } catch { }
                    Plugin.Dbg($"[GameData] hidden arcana {type}: weapons={nWeapons} items={nItems} "
                        + $"name='{ResolveArcanaName(data, type)}' "
                        + $"desc={(ResolveArcanaDescription(data, type) ?? string.Empty).Length}ch");
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
        _arcanaSourceCount = SourceArcanaCount();
        if (_arcanaBuilt && !_loggedArcana)
        {
            _loggedArcana = true;
            Log.LogInfo($"[GameData] Arcanas: {indexed} entries, {WeaponToArcanas.Count} weapon links, {ItemToArcanas.Count} item links");
        }
    }

    /// <summary>How many arcanas the index was built from, to notice the game adding more.</summary>
    private static int _arcanaSourceCount = -1;

    private static int SourceArcanaCount()
    {
        try
        {
            if (_dataManager == null) return -1;
            var all = _dataManager.AllArcanas;
            return all == null ? -1 : all.Count;
        }
        catch { return -1; }
    }

    /// <summary>
    /// Rebuild the arcana index if the game's own table has grown since we read it.
    ///
    /// The index is built once and cached forever, which is right only if the table is complete
    /// when we first see it. Sprites already taught this lesson - an atlas that has not finished
    /// loading answers with a miss that is then cached as though it were an answer - and the same
    /// shape is possible here if the game merges content in later.
    ///
    /// Cheap enough to ask on a miss: it compares two counts and does nothing unless they differ.
    /// </summary>
    public static bool RefreshArcanasIfGrown()
    {
        try
        {
            EnsureLoaded();
            int now = SourceArcanaCount();
            if (now < 0 || now == _arcanaSourceCount) return false;

            Plugin.Dbg($"[GameData] arcana table grew {_arcanaSourceCount} -> {now}, rebuilding index");
            ArcanaNames.Clear();
            ArcanaDescriptions.Clear();
            BuildArcanaCaches();
            return true;
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] refresh arcanas: " + ex.Message);
            return false;
        }
    }

    /// <summary>Arcanas already reported as empty, so a hover does not repeat itself.</summary>
    private static readonly System.Collections.Generic.HashSet<ArcanaType> ReportedEmpty =
        new System.Collections.Generic.HashSet<ArcanaType>();

    /// <summary>
    /// Say what an arcana's record actually holds when it resolves to nothing.
    ///
    /// "No rows" has two very different causes: a record the game never filled in, and a record
    /// we failed to read. They are indistinguishable from the tooltip, and only one of them is a
    /// bug on this side.
    /// </summary>
    private static void ReportEmptyArcana(ArcanaType type)
    {
        if (!Plugin.DebugVerbose) return;
        if (!ReportedEmpty.Add(type)) return;
        try
        {
            ArcanaData data = GetArcanaData(type);
            if (data == null)
            {
                Plugin.Dbg($"[GameData] empty arcana {type}: no record in AllArcanas "
                    + $"(table holds {SourceArcanaCount()})");
                return;
            }
            int nWeapons = -1, nItems = -1;
            try { nWeapons = data.weapons == null ? -1 : data.weapons.Count; } catch { }
            try { nItems = data.items == null ? -1 : data.items.Count; } catch { }
            bool hidden = false;
            try { hidden = data.alwaysHidden; } catch { }
            Plugin.Dbg($"[GameData] empty arcana {type}: record present, weapons={nWeapons} "
                + $"items={nItems} hidden={hidden} name='{ResolveArcanaName(data, type)}' "
                + $"desc={(ResolveArcanaDescription(data, type) ?? string.Empty).Length}ch");
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] empty arcana " + type + ": " + ex.Message);
        }
    }

    /// <summary>
    /// Does the game actually ship a record for this arcana?
    ///
    /// The <c>ArcanaType</c> enum runs ahead of the data table - 1.15 declares ten Darkanas it
    /// holds no record for, one of them still named <c>D07_tbd_bouncy</c>. They are a later
    /// version's content, and nothing about them can be shown because nothing about them exists.
    /// </summary>
    public static bool HasArcanaRecord(ArcanaType type)
    {
        try { return GetArcanaData(type) != null; }
        catch { return false; }
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

    /// <summary>
    /// Description text scraped off the game's own arcana info panel, keyed by arcana.
    ///
    /// The data lookup finds nothing for most of the B and A groups - the term still carries its
    /// placeholder and the record holds a single full stop - yet the game plainly renders text
    /// for them. Whatever it composes that from, the rendered string is the truth, so it is taken
    /// from the panel as the player browses. Same rule as the Bestiary row labels and the Music
    /// titles: prefer what the game drew.
    /// </summary>
    private static readonly System.Collections.Generic.Dictionary<ArcanaType, string> ArcanaPanelText =
        new System.Collections.Generic.Dictionary<ArcanaType, string>();

    /// <summary>Remember what the info panel rendered for this arcana.</summary>
    public static void CaptureArcanaDescription(ArcanaType type, string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        if (LooksLikeLocKey(text)) return;
        if (IsPlaceholderText(text)) return;
        ArcanaPanelText[type] = text.Trim();
        if (Plugin.DebugVerbose)
            Plugin.Dbg($"[GameData] arcana panel text {type}: {text.Trim().Length}ch");
    }

    public static string GetArcanaDescription(ArcanaType type)
    {
        EnsureLoaded();
        // The panel wins over the data lookup, not the other way round: where both answer they
        // agree, and where they differ it is because the data has nothing.
        if (ArcanaPanelText.TryGetValue(type, out string drawn) && !string.IsNullOrEmpty(drawn))
        {
            return drawn;
        }
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
        // T21_BLOODY -> Bloody, D01_SAPPHIRE_MIST -> Sapphire Mist, B004_GENNARO -> Gennaro,
        // A011_CRACKEDMIRROR -> Crackedmirror.
        //
        // The strip used to name T and D explicitly, which was right when they were the only two
        // groups. 1.16 has five - T, D, B (character arcanas), A (adventure arcanas) and SUB -
        // and the unnamed ones fell through to the raw id, so a card with no localized name read
        // "B004 GENNARO". Any letter-then-digits prefix is an id, whatever the group is called.
        return HumanizeEnum(StripArcanaGroupPrefix(type.ToString()));
    }

    /// <summary>
    /// Drop the group-and-id prefix from an arcana enum name: <c>B004_GENNARO</c> -> <c>GENNARO</c>.
    /// Anything that is not letters followed by digits followed by an underscore is left alone.
    /// </summary>
    private static string StripArcanaGroupPrefix(string raw)
    {
        if (string.IsNullOrEmpty(raw)) return raw;
        int i = 0;
        while (i < raw.Length && char.IsLetter(raw[i])) i++;
        if (i == 0) return raw;
        int digits = i;
        while (digits < raw.Length && char.IsDigit(raw[digits])) digits++;
        if (digits == i) return raw;
        if (digits >= raw.Length || raw[digits] != '_') return raw;
        string rest = raw.Substring(digits + 1);
        return string.IsNullOrEmpty(rest) ? raw : rest;
    }

    private static string ResolveArcanaDescription(ArcanaData data, ArcanaType type)
    {
        if (data != null)
        {
            string term = null, rawDesc = null;
            try { term = data.GetLocalizedDescriptionTerm(type); } catch { }
            try { rawDesc = data.description; } catch { }

            // The term the game hands back for the B and A groups still has its placeholder in
            // it - arcanaLang/{201}description - so the lookup cannot hit. Whether the braces are
            // literal in the string table or were meant to be substituted is not knowable from
            // here, so both readings are tried. Costs one dictionary miss when it is wrong.
            foreach (string candidate in TermVariants(term))
            {
                try
                {
                    string t = LocalizeDisplayText(candidate);
                    if (!IsPlaceholderText(t))
                    {
                        if (Plugin.DebugVerbose && candidate != term)
                            Plugin.Dbg($"[GameData] arcana desc {type} via variant '{candidate}'");
                        return t;
                    }
                }
                catch
                {
                }
            }
            try
            {
                string t = LocalizeDisplayText(rawDesc);
                if (!IsPlaceholderText(t)) return t;
            }
            catch
            {
            }

            // 1.16's character (B) and adventure (A) arcanas answer both lookups with a single
            // placeholder character rather than nothing, so an emptiness check passed it straight
            // through and the tooltip rendered a title above one glyph. Log what the game
            // actually returned, so a missing description is distinguishable from a broken term.
            if (Plugin.DebugVerbose)
            {
                string prefix = null;
                try { prefix = data.GetLocalPrefix(type); } catch { }
                Plugin.Dbg($"[GameData] arcana desc miss {type}: term='{term}' raw='{rawDesc}' "
                    + $"prefix='{prefix}'");
            }
        }
        return "";
    }

    /// <summary>
    /// The term as given, then the same with its braces removed and with a space where the
    /// placeholder ended. Duplicates and empties are skipped.
    /// </summary>
    private static System.Collections.Generic.IEnumerable<string> TermVariants(string term)
    {
        if (string.IsNullOrEmpty(term)) yield break;
        yield return term;
        if (term.IndexOf('{') < 0) yield break;

        string bare = term.Replace("{", "").Replace("}", "");
        if (bare != term) yield return bare;

        string spaced = term.Replace("{", "").Replace("}", " ");
        if (spaced != term && spaced != bare) yield return spaced;
    }

    /// <summary>
    /// Is this text too short to be a description? A single glyph is a placeholder, not a
    /// sentence - the game uses one where it has nothing to say, and an IsNullOrEmpty check
    /// treats it as real.
    /// </summary>
    private static bool IsPlaceholderText(string s)
    {
        if (string.IsNullOrWhiteSpace(s)) return true;
        return s.Trim().Length < 2;
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

    /// <summary>
    /// What an arcana touches, as tooltip rows: every weapon it names, then every passive.
    ///
    /// The same two lookups the Collections arcana popup already runs, shaped for the docked
    /// panel. An arcana that names nothing - the several that only change a global rule - gets
    /// no rows at all, and its description carries the tooltip on its own.
    /// </summary>
    public static System.Collections.Generic.List<IconRow> GetArcanaAffectRows(ArcanaType type)
    {
        var rows = new System.Collections.Generic.List<IconRow>();
        try
        {
            var weapons = GetWeaponsAffectedByArcana(type);
            var items = GetItemsAffectedByArcana(type);
            if ((weapons == null || weapons.Count == 0) && (items == null || items.Count == 0))
            {
                ReportEmptyArcana(type);
                return AddArcanaCharacterRow(rows, type);
            }

            if (weapons != null)
            {
                foreach (WeaponType w in weapons)
                {
                    string n = GetWeaponName(w);
                    rows.Add(new IconRow(GetSprite(w), string.IsNullOrEmpty(n) ? HumanizeEnum(w.ToString()) : n));
                }
            }
            if (items != null)
            {
                foreach (ItemType it in items)
                {
                    string n = GetItemName(it);
                    rows.Add(new IconRow(GetItemSprite(it), string.IsNullOrEmpty(n) ? HumanizeEnum(it.ToString()) : n));
                }
            }
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] GetArcanaAffectRows: " + ex.Message);
        }
        return rows;
    }

    /// <summary>
    /// Name the character a B-group arcana belongs to.
    ///
    /// The B group is one arcana per character - <c>B004_GENNARO</c>, <c>B012_CHRISTINE</c> - and
    /// they list no weapons, so the Affects lookup finds nothing and 1.16 ships no description
    /// text for them either. Whose arcana it is is then the only thing left worth saying, and it
    /// is a real answer rather than a blank panel: the id already carries it.
    ///
    /// Anything that does not resolve to a character is left alone, so the A group and any future
    /// prefix simply get no row.
    /// </summary>
    private static System.Collections.Generic.List<IconRow> AddArcanaCharacterRow(
        System.Collections.Generic.List<IconRow> rows, ArcanaType type)
    {
        try
        {
            string raw = type.ToString();
            if (string.IsNullOrEmpty(raw) || raw[0] != 'B') return rows;

            string id = StripArcanaGroupPrefix(raw);
            if (string.IsNullOrEmpty(id) || id == raw) return rows;

            string label = DescribeRewardCharacter(id);
            if (string.IsNullOrWhiteSpace(label) || LooksLikeLocKey(label)) return rows;

            rows.Add(new IconRow(GetCharacterPortrait(id), label));
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] arcana character row: " + ex.Message);
        }
        return rows;
    }

    /// <summary>
    /// A music track's composer and where it came from, plus how it is unlocked.
    ///
    /// The page itself shows a title and nothing else. <c>MusicData</c> carries the author and
    /// source the game credits nowhere in the UI, and the three unlock fields answer the only
    /// question a greyed-out row provokes. Those three are nullable enums, so each is checked
    /// for a value rather than compared against a sentinel.
    /// </summary>
    public static System.Collections.Generic.List<IconRow> GetMusicRows(
        VampireSurvivors.Data.MusicData data, out string description)
    {
        description = null;
        var rows = new System.Collections.Generic.List<IconRow>();
        if (data == null) return rows;

        try
        {
            var lines = new System.Collections.Generic.List<string>();
            string author = null, source = null;
            try { author = data.author; } catch { }
            try { source = data.source; } catch { }
            if (!string.IsNullOrWhiteSpace(author) && !LooksLikeLocKey(author))
                lines.Add("Composed by " + author.Trim());
            if (!string.IsNullOrWhiteSpace(source) && !LooksLikeLocKey(source))
                lines.Add("From " + source.Trim());
            if (lines.Count > 0) description = string.Join("\n", lines.ToArray());

            bool unlocked = true;
            try { unlocked = data.isUnlocked; } catch { }

            try
            {
                var stage = data.unlockedByStage;
                if (stage.HasValue && !IsVoidValue(stage.Value.ToString()))
                    rows.Add(new IconRow(null, DescribeStage(stage.Value.ToString()) + " (stage)"));
            }
            catch { }
            try
            {
                var chr = data.unlockedByCharacter;
                if (chr.HasValue && !IsVoidValue(chr.Value.ToString()))
                {
                    string id = chr.Value.ToString();
                    rows.Add(new IconRow(GetCharacterPortrait(id), DescribeRewardCharacter(id)));
                }
            }
            catch { }
            try
            {
                var item = data.unlockedByItem;
                if (item.HasValue && !IsVoidValue(item.Value.ToString()))
                {
                    ItemType it = item.Value;
                    string n = GetItemName(it);
                    rows.Add(new IconRow(GetItemSprite(it), string.IsNullOrEmpty(n) ? HumanizeEnum(it.ToString()) : n));
                }
            }
            catch { }

            // A track with no unlock field and no credits would otherwise show an empty panel.
            if (rows.Count == 0 && string.IsNullOrEmpty(description) && !unlocked)
                description = "Locked - keep playing to unlock.";
        }
        catch (Exception ex)
        {
            Plugin.Dbg("[GameData] GetMusicRows: " + ex.Message);
        }
        return rows;
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
