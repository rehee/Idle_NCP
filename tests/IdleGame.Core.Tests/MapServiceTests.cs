using IdleGame.Core.Models.Maps;
using IdleGame.Core.Services;

namespace IdleGame.Core.Tests;

public class MapServiceTests
{
    private readonly MapService _mapService;
    private readonly Random _random;

    public MapServiceTests()
    {
        _random = new Random(42);
        _mapService = new MapService(_random);
    }

    [Fact]
    public void GenerateMap_ShouldCreateValidMap()
    {
        var map = _mapService.GenerateMap("forest_clearing");

        Assert.NotNull(map);
        Assert.Equal("forest_clearing", map.DefinitionId);
        Assert.True(map.Width > 0);
        Assert.True(map.Height > 0);
        Assert.NotNull(map.Tiles);
    }

    [Fact]
    public void GenerateMap_ShouldHaveMonsters()
    {
        var map = _mapService.GenerateMap("forest_clearing");

        Assert.True(map.Monsters.Count > 0);
        Assert.True(map.TotalMonsters > 0);
    }

    [Fact]
    public void GenerateMap_ShouldHaveStartPosition()
    {
        var map = _mapService.GenerateMap("forest_clearing");

        Assert.Equal(1, map.PlayerX);
        Assert.Equal(1, map.PlayerY);

        var startTile = map.GetTile(1, 1);
        Assert.NotNull(startTile);
        Assert.Equal(TileType.Start, startTile.Type);
        Assert.True(startTile.IsExplored);
    }

    [Fact]
    public void MovePlayer_ShouldUpdatePosition()
    {
        var map = _mapService.GenerateMap("forest_clearing");
        var initialX = map.PlayerX;
        var initialY = map.PlayerY;

        var moved = map.MovePlayer(2, 1);

        Assert.True(moved);
        Assert.Equal(2, map.PlayerX);
        Assert.Equal(1, map.PlayerY);
    }

    [Fact]
    public void MovePlayer_ShouldNotMoveIntoWall()
    {
        var map = _mapService.GenerateMap("forest_clearing");

        var moved = map.MovePlayer(0, 0); // Edge is wall

        Assert.False(moved);
    }

    [Fact]
    public void GetAvailableMaps_ShouldReturnMapsForLevel()
    {
        var mapsLevel1 = _mapService.GetAvailableMaps(1).ToList();
        var mapsLevel50 = _mapService.GetAvailableMaps(50).ToList();

        Assert.NotEmpty(mapsLevel1);
        Assert.True(mapsLevel50.Count >= mapsLevel1.Count);
    }

    [Fact]
    public void GetAllMaps_ShouldReturnAllMaps()
    {
        var maps = _mapService.GetAllMaps().ToList();

        Assert.NotEmpty(maps);
        Assert.True(maps.Count >= 4);
    }

    [Fact]
    public void GetMonstersAt_ShouldReturnMonstersAtPosition()
    {
        var map = _mapService.GenerateMap("forest_clearing");
        var monsterPosition = map.Monsters.Values.First();

        var monsters = map.GetMonstersAt(monsterPosition.X, monsterPosition.Y).ToList();

        Assert.NotEmpty(monsters);
    }

    [Fact]
    public void MapWithBoss_ShouldHaveBoss()
    {
        var map = _mapService.GenerateMap("dark_cave");

        var bosses = map.Monsters.Values.Where(m => m.Type == Models.Combat.MonsterType.Boss).ToList();

        Assert.NotEmpty(bosses);
    }
}
