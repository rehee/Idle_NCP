namespace IdleGame.Core.Models.Maps;

/// <summary>
/// A map tile
/// </summary>
public class MapTile
{
    public int X { get; set; }
    public int Y { get; set; }
    public TileType Type { get; set; }
    public bool IsExplored { get; set; }
    public bool IsRevealed { get; set; }
    public string? MonsterId { get; set; }
    public string? LootId { get; set; }
}

/// <summary>
/// Types of map tiles
/// </summary>
public enum TileType
{
    Floor,
    Wall,
    Start,
    Exit,
    Chest,
    Shrine
}
