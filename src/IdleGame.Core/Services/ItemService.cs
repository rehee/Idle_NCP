using IdleGame.Core.Models.Affixes;
using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Services;

/// <summary>
/// Service for generating items
/// </summary>
public class ItemService
{
    private readonly AffixService _affixService;
    private readonly Random _random;
    private readonly List<ItemBase> _itemBases = new();

    public ItemService(AffixService affixService, Random? random = null)
    {
        _affixService = affixService;
        _random = random ?? new Random();
        InitializeDefaultItemBases();
    }

    private void InitializeDefaultItemBases()
    {
        // Helmets
        AddItemBase("iron_helmet", "Iron Helmet", ItemBaseType.HeavyHelmet, EquipmentSlot.Helmet, 1, armor: 20);
        AddItemBase("steel_helmet", "Steel Helmet", ItemBaseType.HeavyHelmet, EquipmentSlot.Helmet, 15, armor: 50);
        AddItemBase("leather_cap", "Leather Cap", ItemBaseType.LightHelmet, EquipmentSlot.Helmet, 1, evasion: 20);
        AddItemBase("silk_hood", "Silk Hood", ItemBaseType.MageHood, EquipmentSlot.Helmet, 1, energyShield: 15);

        // Chests
        AddItemBase("iron_plate", "Iron Plate", ItemBaseType.HeavyChest, EquipmentSlot.Chest, 1, armor: 50);
        AddItemBase("steel_plate", "Steel Plate", ItemBaseType.HeavyChest, EquipmentSlot.Chest, 20, armor: 120);
        AddItemBase("leather_vest", "Leather Vest", ItemBaseType.LightChest, EquipmentSlot.Chest, 1, evasion: 50);
        AddItemBase("silk_robe", "Silk Robe", ItemBaseType.MageRobe, EquipmentSlot.Chest, 1, energyShield: 35);

        // Gloves
        AddItemBase("iron_gauntlets", "Iron Gauntlets", ItemBaseType.HeavyGloves, EquipmentSlot.Gloves, 1, armor: 15);
        AddItemBase("leather_gloves", "Leather Gloves", ItemBaseType.LightGloves, EquipmentSlot.Gloves, 1, evasion: 15);
        AddItemBase("silk_gloves", "Silk Gloves", ItemBaseType.MageGloves, EquipmentSlot.Gloves, 1, energyShield: 10);

        // Boots
        AddItemBase("iron_boots", "Iron Boots", ItemBaseType.HeavyBoots, EquipmentSlot.Boots, 1, armor: 15);
        AddItemBase("leather_boots", "Leather Boots", ItemBaseType.LightBoots, EquipmentSlot.Boots, 1, evasion: 15);
        AddItemBase("silk_boots", "Silk Boots", ItemBaseType.MageBoots, EquipmentSlot.Boots, 1, energyShield: 10);

        // Belts
        AddItemBase("leather_belt", "Leather Belt", ItemBaseType.LightBelt, EquipmentSlot.Belt, 1);
        AddItemBase("heavy_belt", "Heavy Belt", ItemBaseType.HeavyBelt, EquipmentSlot.Belt, 10, armor: 30);
        AddItemBase("cloth_belt", "Cloth Belt", ItemBaseType.MageBelt, EquipmentSlot.Belt, 1, energyShield: 20);

        // Accessories
        AddItemBase("gold_amulet", "Gold Amulet", ItemBaseType.Amulet, EquipmentSlot.Amulet, 1);
        AddItemBase("gold_ring", "Gold Ring", ItemBaseType.Ring, EquipmentSlot.RingLeft, 1);

        // Weapons
        AddItemBase("iron_sword", "Iron Sword", ItemBaseType.Sword, EquipmentSlot.MainHand, 1, minDmg: 5, maxDmg: 15, atkSpd: 1.2);
        AddItemBase("steel_sword", "Steel Sword", ItemBaseType.Sword, EquipmentSlot.MainHand, 15, minDmg: 15, maxDmg: 35, atkSpd: 1.2);
        AddItemBase("iron_axe", "Iron Axe", ItemBaseType.Axe, EquipmentSlot.MainHand, 1, minDmg: 8, maxDmg: 18, atkSpd: 1.0);
        AddItemBase("iron_mace", "Iron Mace", ItemBaseType.Mace, EquipmentSlot.MainHand, 1, minDmg: 10, maxDmg: 15, atkSpd: 0.9);
        AddItemBase("iron_dagger", "Iron Dagger", ItemBaseType.Dagger, EquipmentSlot.MainHand, 1, minDmg: 3, maxDmg: 8, atkSpd: 1.5, critChance: 8);
        AddItemBase("wooden_wand", "Wooden Wand", ItemBaseType.Wand, EquipmentSlot.MainHand, 1, minDmg: 2, maxDmg: 6, atkSpd: 1.3);

        // Two-handed
        AddItemBase("iron_greatsword", "Iron Greatsword", ItemBaseType.TwoHandedSword, EquipmentSlot.TwoHand, 1, minDmg: 15, maxDmg: 40, atkSpd: 0.8);
        AddItemBase("wooden_bow", "Wooden Bow", ItemBaseType.Bow, EquipmentSlot.TwoHand, 1, minDmg: 8, maxDmg: 20, atkSpd: 1.4);
        AddItemBase("oak_staff", "Oak Staff", ItemBaseType.Staff, EquipmentSlot.TwoHand, 1, minDmg: 10, maxDmg: 25, atkSpd: 1.0);

        // Off-hand
        AddItemBase("wooden_shield", "Wooden Shield", ItemBaseType.Shield, EquipmentSlot.OffHand, 1, armor: 30);
        AddItemBase("iron_shield", "Iron Shield", ItemBaseType.Shield, EquipmentSlot.OffHand, 10, armor: 60);
        AddItemBase("quiver", "Quiver", ItemBaseType.Quiver, EquipmentSlot.OffHand, 1);
        AddItemBase("crystal_orb", "Crystal Orb", ItemBaseType.Orb, EquipmentSlot.OffHand, 1, energyShield: 25);
    }

