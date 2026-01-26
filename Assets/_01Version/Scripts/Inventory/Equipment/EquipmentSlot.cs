[System.Serializable]
public class EquipmentSlot
{
    public EquipmentSlotType slotType;
    public ItemInstance item;

    public bool IsEmpty => item == null;
}
