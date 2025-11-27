namespace IdleGame.Core.Models.Characters;

/// <summary>
/// A player character
/// </summary>
public class Character
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public int Level { get; set; } = 1;
    public long Experience { get; set; }
    public CharacterClass Class { get; set; }

    /// <summary>
    /// Base stats before equipment
    /// </summary>
    public CharacterStats BaseStats { get; set; } = new();

    /// <summary>
    /// Equipped items
    /// </summary>
    public Equipment Equipment { get; set; } = new();

    /// <summary>
    /// Player inventory
    /// </summary>
    public Inventory Inventory { get; set; } = new();

    /// <summary>
    /// Experience required for next level
    /// </summary>
    public long ExperienceToNextLevel => CalculateExperienceRequired(Level + 1);

    /// <summary>
    /// Calculate experience required for a level
    /// </summary>
    public static long CalculateExperienceRequired(int level)
    {
        // Exponential curve
        return (long)(100 * Math.Pow(1.15, level - 1));
    }

    /// <summary>
    /// Add experience and level up if needed
    /// </summary>
    public bool AddExperience(long amount)
    {
        Experience += amount;
        var leveledUp = false;

        while (Experience >= ExperienceToNextLevel)
        {
            Experience -= ExperienceToNextLevel;
            Level++;
            OnLevelUp();
            leveledUp = true;
        }

        return leveledUp;
    }

    /// <summary>
    /// Called when character levels up
    /// </summary>
    protected virtual void OnLevelUp()
    {
        // Increase base stats based on class
        switch (Class)
        {
            case CharacterClass.Warrior:
                BaseStats.Strength += 3;
                BaseStats.Vitality += 2;
                BaseStats.Dexterity += 1;
                BaseStats.Intelligence += 1;
                break;
            case CharacterClass.Ranger:
                BaseStats.Dexterity += 3;
                BaseStats.Strength += 2;
                BaseStats.Vitality += 1;
                BaseStats.Intelligence += 1;
                break;
            case CharacterClass.Mage:
                BaseStats.Intelligence += 3;
                BaseStats.Vitality += 2;
                BaseStats.Dexterity += 1;
                BaseStats.Strength += 1;
                break;
            case CharacterClass.Rogue:
                BaseStats.Dexterity += 2;
                BaseStats.Strength += 2;
                BaseStats.Intelligence += 2;
                BaseStats.Vitality += 1;
                break;
        }

        BaseStats.MaxLife += 10;
        BaseStats.MaxMana += 5;
    }

    /// <summary>
    /// Calculate total stats including equipment
    /// </summary>
    public CharacterStats CalculateTotalStats()
    {
        var stats = BaseStats.Clone();

        // Apply equipment stats
        foreach (var item in Equipment.GetAllEquipped())
        {
            // Base item stats
            stats.Armor += item.BaseArmor;
            stats.Evasion += item.BaseEvasion;
            stats.EnergyShield += item.BaseEnergyShield;

            if (item.BaseMinDamage > 0)
            {
                stats.PhysicalDamage += (item.BaseMinDamage + item.BaseMaxDamage) / 2.0;
            }

            if (item.BaseAttackSpeed > 0)
            {
                stats.AttackSpeed = item.BaseAttackSpeed;
            }

            if (item.BaseCriticalChance > 0)
            {
                stats.CriticalChance = item.BaseCriticalChance;
            }

            // Apply affixes
            foreach (var modifier in item.GetAllModifiers())
            {
                stats.ApplyModifier(modifier);
            }
        }

        // Apply stat conversions
        stats.MaxLife += stats.Vitality * 5;
        stats.MaxMana += stats.Intelligence * 2;
        stats.PhysicalDamage += stats.Strength * 0.5;
        stats.CriticalChance += stats.Dexterity * 0.05;
        stats.Accuracy += stats.Dexterity * 2;

        return stats;
    }
}

/// <summary>
/// Character classes
/// </summary>
public enum CharacterClass
{
    Warrior,
    Ranger,
    Mage,
    Rogue
}
