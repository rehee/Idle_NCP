using IdleGame.Core.Models.Characters;
using IdleGame.Core.Models.Maps;
using IdleGame.Core.Models.Strategies;

namespace IdleGame.Core.Services;

/// <summary>
/// Game state that can be saved and loaded
/// </summary>
public class GameState
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastPlayedAt { get; set; } = DateTime.UtcNow;
    public TimeSpan TotalPlayTime { get; set; }

    /// <summary>
    /// Player character
    /// </summary>
    public Character? Character { get; set; }

    /// <summary>
    /// Current map (if any)
    /// </summary>
    public MapInstance? CurrentMap { get; set; }

    /// <summary>
    /// Completed maps
    /// </summary>
    public List<string> CompletedMaps { get; set; } = new();

    /// <summary>
    /// Player strategies
    /// </summary>
    public List<Strategy> Strategies { get; set; } = new();

    /// <summary>
    /// Statistics
    /// </summary>
    public GameStatistics Statistics { get; set; } = new();

    /// <summary>
    /// Whether the game is running on server (24/7 idle)
    /// </summary>
    public bool IsServerMode { get; set; }

    /// <summary>
    /// Last time idle rewards were calculated
    /// </summary>
    public DateTime LastIdleCalculation { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Game statistics
/// </summary>
public class GameStatistics
{
    public long TotalMonstersKilled { get; set; }
    public long TotalExperienceGained { get; set; }
    public long TotalGoldEarned { get; set; }
    public int TotalItemsFound { get; set; }
    public int TotalItemsCrafted { get; set; }
    public int TotalMapsCompleted { get; set; }
    public int HighestLevel { get; set; }
    public int TotalDeaths { get; set; }
    public TimeSpan TotalTimePlayed { get; set; }
}

/// <summary>
/// Service for managing game state
/// </summary>
public class GameStateService
{
    private readonly GameService _gameService;

    public GameStateService(GameService gameService)
    {
        _gameService = gameService;
    }

    /// <summary>
    /// Create a new game state
    /// </summary>
    public GameState CreateNewGame(string characterName, CharacterClass characterClass, string? userId = null)
    {
        var character = _gameService.CreateCharacter(characterName, characterClass);

        return new GameState
        {
            UserId = userId,
            Character = character,
            IsServerMode = userId != null
        };
    }

    /// <summary>
    /// Calculate and apply idle rewards since last calculation
    /// </summary>
    public IdleResult? CalculateIdleRewards(GameState state)
    {
        if (state.Character == null || state.CurrentMap == null)
            return null;

        var now = DateTime.UtcNow;
        var timeSinceLastCalculation = now - state.LastIdleCalculation;

        if (timeSinceLastCalculation.TotalSeconds < 1)
            return null;

        // Limit idle time to prevent excessive rewards
        var maxIdleTime = state.IsServerMode ? TimeSpan.FromHours(24) : TimeSpan.FromHours(1);
        if (timeSinceLastCalculation > maxIdleTime)
            timeSinceLastCalculation = maxIdleTime;

        var result = _gameService.SimulateIdle(state.Character, state.CurrentMap, timeSinceLastCalculation);

        // Update statistics
        state.Statistics.TotalMonstersKilled += result.MonstersKilled;
        state.Statistics.TotalExperienceGained += result.ExperienceGained;
        state.Statistics.TotalGoldEarned += result.GoldGained;
        state.Statistics.TotalItemsFound += result.ItemsFound.Count;
        state.Statistics.TotalTimePlayed += timeSinceLastCalculation;

        if (result.PlayerDied)
            state.Statistics.TotalDeaths++;

        if (state.Character.Level > state.Statistics.HighestLevel)
            state.Statistics.HighestLevel = state.Character.Level;

        if (result.MapCompleted && !state.CompletedMaps.Contains(state.CurrentMap.DefinitionId))
        {
            state.CompletedMaps.Add(state.CurrentMap.DefinitionId);
            state.Statistics.TotalMapsCompleted++;
        }

        state.LastIdleCalculation = now;
        state.LastPlayedAt = now;
        state.TotalPlayTime += timeSinceLastCalculation;

        return result;
    }

    /// <summary>
    /// Start a new map run
    /// </summary>
    public MapInstance? StartMap(GameState state, string mapDefinitionId)
    {
        if (state.Character == null) return null;

        state.CurrentMap = _gameService.StartMapRun(state.Character, mapDefinitionId);
        state.LastIdleCalculation = DateTime.UtcNow;

        return state.CurrentMap;
    }

    /// <summary>
    /// Leave current map
    /// </summary>
    public void LeaveMap(GameState state)
    {
        state.CurrentMap = null;
    }

    /// <summary>
    /// Get available maps for the player
    /// </summary>
    public IEnumerable<MapDefinition> GetAvailableMaps(GameState state)
    {
        if (state.Character == null)
            return Enumerable.Empty<MapDefinition>();

        return _gameService.MapService.GetAvailableMaps(state.Character.Level);
    }
}
