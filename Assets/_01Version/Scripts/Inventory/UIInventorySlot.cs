using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;

    private InventorySlot boundSlot;

    // =========================
    // BIND
    // =========================
    public void Bind(InventorySlot slot)
    {
        boundSlot = slot;

        if (slot == null || slot.Item == null)
        {
            Clear();
            return;
        }

        var item = slot.Item;

        icon.sprite = item.Data.icon;
        icon.enabled = true;

        if (ShouldShowQuantity(item))
        {
            quantityText.gameObject.SetActive(true);
            quantityText.text = item.quantity.ToString();
        }
        else
        {
            quantityText.gameObject.SetActive(false);
        }
    }

    // =========================
    // CLEAR
    // =========================
    private void Clear()
    {
        boundSlot = null;

        icon.sprite = null;
        icon.enabled = false;

        quantityText.gameObject.SetActive(false);
    }

    // =========================
    // HELPERS
    // =========================
    private bool ShouldShowQuantity(ItemInstance item)
    {
        return item.IsStackable && item.quantity > 1;
    }

    // =========================
    // INPUT
    // =========================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (boundSlot == null || boundSlot.Item == null)
            return;

        // Có thể mở rộng: right-click / double-click
        UIItemDetail.Instance?.Show(boundSlot);
    }
}
