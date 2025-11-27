# Idle RPG

An idle/incremental RPG game inspired by Diablo and Path of Exile, built with C# .NET 8 and Blazor.

## Features

### Equipment System
- **Rich Equipment**: Various equipment slots including helmet, chest, gloves, boots, belt, amulet, rings, weapons, and shields
- **Random Affixes**: Items can roll random prefixes and suffixes with varying stats
- **Crafting System**: Use currency items to modify and upgrade equipment

### Rarity System
| Rarity | Prefixes | Suffixes | Special |
|--------|----------|----------|---------|
| Normal | 0 | 0 | Base for crafting Artifacts |
| Magic | 1 | 1 | Enhanced affix effects (1.5x) |
| Rare | 1-3 | 1-3 | Standard random affixes |
| Legendary | 2 | 2 | Plus one Legendary affix |
| Unique | Fixed | Fixed | Predetermined stats |
| Artifact | 1-3 | 1-3 | Fixed + random affixes |

### Currency Items
- **Transmutation Orb**: Upgrade Normal → Magic
- **Alchemy Orb**: Upgrade to Rare
- **Augmentation Orb**: Add affix to Magic item
- **Alteration Orb**: Reroll Magic item affixes
- **Scouring Orb**: Remove all affixes
- **Chaos Orb**: Reroll Rare item affixes
- **Exalted Orb**: Add affix to Rare item
- **Divine Orb**: Reroll affix values
- **Legendary Orb**: Upgrade Rare → Legendary
- **Artifact Stone**: Create Artifact from Normal

### Combat System
- Idle/AFK combat mechanics
- 2D square map exploration
- Various monster types (Normal, Magic, Rare, Unique, Boss)
- Multiple damage types and resistances
- Critical strikes and evasion system

### Game Modes
- **Offline Mode**: Play locally on your device
- **Online Mode** (Future): Server-based 24/7 idle progression

## Project Structure

```
IdleGame/
├── src/
│   ├── IdleGame.Core/          # Platform-agnostic game logic
│   │   ├── Models/             # Game entities
│   │   │   ├── Items/          # Equipment and items
│   │   │   ├── Affixes/        # Stat modifiers
│   │   │   ├── Characters/     # Player character
│   │   │   ├── Combat/         # Combat entities
│   │   │   ├── Crafting/       # Currency and recipes
│   │   │   ├── Maps/           # Map system
│   │   │   └── Strategies/     # Automation rules
│   │   └── Services/           # Game services
│   ├── IdleGame.Shared/        # Shared Razor components
│   │   └── Components/         # Blazor UI components
│   ├── IdleGame.Api/           # Web API server
│   └── IdleGame.Web/           # Blazor WebAssembly client
└── tests/
    └── IdleGame.Core.Tests/    # Unit tests
```

## Technology Stack

- **Language**: C# 12
- **Framework**: .NET 8
- **UI**: Blazor WebAssembly
- **Testing**: xUnit

## Getting Started

### Prerequisites
- .NET 8 SDK or later

### Build
```bash
dotnet build
```

### Run Tests
```bash
dotnet test
```

### Run the Web Client
```bash
cd src/IdleGame.Web
dotnet run
```

### Run the API Server
```bash
cd src/IdleGame.Api
dotnet run
```

## Character Classes

- **Warrior**: High strength and vitality, melee focused
- **Ranger**: High dexterity, ranged combat with bows
- **Mage**: High intelligence, magical damage
- **Rogue**: Balanced stats with high critical chance

## Maps

1. **Forest Clearing** (Level 1+): Beginner area with wolves and goblins
2. **Dark Cave** (Level 10+): Cave monsters with Cave Troll boss
3. **Ancient Ruins** (Level 25+): Undead and demons with Ancient Guardian boss
4. **Infernal Pit** (Level 50+): End-game content with Pit Lord boss

## Contributing

Contributions are welcome! Please feel free to submit issues and pull requests.

## License

This project is open source. See LICENSE file for details.