    private void AddItemBase(string id, string name, ItemBaseType baseType, EquipmentSlot slot, int reqLevel,
        int armor = 0, int evasion = 0, int energyShield = 0,
        int minDmg = 0, int maxDmg = 0, double atkSpd = 0, double critChance = 5)
    {
        _itemBases.Add(new ItemBase
        {
            Id = id,
            Name = name,
            BaseType = baseType,
            Slot = slot,
            RequiredLevel = reqLevel,
            BaseArmor = armor,
            BaseEvasion = evasion,
            BaseEnergyShield = energyShield,
            BaseMinDamage = minDmg,
            BaseMaxDamage = maxDmg,
            BaseAttackSpeed = atkSpd,
            BaseCriticalChance = critChance
        });
    }

    /// <summary>
    /// Generate a random item
    /// </summary>
    public Item GenerateItem(int itemLevel, ItemRarity? forcedRarity = null)
    {
        var rarity = forcedRarity ?? RollRarity();
        var validBases = _itemBases.Where(b => b.RequiredLevel <= itemLevel).ToList();
        if (validBases.Count == 0)
            validBases = _itemBases;

        var baseItem = validBases[_random.Next(validBases.Count)];
        return CreateItem(baseItem, itemLevel, rarity);
    }

    /// <summary>
    /// Create an item from a base
    /// </summary>
    public Item CreateItem(ItemBase baseItem, int itemLevel, ItemRarity rarity)
    {
        var item = new Item
        {
            Name = GenerateItemName(baseItem, rarity),
            BaseId = baseItem.Id,
            BaseType = baseItem.BaseType,
            Slot = baseItem.Slot,
            Rarity = rarity,
            ItemLevel = itemLevel,
            RequiredLevel = baseItem.RequiredLevel,
            BaseArmor = baseItem.BaseArmor,
            BaseEvasion = baseItem.BaseEvasion,
            BaseEnergyShield = baseItem.BaseEnergyShield,
            BaseMinDamage = baseItem.BaseMinDamage,
            BaseMaxDamage = baseItem.BaseMaxDamage,
            BaseAttackSpeed = baseItem.BaseAttackSpeed,
            BaseCriticalChance = baseItem.BaseCriticalChance
        };

        // Add affixes based on rarity
        AddAffixesForRarity(item);

        return item;
    }

