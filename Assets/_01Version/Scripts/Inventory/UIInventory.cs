using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public InventorySystem inventory;
    public Transform content;
    public UIInventorySlot slotPrefab;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in content)
            Destroy(child.gameObject);

        foreach (var slot in inventory.slots)
        {
            var uiSlot = Instantiate(slotPrefab, content);
            uiSlot.SetSlot(slot);
        }
    }
}
