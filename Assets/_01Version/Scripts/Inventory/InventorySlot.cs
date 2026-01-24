[System.Serializable]
public class InventorySlot
{
    public ItemInstance item;

    public InventorySlot(ItemInstance item)
    {
        this.item = item;
    }

    public bool IsEmpty => item == null;
}