    private void AddAffixesForRarity(Item item)
    {
        switch (item.Rarity)
        {
            case ItemRarity.Normal:
                // No affixes
                break;

            case ItemRarity.Magic:
                // 1 prefix, 1 suffix with enhanced effects
                var magicPrefix = _affixService.RollAffix(item, AffixType.Prefix);
                if (magicPrefix != null) item.Prefixes.Add(magicPrefix);

                var magicSuffix = _affixService.RollAffix(item, AffixType.Suffix);
                if (magicSuffix != null) item.Suffixes.Add(magicSuffix);
                break;

            case ItemRarity.Rare:
                // 1-3 prefixes, 1-3 suffixes
                var prefixCount = _random.Next(1, 4);
                var suffixCount = _random.Next(1, 4);

                for (var i = 0; i < prefixCount && item.CanAddPrefix(); i++)
                {
                    var prefix = _affixService.RollAffix(item, AffixType.Prefix);
                    if (prefix != null) item.Prefixes.Add(prefix);
                }

                for (var i = 0; i < suffixCount && item.CanAddSuffix(); i++)
                {
                    var suffix = _affixService.RollAffix(item, AffixType.Suffix);
                    if (suffix != null) item.Suffixes.Add(suffix);
                }
                break;

            case ItemRarity.Legendary:
                // 2 prefixes, 2 suffixes, plus legendary affix
                for (var i = 0; i < 2; i++)
                {
                    var prefix = _affixService.RollAffix(item, AffixType.Prefix);
                    if (prefix != null) item.Prefixes.Add(prefix);

                    var suffix = _affixService.RollAffix(item, AffixType.Suffix);
                    if (suffix != null) item.Suffixes.Add(suffix);
                }

                item.LegendaryAffix = _affixService.RollLegendaryAffix(item.ItemLevel);
                break;

            case ItemRarity.Artifact:
                // Fixed + random affixes (similar to rare)
                // Add 1-2 fixed affixes
                for (var i = 0; i < _random.Next(1, 3); i++)
                {
                    var fixedAffix = _affixService.RollAffix(item, AffixType.Prefix);
                    if (fixedAffix != null)
                    {
                        fixedAffix.AffixType = AffixType.Fixed;
                        item.FixedAffixes.Add(fixedAffix);
                    }
                }

                // Add random affixes
                for (var i = 0; i < _random.Next(2, 4); i++)
                {
                    var prefix = _affixService.RollAffix(item, AffixType.Prefix);
                    if (prefix != null) item.Prefixes.Add(prefix);
                }

                for (var i = 0; i < _random.Next(2, 4); i++)
                {
                    var suffix = _affixService.RollAffix(item, AffixType.Suffix);
                    if (suffix != null) item.Suffixes.Add(suffix);
                }
                break;
        }
    }

    private string GenerateItemName(ItemBase baseItem, ItemRarity rarity)
    {
        return rarity switch
        {
            ItemRarity.Normal => baseItem.Name,
            ItemRarity.Magic => $"Enchanted {baseItem.Name}",
            ItemRarity.Rare => GenerateRareName(baseItem),
            ItemRarity.Legendary => $"Legendary {baseItem.Name}",
            ItemRarity.Unique => baseItem.Name,
            ItemRarity.Artifact => $"Artifact {baseItem.Name}",
            _ => baseItem.Name
        };
    }

    private string GenerateRareName(ItemBase baseItem)
    {
        var prefixes = new[] { "Ancient", "Mystic", "Shadow", "Storm", "Blood", "Iron", "Bone", "Soul", "Dragon" };
        var suffixes = new[] { "Bane", "Edge", "Heart", "Guard", "Fist", "Eye", "Fury", "Grace", "Spirit" };

        var prefix = prefixes[_random.Next(prefixes.Length)];
        var suffix = suffixes[_random.Next(suffixes.Length)];

        return $"{prefix} {suffix}";
    }

    private ItemRarity RollRarity()
    {
        var roll = _random.NextDouble() * 100;
        return roll switch
        {
            < 60 => ItemRarity.Normal,
            < 85 => ItemRarity.Magic,
            < 97 => ItemRarity.Rare,
            < 99.5 => ItemRarity.Legendary,
            _ => ItemRarity.Unique
        };
    }

    /// <summary>
    /// Get item base by ID
    /// </summary>
    public ItemBase? GetItemBase(string id)
    {
        return _itemBases.FirstOrDefault(b => b.Id == id);
    }

    /// <summary>
    /// Get all item bases
    /// </summary>
    public IEnumerable<ItemBase> GetAllItemBases()
    {
        return _itemBases;
    }

    /// <summary>
    /// Add custom item base
    /// </summary>
    public void AddItemBase(ItemBase itemBase)
    {
        _itemBases.Add(itemBase);
    }
}
