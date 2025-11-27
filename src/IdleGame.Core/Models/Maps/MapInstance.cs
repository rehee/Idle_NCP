using IdleGame.Core.Models.Combat;

namespace IdleGame.Core.Models.Maps;

/// <summary>
/// A game map definition
/// </summary>
public class MapDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int BaseLevel { get; set; }
    public int Width { get; set; } = 20;
    public int Height { get; set; } = 20;

    /// <summary>
    /// Monster spawn rate (monsters per tile)
    /// </summary>
    public double MonsterDensity { get; set; } = 0.3;

    /// <summary>
    /// Possible monsters that can spawn
    /// </summary>
    public List<string> PossibleMonsters { get; set; } = new();

    /// <summary>
    /// Boss monster ID if any
    /// </summary>
    public string? BossId { get; set; }

    /// <summary>
    /// Map modifiers
    /// </summary>
    public List<MapModifier> Modifiers { get; set; } = new();
}

/// <summary>
/// Map instance - an active map being explored
/// </summary>
public class MapInstance
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string DefinitionId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    /// <summary>
    /// 2D grid of tiles
    /// </summary>
    public MapTile[,] Tiles { get; set; } = new MapTile[0, 0];

    /// <summary>
    /// Active monsters on the map
    /// </summary>
    public Dictionary<string, Monster> Monsters { get; set; } = new();

    /// <summary>
    /// Map progress
    /// </summary>
    public int MonstersKilled { get; set; }
    public int TotalMonsters { get; set; }
    public bool BossKilled { get; set; }
    public bool IsCompleted => MonstersKilled >= TotalMonsters && (string.IsNullOrEmpty(DefinitionId) || BossKilled);

    /// <summary>
    /// Player position
    /// </summary>
    public int PlayerX { get; set; }
    public int PlayerY { get; set; }

    /// <summary>
    /// Time spent on this map
    /// </summary>
    public TimeSpan TimeSpent { get; set; }

    /// <summary>
    /// Map modifiers
    /// </summary>
    public List<MapModifier> Modifiers { get; set; } = new();

    /// <summary>
    /// Get tile at position
    /// </summary>
    public MapTile? GetTile(int x, int y)
    {
        if (x < 0 || x >= Width || y < 0 || y >= Height)
            return null;
        return Tiles[x, y];
    }

    /// <summary>
    /// Get monsters at position
    /// </summary>
    public IEnumerable<Monster> GetMonstersAt(int x, int y)
    {
        return Monsters.Values.Where(m => m.X == x && m.Y == y && m.IsAlive);
    }

    /// <summary>
    /// Get adjacent positions
    /// </summary>
    public IEnumerable<(int x, int y)> GetAdjacentPositions(int x, int y)
    {
        if (x > 0) yield return (x - 1, y);
        if (x < Width - 1) yield return (x + 1, y);
        if (y > 0) yield return (x, y - 1);
        if (y < Height - 1) yield return (x, y + 1);
    }

    /// <summary>
    /// Move player to position
    /// </summary>
    public bool MovePlayer(int x, int y)
    {
        var tile = GetTile(x, y);
        if (tile == null || tile.Type == TileType.Wall)
            return false;

        PlayerX = x;
        PlayerY = y;
        tile.IsExplored = true;
        RevealAround(x, y);

        return true;
    }

    /// <summary>
    /// Reveal tiles around a position
    /// </summary>
    public void RevealAround(int x, int y, int radius = 1)
    {
        for (var dx = -radius; dx <= radius; dx++)
        {
            for (var dy = -radius; dy <= radius; dy++)
            {
                var tile = GetTile(x + dx, y + dy);
                if (tile != null)
                    tile.IsRevealed = true;
            }
        }
    }
}

/// <summary>
/// Map modifiers that affect gameplay
/// </summary>
public class MapModifier
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public MapModifierType Type { get; set; }
    public double Value { get; set; }
}

public enum MapModifierType
{
    MonsterLife,
    MonsterDamage,
    MonsterSpeed,
    PlayerDamageReduction,
    PlayerDamageIncrease,
    ExperienceBonus,
    ItemQuantityBonus,
    ItemRarityBonus
}
