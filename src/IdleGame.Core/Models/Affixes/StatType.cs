namespace IdleGame.Core.Models.Affixes;

/// <summary>
/// Types of stat modifications
/// </summary>
public enum StatType
{
    // Primary stats
    Strength,
    Dexterity,
    Intelligence,
    Vitality,

    // Defensive
    Armor,
    Evasion,
    EnergyShield,
    MaxLife,
    MaxMana,
    LifeRegeneration,
    ManaRegeneration,
    BlockChance,
    DodgeChance,

    // Offensive
    PhysicalDamage,
    FireDamage,
    ColdDamage,
    LightningDamage,
    ChaosDamage,
    AttackSpeed,
    CriticalChance,
    CriticalMultiplier,
    Accuracy,

    // Resistances
    FireResistance,
    ColdResistance,
    LightningResistance,
    ChaosResistance,
    AllResistances,

    // Utility
    MovementSpeed,
    ItemQuantity,
    ItemRarity,
    ExperienceGain,
    GoldFind
}

/// <summary>
/// Affix position type
/// </summary>
public enum AffixType
{
    Prefix,
    Suffix,
    Implicit,
    Legendary,
    Fixed
}
