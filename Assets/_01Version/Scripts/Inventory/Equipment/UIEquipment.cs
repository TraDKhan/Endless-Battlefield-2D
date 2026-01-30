using System.Collections.Generic;
using UnityEngine;

public class UIEquipment : MonoBehaviour
{
    [Header("Predefined Slots")]
    [SerializeField] private List<UIEquipmentSlot> slots;

    private EquipmentSystem equipSystem;
    private UIEquipmentSlot selectedSlot;

    private void Awake()
    {
        equipSystem = PlayerController.Instance.EquipSystem;

        foreach (var slot in slots)
            slot.Init(this);

        RefreshAll();

        equipSystem.OnEquipmentChanged += RefreshAll;
    }

    private void OnDestroy()
    {
        if (equipSystem != null)
            equipSystem.OnEquipmentChanged -= RefreshAll;
    }

    // =========================
    // REFRESH
    // =========================
    private void RefreshAll()
    {
        foreach (var uiSlot in slots)
        {
            ItemInstance equippedItem =
                equipSystem.GetEquippedItem(uiSlot.SlotType);

            uiSlot.Refresh(equippedItem);
        }
    }

    // =========================
    // SELECTION
    // =========================
    public void SelectSlot(UIEquipmentSlot slot)
    {
        if (selectedSlot != null)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);

        if (slot.Item != null)
            UIItemDetail.Instance.ShowEquipped(slot.Item);
    }
}
