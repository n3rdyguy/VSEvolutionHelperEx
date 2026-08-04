using System.Collections.Generic;
using VampireSurvivors.Data;

namespace VSItemTooltips;

/// <summary>
/// Curated progression / gameplay tips for well-known stages.
/// Shown under game-localized tips in the Stage Guide panel (not official copy).
/// </summary>
public static class StageExtraTips
{
	private static readonly Dictionary<StageType, string> Tips = new Dictionary<StageType, string>
	{
		// Base game
		[StageType.FOREST] =
			"Mad Forest — early main path. Survive the coffin encounter and clear the map for Green Acres / progression unlocks. Watch for the relic and the merchant.",
		[StageType.LIBRARY] =
			"Inlaid Library — tight corridors, strong for spell/projectiles. Spellbound / library-linked unlocks and relics often sit along the shelves; explore both ends of the hall.",
		// SINKING is Moongolow in current builds (sunken moon city), not Dairy Plant
		[StageType.SINKING] =
			"Moongolow — secret sunken city under the moon. Usually unlocked via the inverse/coffin path. Short map; grab the Glass Vizard and any stage disks, then leave or linger for pickups.",
		[StageType.CASTLE] =
			"Dairy Plant / castle line — open lanes and heavy pressure. Hyper mode and plant-related unlocks reward long survival; keep moving for chest spawns.",
		[StageType.ENTRANCE] =
			"Cappella Magna / entrance line — later main path. Bring strong evolution setups; boss waves hit hard.",
		[StageType.GREENACRES] =
			"Green Acres — special “random events” stage (the ?). Great for farming eggs/gold and chaotic modifiers. Progress here is optional for many unlocks.",
		[StageType.MOLISE] =
			"Il Molise — bonus stage, low combat. Walk around, collect pickups, and leave when ready. Useful for safe farming and some bonus unlocks.",
		[StageType.BONEZONE] =
			"The Bone Zone — no classic level-ups; gold and pickups rule. Build from floor loot and treasure. Challenge stages mark hard progression.",
		[StageType.WAREHOUSE] =
			"Cursed skull / warehouse line — dense spawns. Use walls and evolutions with good clear. Check for stage relics before ending.",
		[StageType.TOWER] =
			"Gallo Tower — climb carefully; verticality and bridges. Hyper unlock and tower relics are the main goals.",
		[StageType.CHAPEL] =
			"Chapel / sacred grounds — tight space, dense enemies. Strong AOE and knockback help. Look for coffin and relic spawns.",
		[StageType.STAGEX] =
			"Boss Rash / Stage X style — condensed boss pressure. Bring maxed evolutions and defensive passives.",
		[StageType.RASH] =
			"Boss rush pressure — short, intense. Prioritize damage and revives over slow farm builds.",
		[StageType.MACHINE] =
			"Eudaimonia Machine / machine stages — meta progression and special encounters. Read on-screen prompts; not a normal farm map.",
		[StageType.MOONSPELL] =
			"Mt. Moonspell (DLC) — snow, gates, and multi-path layout. Map tokens and moonspell relics matter; explore gates when unlocked.",
		[StageType.BATCOUNTRY] =
			"Bat Country — dense airborne packs. Area and projectile count shine. Watch for unique stage items.",
		[StageType.ASTRALSTAIR] =
			"Astral Stair — vertical climb with limited ground. Mobility and upward clear help. Collect stage relics on landings.",
		[StageType.FOSCARI] =
			"Lake Foscari (DLC) — swamp paths and multi-biome progression. Follow map tokens; foscari relics unlock further content.",
		[StageType.FOSCARI2] =
			"Abyss Foscari — harder foscari path. Expect denser elite pressure; bring evolved kits and map awareness.",
		[StageType.WHITEOUT] =
			"Whiteout / snow challenge — limited visibility themes and harsh modifiers. Defensive builds and vacuum help.",
		[StageType.POLUS] =
			"Space / Polus line — sci-fi map layout. Use the minimap for tokens and objectives.",
		[StageType.FB_GALUGA] =
			"Neo Galuga (Operation Guns) — run-and-gun density. Movement and fire rate matter more than pure tanking.",
		[StageType.FB_HIGHWAY] =
			"Highway stages — lateral pressure and vehicles themes. Keep horizontal clear strong.",
		[StageType.EMERALD] =
			"Emerald Diorama content — puzzle/adventure structure. Follow stage goals and merchants; not pure arena farming.",
		[StageType.EX_WESTWOODS] =
			"Ode to Castlevania woods — large map with secrets. Explore thoroughly for relics and progression items.",
		[StageType.EX_MAZERELLA] =
			"Maze-like Castlevania stage — mapping and backtracking help. Mark relic icons on the pause map.",
		[StageType.EX_LYCAEUM] =
			"Lycaeum — library/academia vibes with dense elites. AOE evolutions recommended.",
		[StageType.TP_CHAPEL] =
			"Castlevania chapel variant — tight sacred combat. Watch for unique coffins and stage items.",
		[StageType.TP_CASTLE] =
			"Castlevania castle — multi-route exploration. Use the map heavily for tokens, relics, and bosses.",
		[StageType.BLACK] =
			"Inverse / dark variants — inverted rules and tougher packing. Treat as challenge progression.",
		[StageType.WHITE] =
			"Light / special variant stages — read modifiers on the left panel before starting.",
		[StageType.TOWERBRIDGE] =
			"Bridge / tower connections — choke points. Control lanes and don’t get surrounded mid-bridge.",
		[StageType.LABORRATORY] =
			"Laboratory — science/horror packing. Expect elite experiments and tight rooms.",
		[StageType.COOP] =
			"Co-op focused layout — leave space for allies if multiplayer; still fully playable solo.",
		[StageType.CARLOCART] =
			"Carlo Cart / ride stages — movement is constrained. Time attacks and positioning matter more than free roam.",
		[StageType.MACHINE2] =
			"Machine stage variant — special encounter rules. Read modifiers carefully; meta progression is the real reward.",
		[StageType.SPAZIE] =
			"Space / Spazie layout — open field with sci-fi pressure. Mobility and vacuum help with scattered pickups.",
		[StageType.DEVILROOM] =
			"Devil Room style — high risk, high reward packing. Expect elite density and tight timing windows.",
		[StageType.LEGION_TEST] =
			"Legion test arena — scripted pressure for a featured boss/pack. Bring evolved damage, not farm builds.",
		[StageType.BEELZEBUB_TEST] =
			"Beelzebub test arena — boss-focused. Prioritize survivability and burst for phase transitions.",
		[StageType.DOPPLEGANGER_TEST] =
			"Doppelganger test — mirror/duel pressure. Clean positioning beats raw greed.",
		[StageType.TP_CASTLE_TEST] =
			"Castlevania castle test — condensed exploration combat. Map knowledge and AOE clear help.",
		[StageType.LEM_CHAMBER] =
			"Lemmings / chamber stage — constrained space. Control choke points and don’t over-extend.",
		// Adventure stage ids (StageType ADV_* used when adventures reuse stage select)
		[StageType.ADV_BAZAAR] =
			"Adventure bazaar — hub/shop between adventure legs. Stock up, then continue the chain.",
		[StageType.ADV_POE_1_Acres] =
			"Poe adventure (Acres) — early leg of a multi-stage adventure. Treat as a chapter, not a normal farm map.",
		[StageType.ADV_IME_1_Chapel] =
			"Imelda adventure (Chapel) — story/challenge chain. Follow adventure goals over free roaming.",
		[StageType.ADV_FB_001_Attack] =
			"First Blood adventure — run-and-gun chapter. Movement and fire rate over pure tanking.",
		[StageType.ADV_OTC_001_001_Woods] =
			"Ode to Castlevania adventure woods — large secret-heavy chapter. Explore thoroughly for progression.",
	};

	public static bool TryGet(StageType type, out string tip)
	{
		return Tips.TryGetValue(type, out tip) && !string.IsNullOrWhiteSpace(tip);
	}
}
