using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TMP_Text quantityText;

    private InventorySlot slot;

    public void SetSlot(InventorySlot slot)
    {
        this.slot = slot;

        var item = slot.item;

        icon.sprite = item.data.icon;
        icon.enabled = true;

        quantityText.text = item.data.stackable && item.quantity > 1
            ? item.quantity.ToString()
            : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIItemDetail.Instance.Show(slot);
    }
}
