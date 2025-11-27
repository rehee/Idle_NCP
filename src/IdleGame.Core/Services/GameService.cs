using IdleGame.Core.Models.Characters;
using IdleGame.Core.Models.Combat;
using IdleGame.Core.Models.Crafting;
using IdleGame.Core.Models.Items;
using IdleGame.Core.Models.Maps;

namespace IdleGame.Core.Services;

/// <summary>
/// Main game service that orchestrates gameplay
/// </summary>
public class GameService
{
    private readonly AffixService _affixService;
    private readonly ItemService _itemService;
    private readonly CraftingService _craftingService;
    private readonly CombatService _combatService;
    private readonly MapService _mapService;
    private readonly Random _random;

    public GameService(Random? random = null)
    {
        _random = random ?? new Random();
        _affixService = new AffixService(_random);
        _itemService = new ItemService(_affixService, _random);
        _craftingService = new CraftingService(_affixService, _itemService, _random);
        _combatService = new CombatService(_itemService, _random);
        _mapService = new MapService(_random);
    }

    // Service accessors
    public AffixService AffixService => _affixService;
    public ItemService ItemService => _itemService;
    public CraftingService CraftingService => _craftingService;
    public CombatService CombatService => _combatService;
    public MapService MapService => _mapService;

    /// <summary>
    /// Create a new character
    /// </summary>
    public Character CreateCharacter(string name, CharacterClass characterClass)
    {
        var character = new Character
        {
            Name = name,
            Class = characterClass,
            Level = 1,
            BaseStats = GetStartingStats(characterClass)
        };

        // Give starter equipment
        EquipStarterGear(character);

        return character;
    }

    private CharacterStats GetStartingStats(CharacterClass characterClass)
    {
        var stats = new CharacterStats
        {
            MaxLife = 100,
            CurrentLife = 100,
            MaxMana = 50,
            CurrentMana = 50,
            LifeRegeneration = 1,
            ManaRegeneration = 1,
            PhysicalDamage = 5
        };

        switch (characterClass)
        {
            case CharacterClass.Warrior:
                stats.Strength = 15;
                stats.Dexterity = 10;
                stats.Intelligence = 5;
                stats.Vitality = 12;
                stats.MaxLife = 120;
                stats.CurrentLife = 120;
                break;
            case CharacterClass.Ranger:
                stats.Strength = 10;
                stats.Dexterity = 15;
                stats.Intelligence = 8;
                stats.Vitality = 9;
                stats.CriticalChance = 8;
                break;
            case CharacterClass.Mage:
                stats.Strength = 5;
                stats.Dexterity = 8;
                stats.Intelligence = 15;
                stats.Vitality = 7;
                stats.MaxMana = 80;
                stats.CurrentMana = 80;
                break;
            case CharacterClass.Rogue:
                stats.Strength = 10;
                stats.Dexterity = 14;
                stats.Intelligence = 10;
                stats.Vitality = 8;
                stats.CriticalChance = 10;
                stats.AttackSpeed = 1.2;
                break;
        }

        return stats;
    }

    private void EquipStarterGear(Character character)
    {
        // Generate starter weapon
        var weaponBase = character.Class switch
        {
            CharacterClass.Warrior => _itemService.GetItemBase("iron_sword"),
            CharacterClass.Ranger => _itemService.GetItemBase("wooden_bow"),
            CharacterClass.Mage => _itemService.GetItemBase("oak_staff"),
            CharacterClass.Rogue => _itemService.GetItemBase("iron_dagger"),
            _ => _itemService.GetItemBase("iron_sword")
        };

        if (weaponBase != null)
        {
            var weapon = _itemService.CreateItem(weaponBase, 1, ItemRarity.Normal);
            character.Equipment.Equip(weapon);
        }

        // Generate starter armor
        var chestBase = character.Class switch
        {
            CharacterClass.Warrior => _itemService.GetItemBase("iron_plate"),
            CharacterClass.Ranger => _itemService.GetItemBase("leather_vest"),
            CharacterClass.Mage => _itemService.GetItemBase("silk_robe"),
            CharacterClass.Rogue => _itemService.GetItemBase("leather_vest"),
            _ => _itemService.GetItemBase("iron_plate")
        };

        if (chestBase != null)
        {
            var chest = _itemService.CreateItem(chestBase, 1, ItemRarity.Normal);
            character.Equipment.Equip(chest);
        }
    }

    /// <summary>
    /// Start a new map run
    /// </summary>
    public MapInstance StartMapRun(Character character, string mapDefinitionId)
    {
        var levelModifier = Math.Max(0, character.Level - 1);
        return _mapService.GenerateMap(mapDefinitionId, levelModifier / 5);
    }

