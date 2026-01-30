using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentSystem : IStatSource
{
    private readonly List<EquipmentSlot> slots;
    private readonly StatSystem statSystem;

    public event Action OnEquipmentChanged;

    public EquipmentSystem(
        StatSystem statSystem,
        IEnumerable<EquipmentSlotType> slotTypes)
    {
        this.statSystem = statSystem;

        slots = slotTypes
            .Select(t => new EquipmentSlot { slotType = t })
            .ToList();

        statSystem.AddSource(this);
    }

    // =========================
    // EQUIP (SWAP SAFE)
    // =========================
    public ItemInstance Equip(ItemInstance item)
    {
        if (item == null || !item.IsEquipment)
            return null;

        if (item.quantity != 1)
        {
            Debug.LogError("Equipment must have quantity = 1");
            return null;
        }

        var slot = GetSlot(item.Data.equipSlot);
        if (slot == null)
            return null;

        ItemInstance oldItem = slot.item;
        slot.item = item;

        statSystem.Recalculate();
        OnEquipmentChanged?.Invoke();

        return oldItem;
    }

    // =========================
    // UNEQUIP
    // =========================
    public ItemInstance Unequip(EquipmentSlotType slotType)
    {
        var slot = GetSlot(slotType);
        if (slot == null || slot.IsEmpty)
            return null;

        ItemInstance item = slot.item;
        slot.item = null;

        statSystem.Recalculate();
        OnEquipmentChanged?.Invoke();

        return item;
    }

    // =========================
    // QUERY
    // =========================
    public ItemInstance GetEquippedItem(EquipmentSlotType slotType)
    {
        return GetSlot(slotType)?.item;
    }

    public IReadOnlyList<EquipmentSlot> GetAllSlots() => slots;

    // =========================
    // IStatSource
    // =========================
    public IEnumerable<StatModifier> GetModifiers()
    {
        foreach (var slot in slots)
        {
            if (slot.item == null)
                continue;

            foreach (var mod in BuildModifiers(slot.item))
                yield return mod;
        }
    }

    // =========================
    // INTERNAL
    // =========================
    private EquipmentSlot GetSlot(EquipmentSlotType type)
    {
        return slots.FirstOrDefault(s => s.slotType == type);
    }

    private IEnumerable<StatModifier> BuildModifiers(ItemInstance item)
    {
        foreach (var stat in item.Data.stats)
            yield return CreateModifier(stat);

        foreach (var stat in item.bonusStats)
            yield return CreateModifier(stat);
    }

    private StatModifier CreateModifier(StatEntry stat)
    {
        return new StatModifier
        {
            statType = stat.statType,
            value = stat.value,
            modType = stat.modType,
            context = stat.context // 👈 khuyến nghị dùng context từ data
        };
    }
}
