using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentSystem
{
    private readonly Dictionary<EquipmentSlot, EquipmentSlotRuntime> slots;
    private readonly StatSystem statSystem;

    public event Action OnEquipmentChanged;

    // =========================
    // CONSTRUCTOR
    // =========================
    public EquipmentSystem(StatSystem statSystem)
    {
        this.statSystem = statSystem;

        slots = Enum.GetValues(typeof(EquipmentSlot))
            .Cast<EquipmentSlot>()
            .ToDictionary(
                s => s,
                s => new EquipmentSlotRuntime { slotType = s }
            );
    }

    // =========================
    // QUERY
    // =========================
    public ItemData GetItem(EquipmentSlot slot)
    {
        return slots[slot].Item;
    }

    public EquipmentSlotRuntime GetSlot(EquipmentSlot slot)
    {
        return slots[slot];
    }

    public bool IsEquipped(ItemData item)
    {
        if (item == null || item.itemType != ItemType.Equipment)
            return false;

        var slot = item.equipmentSlot;
        return slots.TryGetValue(slot, out var s) && s.Item == item;
    }

    /// =========================
    // EQUIP
    // =========================
    public void Equip(ItemData item)
    {
        if (item == null) return;

        if (item.itemType != ItemType.Equipment)
        {
            Debug.LogWarning(
                $"Item {item.name} is not Equipment, cannot equip!"
            );
            return;
        }

        var slotType = item.equipmentSlot;

        if (!slots.TryGetValue(slotType, out var slot))
        {
            Debug.LogWarning(
                $"Slot {slotType} not found for item {item.name}"
            );
            return;
        }

        // ---------- Unequip cũ ----------
        if (!slot.IsEmpty)
        {
            statSystem.RemoveSource(slot.equippedItem);
            slot.Unequip();
        }

        // ---------- Equip mới ----------
        var source = new EquippedItemStatSource(item);
        slot.Equip(source);
        statSystem.AddSource(source);

        OnEquipmentChanged?.Invoke();
    }

    // =========================
    // UNEQUIP
    // =========================
    public void Unequip(EquipmentSlot slotType)
    {
        if (!slots.TryGetValue(slotType, out var slot))
            return;

        if (slot.IsEmpty) return;

        statSystem.RemoveSource(slot.equippedItem);
        slot.Unequip();

        OnEquipmentChanged?.Invoke();
    }
}