    /// <summary>
    /// Simulate idle combat for a duration
    /// </summary>
    public IdleResult SimulateIdle(Character character, MapInstance map, TimeSpan duration)
    {
        var result = new IdleResult
        {
            Duration = duration,
            StartLevel = character.Level
        };

        var playerEntity = _combatService.CreateCombatEntity(character);
        var stats = character.CalculateTotalStats();
        var remainingSeconds = duration.TotalSeconds;

        while (remainingSeconds > 0 && playerEntity.IsAlive)
        {
            // Find monsters to fight
            var monstersAtPosition = map.GetMonstersAt(map.PlayerX, map.PlayerY)
                .Where(m => m.IsAlive)
                .ToList();

            if (monstersAtPosition.Count > 0)
            {
                // Fight monsters at current position
                foreach (var monster in monstersAtPosition)
                {
                    if (remainingSeconds <= 0 || !playerEntity.IsAlive) break;

                    var combatResult = _combatService.SimulateCombat(playerEntity, monster, remainingSeconds);
                    remainingSeconds -= combatResult.TimeElapsed;

                    if (combatResult.MonsterKilled)
                    {
                        result.MonstersKilled++;
                        result.ExperienceGained += monster.ExperienceReward;
                        result.GoldGained += monster.GoldReward;
                        map.MonstersKilled++;

                        // Generate loot
                        var loot = _combatService.GenerateLoot(monster, stats.ItemQuantity, stats.ItemRarity);
                        result.ItemsFound.AddRange(loot);

                        // Generate currency drops
                        var currencies = _combatService.GenerateCurrencyDrops(monster);
                        foreach (var currency in currencies)
                        {
                            var existing = result.CurrencyGained.FirstOrDefault(c => c.Type == currency.Type);
                            if (existing != null)
                                existing.Add(currency.Amount);
                            else
                                result.CurrencyGained.Add(currency);
                        }

                        // Check if boss
                        if (monster.Type == MonsterType.Boss)
                        {
                            map.BossKilled = true;
                        }
                    }

                    if (combatResult.PlayerDied)
                    {
                        result.PlayerDied = true;
                        break;
                    }
                }
            }
            else
            {
                // Move to next position with monsters
                var moved = MoveToNextTarget(map);
                if (!moved)
                {
                    // No more monsters or can't reach them
                    break;
                }
                remainingSeconds -= 1; // 1 second to move
            }
        }

        // Apply rewards to character
        if (result.ExperienceGained > 0)
        {
            var scaledExp = (long)(result.ExperienceGained * (1 + stats.ExperienceGain / 100));
            result.LeveledUp = character.AddExperience(scaledExp);
        }

        character.Inventory.Gold += (long)(result.GoldGained * (1 + stats.GoldFind / 100));

        foreach (var item in result.ItemsFound)
        {
            if (!character.Inventory.IsFull)
            {
                character.Inventory.AddItem(item);
            }
        }

        foreach (var currency in result.CurrencyGained)
        {
            character.Inventory.AddCurrency(currency.Type, currency.Amount);
        }

        result.EndLevel = character.Level;
        result.MapCompleted = map.IsCompleted;
        map.TimeSpent += duration;

        return result;
    }

    private bool MoveToNextTarget(MapInstance map)
    {
        // Simple pathfinding - find nearest monster
        var nearestMonster = map.Monsters.Values
            .Where(m => m.IsAlive)
            .OrderBy(m => Math.Abs(m.X - map.PlayerX) + Math.Abs(m.Y - map.PlayerY))
            .FirstOrDefault();

        if (nearestMonster == null) return false;

        // Move towards monster
        var dx = nearestMonster.X - map.PlayerX;
        var dy = nearestMonster.Y - map.PlayerY;

        if (Math.Abs(dx) > Math.Abs(dy))
        {
            map.MovePlayer(map.PlayerX + Math.Sign(dx), map.PlayerY);
        }
        else
        {
            map.MovePlayer(map.PlayerX, map.PlayerY + Math.Sign(dy));
        }

        return true;
    }

    /// <summary>
    /// Use currency on an item
    /// </summary>
    public CraftingResult Craft(Character character, Item item, CurrencyType currency)
    {
        if (!character.Inventory.RemoveCurrency(currency, 1))
        {
            return new CraftingResult { Success = false, Message = "Not enough currency" };
        }

        return _craftingService.ApplyCurrency(item, currency);
    }

    /// <summary>
    /// Equip an item from inventory
    /// </summary>
    public Item? EquipItem(Character character, Item item)
    {
        if (!character.Inventory.Items.Contains(item))
            return null;

        var oldItem = character.Equipment.Equip(item);
        character.Inventory.RemoveItem(item);

        if (oldItem != null)
        {
            character.Inventory.AddItem(oldItem);
        }

        return oldItem;
    }

    /// <summary>
    /// Sell an item for gold
    /// </summary>
    public long SellItem(Character character, Item item)
    {
        if (!character.Inventory.Items.Contains(item))
            return 0;

        var value = CalculateItemValue(item);
        character.Inventory.RemoveItem(item);
        character.Inventory.Gold += value;

        return value;
    }

    private long CalculateItemValue(Item item)
    {
        var baseValue = item.ItemLevel * 10;
        var rarityMultiplier = item.Rarity switch
        {
            ItemRarity.Normal => 1,
            ItemRarity.Magic => 3,
            ItemRarity.Rare => 10,
            ItemRarity.Legendary => 50,
            ItemRarity.Unique => 100,
            ItemRarity.Artifact => 200,
            _ => 1
        };

        return baseValue * rarityMultiplier;
    }
}

/// <summary>
/// Result of idle simulation
/// </summary>
public class IdleResult
{
    public TimeSpan Duration { get; set; }
    public int MonstersKilled { get; set; }
    public long ExperienceGained { get; set; }
    public long GoldGained { get; set; }
    public List<Item> ItemsFound { get; set; } = new();
    public List<Currency> CurrencyGained { get; set; } = new();
    public bool PlayerDied { get; set; }
    public bool LeveledUp { get; set; }
    public int StartLevel { get; set; }
    public int EndLevel { get; set; }
    public bool MapCompleted { get; set; }
}
