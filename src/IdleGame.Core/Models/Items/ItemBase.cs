namespace IdleGame.Core.Models.Items;

/// <summary>
/// Base item definition - the template for items
/// </summary>
public class ItemBase
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ItemBaseType BaseType { get; set; }
    public EquipmentSlot Slot { get; set; }
    public int RequiredLevel { get; set; }
    public int BaseArmor { get; set; }
    public int BaseEvasion { get; set; }
    public int BaseEnergyShield { get; set; }
    public int BaseMinDamage { get; set; }
    public int BaseMaxDamage { get; set; }
    public double BaseAttackSpeed { get; set; } = 1.0;
    public double BaseCriticalChance { get; set; } = 5.0;

    /// <summary>
    /// Item level - affects which affixes can roll
    /// </summary>
    public int ItemLevel { get; set; } = 1;
}
