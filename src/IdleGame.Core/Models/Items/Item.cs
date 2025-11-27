using IdleGame.Core.Models.Affixes;

namespace IdleGame.Core.Models.Items;

/// <summary>
/// An actual item instance in the game
/// </summary>
public class Item
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string BaseId { get; set; } = string.Empty;
    public ItemBaseType BaseType { get; set; }
    public EquipmentSlot Slot { get; set; }
    public ItemRarity Rarity { get; set; }
    public int ItemLevel { get; set; }
    public int RequiredLevel { get; set; }

    // Base stats
    public int BaseArmor { get; set; }
    public int BaseEvasion { get; set; }
    public int BaseEnergyShield { get; set; }
    public int BaseMinDamage { get; set; }
    public int BaseMaxDamage { get; set; }
    public double BaseAttackSpeed { get; set; }
    public double BaseCriticalChance { get; set; }

    // Affixes
    public List<ItemAffix> Prefixes { get; set; } = new();
    public List<ItemAffix> Suffixes { get; set; } = new();
    public List<ItemAffix> ImplicitAffixes { get; set; } = new();
    public ItemAffix? LegendaryAffix { get; set; }
    public List<ItemAffix> FixedAffixes { get; set; } = new();

    /// <summary>
    /// For Unique items - the unique identifier
    /// </summary>
    public string? UniqueId { get; set; }

    /// <summary>
    /// Get all affixes on this item
    /// </summary>
    public IEnumerable<ItemAffix> GetAllAffixes()
    {
        foreach (var affix in ImplicitAffixes) yield return affix;
        foreach (var affix in FixedAffixes) yield return affix;
        foreach (var affix in Prefixes) yield return affix;
        foreach (var affix in Suffixes) yield return affix;
        if (LegendaryAffix != null) yield return LegendaryAffix;
    }

    /// <summary>
    /// Get all stat modifiers from this item
    /// </summary>
    public IEnumerable<StatModifier> GetAllModifiers()
    {
        return GetAllAffixes().SelectMany(a => a.GetEffectiveModifiers());
    }

    /// <summary>
    /// Get the maximum number of prefixes based on rarity
    /// </summary>
    public int GetMaxPrefixes()
    {
        return Rarity switch
        {
            ItemRarity.Normal => 0,
            ItemRarity.Magic => 1,
            ItemRarity.Rare => 3,
            ItemRarity.Legendary => 2,
            ItemRarity.Unique => 0,
            ItemRarity.Artifact => 3,
            _ => 0
        };
    }

    /// <summary>
    /// Get the maximum number of suffixes based on rarity
    /// </summary>
    public int GetMaxSuffixes()
    {
        return Rarity switch
        {
            ItemRarity.Normal => 0,
            ItemRarity.Magic => 1,
            ItemRarity.Rare => 3,
            ItemRarity.Legendary => 2,
            ItemRarity.Unique => 0,
            ItemRarity.Artifact => 3,
            _ => 0
        };
    }

    /// <summary>
    /// Check if item can have more prefixes
    /// </summary>
    public bool CanAddPrefix() => Prefixes.Count < GetMaxPrefixes();

    /// <summary>
    /// Check if item can have more suffixes
    /// </summary>
    public bool CanAddSuffix() => Suffixes.Count < GetMaxSuffixes();

    /// <summary>
    /// Get affix effect multiplier based on rarity
    /// </summary>
    public double GetAffixEffectMultiplier()
    {
        return Rarity switch
        {
            ItemRarity.Magic => 1.5, // Magic items have enhanced affix effects
            _ => 1.0
        };
    }

    public override string ToString()
    {
        return $"[{Rarity}] {Name} (iLvl {ItemLevel})";
    }
}
