namespace IdleGame.Core.Models.Items;

/// <summary>
/// Item rarity levels - rarity doesn't directly affect item strength
/// </summary>
public enum ItemRarity
{
    /// <summary>
    /// Normal items - can be used as base for crafting Artifacts
    /// </summary>
    Normal = 0,

    /// <summary>
    /// Magic items - one prefix and one suffix with enhanced affix effects
    /// </summary>
    Magic = 1,

    /// <summary>
    /// Rare items - up to 3 prefixes and 3 suffixes
    /// </summary>
    Rare = 2,

    /// <summary>
    /// Legendary items - 2 prefixes, 2 suffixes, fixed normal affixes, plus one legendary affix
    /// </summary>
    Legendary = 3,

    /// <summary>
    /// Unique items - fixed properties, no random affixes
    /// </summary>
    Unique = 4,

    /// <summary>
    /// Artifact items - crafted from Normal items using currency, has fixed and random affixes
    /// </summary>
    Artifact = 5
}
