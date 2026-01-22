using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TMP_Text quantityText;
    private InventoryItem item;

    public void SetItem(InventoryItem item)
    {
        this.item = item;

        icon.sprite = item.data.icon;
        icon.enabled = true;

        quantityText.text = item.data.stackable ? item.quantity.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click2");
        UIItemDetail.Instance.Show(item);
    }

}
