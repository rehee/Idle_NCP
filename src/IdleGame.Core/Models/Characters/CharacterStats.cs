using IdleGame.Core.Models.Affixes;

namespace IdleGame.Core.Models.Characters;

/// <summary>
/// Character statistics
/// </summary>
public class CharacterStats
{
    // Primary stats
    public double Strength { get; set; }
    public double Dexterity { get; set; }
    public double Intelligence { get; set; }
    public double Vitality { get; set; }

    // Life and Mana
    public double MaxLife { get; set; }
    public double CurrentLife { get; set; }
    public double MaxMana { get; set; }
    public double CurrentMana { get; set; }
    public double LifeRegeneration { get; set; }
    public double ManaRegeneration { get; set; }

    // Defense
    public double Armor { get; set; }
    public double Evasion { get; set; }
    public double EnergyShield { get; set; }
    public double BlockChance { get; set; }
    public double DodgeChance { get; set; }

    // Resistances
    public double FireResistance { get; set; }
    public double ColdResistance { get; set; }
    public double LightningResistance { get; set; }
    public double ChaosResistance { get; set; }

    // Offense
    public double PhysicalDamage { get; set; }
    public double FireDamage { get; set; }
    public double ColdDamage { get; set; }
    public double LightningDamage { get; set; }
    public double ChaosDamage { get; set; }
    public double AttackSpeed { get; set; } = 1.0;
    public double CriticalChance { get; set; } = 5.0;
    public double CriticalMultiplier { get; set; } = 150.0;
    public double Accuracy { get; set; } = 100;

    // Utility
    public double MovementSpeed { get; set; } = 100;
    public double ItemQuantity { get; set; }
    public double ItemRarity { get; set; }
    public double ExperienceGain { get; set; } = 100;
    public double GoldFind { get; set; } = 100;

    /// <summary>
    /// Apply a stat modifier
    /// </summary>
    public void ApplyModifier(StatModifier modifier)
    {
        switch (modifier.StatType)
        {
            case StatType.Strength:
                Strength += modifier.FlatValue;
                Strength *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.Dexterity:
                Dexterity += modifier.FlatValue;
                Dexterity *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.Intelligence:
                Intelligence += modifier.FlatValue;
                Intelligence *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.Vitality:
                Vitality += modifier.FlatValue;
                Vitality *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.MaxLife:
                MaxLife += modifier.FlatValue;
                MaxLife *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.MaxMana:
                MaxMana += modifier.FlatValue;
                MaxMana *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.LifeRegeneration:
                LifeRegeneration += modifier.FlatValue;
                break;
            case StatType.ManaRegeneration:
                ManaRegeneration += modifier.FlatValue;
                break;
            case StatType.Armor:
                Armor += modifier.FlatValue;
                Armor *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.Evasion:
                Evasion += modifier.FlatValue;
                Evasion *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.EnergyShield:
                EnergyShield += modifier.FlatValue;
                EnergyShield *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.BlockChance:
                BlockChance += modifier.FlatValue;
                break;
            case StatType.DodgeChance:
                DodgeChance += modifier.FlatValue;
                break;
            case StatType.FireResistance:
                FireResistance += modifier.FlatValue;
                break;
            case StatType.ColdResistance:
                ColdResistance += modifier.FlatValue;
                break;
            case StatType.LightningResistance:
                LightningResistance += modifier.FlatValue;
                break;
            case StatType.ChaosResistance:
                ChaosResistance += modifier.FlatValue;
                break;
            case StatType.AllResistances:
                FireResistance += modifier.FlatValue;
                ColdResistance += modifier.FlatValue;
                LightningResistance += modifier.FlatValue;
                break;
            case StatType.PhysicalDamage:
                PhysicalDamage += modifier.FlatValue;
                PhysicalDamage *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.FireDamage:
                FireDamage += modifier.FlatValue;
                FireDamage *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.ColdDamage:
                ColdDamage += modifier.FlatValue;
                ColdDamage *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.LightningDamage:
                LightningDamage += modifier.FlatValue;
                LightningDamage *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.ChaosDamage:
                ChaosDamage += modifier.FlatValue;
                ChaosDamage *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.AttackSpeed:
                AttackSpeed *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.CriticalChance:
                CriticalChance += modifier.FlatValue;
                CriticalChance *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.CriticalMultiplier:
                CriticalMultiplier += modifier.FlatValue;
                break;
            case StatType.Accuracy:
                Accuracy += modifier.FlatValue;
                Accuracy *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.MovementSpeed:
                MovementSpeed += modifier.FlatValue;
                MovementSpeed *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.ItemQuantity:
                ItemQuantity += modifier.FlatValue;
                break;
            case StatType.ItemRarity:
                ItemRarity += modifier.FlatValue;
                break;
            case StatType.ExperienceGain:
                ExperienceGain += modifier.FlatValue;
                ExperienceGain *= 1 + modifier.PercentValue / 100;
                break;
            case StatType.GoldFind:
                GoldFind += modifier.FlatValue;
                GoldFind *= 1 + modifier.PercentValue / 100;
                break;
        }
    }

    /// <summary>
    /// Calculate total DPS
    /// </summary>
    public double CalculateDPS()
    {
        var totalDamage = PhysicalDamage + FireDamage + ColdDamage + LightningDamage + ChaosDamage;
        var critMultiplier = 1 + (CriticalChance / 100) * ((CriticalMultiplier - 100) / 100);
        return totalDamage * AttackSpeed * critMultiplier;
    }

    /// <summary>
    /// Clone stats for calculation
    /// </summary>
    public CharacterStats Clone()
    {
        return (CharacterStats)MemberwiseClone();
    }
}
