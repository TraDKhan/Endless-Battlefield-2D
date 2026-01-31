using UnityEngine;

[System.Serializable]
public class EquipmentSlot
{
    public EquipmentSlotType slotType;
    public ItemInstance Item { get; private set; }

    public bool IsEmpty => Item == null;

    public EquipmentSlot(EquipmentSlotType type)
    {
        slotType = type;
    }

    public ItemInstance Equip(ItemInstance newItem)
    {
        Debug.Log($"[EquipmentSlot] Equip {newItem.Data.itemName} to {slotType}");

        ItemInstance oldItem = Item;
        Item = newItem;
        return oldItem; // trả lại item cũ để swap
    }

    public ItemInstance Unequip()
    {
        ItemInstance oldItem = Item;
        Item = null;
        return oldItem;
    }
}
