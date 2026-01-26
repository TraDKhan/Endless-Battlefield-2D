using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private GameObject highlight;

    private InventorySlot boundSlot;
    private UIInventory owner;

    // =========================
    // BIND
    // =========================
    public void Bind(InventorySlot slot, UIInventory owner)
    {
        this.owner = owner;
        boundSlot = slot;

        if (slot == null || slot.Item == null)
        {
            Clear();
            return;
        }

        var item = slot.Item;

        icon.sprite = item.Data.icon;
        icon.enabled = true;

        if (item.IsStackable && item.quantity > 1)
        {
            quantityText.text = item.quantity.ToString();
            quantityText.gameObject.SetActive(true);
        }
        else
        {
            quantityText.gameObject.SetActive(false);
            quantityText.text = string.Empty;
        }
    }


    // =========================
    // SELECTION
    // =========================
    public void SetSelected(bool selected)
    {
        if (highlight != null)
            highlight.SetActive(selected);
    }

    // =========================
    // CLEAR
    // =========================
    private void Clear()
    {
        boundSlot = null;

        icon.sprite = null;
        icon.enabled = false;

        quantityText.text = string.Empty;
        quantityText.gameObject.SetActive(false);

        SetSelected(false);
    }


    // =========================
    // INPUT
    // =========================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (boundSlot == null)
            return;

        owner?.SelectSlot(this, boundSlot);
    }
}
