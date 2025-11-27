using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Models.Affixes;

/// <summary>
/// Definition of an affix that can be rolled on items
/// </summary>
public class AffixDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public AffixType AffixType { get; set; }

    /// <summary>
    /// Tier of the affix (higher = better rolls)
    /// </summary>
    public int Tier { get; set; }

    /// <summary>
    /// Minimum item level required to roll this affix
    /// </summary>
    public int RequiredItemLevel { get; set; }

    /// <summary>
    /// The stat modifiers this affix provides
    /// </summary>
    public List<StatModifierRange> ModifierRanges { get; set; } = new();

    /// <summary>
    /// Weight for random selection (higher = more common)
    /// </summary>
    public int Weight { get; set; } = 100;

    /// <summary>
    /// Item types this affix can roll on
    /// </summary>
    public List<ItemBaseType> ValidItemTypes { get; set; } = new();

    /// <summary>
    /// Tags that determine affix grouping (same group = mutually exclusive)
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// Whether this is a legendary-only affix
    /// </summary>
    public bool IsLegendary { get; set; }
}

/// <summary>
/// Range for a stat modifier roll
/// </summary>
public class StatModifierRange
{
    public StatType StatType { get; set; }
    public double MinFlatValue { get; set; }
    public double MaxFlatValue { get; set; }
    public double MinPercentValue { get; set; }
    public double MaxPercentValue { get; set; }
    public double MinMoreMultiplier { get; set; }
    public double MaxMoreMultiplier { get; set; }

    public StatModifier Roll(Random random)
    {
        return new StatModifier
        {
            StatType = StatType,
            FlatValue = RollValue(random, MinFlatValue, MaxFlatValue),
            PercentValue = RollValue(random, MinPercentValue, MaxPercentValue),
            MoreMultiplier = RollValue(random, MinMoreMultiplier, MaxMoreMultiplier)
        };
    }

    private static double RollValue(Random random, double min, double max)
    {
        if (min == max) return min;
        return min + random.NextDouble() * (max - min);
    }
}
