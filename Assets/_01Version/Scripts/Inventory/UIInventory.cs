using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private InventorySystem inventory;

    [Header("UI")]
    [SerializeField] private Transform content;
    [SerializeField] private UIInventorySlot slotPrefab;

    private readonly List<UIInventorySlot> uiSlots = new();
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
                uiSlots[i].Bind(slots[i]);
            }
            else
            {
                uiSlots[i].gameObject.SetActive(false);
            }
        }

        if (slots.Count == 0)
        {
            hasAutoSelected = false;
            UIItemDetail.Instance?.Clear();
        }


        AutoShowFirstSlot(slots);
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
    private void AutoShowFirstSlot(IReadOnlyList<InventorySlot> slots)
    {
        if (hasAutoSelected)
            return;

        if (slots == null || slots.Count == 0)
            return;

        var firstSlot = slots[0];
        if (firstSlot == null || firstSlot.Item == null)
            return;

        UIItemDetail.Instance?.Show(firstSlot);
        hasAutoSelected = true;
    }

}
