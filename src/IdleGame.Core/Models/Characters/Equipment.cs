using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Models.Characters;

/// <summary>
/// Equipment slots for a character
/// </summary>
public class Equipment
{
    public Item? Helmet { get; set; }
    public Item? Chest { get; set; }
    public Item? Gloves { get; set; }
    public Item? Boots { get; set; }
    public Item? Belt { get; set; }
    public Item? Amulet { get; set; }
    public Item? RingLeft { get; set; }
    public Item? RingRight { get; set; }
    public Item? MainHand { get; set; }
    public Item? OffHand { get; set; }

    /// <summary>
    /// Get item in a specific slot
    /// </summary>
    public Item? GetItem(EquipmentSlot slot)
    {
        return slot switch
        {
            EquipmentSlot.Helmet => Helmet,
            EquipmentSlot.Chest => Chest,
            EquipmentSlot.Gloves => Gloves,
            EquipmentSlot.Boots => Boots,
            EquipmentSlot.Belt => Belt,
            EquipmentSlot.Amulet => Amulet,
            EquipmentSlot.RingLeft => RingLeft,
            EquipmentSlot.RingRight => RingRight,
            EquipmentSlot.MainHand => MainHand,
            EquipmentSlot.OffHand => OffHand,
            EquipmentSlot.TwoHand => MainHand,
            _ => null
        };
    }

    /// <summary>
    /// Set item in a specific slot
    /// </summary>
    public void SetItem(EquipmentSlot slot, Item? item)
    {
        switch (slot)
        {
            case EquipmentSlot.Helmet:
                Helmet = item;
                break;
            case EquipmentSlot.Chest:
                Chest = item;
                break;
            case EquipmentSlot.Gloves:
                Gloves = item;
                break;
            case EquipmentSlot.Boots:
                Boots = item;
                break;
            case EquipmentSlot.Belt:
                Belt = item;
                break;
            case EquipmentSlot.Amulet:
                Amulet = item;
                break;
            case EquipmentSlot.RingLeft:
                RingLeft = item;
                break;
            case EquipmentSlot.RingRight:
                RingRight = item;
                break;
            case EquipmentSlot.MainHand:
                MainHand = item;
                break;
            case EquipmentSlot.OffHand:
                OffHand = item;
                break;
            case EquipmentSlot.TwoHand:
                MainHand = item;
                OffHand = null;
                break;
        }
    }

    /// <summary>
    /// Get all equipped items
    /// </summary>
    public IEnumerable<Item> GetAllEquipped()
    {
        if (Helmet != null) yield return Helmet;
        if (Chest != null) yield return Chest;
        if (Gloves != null) yield return Gloves;
        if (Boots != null) yield return Boots;
        if (Belt != null) yield return Belt;
        if (Amulet != null) yield return Amulet;
        if (RingLeft != null) yield return RingLeft;
        if (RingRight != null) yield return RingRight;
        if (MainHand != null) yield return MainHand;
        if (OffHand != null) yield return OffHand;
    }

    /// <summary>
    /// Equip an item, returning the previously equipped item if any
    /// </summary>
    public Item? Equip(Item item)
    {
        var oldItem = GetItem(item.Slot);
        SetItem(item.Slot, item);
        return oldItem;
    }

    /// <summary>
    /// Unequip an item from a slot
    /// </summary>
    public Item? Unequip(EquipmentSlot slot)
    {
        var item = GetItem(slot);
        SetItem(slot, null);
        return item;
    }
}
