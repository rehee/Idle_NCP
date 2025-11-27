using IdleGame.Core.Models.Crafting;
using IdleGame.Core.Models.Items;

namespace IdleGame.Core.Models.Characters;

/// <summary>
/// Player inventory
/// </summary>
public class Inventory
{
    public int MaxSlots { get; set; } = 60;
    public List<Item> Items { get; set; } = new();
    public List<Currency> Currencies { get; set; } = new();
    public long Gold { get; set; }

    /// <summary>
    /// Add an item to inventory
    /// </summary>
    public bool AddItem(Item item)
    {
        if (Items.Count >= MaxSlots)
            return false;

        Items.Add(item);
        return true;
    }

    /// <summary>
    /// Remove an item from inventory
    /// </summary>
    public bool RemoveItem(Item item)
    {
        return Items.Remove(item);
    }

    /// <summary>
    /// Get item by ID
    /// </summary>
    public Item? GetItemById(string id)
    {
        return Items.FirstOrDefault(i => i.Id == id);
    }

    /// <summary>
    /// Add currency
    /// </summary>
    public void AddCurrency(CurrencyType type, int amount)
    {
        var existing = Currencies.FirstOrDefault(c => c.Type == type);
        if (existing != null)
        {
            existing.Add(amount);
        }
        else
        {
            Currencies.Add(new Currency(type, amount));
        }
    }

    /// <summary>
    /// Remove currency
    /// </summary>
    public bool RemoveCurrency(CurrencyType type, int amount)
    {
        var existing = Currencies.FirstOrDefault(c => c.Type == type);
        if (existing == null || existing.Amount < amount)
            return false;

        return existing.Remove(amount);
    }

    /// <summary>
    /// Get currency amount
    /// </summary>
    public int GetCurrencyAmount(CurrencyType type)
    {
        return Currencies.FirstOrDefault(c => c.Type == type)?.Amount ?? 0;
    }

    /// <summary>
    /// Check available slots
    /// </summary>
    public int AvailableSlots => MaxSlots - Items.Count;

    /// <summary>
    /// Check if inventory is full
    /// </summary>
    public bool IsFull => Items.Count >= MaxSlots;
}
