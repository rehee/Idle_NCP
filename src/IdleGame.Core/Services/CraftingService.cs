using IdleGame.Core.Models.Affixes;
using IdleGame.Core.Models.Crafting;
using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Services;

/// <summary>
/// Service for crafting and modifying items
/// </summary>
public class CraftingService
{
    private readonly AffixService _affixService;
    private readonly ItemService _itemService;
    private readonly Random _random;

    public CraftingService(AffixService affixService, ItemService itemService, Random? random = null)
    {
        _affixService = affixService;
        _itemService = itemService;
        _random = random ?? new Random();
    }

    /// <summary>
    /// Apply a currency to an item
    /// </summary>
    public CraftingResult ApplyCurrency(Item item, CurrencyType currency)
    {
        return currency switch
        {
            CurrencyType.TransmutationOrb => ApplyTransmutation(item),
            CurrencyType.AlchemyOrb => ApplyAlchemy(item),
            CurrencyType.AugmentationOrb => ApplyAugmentation(item),
            CurrencyType.AlterationOrb => ApplyAlteration(item),
            CurrencyType.ScouringOrb => ApplyScouring(item),
            CurrencyType.ChaosOrb => ApplyChaos(item),
            CurrencyType.ExaltedOrb => ApplyExalted(item),
            CurrencyType.DivineOrb => ApplyDivine(item),
            CurrencyType.LegendaryOrb => ApplyLegendary(item),
            CurrencyType.ArtifactStone => ApplyArtifactStone(item),
            CurrencyType.BlessedOrb => ApplyBlessed(item),
            _ => new CraftingResult { Success = false, Message = "Unknown currency type" }
        };
    }

    /// <summary>
    /// Upgrade Normal to Magic
    /// </summary>
    private CraftingResult ApplyTransmutation(Item item)
    {
        if (item.Rarity != ItemRarity.Normal)
            return new CraftingResult { Success = false, Message = "Item must be Normal rarity" };

        item.Rarity = ItemRarity.Magic;
        item.Prefixes.Clear();
        item.Suffixes.Clear();

        // Add 1-2 mods (at least 1 prefix and optionally 1 suffix)
        var prefix = _affixService.RollAffix(item, AffixType.Prefix);
        if (prefix != null) item.Prefixes.Add(prefix);

        if (_random.NextDouble() < 0.5)
        {
            var suffix = _affixService.RollAffix(item, AffixType.Suffix);
            if (suffix != null) item.Suffixes.Add(suffix);
        }

        item.Name = $"Enchanted {_itemService.GetItemBase(item.BaseId)?.Name ?? "Item"}";

        return new CraftingResult { Success = true, Message = "Item upgraded to Magic" };
    }

    /// <summary>
    /// Upgrade Normal/Magic to Rare
    /// </summary>
    private CraftingResult ApplyAlchemy(Item item)
    {
        if (item.Rarity == ItemRarity.Normal)
        {
            item.Rarity = ItemRarity.Rare;
            item.Prefixes.Clear();
            item.Suffixes.Clear();
            RollRareAffixes(item);
            item.Name = GenerateRareName();
            return new CraftingResult { Success = true, Message = "Item upgraded to Rare" };
        }

        if (item.Rarity == ItemRarity.Magic)
        {
            item.Rarity = ItemRarity.Rare;
            // Keep existing mods, add more
            while (item.Prefixes.Count < 3 && _random.NextDouble() < 0.7)
            {
                var prefix = _affixService.RollAffix(item, AffixType.Prefix);
                if (prefix != null) item.Prefixes.Add(prefix);
            }
            while (item.Suffixes.Count < 3 && _random.NextDouble() < 0.7)
            {
                var suffix = _affixService.RollAffix(item, AffixType.Suffix);
                if (suffix != null) item.Suffixes.Add(suffix);
            }
            item.Name = GenerateRareName();
            return new CraftingResult { Success = true, Message = "Item upgraded to Rare" };
        }

        return new CraftingResult { Success = false, Message = "Item must be Normal or Magic rarity" };
    }

