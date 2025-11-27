using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Models.Combat;

/// <summary>
/// A monster definition
/// </summary>
public class MonsterDefinition
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public MonsterType Type { get; set; }
    public int BaseLevel { get; set; }

    // Base stats
    public double BaseLife { get; set; }
    public double BaseDamage { get; set; }
    public double BaseArmor { get; set; }
    public double BaseEvasion { get; set; }
    public double BaseAccuracy { get; set; } = 100;
    public double BaseAttackSpeed { get; set; } = 1.0;

    // Resistances
    public double FireResistance { get; set; }
    public double ColdResistance { get; set; }
    public double LightningResistance { get; set; }
    public double ChaosResistance { get; set; }

    // Damage types dealt
    public double PhysicalDamagePercent { get; set; } = 100;
    public double FireDamagePercent { get; set; }
    public double ColdDamagePercent { get; set; }
    public double LightningDamagePercent { get; set; }
    public double ChaosDamagePercent { get; set; }

    // Rewards
    public long BaseExperience { get; set; }
    public int BaseGold { get; set; }
    public double ItemDropChance { get; set; } = 10;
    public double CurrencyDropChance { get; set; } = 5;

    /// <summary>
    /// Possible items this monster can drop
    /// </summary>
    public List<string> PossibleDrops { get; set; } = new();
}

/// <summary>
/// Monster types
/// </summary>
public enum MonsterType
{
    Normal,
    Magic,
    Rare,
    Unique,
    Boss
}

/// <summary>
/// A monster instance in combat
/// </summary>
public class Monster : CombatEntity
{
    public string DefinitionId { get; set; } = string.Empty;
    public MonsterType Type { get; set; }
    public long ExperienceReward { get; set; }
    public int GoldReward { get; set; }
    public double ItemDropChance { get; set; }
    public double CurrencyDropChance { get; set; }
    public List<string> PossibleDrops { get; set; } = new();

    /// <summary>
    /// Create a monster from definition with level scaling
    /// </summary>
    public static Monster FromDefinition(MonsterDefinition def, int mapLevel, Random random)
    {
        var level = def.BaseLevel + mapLevel;
        var levelMultiplier = Math.Pow(1.1, level - 1);

        var monster = new Monster
        {
            DefinitionId = def.Id,
            Name = def.Name,
            Level = level,
            Type = def.Type,
            MaxLife = def.BaseLife * levelMultiplier,
            PhysicalDamage = def.BaseDamage * levelMultiplier * def.PhysicalDamagePercent / 100,
            FireDamage = def.BaseDamage * levelMultiplier * def.FireDamagePercent / 100,
            ColdDamage = def.BaseDamage * levelMultiplier * def.ColdDamagePercent / 100,
            LightningDamage = def.BaseDamage * levelMultiplier * def.LightningDamagePercent / 100,
            ChaosDamage = def.BaseDamage * levelMultiplier * def.ChaosDamagePercent / 100,
            Armor = def.BaseArmor * levelMultiplier,
            Evasion = def.BaseEvasion * levelMultiplier,
            Accuracy = def.BaseAccuracy * levelMultiplier,
            AttackSpeed = def.BaseAttackSpeed,
            FireResistance = def.FireResistance,
            ColdResistance = def.ColdResistance,
            LightningResistance = def.LightningResistance,
            ChaosResistance = def.ChaosResistance,
            ExperienceReward = (long)(def.BaseExperience * levelMultiplier),
            GoldReward = (int)(def.BaseGold * levelMultiplier),
            ItemDropChance = def.ItemDropChance,
            CurrencyDropChance = def.CurrencyDropChance,
            PossibleDrops = def.PossibleDrops.ToList()
        };

        // Apply modifiers based on monster type
        switch (def.Type)
        {
            case MonsterType.Magic:
                monster.MaxLife *= 2;
                monster.PhysicalDamage *= 1.5;
                monster.ExperienceReward *= 3;
                monster.GoldReward *= 2;
                monster.ItemDropChance *= 2;
                break;
            case MonsterType.Rare:
                monster.MaxLife *= 4;
                monster.PhysicalDamage *= 2;
                monster.ExperienceReward *= 6;
                monster.GoldReward *= 4;
                monster.ItemDropChance *= 4;
                break;
            case MonsterType.Unique:
                monster.MaxLife *= 8;
                monster.PhysicalDamage *= 3;
                monster.ExperienceReward *= 12;
                monster.GoldReward *= 8;
                monster.ItemDropChance *= 8;
                break;
            case MonsterType.Boss:
                monster.MaxLife *= 20;
                monster.PhysicalDamage *= 5;
                monster.ExperienceReward *= 50;
                monster.GoldReward *= 20;
                monster.ItemDropChance = 100;
                break;
        }

        monster.CurrentLife = monster.MaxLife;

        return monster;
    }
}
