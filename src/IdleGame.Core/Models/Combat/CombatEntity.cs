namespace IdleGame.Core.Models.Combat;

/// <summary>
/// Represents an entity in combat (player or monster)
/// </summary>
public class CombatEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; }

    // Stats
    public double MaxLife { get; set; }
    public double CurrentLife { get; set; }
    public double PhysicalDamage { get; set; }
    public double FireDamage { get; set; }
    public double ColdDamage { get; set; }
    public double LightningDamage { get; set; }
    public double ChaosDamage { get; set; }
    public double AttackSpeed { get; set; } = 1.0;
    public double CriticalChance { get; set; } = 5.0;
    public double CriticalMultiplier { get; set; } = 150.0;
    public double Accuracy { get; set; } = 100;
    public double Armor { get; set; }
    public double Evasion { get; set; }
    public double EnergyShield { get; set; }
    public double BlockChance { get; set; }
    public double DodgeChance { get; set; }
    public double FireResistance { get; set; }
    public double ColdResistance { get; set; }
    public double LightningResistance { get; set; }
    public double ChaosResistance { get; set; }
    public double LifeRegeneration { get; set; }

    /// <summary>
    /// Position on the map
    /// </summary>
    public int X { get; set; }
    public int Y { get; set; }

    /// <summary>
    /// Whether this entity is alive
    /// </summary>
    public bool IsAlive => CurrentLife > 0;

    /// <summary>
    /// Deal damage to this entity
    /// </summary>
    public DamageResult TakeDamage(DamageInstance damage, Random random)
    {
        var result = new DamageResult();

        // Check dodge
        if (random.NextDouble() * 100 < DodgeChance)
        {
            result.Dodged = true;
            return result;
        }

        // Check block
        if (random.NextDouble() * 100 < BlockChance)
        {
            result.Blocked = true;
            return result;
        }

        // Calculate damage after resistance
        var physicalDamage = CalculatePhysicalDamage(damage.PhysicalDamage);
        var fireDamage = damage.FireDamage * (1 - Math.Min(FireResistance, 75) / 100);
        var coldDamage = damage.ColdDamage * (1 - Math.Min(ColdResistance, 75) / 100);
        var lightningDamage = damage.LightningDamage * (1 - Math.Min(LightningResistance, 75) / 100);
        var chaosDamage = damage.ChaosDamage * (1 - Math.Min(ChaosResistance, 75) / 100);

        result.PhysicalDamage = physicalDamage;
        result.FireDamage = fireDamage;
        result.ColdDamage = coldDamage;
        result.LightningDamage = lightningDamage;
        result.ChaosDamage = chaosDamage;
        result.TotalDamage = physicalDamage + fireDamage + coldDamage + lightningDamage + chaosDamage;
        result.IsCritical = damage.IsCritical;

        CurrentLife -= result.TotalDamage;
        if (CurrentLife < 0) CurrentLife = 0;

        result.KilledTarget = !IsAlive;

        return result;
    }

    private double CalculatePhysicalDamage(double rawDamage)
    {
        // Armor formula: damage reduction = armor / (armor + 10 * damage)
        var damageReduction = Armor / (Armor + 10 * rawDamage);
        return rawDamage * (1 - damageReduction);
    }

    /// <summary>
    /// Regenerate life
    /// </summary>
    public void Regenerate(double seconds)
    {
        if (LifeRegeneration > 0)
        {
            CurrentLife = Math.Min(MaxLife, CurrentLife + LifeRegeneration * seconds);
        }
    }
}

/// <summary>
/// A damage instance
/// </summary>
public class DamageInstance
{
    public double PhysicalDamage { get; set; }
    public double FireDamage { get; set; }
    public double ColdDamage { get; set; }
    public double LightningDamage { get; set; }
    public double ChaosDamage { get; set; }
    public bool IsCritical { get; set; }

    public double TotalDamage => PhysicalDamage + FireDamage + ColdDamage + LightningDamage + ChaosDamage;
}

/// <summary>
/// Result of taking damage
/// </summary>
public class DamageResult
{
    public double PhysicalDamage { get; set; }
    public double FireDamage { get; set; }
    public double ColdDamage { get; set; }
    public double LightningDamage { get; set; }
    public double ChaosDamage { get; set; }
    public double TotalDamage { get; set; }
    public bool IsCritical { get; set; }
    public bool Dodged { get; set; }
    public bool Blocked { get; set; }
    public bool KilledTarget { get; set; }
}
