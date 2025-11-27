namespace IdleGame.Core.Models.Crafting;

/// <summary>
/// Types of currency items used for crafting
/// </summary>
public enum CurrencyType
{
    /// <summary>
    /// Upgrades Normal item to Magic
    /// </summary>
    TransmutationOrb,

    /// <summary>
    /// Upgrades Magic item to Rare
    /// </summary>
    AlchemyOrb,

    /// <summary>
    /// Adds a random prefix or suffix to a Magic item
    /// </summary>
    AugmentationOrb,

    /// <summary>
    /// Rerolls all affixes on a Magic item
    /// </summary>
    AlterationOrb,

    /// <summary>
    /// Removes all affixes, returns to Normal rarity
    /// </summary>
    ScouringOrb,

    /// <summary>
    /// Rerolls all affixes on a Rare item
    /// </summary>
    ChaosOrb,

    /// <summary>
    /// Adds an affix to a Rare item
    /// </summary>
    ExaltedOrb,

    /// <summary>
    /// Rerolls numeric values on item
    /// </summary>
    DivineOrb,

    /// <summary>
    /// Upgrades Rare item to Legendary
    /// </summary>
    LegendaryOrb,

    /// <summary>
    /// Creates Artifact from Normal item
    /// </summary>
    ArtifactStone,

    /// <summary>
    /// Improves item quality
    /// </summary>
    QualityScroll,

    /// <summary>
    /// Identifies unidentified items
    /// </summary>
    IdentificationScroll,

    /// <summary>
    /// Randomizes implicit modifier
    /// </summary>
    BlessedOrb,

    /// <summary>
    /// Locks prefixes when using other currency
    /// </summary>
    PrefixLock,

    /// <summary>
    /// Locks suffixes when using other currency
    /// </summary>
    SuffixLock
}

/// <summary>
/// A stack of currency
/// </summary>
public class Currency
{
    public CurrencyType Type { get; set; }
    public int Amount { get; set; }
    public int MaxStackSize { get; set; } = 20;

    public Currency() { }

    public Currency(CurrencyType type, int amount = 1)
    {
        Type = type;
        Amount = amount;
    }

    public bool CanStack(int additional) => Amount + additional <= MaxStackSize;

    public void Add(int amount)
    {
        Amount = Math.Min(Amount + amount, MaxStackSize);
    }

    public bool Remove(int amount)
    {
        if (Amount < amount) return false;
        Amount -= amount;
        return true;
    }
}
