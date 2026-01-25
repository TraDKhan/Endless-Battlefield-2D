using UnityEngine;

[System.Serializable]
public class EquipmentSlotRuntime
{
    public EquipmentSlot slotType;
    public EquippedItemStatSource equippedItem;

    public bool IsEmpty => equippedItem == null;

    //public ItemData Item => equippedItem?.Data;

    public void Equip(EquippedItemStatSource source)
    {
        equippedItem = source;
    }

    public EquippedItemStatSource Unequip()
    {
        var old = equippedItem;
        equippedItem = null;
        return old;
    }
}
