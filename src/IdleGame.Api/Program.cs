using IdleGame.Core.Models.Characters;
using IdleGame.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Register game services
builder.Services.AddSingleton<GameService>();
builder.Services.AddSingleton<GameStateService>(sp => new GameStateService(sp.GetRequiredService<GameService>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

// Game API endpoints
var gameGroup = app.MapGroup("/api/game").WithTags("Game");

// Create a new game
gameGroup.MapPost("/new", (CreateGameRequest request, GameStateService stateService) =>
{
    var state = stateService.CreateNewGame(request.CharacterName, request.CharacterClass, request.UserId);
    return Results.Ok(new GameStateResponse(state));
})
.WithName("CreateGame")
.WithOpenApi();

// Get available maps
gameGroup.MapGet("/maps", (GameService gameService) =>
{
    var maps = gameService.MapService.GetAllMaps();
    return Results.Ok(maps);
})
.WithName("GetMaps")
.WithOpenApi();

// Start a map run
gameGroup.MapPost("/map/start", (StartMapRequest request, GameService gameService) =>
{
    var character = gameService.CreateCharacter("Player", CharacterClass.Warrior);
    var map = gameService.StartMapRun(character, request.MapId);
    return Results.Ok(map);
})
.WithName("StartMap")
.WithOpenApi();

// Generate a random item
gameGroup.MapGet("/item/generate", (int level, string? rarity, GameService gameService) =>
{
    var forcedRarity = rarity != null ? Enum.Parse<IdleGame.Core.Models.Items.ItemRarity>(rarity) : (IdleGame.Core.Models.Items.ItemRarity?)null;
    var item = gameService.ItemService.GenerateItem(level, forcedRarity);
    return Results.Ok(item);
})
.WithName("GenerateItem")
.WithOpenApi();

// Get item bases
gameGroup.MapGet("/items/bases", (GameService gameService) =>
{
    var bases = gameService.ItemService.GetAllItemBases();
    return Results.Ok(bases);
})
.WithName("GetItemBases")
.WithOpenApi();

app.Run();

// Request/Response DTOs
record CreateGameRequest(string CharacterName, CharacterClass CharacterClass, string? UserId = null);
record StartMapRequest(string MapId);

record GameStateResponse(
    string Id,
    string? UserId,
    CharacterResponse? Character,
    DateTime CreatedAt,
    DateTime LastPlayedAt)
{
    public GameStateResponse(GameState state) : this(
        state.Id,
        state.UserId,
        state.Character != null ? new CharacterResponse(state.Character) : null,
        state.CreatedAt,
        state.LastPlayedAt)
    { }
}

record CharacterResponse(
    string Id,
    string Name,
    int Level,
    long Experience,
    CharacterClass Class,
    long Gold)
{
    public CharacterResponse(Character character) : this(
        character.Id,
        character.Name,
        character.Level,
        character.Experience,
        character.Class,
        character.Inventory.Gold)
    { }
}
