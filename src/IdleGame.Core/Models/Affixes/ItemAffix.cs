namespace IdleGame.Core.Models.Affixes;

/// <summary>
/// An actual affix instance on an item
/// </summary>
public class ItemAffix
{
    /// <summary>
    /// Reference to the definition
    /// </summary>
    public string DefinitionId { get; set; } = string.Empty;

    /// <summary>
    /// Display name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Type of affix
    /// </summary>
    public AffixType AffixType { get; set; }

    /// <summary>
    /// The rolled modifiers
    /// </summary>
    public List<StatModifier> Modifiers { get; set; } = new();

    /// <summary>
    /// Tier of the affix
    /// </summary>
    public int Tier { get; set; }

    /// <summary>
    /// Multiplier applied to magic items
    /// </summary>
    public double EffectMultiplier { get; set; } = 1.0;

    /// <summary>
    /// Get the effective modifiers (with multiplier applied)
    /// </summary>
    public IEnumerable<StatModifier> GetEffectiveModifiers()
    {
        return Modifiers.Select(m => new StatModifier
        {
            StatType = m.StatType,
            FlatValue = m.FlatValue * EffectMultiplier,
            PercentValue = m.PercentValue * EffectMultiplier,
            MoreMultiplier = m.MoreMultiplier * EffectMultiplier
        });
    }

    public override string ToString()
    {
        var modStrings = Modifiers.Select(m =>
        {
            var multiplier = EffectMultiplier != 1.0 ? $" (x{EffectMultiplier:F1})" : "";
            return m.ToString() + multiplier;
        });
        return $"{Name}: {string.Join(", ", modStrings)}";
    }
}
