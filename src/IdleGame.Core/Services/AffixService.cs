using IdleGame.Core.Models.Affixes;
using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Services;

/// <summary>
/// Service for generating and managing affixes
/// </summary>
public class AffixService
{
    private readonly List<AffixDefinition> _affixDefinitions = new();
    private readonly Random _random;

    public AffixService(Random? random = null)
    {
        _random = random ?? new Random();
        InitializeDefaultAffixes();
    }

    /// <summary>
    /// Initialize default affix definitions
    /// </summary>
    private void InitializeDefaultAffixes()
    {
        // Prefixes - Offensive
        AddAffix("prefix_physical_damage", "Heavy", AffixType.Prefix, 1, 1, 
            new StatModifierRange { StatType = StatType.PhysicalDamage, MinFlatValue = 5, MaxFlatValue = 15 });
        AddAffix("prefix_physical_damage_2", "Cruel", AffixType.Prefix, 2, 20,
            new StatModifierRange { StatType = StatType.PhysicalDamage, MinFlatValue = 16, MaxFlatValue = 30 });
        AddAffix("prefix_physical_damage_3", "Tyrannical", AffixType.Prefix, 3, 40,
            new StatModifierRange { StatType = StatType.PhysicalDamage, MinFlatValue = 31, MaxFlatValue = 50 });

        AddAffix("prefix_fire_damage", "Fiery", AffixType.Prefix, 1, 1,
            new StatModifierRange { StatType = StatType.FireDamage, MinFlatValue = 3, MaxFlatValue = 10 });
        AddAffix("prefix_cold_damage", "Icy", AffixType.Prefix, 1, 1,
            new StatModifierRange { StatType = StatType.ColdDamage, MinFlatValue = 3, MaxFlatValue = 10 });
        AddAffix("prefix_lightning_damage", "Shocking", AffixType.Prefix, 1, 1,
            new StatModifierRange { StatType = StatType.LightningDamage, MinFlatValue = 3, MaxFlatValue = 10 });

        // Prefixes - Defensive
        AddAffix("prefix_life", "Stout", AffixType.Prefix, 1, 1,
            new StatModifierRange { StatType = StatType.MaxLife, MinFlatValue = 20, MaxFlatValue = 40 });
        AddAffix("prefix_life_2", "Robust", AffixType.Prefix, 2, 20,
            new StatModifierRange { StatType = StatType.MaxLife, MinFlatValue = 41, MaxFlatValue = 70 });
        AddAffix("prefix_life_3", "Vital", AffixType.Prefix, 3, 40,
            new StatModifierRange { StatType = StatType.MaxLife, MinFlatValue = 71, MaxFlatValue = 100 });

        AddAffix("prefix_armor", "Armored", AffixType.Prefix, 1, 1,
            new StatModifierRange { StatType = StatType.Armor, MinFlatValue = 20, MaxFlatValue = 50 });
        AddAffix("prefix_evasion", "Nimble", AffixType.Prefix, 1, 1,
            new StatModifierRange { StatType = StatType.Evasion, MinFlatValue = 20, MaxFlatValue = 50 });
        AddAffix("prefix_energy_shield", "Shielded", AffixType.Prefix, 1, 1,
            new StatModifierRange { StatType = StatType.EnergyShield, MinFlatValue = 15, MaxFlatValue = 35 });

        // Suffixes - Stats
        AddAffix("suffix_strength", "of Might", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.Strength, MinFlatValue = 5, MaxFlatValue = 15 });
        AddAffix("suffix_dexterity", "of Dexterity", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.Dexterity, MinFlatValue = 5, MaxFlatValue = 15 });
        AddAffix("suffix_intelligence", "of Intellect", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.Intelligence, MinFlatValue = 5, MaxFlatValue = 15 });
        AddAffix("suffix_vitality", "of Vitality", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.Vitality, MinFlatValue = 5, MaxFlatValue = 15 });

        // Suffixes - Resistances
        AddAffix("suffix_fire_res", "of the Flame", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.FireResistance, MinFlatValue = 10, MaxFlatValue = 25 });
        AddAffix("suffix_cold_res", "of the Frost", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.ColdResistance, MinFlatValue = 10, MaxFlatValue = 25 });
        AddAffix("suffix_lightning_res", "of the Storm", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.LightningResistance, MinFlatValue = 10, MaxFlatValue = 25 });
        AddAffix("suffix_all_res", "of Resistance", AffixType.Suffix, 2, 30,
            new StatModifierRange { StatType = StatType.AllResistances, MinFlatValue = 8, MaxFlatValue = 15 });

        // Suffixes - Utility
        AddAffix("suffix_attack_speed", "of Speed", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.AttackSpeed, MinPercentValue = 5, MaxPercentValue = 15 });
        AddAffix("suffix_crit_chance", "of Precision", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.CriticalChance, MinFlatValue = 10, MaxFlatValue = 25 });
        AddAffix("suffix_crit_multi", "of Devastation", AffixType.Suffix, 2, 25,
            new StatModifierRange { StatType = StatType.CriticalMultiplier, MinFlatValue = 15, MaxFlatValue = 35 });
        AddAffix("suffix_life_regen", "of Regeneration", AffixType.Suffix, 1, 1,
            new StatModifierRange { StatType = StatType.LifeRegeneration, MinFlatValue = 2, MaxFlatValue = 8 });

        // Legendary affixes
        AddLegendaryAffix("legendary_massive_damage", "Legendary Power", 50,
            new StatModifierRange { StatType = StatType.PhysicalDamage, MinPercentValue = 50, MaxPercentValue = 100 });
        AddLegendaryAffix("legendary_immortal", "Undying", 50,
            new StatModifierRange { StatType = StatType.MaxLife, MinPercentValue = 30, MaxPercentValue = 50 },
            new StatModifierRange { StatType = StatType.LifeRegeneration, MinFlatValue = 10, MaxFlatValue = 20 });
        AddLegendaryAffix("legendary_elemental", "Elemental Fury", 50,
            new StatModifierRange { StatType = StatType.FireDamage, MinFlatValue = 20, MaxFlatValue = 40 },
            new StatModifierRange { StatType = StatType.ColdDamage, MinFlatValue = 20, MaxFlatValue = 40 },
            new StatModifierRange { StatType = StatType.LightningDamage, MinFlatValue = 20, MaxFlatValue = 40 });
    }

    private void AddAffix(string id, string name, AffixType type, int tier, int reqLevel, params StatModifierRange[] ranges)
    {
        _affixDefinitions.Add(new AffixDefinition
        {
            Id = id,
            Name = name,
            DisplayName = name,
            AffixType = type,
            Tier = tier,
            RequiredItemLevel = reqLevel,
            ModifierRanges = ranges.ToList(),
            Weight = 100
        });
    }

    private void AddLegendaryAffix(string id, string name, int reqLevel, params StatModifierRange[] ranges)
    {
        _affixDefinitions.Add(new AffixDefinition
        {
            Id = id,
            Name = name,
            DisplayName = name,
            AffixType = AffixType.Legendary,
            Tier = 1,
            RequiredItemLevel = reqLevel,
            ModifierRanges = ranges.ToList(),
            Weight = 100,
            IsLegendary = true
        });
    }

    /// <summary>
    /// Get available affixes for an item
    /// </summary>
    public List<AffixDefinition> GetAvailableAffixes(Item item, AffixType type)
    {
        return _affixDefinitions
            .Where(a => a.AffixType == type)
            .Where(a => a.RequiredItemLevel <= item.ItemLevel)
            .Where(a => !a.IsLegendary || item.Rarity == ItemRarity.Legendary)
            .ToList();
    }

    /// <summary>
    /// Roll a random affix for an item
    /// </summary>
    public ItemAffix? RollAffix(Item item, AffixType type)
    {
        var available = GetAvailableAffixes(item, type);
        if (available.Count == 0) return null;

        // Weighted random selection
        var totalWeight = available.Sum(a => a.Weight);
        var roll = _random.Next(totalWeight);
        var cumulative = 0;

        AffixDefinition? selected = null;
        foreach (var affix in available)
        {
            cumulative += affix.Weight;
            if (roll < cumulative)
            {
                selected = affix;
                break;
            }
        }

        if (selected == null) return null;

        return CreateAffixFromDefinition(selected, item.GetAffixEffectMultiplier());
    }

    /// <summary>
    /// Create an affix instance from a definition
    /// </summary>
    public ItemAffix CreateAffixFromDefinition(AffixDefinition definition, double effectMultiplier = 1.0)
    {
        var affix = new ItemAffix
        {
            DefinitionId = definition.Id,
            Name = definition.DisplayName,
            AffixType = definition.AffixType,
            Tier = definition.Tier,
            EffectMultiplier = effectMultiplier,
            Modifiers = definition.ModifierRanges.Select(r => r.Roll(_random)).ToList()
        };

        return affix;
    }

    /// <summary>
    /// Get a random legendary affix
    /// </summary>
    public ItemAffix? RollLegendaryAffix(int itemLevel)
    {
        var available = _affixDefinitions
            .Where(a => a.IsLegendary && a.RequiredItemLevel <= itemLevel)
            .ToList();

        if (available.Count == 0) return null;

        var selected = available[_random.Next(available.Count)];
        return CreateAffixFromDefinition(selected);
    }

    /// <summary>
    /// Add a custom affix definition
    /// </summary>
    public void AddAffixDefinition(AffixDefinition definition)
    {
        _affixDefinitions.Add(definition);
    }
}