    /// <summary>
    /// Add a mod to Magic item
    /// </summary>
    private CraftingResult ApplyAugmentation(Item item)
    {
        if (item.Rarity != ItemRarity.Magic)
            return new CraftingResult { Success = false, Message = "Item must be Magic rarity" };

        if (item.Prefixes.Count >= 1 && item.Suffixes.Count >= 1)
            return new CraftingResult { Success = false, Message = "Item already has maximum mods" };

        if (item.Prefixes.Count < 1)
        {
            var prefix = _affixService.RollAffix(item, AffixType.Prefix);
            if (prefix != null) item.Prefixes.Add(prefix);
            return new CraftingResult { Success = true, Message = "Added prefix" };
        }
        else
        {
            var suffix = _affixService.RollAffix(item, AffixType.Suffix);
            if (suffix != null) item.Suffixes.Add(suffix);
            return new CraftingResult { Success = true, Message = "Added suffix" };
        }
    }

    /// <summary>
    /// Reroll mods on Magic item
    /// </summary>
    private CraftingResult ApplyAlteration(Item item)
    {
        if (item.Rarity != ItemRarity.Magic)
            return new CraftingResult { Success = false, Message = "Item must be Magic rarity" };

        item.Prefixes.Clear();
        item.Suffixes.Clear();

        var prefix = _affixService.RollAffix(item, AffixType.Prefix);
        if (prefix != null) item.Prefixes.Add(prefix);

        if (_random.NextDouble() < 0.5)
        {
            var suffix = _affixService.RollAffix(item, AffixType.Suffix);
            if (suffix != null) item.Suffixes.Add(suffix);
        }

        return new CraftingResult { Success = true, Message = "Rerolled mods" };
    }

    /// <summary>
    /// Remove all mods, return to Normal
    /// </summary>
    private CraftingResult ApplyScouring(Item item)
    {
        if (item.Rarity == ItemRarity.Normal)
            return new CraftingResult { Success = false, Message = "Item is already Normal rarity" };

        if (item.Rarity == ItemRarity.Unique || item.Rarity == ItemRarity.Artifact)
            return new CraftingResult { Success = false, Message = "Cannot scour Unique or Artifact items" };

        item.Rarity = ItemRarity.Normal;
        item.Prefixes.Clear();
        item.Suffixes.Clear();
        item.LegendaryAffix = null;
        item.Name = _itemService.GetItemBase(item.BaseId)?.Name ?? "Item";

        return new CraftingResult { Success = true, Message = "Item returned to Normal" };
    }

    /// <summary>
    /// Reroll all mods on Rare item
    /// </summary>
    private CraftingResult ApplyChaos(Item item)
    {
        if (item.Rarity != ItemRarity.Rare)
            return new CraftingResult { Success = false, Message = "Item must be Rare rarity" };

        item.Prefixes.Clear();
        item.Suffixes.Clear();
        RollRareAffixes(item);
        item.Name = GenerateRareName();

        return new CraftingResult { Success = true, Message = "Rerolled all mods" };
    }

    /// <summary>
    /// Add a mod to Rare item
    /// </summary>
    private CraftingResult ApplyExalted(Item item)
    {
        if (item.Rarity != ItemRarity.Rare)
            return new CraftingResult { Success = false, Message = "Item must be Rare rarity" };

        if (item.Prefixes.Count >= 3 && item.Suffixes.Count >= 3)
            return new CraftingResult { Success = false, Message = "Item already has maximum mods" };

        // Prefer to add to the slot with fewer mods
        if (item.Prefixes.Count < item.Suffixes.Count || (item.Prefixes.Count == item.Suffixes.Count && _random.NextDouble() < 0.5))
        {
            if (item.Prefixes.Count < 3)
            {
                var prefix = _affixService.RollAffix(item, AffixType.Prefix);
                if (prefix != null) item.Prefixes.Add(prefix);
                return new CraftingResult { Success = true, Message = "Added prefix" };
            }
        }

        if (item.Suffixes.Count < 3)
        {
            var suffix = _affixService.RollAffix(item, AffixType.Suffix);
            if (suffix != null) item.Suffixes.Add(suffix);
            return new CraftingResult { Success = true, Message = "Added suffix" };
        }

        return new CraftingResult { Success = false, Message = "Failed to add mod" };
    }

