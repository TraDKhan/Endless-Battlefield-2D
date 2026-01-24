using System.Collections.Generic;

[System.Serializable]
public class ItemInstance
{
    public ItemData data;
    public int quantity;

    // ===== Equipment runtime =====
    public int upgradeLevel;
    public List<StatEntry> bonusStats;

    public ItemInstance(ItemData data, int quantity = 1)
    {
        this.data = data;
        this.quantity = quantity;
        upgradeLevel = 0;
        bonusStats = new();
    }

    public bool IsStackable => data.stackable;
    public bool IsEquipment => data.itemType == ItemType.Equipment;
}
