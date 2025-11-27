using IdleGame.Core.Models.Affixes;
using IdleGame.Core.Models.Crafting;
using IdleGame.Core.Models.Items;
using IdleGame.Core.Services;

namespace IdleGame.Core.Tests;

public class CraftingServiceTests
{
    private readonly AffixService _affixService;
    private readonly ItemService _itemService;
    private readonly CraftingService _craftingService;
    private readonly Random _random;

    public CraftingServiceTests()
    {
        _random = new Random(42);
        _affixService = new AffixService(_random);
        _itemService = new ItemService(_affixService, _random);
        _craftingService = new CraftingService(_affixService, _itemService, _random);
    }

    [Fact]
    public void TransmutationOrb_ShouldUpgradeNormalToMagic()
    {
        var item = _itemService.GenerateItem(10, ItemRarity.Normal);
        Assert.Equal(ItemRarity.Normal, item.Rarity);

        var result = _craftingService.ApplyCurrency(item, CurrencyType.TransmutationOrb);

        Assert.True(result.Success);
        Assert.Equal(ItemRarity.Magic, item.Rarity);
        Assert.True(item.Prefixes.Count + item.Suffixes.Count >= 1);
    }

    [Fact]
    public void TransmutationOrb_ShouldFailOnNonNormalItem()
    {
        var item = _itemService.GenerateItem(10, ItemRarity.Magic);

        var result = _craftingService.ApplyCurrency(item, CurrencyType.TransmutationOrb);

        Assert.False(result.Success);
        Assert.Equal(ItemRarity.Magic, item.Rarity);
    }

    [Fact]
    public void AlchemyOrb_ShouldUpgradeToRare()
    {
        var item = _itemService.GenerateItem(10, ItemRarity.Normal);

        var result = _craftingService.ApplyCurrency(item, CurrencyType.AlchemyOrb);

        Assert.True(result.Success);
        Assert.Equal(ItemRarity.Rare, item.Rarity);
    }

    [Fact]
    public void ScouringOrb_ShouldReturnToNormal()
    {
        var item = _itemService.GenerateItem(10, ItemRarity.Rare);
        Assert.Equal(ItemRarity.Rare, item.Rarity);

        var result = _craftingService.ApplyCurrency(item, CurrencyType.ScouringOrb);

        Assert.True(result.Success);
        Assert.Equal(ItemRarity.Normal, item.Rarity);
        Assert.Empty(item.Prefixes);
        Assert.Empty(item.Suffixes);
    }

    [Fact]
    public void ScouringOrb_ShouldFailOnNormalItem()
    {
        var item = _itemService.GenerateItem(10, ItemRarity.Normal);

        var result = _craftingService.ApplyCurrency(item, CurrencyType.ScouringOrb);

        Assert.False(result.Success);
    }

    [Fact]
    public void ChaosOrb_ShouldRerollRareAffixes()
    {
        var item = _itemService.GenerateItem(50, ItemRarity.Rare);
        var originalPrefixes = item.Prefixes.Select(p => p.DefinitionId).ToList();

        var result = _craftingService.ApplyCurrency(item, CurrencyType.ChaosOrb);

        Assert.True(result.Success);
        Assert.Equal(ItemRarity.Rare, item.Rarity);
    }

    [Fact]
    public void LegendaryOrb_ShouldUpgradeRareToLegendary()
    {
        var item = _itemService.GenerateItem(50, ItemRarity.Rare);

        var result = _craftingService.ApplyCurrency(item, CurrencyType.LegendaryOrb);

        Assert.True(result.Success);
        Assert.Equal(ItemRarity.Legendary, item.Rarity);
        Assert.NotNull(item.LegendaryAffix);
        Assert.True(item.Prefixes.Count <= 2);
        Assert.True(item.Suffixes.Count <= 2);
    }

    [Fact]
    public void ArtifactStone_ShouldCreateArtifactFromNormal()
    {
        var item = _itemService.GenerateItem(50, ItemRarity.Normal);

        var result = _craftingService.ApplyCurrency(item, CurrencyType.ArtifactStone);

        Assert.True(result.Success);
        Assert.Equal(ItemRarity.Artifact, item.Rarity);
        Assert.NotEmpty(item.FixedAffixes);
    }
}
