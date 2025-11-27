namespace IdleGame.Core.Models.Affixes;

/// <summary>
/// A single stat modifier
/// </summary>
public class StatModifier
{
    public StatType StatType { get; set; }

    /// <summary>
    /// Flat value added (e.g., +50 to Life)
    /// </summary>
    public double FlatValue { get; set; }

    /// <summary>
    /// Percentage increase (e.g., 10% increased Life)
    /// </summary>
    public double PercentValue { get; set; }

    /// <summary>
    /// More multiplier (e.g., 20% more damage)
    /// </summary>
    public double MoreMultiplier { get; set; }

    public StatModifier() { }

    public StatModifier(StatType statType, double flatValue = 0, double percentValue = 0, double moreMultiplier = 0)
    {
        StatType = statType;
        FlatValue = flatValue;
        PercentValue = percentValue;
        MoreMultiplier = moreMultiplier;
    }

    public override string ToString()
    {
        var parts = new List<string>();

        if (FlatValue != 0)
            parts.Add($"+{FlatValue} {StatType}");

        if (PercentValue != 0)
            parts.Add($"{PercentValue:+0;-0}% increased {StatType}");

        if (MoreMultiplier != 0)
            parts.Add($"{MoreMultiplier:+0;-0}% more {StatType}");

        return string.Join(", ", parts);
    }
}
