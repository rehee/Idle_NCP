using IdleGame.Core.Models.Characters;
using IdleGame.Core.Models.Combat;
using IdleGame.Core.Models.Crafting;
using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Services;

/// <summary>
/// Service for combat calculations
/// </summary>
public class CombatService
{
    private readonly ItemService _itemService;
    private readonly Random _random;

    public CombatService(ItemService itemService, Random? random = null)
    {
        _itemService = itemService;
        _random = random ?? new Random();
    }

    /// <summary>
    /// Create a combat entity from a character
    /// </summary>
    public CombatEntity CreateCombatEntity(Character character)
    {
        var stats = character.CalculateTotalStats();

        return new CombatEntity
        {
            Id = character.Id,
            Name = character.Name,
            Level = character.Level,
            MaxLife = stats.MaxLife,
            CurrentLife = stats.CurrentLife > 0 ? stats.CurrentLife : stats.MaxLife,
            PhysicalDamage = stats.PhysicalDamage,
            FireDamage = stats.FireDamage,
            ColdDamage = stats.ColdDamage,
            LightningDamage = stats.LightningDamage,
            ChaosDamage = stats.ChaosDamage,
            AttackSpeed = stats.AttackSpeed,
            CriticalChance = stats.CriticalChance,
            CriticalMultiplier = stats.CriticalMultiplier,
            Accuracy = stats.Accuracy,
            Armor = stats.Armor,
            Evasion = stats.Evasion,
            EnergyShield = stats.EnergyShield,
            BlockChance = stats.BlockChance,
            DodgeChance = stats.DodgeChance,
            FireResistance = stats.FireResistance,
            ColdResistance = stats.ColdResistance,
            LightningResistance = stats.LightningResistance,
            ChaosResistance = stats.ChaosResistance,
            LifeRegeneration = stats.LifeRegeneration
        };
    }

    /// <summary>
    /// Perform an attack
    /// </summary>
    public AttackResult PerformAttack(CombatEntity attacker, CombatEntity defender)
    {
        var result = new AttackResult
        {
            AttackerId = attacker.Id,
            DefenderId = defender.Id
        };

        // Check hit chance
        var hitChance = CalculateHitChance(attacker.Accuracy, defender.Evasion);
        if (_random.NextDouble() * 100 > hitChance)
        {
            result.Missed = true;
            return result;
        }

        // Calculate damage
        var isCritical = _random.NextDouble() * 100 < attacker.CriticalChance;
        var critMultiplier = isCritical ? attacker.CriticalMultiplier / 100 : 1.0;

        var damage = new DamageInstance
        {
            PhysicalDamage = attacker.PhysicalDamage * critMultiplier,
            FireDamage = attacker.FireDamage * critMultiplier,
            ColdDamage = attacker.ColdDamage * critMultiplier,
            LightningDamage = attacker.LightningDamage * critMultiplier,
            ChaosDamage = attacker.ChaosDamage * critMultiplier,
            IsCritical = isCritical
        };

        result.DamageResult = defender.TakeDamage(damage, _random);
        result.IsCritical = isCritical;

        return result;
    }

    /// <summary>
    /// Calculate hit chance
    /// </summary>
    private double CalculateHitChance(double accuracy, double evasion)
    {
        // Formula: accuracy / (accuracy + evasion * 0.25) * 100
        if (accuracy <= 0) return 5; // Minimum 5% hit chance
        return Math.Min(95, accuracy / (accuracy + evasion * 0.25) * 100);
    }

