using System.Collections.Generic;

[System.Serializable]
public class ItemInstance
{
    public ItemData Data { get; private set; }
    public int quantity;

    public int upgradeLevel;

    public ItemInstance(ItemData data, int quantity = 1)
    {
        Data = data;
        this.quantity = quantity;

        upgradeLevel = 0;
    }

    public bool IsStackable => Data.stackable;
    public bool IsEquipment => Data is EquipmentItemData;

    public int MaxStack => Data.maxStack;

    public ItemInstance Clone(int newQuantity)
    {
        return new ItemInstance(Data, newQuantity)
        {
            upgradeLevel = upgradeLevel,
        };
    }

    public bool CanStackWith(ItemInstance other)
    {
        if (other == null) return false;

        if (Data.itemID != other.Data.itemID) return false;

        if (!IsStackable) return false;

        if (upgradeLevel != other.upgradeLevel) return false;

        return true;
    }
}
