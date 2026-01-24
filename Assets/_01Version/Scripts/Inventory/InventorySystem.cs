using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventorySlot> slots = new();

    public void AddItem(ItemData data, int amount = 1)
    {
        AddItem(new ItemInstance(data, amount));
    }

    public void AddItem(ItemInstance newItem)
    {
        if (newItem.IsStackable)
        {
            foreach (var slot in slots)
            {
                var item = slot.item;
                if (item.data == newItem.data &&
                    item.quantity < item.data.maxStack)
                {
                    int space = item.data.maxStack - item.quantity;
                    int move = Mathf.Min(space, newItem.quantity);

                    item.quantity += move;
                    newItem.quantity -= move;

                    if (newItem.quantity <= 0)
                        return;
                }
            }
        }

        // Còn dư → tạo slot mới
        slots.Add(new InventorySlot(newItem));
    }

    public void RemoveItem(InventorySlot slot)
    {
        slots.Remove(slot);
    }
}
