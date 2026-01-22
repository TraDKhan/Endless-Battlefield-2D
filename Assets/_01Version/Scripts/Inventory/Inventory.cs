using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new();

    public void AddItem(ItemData itemData, int amount = 1)
    {
        if (itemData.stackable)
        {
            var item = items.Find(i => i.data == itemData);
            if (item != null)
            {
                item.quantity += amount;
                return;
            }
        }

        items.Add(new InventoryItem(itemData, amount));
    }
}
