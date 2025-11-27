using IdleGame.Core.Models.Affixes;
using IdleGame.Core.Models.Items;
using IdleGame.Core.Services;

namespace IdleGame.Core.Tests;

public class ItemServiceTests
{
    private readonly AffixService _affixService;
    private readonly ItemService _itemService;
    private readonly Random _random;

    public ItemServiceTests()
    {
        _random = new Random(42); // Fixed seed for reproducibility
        _affixService = new AffixService(_random);
        _itemService = new ItemService(_affixService, _random);
    }

    [Fact]
    public void GenerateItem_ShouldCreateValidItem()
    {
        var item = _itemService.GenerateItem(10);

        Assert.NotNull(item);
        Assert.NotEmpty(item.Id);
        Assert.NotEmpty(item.Name);
        Assert.True(item.ItemLevel > 0);
    }

    [Theory]
    [InlineData(ItemRarity.Normal)]
    [InlineData(ItemRarity.Magic)]
    [InlineData(ItemRarity.Rare)]
    [InlineData(ItemRarity.Legendary)]
    public void GenerateItem_WithForcedRarity_ShouldHaveCorrectRarity(ItemRarity rarity)
    {
        var item = _itemService.GenerateItem(50, rarity);

        Assert.Equal(rarity, item.Rarity);
    }

    [Fact]
    public void NormalItem_ShouldHaveNoAffixes()
    {
        var item = _itemService.GenerateItem(10, ItemRarity.Normal);

        Assert.Empty(item.Prefixes);
        Assert.Empty(item.Suffixes);
    }

    [Fact]
    public void MagicItem_ShouldHaveCorrectAffixCount()
    {
        var item = _itemService.GenerateItem(10, ItemRarity.Magic);

        Assert.True(item.Prefixes.Count <= 1);
        Assert.True(item.Suffixes.Count <= 1);
        Assert.True(item.Prefixes.Count + item.Suffixes.Count >= 1);
    }

    [Fact]
    public void RareItem_ShouldHaveMultipleAffixes()
    {
        var item = _itemService.GenerateItem(50, ItemRarity.Rare);

        Assert.True(item.Prefixes.Count <= 3);
        Assert.True(item.Suffixes.Count <= 3);
        Assert.True(item.Prefixes.Count + item.Suffixes.Count >= 2);
    }

    [Fact]
    public void LegendaryItem_ShouldHaveLegendaryAffix()
    {
        var item = _itemService.GenerateItem(60, ItemRarity.Legendary);

        Assert.NotNull(item.LegendaryAffix);
        Assert.Equal(2, item.Prefixes.Count);
        Assert.Equal(2, item.Suffixes.Count);
    }

    [Fact]
    public void GetItemBase_ShouldReturnCorrectBase()
    {
        var itemBase = _itemService.GetItemBase("iron_sword");

        Assert.NotNull(itemBase);
        Assert.Equal("Iron Sword", itemBase.Name);
        Assert.Equal(ItemBaseType.Sword, itemBase.BaseType);
    }

    [Fact]
    public void GetAllItemBases_ShouldReturnMultipleBases()
    {
        var bases = _itemService.GetAllItemBases().ToList();

        Assert.NotEmpty(bases);
        Assert.True(bases.Count > 10);
    }
}
