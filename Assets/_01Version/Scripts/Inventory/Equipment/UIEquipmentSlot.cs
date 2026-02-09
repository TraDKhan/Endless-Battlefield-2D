using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIEquipmentSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("Config")]
    [SerializeField] private EquipmentSlotType slotType;

    [Header("UI")]
    [SerializeField] private Image iconImage;

    private void Awake()
    {
        Clear();
    }

    private void Start()
    {
        Debug.Log($"[UIEquipmentSlot] Enabled slotType = {slotType}");

        // Đảm bảo EquipmentSystem đã khởi tạo trước khi đăng ký sự kiện
        if (EquipmentSystem.Instance != null)
        {
            EquipmentSystem.Instance.OnEquipmentChanged += Refresh;
            Refresh();
        }
        else
        {
            Clear();
        }
    }

    private void OnDisable()
    {
        if (EquipmentSystem.Instance != null)
            EquipmentSystem.Instance.OnEquipmentChanged -= Refresh;
    }

    public void Refresh()
    {
        if (EquipmentSystem.Instance == null)
        {
            Clear();
            return;
        }

        var slot = EquipmentSystem.Instance.Slots.FirstOrDefault(s => s.slotType == slotType);

        if (slot == null || slot.IsEmpty)
        {
            Clear();
            return;
        }

        var itemData = slot.Item.Data;
        iconImage.sprite = itemData.icon;
        iconImage.enabled = true;
    }

    private void Clear()
    {
        iconImage.sprite = null;
        iconImage.enabled = false;
    }

    // =========================
    // INPUT
    // =========================
    public void OnPointerClick(PointerEventData eventData)
    {
        if (EquipmentSystem.Instance == null)
            return;

        var slot = EquipmentSystem.Instance.Slots
            .FirstOrDefault(s => s.slotType == slotType);

        if (slot == null || slot.IsEmpty)
            return;

        // Show item detail
        UIItemDetail.Instance?.ShowEquipped(slot.Item);
    }
}