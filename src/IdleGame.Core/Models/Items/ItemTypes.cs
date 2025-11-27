namespace IdleGame.Core.Models.Items;

/// <summary>
/// Types of equipment slots
/// </summary>
public enum EquipmentSlot
{
    None = 0,
    Helmet = 1,
    Chest = 2,
    Gloves = 3,
    Boots = 4,
    Belt = 5,
    Amulet = 6,
    RingLeft = 7,
    RingRight = 8,
    MainHand = 9,
    OffHand = 10,
    TwoHand = 11
}

/// <summary>
/// Base types of items
/// </summary>
public enum ItemBaseType
{
    // Armor
    HeavyHelmet,
    LightHelmet,
    MageHood,
    HeavyChest,
    LightChest,
    MageRobe,
    HeavyGloves,
    LightGloves,
    MageGloves,
    HeavyBoots,
    LightBoots,
    MageBoots,
    HeavyBelt,
    LightBelt,
    MageBelt,

    // Accessories
    Amulet,
    Ring,

    // One-handed weapons
    Sword,
    Axe,
    Mace,
    Dagger,
    Wand,
    Scepter,

    // Two-handed weapons
    TwoHandedSword,
    TwoHandedAxe,
    TwoHandedMace,
    Staff,
    Bow,

    // Off-hand
    Shield,
    Quiver,
    Orb
}
