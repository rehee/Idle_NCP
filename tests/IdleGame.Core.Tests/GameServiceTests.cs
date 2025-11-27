using IdleGame.Core.Models.Characters;
using IdleGame.Core.Services;

namespace IdleGame.Core.Tests;

public class GameServiceTests
{
    private readonly GameService _gameService;
    private readonly Random _random;

    public GameServiceTests()
    {
        _random = new Random(42);
        _gameService = new GameService(_random);
    }

    [Fact]
    public void CreateCharacter_ShouldCreateValidCharacter()
    {
        var character = _gameService.CreateCharacter("TestHero", CharacterClass.Warrior);

        Assert.NotNull(character);
        Assert.Equal("TestHero", character.Name);
        Assert.Equal(CharacterClass.Warrior, character.Class);
        Assert.Equal(1, character.Level);
    }

    [Theory]
    [InlineData(CharacterClass.Warrior)]
    [InlineData(CharacterClass.Ranger)]
    [InlineData(CharacterClass.Mage)]
    [InlineData(CharacterClass.Rogue)]
    public void CreateCharacter_AllClasses_ShouldHaveStarterGear(CharacterClass characterClass)
    {
        var character = _gameService.CreateCharacter("TestHero", characterClass);

        Assert.NotNull(character.Equipment.MainHand ?? character.Equipment.GetItem(Models.Items.EquipmentSlot.TwoHand));
        Assert.NotNull(character.Equipment.Chest);
    }

    [Fact]
    public void StartMapRun_ShouldCreateValidMap()
    {
        var character = _gameService.CreateCharacter("TestHero", CharacterClass.Warrior);
        var map = _gameService.StartMapRun(character, "forest_clearing");

        Assert.NotNull(map);
        Assert.Equal("forest_clearing", map.DefinitionId);
        Assert.True(map.Width > 0);
        Assert.True(map.Height > 0);
        Assert.True(map.TotalMonsters > 0);
    }

    [Fact]
    public void SimulateIdle_ShouldProduceResults()
    {
        var character = _gameService.CreateCharacter("TestHero", CharacterClass.Warrior);
        var map = _gameService.StartMapRun(character, "forest_clearing");

        var result = _gameService.SimulateIdle(character, map, TimeSpan.FromSeconds(30));

        Assert.NotNull(result);
        Assert.True(result.MonstersKilled >= 0);
        Assert.True(result.ExperienceGained >= 0);
    }

    [Fact]
    public void Character_AddExperience_ShouldLevelUp()
    {
        var character = _gameService.CreateCharacter("TestHero", CharacterClass.Warrior);
        var expNeeded = character.ExperienceToNextLevel;

        var leveledUp = character.AddExperience(expNeeded);

        Assert.True(leveledUp);
        Assert.Equal(2, character.Level);
    }

    [Fact]
    public void EquipItem_ShouldEquipAndReturnOldItem()
    {
        var character = _gameService.CreateCharacter("TestHero", CharacterClass.Warrior);
        var newItem = _gameService.ItemService.GenerateItem(10, Models.Items.ItemRarity.Rare);
        newItem.Slot = character.Equipment.MainHand!.Slot;
        character.Inventory.AddItem(newItem);

        var oldItem = _gameService.EquipItem(character, newItem);

        Assert.NotNull(oldItem);
        Assert.Contains(oldItem, character.Inventory.Items);
        Assert.DoesNotContain(newItem, character.Inventory.Items);
    }

    [Fact]
    public void SellItem_ShouldGiveGold()
    {
        var character = _gameService.CreateCharacter("TestHero", CharacterClass.Warrior);
        var item = _gameService.ItemService.GenerateItem(10, Models.Items.ItemRarity.Rare);
        character.Inventory.AddItem(item);
        var initialGold = character.Inventory.Gold;

        var goldEarned = _gameService.SellItem(character, item);

        Assert.True(goldEarned > 0);
        Assert.Equal(initialGold + goldEarned, character.Inventory.Gold);
        Assert.DoesNotContain(item, character.Inventory.Items);
    }
}
