using System.Collections.Generic;

[System.Serializable]
public class ItemInstance
{
    public ItemData Data { get; private set; }
    public int quantity;

    // Equipment runtime
    public int upgradeLevel;
    public List<StatEntry> bonusStats;

    public ItemInstance(ItemData data, int quantity = 1)
    {
        Data = data;
        this.quantity = quantity;

        upgradeLevel = 0;
        bonusStats = new();
    }

    // =========================
    // HELPERS
    // =========================
    public bool IsStackable => Data.stackable;
    public bool IsEquipment => Data.itemType == ItemType.Equipment;
    public int MaxStack => Data.maxStack;

    public ItemInstance Clone(int newQuantity)
    {
        return new ItemInstance(Data, newQuantity)
        {
            upgradeLevel = upgradeLevel,
            bonusStats = new List<StatEntry>(bonusStats)
        };
    }

    public bool CanStackWith(ItemInstance other)
    {
        if (other == null) return false;

        if (Data != other.Data) return false;
        if (!IsStackable) return false;

        if (upgradeLevel != other.upgradeLevel) return false;

        if (bonusStats.Count != other.bonusStats.Count)
            return false;

        for (int i = 0; i < bonusStats.Count; i++)
        {
            if (!bonusStats[i].Equals(other.bonusStats[i]))
                return false;
        }

        return true;
    }
}
