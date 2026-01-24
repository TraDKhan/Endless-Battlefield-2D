using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentSystem
{
    private Dictionary<EquipmentSlot, EquipmentSlotRuntime> slots;
    private CharacterStats stats;

    public event Action OnEquipmentChanged;

    public EquipmentSystem(CharacterStats stats)
    {
        this.stats = stats;

        // Tự tạo slot theo enum
        slots = System.Enum.GetValues(typeof(EquipmentSlot))
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
        var slot = item.equipmentSlot;
        return slots[slot].Item == item;
    }

    // =========================
    // EQUIP
    // =========================
    public void Equip(ItemData item)
    {
        if (item == null) return;

        if (item.itemType != ItemType.Equipment)
        {
            Debug.LogWarning($"Item {item.name} is not Equipment, cannot equip!");
            return;
        }

        var slotType = item.equipmentSlot;

        if (!slots.ContainsKey(slotType))
        {
            Debug.LogWarning($"Slot {slotType} not found for item {item.name}");
            return;
        }

        var slot = slots[slotType];

        // tháo đồ cũ
        if (!slot.IsEmpty)
        {
            stats.RemoveSource(slot.equippedItem);
            slot.Unequip();
        }

        var source = new EquippedItemStatSource(item);
        slot.Equip(source);
        stats.AddSource(source);

        OnEquipmentChanged?.Invoke();
    }

    // =========================
    // UNEQUIP
    // =========================
    public void Unequip(EquipmentSlot slotType)
    {
        var slot = slots[slotType];
        if (slot.IsEmpty) return;

        stats.RemoveSource(slot.equippedItem);
        slot.Unequip();

        OnEquipmentChanged?.Invoke();
    }
}
