using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance;
    [Header("Config")]
    [SerializeField] private int maxSlots = 30;

    [Header("Runtime")]
    [SerializeField] private List<InventorySlot> slots = new();

    public IReadOnlyList<InventorySlot> Slots => slots;

    public event Action OnInventoryChanged;

    public bool IsFull => slots.Count >= maxSlots;
    public int FreeSlots => maxSlots - slots.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // =========================
    // ADD (ENTRY)
    // =========================
    public int AddItem(ItemData data, int amount = 1)
    {
        if (data == null || amount <= 0)
            return 0;

        var instance = new ItemInstance(data, amount);
        return AddItem(instance);
    }

    public int AddItem(ItemInstance item)
    {
        if (item == null || item.quantity <= 0)
            return 0;

        bool changed = false;
        int added = 0;

        if (item.IsStackable)
            added += AddStackable(item, ref changed);
        else
            added += AddNonStackable(item, ref changed);

        if (changed)
            NotifyChanged();

        return added;
    }
    /// <summary>
    /// Thêm không cộng dồn slot
    /// </summary>
    private int AddNonStackable(ItemInstance item, ref bool changed)
    {
        int added = 0;

        while (item.quantity > 0 && !IsFull)
        {
            slots.Add(new InventorySlot(item.Clone(1)));
            item.quantity--;
            added++;
            changed = true;
        }

        return added;
    }

    /// <summary>
    /// Thêm có cộng dồn
    /// </summary>
    private int AddStackable(ItemInstance item, ref bool changed)
    {
        int added = 0;

        foreach (var slot in slots)
        {
            if (item.quantity <= 0)
                break;

            var slotItem = slot.Item;
            if (!slotItem.CanStackWith(item))
                continue;

            int space = slotItem.MaxStack - slotItem.quantity;
            if (space <= 0)
                continue;

            int add = Mathf.Min(space, item.quantity);
            slotItem.quantity += add;
            item.quantity -= add;

            added += add;
            changed = true;
        }

        while (item.quantity > 0 && !IsFull)
        {
            int stackAmount = Mathf.Min(item.quantity, item.MaxStack);
            slots.Add(new InventorySlot(item.Clone(stackAmount)));

            item.quantity -= stackAmount;
            added += stackAmount;
            changed = true;
        }

        return added;
    }

    // =========================
    // REMOVE
    // =========================
    public void RemoveSlot(InventorySlot slot)
    {
        if (slot == null)
            return;

        if (slots.Remove(slot))
            NotifyChanged();
    }

    public int RemoveFromSlot(InventorySlot slot, int amount)
    {
        if (slot == null || slot.Item == null || amount <= 0)
            return 0;

        int removed = Mathf.Min(slot.Item.quantity, amount);
        slot.Item.quantity -= removed;

        if (slot.Item.quantity <= 0)
            slots.Remove(slot);

        if (removed > 0)
            NotifyChanged();

        return removed;
    }

    public bool RemoveExact(ItemInstance instance)
    {
        if (instance == null)
            return false;

        var slot = slots.FirstOrDefault(s => s.Item == instance);
        if (slot == null)
            return false;

        slots.Remove(slot);
        NotifyChanged();
        return true;
    }

    // =========================
    // INTERNAL
    // =========================
    private void NotifyChanged()
    {
        OnInventoryChanged?.Invoke();
    }


#if UNITY_EDITOR
    [System.Serializable]
    public struct DebugItem
    {
        public ItemData data;
        public int amount;
    }

    [Header("DEBUG")]
    [SerializeField] private List<DebugItem> debugItems;

    private void Start()
    {
        slots.Clear();

        foreach (var entry in debugItems)
        {
            if (entry.data == null || entry.amount <= 0)
                continue;

            AddItem(entry.data, entry.amount);
        }
    }
#endif
}
