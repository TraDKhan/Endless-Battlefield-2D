using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private InventorySystem inventory;

    [Header("UI")]
    [SerializeField] private Transform content;
    [SerializeField] private UIInventorySlot slotPrefab;

    private readonly List<UIInventorySlot> uiSlots = new();
    private UIInventorySlot selectedUISlot;
    private InventorySlot selectedSlot;
    private bool hasAutoSelected = false;

    private void Awake()
    {
        ClearContentChildren();
    }

    // =========================
    // LIFECYCLE
    // =========================
    private void OnEnable()
    {
        if (inventory == null)
        {
            Debug.LogError("UIInventory: InventorySystem is null");
            return;
        }

        inventory.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= Refresh;
    }

    // =========================
    // REFRESH
    // =========================
    public void Refresh()
    {
        var slots = inventory.Slots;

        EnsureUISlotCount(slots.Count);

        for (int i = 0; i < uiSlots.Count; i++)
        {
            if (i < slots.Count)
            {
                uiSlots[i].gameObject.SetActive(true);
                uiSlots[i].Bind(slots[i], this);
            }
            else
            {
                uiSlots[i].gameObject.SetActive(false);
            }
        }

        if (slots.Count == 0)
        {
            hasAutoSelected = false;
            selectedUISlot = null;
            UIItemDetail.Instance?.Clear();
        }

        if (selectedSlot != null && !slots.Contains(selectedSlot))
        {
            selectedSlot = null;
            selectedUISlot = null;
            hasAutoSelected = false;
        }

        AutoSelectFirst(slots);
    }

    // =========================
    // INTERNAL
    // =========================
    private void EnsureUISlotCount(int count)
    {
        while (uiSlots.Count < count)
        {
            var uiSlot = Instantiate(slotPrefab, content);
            uiSlots.Add(uiSlot);
        }
    }
    private void ClearContentChildren()
    {
        uiSlots.Clear();

        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
    private void AutoSelectFirst(IReadOnlyList<InventorySlot> slots)
    {
        if (selectedSlot != null)
            return;

        if (slots == null || slots.Count == 0)
            return;

        SelectSlot(uiSlots[0], slots[0]);
    }


    public void SelectSlot(UIInventorySlot uiSlot, InventorySlot slot)
    {
        if (selectedSlot == slot)
            return;

        if (selectedUISlot != null)
            selectedUISlot.SetSelected(false);

        selectedUISlot = uiSlot;
        selectedSlot = slot;

        selectedUISlot.SetSelected(true);
        UIItemDetail.Instance?.Show(slot);
    }

}
