using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Config")]
    [SerializeField] private EquipmentSlotType slotType;

    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text emptyLabel;
    [SerializeField] private GameObject highlight;

    private ItemInstance item;
    private UIEquipment owner;

    // =========================
    // INIT
    // =========================
    public void Init(UIEquipment owner)
    {
        this.owner = owner;
        Refresh(null);
    }

    // =========================
    // REFRESH
    // =========================
    public void Refresh(ItemInstance newItem)
    {
        item = newItem;

        if (item == null)
        {
            icon.enabled = false;
            icon.sprite = null;

            if (emptyLabel != null)
                emptyLabel.gameObject.SetActive(true);
        }
        else
        {
            icon.sprite = item.Data.icon;
            icon.enabled = true;

            if (emptyLabel != null)
                emptyLabel.gameObject.SetActive(false);
        }

        SetSelected(false);
    }

    // =========================
    // ACCESS
    // =========================
    public EquipmentSlotType SlotType => slotType;
    public ItemInstance Item => item;

    // =========================
    // SELECTION
    // =========================
    public void SetSelected(bool selected)
    {
        if (highlight != null)
            highlight.SetActive(selected);
    }

    // =========================
    // INPUT
    // =========================
    public void OnPointerClick(PointerEventData eventData)
    {
        owner?.SelectSlot(this);
    }
}
