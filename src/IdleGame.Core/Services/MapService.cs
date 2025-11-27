using IdleGame.Core.Models.Combat;
using IdleGame.Core.Models.Maps;

namespace IdleGame.Core.Services;

/// <summary>
/// Service for map generation and management
/// </summary>
public class MapService
{
    private readonly List<MapDefinition> _mapDefinitions = new();
    private readonly List<MonsterDefinition> _monsterDefinitions = new();
    private readonly Random _random;

    public MapService(Random? random = null)
    {
        _random = random ?? new Random();
        InitializeDefaultMaps();
        InitializeDefaultMonsters();
    }

    private void InitializeDefaultMaps()
    {
        _mapDefinitions.Add(new MapDefinition
        {
            Id = "forest_clearing",
            Name = "Forest Clearing",
            Description = "A peaceful clearing in the forest. Weak monsters roam here.",
            BaseLevel = 1,
            Width = 15,
            Height = 15,
            MonsterDensity = 0.2,
            PossibleMonsters = new List<string> { "forest_wolf", "goblin", "forest_spider" }
        });

        _mapDefinitions.Add(new MapDefinition
        {
            Id = "dark_cave",
            Name = "Dark Cave",
            Description = "A dark and dangerous cave system.",
            BaseLevel = 10,
            Width = 20,
            Height = 20,
            MonsterDensity = 0.3,
            PossibleMonsters = new List<string> { "cave_bat", "skeleton", "cave_spider", "zombie" },
            BossId = "cave_troll"
        });

        _mapDefinitions.Add(new MapDefinition
        {
            Id = "ancient_ruins",
            Name = "Ancient Ruins",
            Description = "Mysterious ruins filled with powerful creatures.",
            BaseLevel = 25,
            Width = 25,
            Height = 25,
            MonsterDensity = 0.35,
            PossibleMonsters = new List<string> { "skeleton_warrior", "ghost", "golem", "demon" },
            BossId = "ancient_guardian"
        });

        _mapDefinitions.Add(new MapDefinition
        {
            Id = "infernal_pit",
            Name = "Infernal Pit",
            Description = "A hellish realm of fire and demons.",
            BaseLevel = 50,
            Width = 30,
            Height = 30,
            MonsterDensity = 0.4,
            PossibleMonsters = new List<string> { "fire_demon", "hellhound", "succubus", "imp" },
            BossId = "pit_lord"
        });
    }

    private void InitializeDefaultMonsters()
    {
        // Forest monsters
        AddMonster("forest_wolf", "Forest Wolf", MonsterType.Normal, 1, 30, 5, 5, 0, 50, 10, 5);
        AddMonster("goblin", "Goblin", MonsterType.Normal, 1, 25, 6, 3, 5, 40, 8, 4);
        AddMonster("forest_spider", "Forest Spider", MonsterType.Normal, 2, 20, 7, 2, 10, 35, 7, 3);

        // Cave monsters
        AddMonster("cave_bat", "Cave Bat", MonsterType.Normal, 10, 50, 12, 5, 30, 60, 15, 8);
        AddMonster("skeleton", "Skeleton", MonsterType.Normal, 10, 60, 15, 20, 0, 70, 18, 10);
        AddMonster("cave_spider", "Cave Spider", MonsterType.Magic, 12, 80, 20, 10, 25, 120, 30, 15);
        AddMonster("zombie", "Zombie", MonsterType.Normal, 11, 100, 18, 15, 0, 80, 20, 12);
        AddMonster("cave_troll", "Cave Troll", MonsterType.Boss, 15, 500, 50, 100, 0, 1000, 200, 100);

        // Ruins monsters
        AddMonster("skeleton_warrior", "Skeleton Warrior", MonsterType.Normal, 25, 150, 40, 50, 10, 200, 50, 25);
        AddMonster("ghost", "Ghost", MonsterType.Magic, 26, 100, 60, 0, 80, 300, 80, 40);
        AddMonster("golem", "Stone Golem", MonsterType.Rare, 28, 400, 35, 150, 0, 500, 120, 60);
        AddMonster("demon", "Lesser Demon", MonsterType.Magic, 27, 200, 70, 30, 20, 400, 100, 50);
        AddMonster("ancient_guardian", "Ancient Guardian", MonsterType.Boss, 30, 2000, 150, 200, 50, 5000, 1000, 500);

        // Infernal monsters
        AddMonster("fire_demon", "Fire Demon", MonsterType.Normal, 50, 400, 120, 50, 30, 600, 150, 80);
        AddMonster("hellhound", "Hellhound", MonsterType.Normal, 50, 350, 140, 30, 50, 550, 140, 75);
        AddMonster("succubus", "Succubus", MonsterType.Magic, 52, 300, 180, 20, 60, 800, 200, 100);
        AddMonster("imp", "Imp", MonsterType.Normal, 48, 200, 100, 15, 40, 400, 100, 50);
        AddMonster("pit_lord", "Pit Lord", MonsterType.Boss, 55, 10000, 500, 300, 100, 25000, 5000, 2500);
    }

    private void AddMonster(string id, string name, MonsterType type, int level, double life, double damage,
        double armor, double evasion, long exp, int gold, double dropChance)
    {
        _monsterDefinitions.Add(new MonsterDefinition
        {
            Id = id,
            Name = name,
            Type = type,
            BaseLevel = level,
            BaseLife = life,
            BaseDamage = damage,
            BaseArmor = armor,
            BaseEvasion = evasion,
            BaseExperience = exp,
            BaseGold = gold,
            ItemDropChance = dropChance
        });
    }

