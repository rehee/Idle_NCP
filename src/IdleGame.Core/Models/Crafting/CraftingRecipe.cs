using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Models.Crafting;

/// <summary>
/// A crafting recipe/formula
/// </summary>
public class CraftingRecipe
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Required currency to use this recipe
    /// </summary>
    public List<CurrencyRequirement> CurrencyRequirements { get; set; } = new();

    /// <summary>
    /// Required item types
    /// </summary>
    public List<ItemRequirement> ItemRequirements { get; set; } = new();

    /// <summary>
    /// The type of crafting effect
    /// </summary>
    public CraftingEffectType EffectType { get; set; }

    /// <summary>
    /// Additional parameters for the crafting effect
    /// </summary>
    public Dictionary<string, string> Parameters { get; set; } = new();
}

public class CurrencyRequirement
{
    public CurrencyType CurrencyType { get; set; }
    public int Amount { get; set; }
}

public class ItemRequirement
{
    public ItemRarity? RequiredRarity { get; set; }
    public ItemBaseType? RequiredBaseType { get; set; }
    public int? MinItemLevel { get; set; }
    public int? MaxItemLevel { get; set; }
    public bool ConsumeItem { get; set; }
}

public enum CraftingEffectType
{
    /// <summary>
    /// Change item rarity
    /// </summary>
    ChangeRarity,

    /// <summary>
    /// Add random affix
    /// </summary>
    AddAffix,

    /// <summary>
    /// Remove all affixes
    /// </summary>
    RemoveAffixes,

    /// <summary>
    /// Reroll all affixes
    /// </summary>
    RerollAffixes,

    /// <summary>
    /// Reroll affix values
    /// </summary>
    RerollValues,

    /// <summary>
    /// Reroll implicit
    /// </summary>
    RerollImplicit,

    /// <summary>
    /// Add specific affix
    /// </summary>
    AddSpecificAffix,

    /// <summary>
    /// Create artifact
    /// </summary>
    CreateArtifact,

    /// <summary>
    /// Improve quality
    /// </summary>
    ImproveQuality,

    /// <summary>
    /// Add legendary affix
    /// </summary>
    AddLegendaryAffix
}
