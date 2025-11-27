using IdleGame.Core.Models.Items;
using IdleGame.Core.Models.Crafting;

namespace IdleGame.Core.Models.Strategies;

/// <summary>
/// A strategy/automation rule
/// </summary>
public class Strategy
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsEnabled { get; set; } = true;
    public int Priority { get; set; }

    /// <summary>
    /// Conditions that must all be true
    /// </summary>
    public List<StrategyCondition> Conditions { get; set; } = new();

    /// <summary>
    /// Actions to execute when conditions are met
    /// </summary>
    public List<StrategyAction> Actions { get; set; } = new();
}

/// <summary>
/// A condition for a strategy
/// </summary>
public class StrategyCondition
{
    public ConditionType Type { get; set; }
    public string Target { get; set; } = string.Empty;
    public ComparisonOperator Operator { get; set; }
    public string Value { get; set; } = string.Empty;
}

public enum ConditionType
{
    // Player conditions
    PlayerLevel,
    PlayerLifePercent,
    PlayerManaPercent,

    // Item conditions
    ItemRarity,
    ItemLevel,
    ItemSlot,
    ItemHasAffix,
    InventoryFull,
    InventorySlots,

    // Currency conditions
    CurrencyAmount,
    GoldAmount,

    // Combat conditions
    MonsterType,
    MonsterLifePercent,
    InCombat,
    MapProgress,

    // Time conditions
    IdleTime,
    TotalPlayTime
}

public enum ComparisonOperator
{
    Equal,
    NotEqual,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual,
    Contains,
    NotContains
}

/// <summary>
/// An action to execute
/// </summary>
public class StrategyAction
{
    public ActionType Type { get; set; }
    public Dictionary<string, string> Parameters { get; set; } = new();
}

public enum ActionType
{
    // Item actions
    EquipItem,
    UnequipItem,
    SellItem,
    DropItem,
    UseOnItem,

    // Crafting actions
    UseCurrency,
    Craft,

    // Combat actions
    AttackTarget,
    UseSkill,
    Retreat,

    // Movement actions
    MoveTo,
    ExploreMap,
    EnterMap,
    LeaveMap,

    // Inventory actions
    SortInventory,
    StashItem,
    RetrieveFromStash,

    // Misc actions
    Rest,
    Wait
}
