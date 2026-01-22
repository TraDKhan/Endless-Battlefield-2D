using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform content;
    public UIInventorySlot slotPrefab;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        bool isFirst = true;

        foreach (var item in inventory.items)
        {
            var slot = Instantiate(slotPrefab, content);
            slot.SetItem(item);

            if (isFirst)
            {
                UIItemDetail.Instance.Show(item);
                isFirst = false;
            }
        }
    }
}