    /// <summary>
    /// Simulate combat for a duration
    /// </summary>
    public CombatSimulationResult SimulateCombat(CombatEntity player, Monster monster, double seconds)
    {
        var result = new CombatSimulationResult
        {
            PlayerStartLife = player.CurrentLife,
            MonsterStartLife = monster.CurrentLife
        };

        var elapsed = 0.0;
        var playerCooldown = 0.0;
        var monsterCooldown = 0.0;

        while (elapsed < seconds && player.IsAlive && monster.IsAlive)
        {
            var nextPlayerAttack = playerCooldown <= 0 ? 0 : playerCooldown;
            var nextMonsterAttack = monsterCooldown <= 0 ? 0 : monsterCooldown;
            var nextEvent = Math.Min(nextPlayerAttack, nextMonsterAttack);
            var regenTime = Math.Min(seconds - elapsed, Math.Max(0.1, nextEvent));

            // Regeneration
            player.Regenerate(regenTime);
            monster.Regenerate(regenTime);

            elapsed += regenTime;
            playerCooldown -= regenTime;
            monsterCooldown -= regenTime;

            if (elapsed >= seconds) break;

            // Player attacks
            if (playerCooldown <= 0)
            {
                var attackResult = PerformAttack(player, monster);
                result.PlayerAttacks.Add(attackResult);
                playerCooldown = 1.0 / player.AttackSpeed;

                if (!monster.IsAlive)
                {
                    result.MonsterKilled = true;
                    break;
                }
            }

            // Monster attacks
            if (monsterCooldown <= 0 && monster.IsAlive)
            {
                var attackResult = PerformAttack(monster, player);
                result.MonsterAttacks.Add(attackResult);
                monsterCooldown = 1.0 / monster.AttackSpeed;

                if (!player.IsAlive)
                {
                    result.PlayerDied = true;
                    break;
                }
            }
        }

        result.PlayerEndLife = player.CurrentLife;
        result.MonsterEndLife = monster.CurrentLife;
        result.TimeElapsed = elapsed;

        return result;
    }

    /// <summary>
    /// Generate loot from a killed monster
    /// </summary>
    public List<Item> GenerateLoot(Monster monster, double itemQuantityBonus = 0, double itemRarityBonus = 0)
    {
        var loot = new List<Item>();

        var dropChance = monster.ItemDropChance * (1 + itemQuantityBonus / 100);

        // Roll for item drops
        var numDrops = (int)(dropChance / 100);
        var remainingChance = dropChance % 100;

        if (_random.NextDouble() * 100 < remainingChance)
            numDrops++;

        for (var i = 0; i < numDrops; i++)
        {
            var item = _itemService.GenerateItem(monster.Level);
            loot.Add(item);
        }

        return loot;
    }

    /// <summary>
    /// Generate currency from a killed monster
    /// </summary>
    public List<Currency> GenerateCurrencyDrops(Monster monster)
    {
        var currencies = new List<Currency>();

        if (_random.NextDouble() * 100 < monster.CurrencyDropChance)
        {
            // Roll for currency type
            var currencyTypes = Enum.GetValues<CurrencyType>();
            var weights = new Dictionary<CurrencyType, int>
            {
                { CurrencyType.TransmutationOrb, 100 },
                { CurrencyType.AugmentationOrb, 80 },
                { CurrencyType.AlterationOrb, 70 },
                { CurrencyType.AlchemyOrb, 30 },
                { CurrencyType.ScouringOrb, 20 },
                { CurrencyType.ChaosOrb, 10 },
                { CurrencyType.ExaltedOrb, 2 },
                { CurrencyType.DivineOrb, 1 },
                { CurrencyType.LegendaryOrb, 1 },
                { CurrencyType.QualityScroll, 50 },
                { CurrencyType.IdentificationScroll, 100 }
            };

            var totalWeight = weights.Values.Sum();
            var roll = _random.Next(totalWeight);
            var cumulative = 0;

            foreach (var kvp in weights)
            {
                cumulative += kvp.Value;
                if (roll < cumulative)
                {
                    currencies.Add(new Currency(kvp.Key, 1));
                    break;
                }
            }
        }

        return currencies;
    }
}

/// <summary>
/// Result of an attack
/// </summary>
public class AttackResult
{
    public string AttackerId { get; set; } = string.Empty;
    public string DefenderId { get; set; } = string.Empty;
    public bool Missed { get; set; }
    public bool IsCritical { get; set; }
    public DamageResult? DamageResult { get; set; }
}

/// <summary>
/// Result of combat simulation
/// </summary>
public class CombatSimulationResult
{
    public double PlayerStartLife { get; set; }
    public double PlayerEndLife { get; set; }
    public double MonsterStartLife { get; set; }
    public double MonsterEndLife { get; set; }
    public double TimeElapsed { get; set; }
    public bool MonsterKilled { get; set; }
    public bool PlayerDied { get; set; }
    public List<AttackResult> PlayerAttacks { get; set; } = new();
    public List<AttackResult> MonsterAttacks { get; set; } = new();

    public double PlayerDamageTaken => PlayerStartLife - PlayerEndLife;
    public double MonsterDamageTaken => MonsterStartLife - MonsterEndLife;
    public double PlayerDPS => TimeElapsed > 0 ? MonsterDamageTaken / TimeElapsed : 0;
    public double MonsterDPS => TimeElapsed > 0 ? PlayerDamageTaken / TimeElapsed : 0;
}
