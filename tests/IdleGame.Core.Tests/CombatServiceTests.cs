using IdleGame.Core.Models.Combat;
using IdleGame.Core.Services;

namespace IdleGame.Core.Tests;

public class CombatServiceTests
{
    private readonly GameService _gameService;
    private readonly CombatService _combatService;
    private readonly Random _random;

    public CombatServiceTests()
    {
        _random = new Random(42);
        _gameService = new GameService(_random);
        _combatService = _gameService.CombatService;
    }

    [Fact]
    public void CreateCombatEntity_ShouldCreateFromCharacter()
    {
        var character = _gameService.CreateCharacter("TestHero", Models.Characters.CharacterClass.Warrior);
        var entity = _combatService.CreateCombatEntity(character);

        Assert.NotNull(entity);
        Assert.Equal(character.Name, entity.Name);
        Assert.True(entity.MaxLife > 0);
        Assert.True(entity.CurrentLife > 0);
    }

    [Fact]
    public void PerformAttack_ShouldDealDamage()
    {
        var attacker = new CombatEntity
        {
            Name = "Attacker",
            PhysicalDamage = 50,
            Accuracy = 1000,
            CriticalChance = 0
        };

        var defender = new CombatEntity
        {
            Name = "Defender",
            MaxLife = 100,
            CurrentLife = 100,
            Armor = 0,
            Evasion = 0,
            BlockChance = 0,
            DodgeChance = 0
        };

        var result = _combatService.PerformAttack(attacker, defender);

        Assert.False(result.Missed);
        Assert.NotNull(result.DamageResult);
        Assert.True(result.DamageResult.TotalDamage > 0);
        Assert.True(defender.CurrentLife < 100);
    }

    [Fact]
    public void TakeDamage_WithArmor_ShouldReduceDamage()
    {
        var attacker = new CombatEntity
        {
            PhysicalDamage = 100,
            Accuracy = 1000,
            CriticalChance = 0
        };

        var defenderWithArmor = new CombatEntity
        {
            MaxLife = 1000,
            CurrentLife = 1000,
            Armor = 100,
            Evasion = 0
        };

        var defenderWithoutArmor = new CombatEntity
        {
            MaxLife = 1000,
            CurrentLife = 1000,
            Armor = 0,
            Evasion = 0
        };

        _combatService.PerformAttack(attacker, defenderWithArmor);
        _combatService.PerformAttack(attacker, defenderWithoutArmor);

        Assert.True(defenderWithArmor.CurrentLife > defenderWithoutArmor.CurrentLife);
    }

    [Fact]
    public void TakeDamage_WithResistance_ShouldReduceElementalDamage()
    {
        var random = new Random(42);

        var damage = new DamageInstance
        {
            FireDamage = 100
        };

        var defender = new CombatEntity
        {
            MaxLife = 1000,
            CurrentLife = 1000,
            FireResistance = 50
        };

        var result = defender.TakeDamage(damage, random);

        Assert.Equal(50, result.FireDamage);
    }

    [Fact]
    public void SimulateCombat_ShouldProduceValidResult()
    {
        var character = _gameService.CreateCharacter("TestHero", Models.Characters.CharacterClass.Warrior);
        var player = _combatService.CreateCombatEntity(character);

        var monster = new Monster
        {
            Name = "Test Monster",
            Level = 1,
            MaxLife = 50,
            CurrentLife = 50,
            PhysicalDamage = 5,
            AttackSpeed = 1.0
        };

        var result = _combatService.SimulateCombat(player, monster, 10);

        Assert.NotNull(result);
        Assert.True(result.TimeElapsed > 0);
        Assert.True(result.PlayerAttacks.Count > 0 || result.MonsterAttacks.Count > 0);
    }

    [Fact]
    public void GenerateLoot_ShouldCreateItems()
    {
        var monster = new Monster
        {
            Level = 10,
            ItemDropChance = 100
        };

        var loot = _combatService.GenerateLoot(monster, 0, 0);

        Assert.NotEmpty(loot);
    }
}