    /// <summary>
    /// Generate a map instance
    /// </summary>
    public MapInstance GenerateMap(string definitionId, int levelModifier = 0)
    {
        var definition = _mapDefinitions.FirstOrDefault(m => m.Id == definitionId);
        if (definition == null)
            throw new ArgumentException($"Map definition not found: {definitionId}");

        return GenerateMap(definition, levelModifier);
    }

    /// <summary>
    /// Generate a map instance from definition
    /// </summary>
    public MapInstance GenerateMap(MapDefinition definition, int levelModifier = 0)
    {
        var instance = new MapInstance
        {
            DefinitionId = definition.Id,
            Name = definition.Name,
            Level = definition.BaseLevel + levelModifier,
            Width = definition.Width,
            Height = definition.Height,
            Tiles = new MapTile[definition.Width, definition.Height],
            Modifiers = definition.Modifiers.ToList()
        };

        // Generate tiles
        GenerateTiles(instance);

        // Spawn monsters
        SpawnMonsters(instance, definition);

        return instance;
    }

    private void GenerateTiles(MapInstance instance)
    {
        // Simple room generation
        for (var x = 0; x < instance.Width; x++)
        {
            for (var y = 0; y < instance.Height; y++)
            {
                var tile = new MapTile
                {
                    X = x,
                    Y = y,
                    Type = TileType.Floor
                };

                // Create walls at edges
                if (x == 0 || y == 0 || x == instance.Width - 1 || y == instance.Height - 1)
                {
                    tile.Type = TileType.Wall;
                }

                // Add some random walls inside (10% chance)
                if (tile.Type == TileType.Floor && _random.NextDouble() < 0.1)
                {
                    tile.Type = TileType.Wall;
                }

                instance.Tiles[x, y] = tile;
            }
        }

        // Set start position
        instance.Tiles[1, 1].Type = TileType.Start;
        instance.PlayerX = 1;
        instance.PlayerY = 1;
        instance.Tiles[1, 1].IsExplored = true;
        instance.RevealAround(1, 1, 2);

        // Set exit position
        instance.Tiles[instance.Width - 2, instance.Height - 2].Type = TileType.Exit;
    }

    private void SpawnMonsters(MapInstance instance, MapDefinition definition)
    {
        var floorTiles = new List<MapTile>();
        for (var x = 0; x < instance.Width; x++)
        {
            for (var y = 0; y < instance.Height; y++)
            {
                var tile = instance.Tiles[x, y];
                if (tile.Type == TileType.Floor && !(x == 1 && y == 1))
                {
                    floorTiles.Add(tile);
                }
            }
        }

        // Calculate number of monsters
        var monsterCount = (int)(floorTiles.Count * definition.MonsterDensity);
        instance.TotalMonsters = monsterCount;

        // Spawn normal monsters
        var possibleMonsters = definition.PossibleMonsters
            .Select(id => _monsterDefinitions.FirstOrDefault(m => m.Id == id))
            .Where(m => m != null)
            .Cast<MonsterDefinition>()
            .ToList();

        if (possibleMonsters.Count == 0) return;

        for (var i = 0; i < monsterCount && floorTiles.Count > 0; i++)
        {
            var tileIndex = _random.Next(floorTiles.Count);
            var tile = floorTiles[tileIndex];
            floorTiles.RemoveAt(tileIndex);

            var monsterDef = possibleMonsters[_random.Next(possibleMonsters.Count)];
            var monster = Monster.FromDefinition(monsterDef, instance.Level, _random);
            monster.X = tile.X;
            monster.Y = tile.Y;
            tile.MonsterId = monster.Id;

            instance.Monsters[monster.Id] = monster;
        }

        // Spawn boss if defined
        if (!string.IsNullOrEmpty(definition.BossId))
        {
            var bossDef = _monsterDefinitions.FirstOrDefault(m => m.Id == definition.BossId);
            if (bossDef != null && floorTiles.Count > 0)
            {
                var bossX = instance.Width - 2;
                var bossY = instance.Height - 2;
                var boss = Monster.FromDefinition(bossDef, instance.Level, _random);
                boss.X = bossX;
                boss.Y = bossY;

                instance.Monsters[boss.Id] = boss;
                instance.TotalMonsters++;
            }
        }
    }

    /// <summary>
    /// Get available maps
    /// </summary>
    public IEnumerable<MapDefinition> GetAvailableMaps(int playerLevel)
    {
        return _mapDefinitions.Where(m => m.BaseLevel <= playerLevel + 5);
    }

    /// <summary>
    /// Get all map definitions
    /// </summary>
    public IEnumerable<MapDefinition> GetAllMaps()
    {
        return _mapDefinitions;
    }

    /// <summary>
    /// Get monster definition
    /// </summary>
    public MonsterDefinition? GetMonsterDefinition(string id)
    {
        return _monsterDefinitions.FirstOrDefault(m => m.Id == id);
    }

    /// <summary>
    /// Add custom map definition
    /// </summary>
    public void AddMapDefinition(MapDefinition definition)
    {
        _mapDefinitions.Add(definition);
    }

    /// <summary>
    /// Add custom monster definition
    /// </summary>
    public void AddMonsterDefinition(MonsterDefinition definition)
    {
        _monsterDefinitions.Add(definition);
    }
}