    /// <summary>
    /// Reroll numeric values
    /// </summary>
    private CraftingResult ApplyDivine(Item item)
    {
        if (item.Rarity == ItemRarity.Normal)
            return new CraftingResult { Success = false, Message = "Item has no mods to reroll" };

        foreach (var affix in item.GetAllAffixes())
        {
            var definition = GetAffixDefinition(affix.DefinitionId);
            if (definition != null)
            {
                affix.Modifiers = definition.ModifierRanges.Select(r => r.Roll(_random)).ToList();
            }
        }

        return new CraftingResult { Success = true, Message = "Rerolled mod values" };
    }

    /// <summary>
    /// Upgrade Rare to Legendary
    /// </summary>
    private CraftingResult ApplyLegendary(Item item)
    {
        if (item.Rarity != ItemRarity.Rare)
            return new CraftingResult { Success = false, Message = "Item must be Rare rarity" };

        item.Rarity = ItemRarity.Legendary;

        // Keep 2 prefixes and 2 suffixes
        while (item.Prefixes.Count > 2)
            item.Prefixes.RemoveAt(item.Prefixes.Count - 1);
        while (item.Suffixes.Count > 2)
            item.Suffixes.RemoveAt(item.Suffixes.Count - 1);

        // Add legendary affix
        item.LegendaryAffix = _affixService.RollLegendaryAffix(item.ItemLevel);
        item.Name = $"Legendary {_itemService.GetItemBase(item.BaseId)?.Name ?? "Item"}";

        return new CraftingResult { Success = true, Message = "Item upgraded to Legendary" };
    }

    /// <summary>
    /// Create Artifact from Normal item
    /// </summary>
    private CraftingResult ApplyArtifactStone(Item item)
    {
        if (item.Rarity != ItemRarity.Normal)
            return new CraftingResult { Success = false, Message = "Item must be Normal rarity" };

        item.Rarity = ItemRarity.Artifact;
        item.Prefixes.Clear();
        item.Suffixes.Clear();

        // Add fixed affixes
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
        RollRareAffixes(item);
        item.Name = $"Artifact {_itemService.GetItemBase(item.BaseId)?.Name ?? "Item"}";

        return new CraftingResult { Success = true, Message = "Item transformed into Artifact" };
    }

    /// <summary>
    /// Reroll implicit mod
    /// </summary>
    private CraftingResult ApplyBlessed(Item item)
    {
        if (item.ImplicitAffixes.Count == 0)
            return new CraftingResult { Success = false, Message = "Item has no implicit mods" };

        foreach (var implicit_ in item.ImplicitAffixes)
        {
            var definition = GetAffixDefinition(implicit_.DefinitionId);
            if (definition != null)
            {
                implicit_.Modifiers = definition.ModifierRanges.Select(r => r.Roll(_random)).ToList();
            }
        }

        return new CraftingResult { Success = true, Message = "Rerolled implicit values" };
    }

    private void RollRareAffixes(Item item)
    {
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
    }

    private string GenerateRareName()
    {
        var prefixes = new[] { "Ancient", "Mystic", "Shadow", "Storm", "Blood", "Iron", "Bone", "Soul", "Dragon" };
        var suffixes = new[] { "Bane", "Edge", "Heart", "Guard", "Fist", "Eye", "Fury", "Grace", "Spirit" };

        var prefix = prefixes[_random.Next(prefixes.Length)];
        var suffix = suffixes[_random.Next(suffixes.Length)];

        return $"{prefix} {suffix}";
    }

    private AffixDefinition? GetAffixDefinition(string id)
    {
        // In a real implementation, this would look up the definition from a data store
        return null;
    }
}

/// <summary>
/// Result of a crafting operation
/// </summary>
public class CraftingResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public Item? ResultItem { get; set; }
}
