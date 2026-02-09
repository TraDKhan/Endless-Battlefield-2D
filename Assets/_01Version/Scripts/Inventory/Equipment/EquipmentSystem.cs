using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance;

    [SerializeField] private List<EquipmentSlot> slots = new();

    public IReadOnlyList<EquipmentSlot> Slots => slots;

    public event Action OnEquipmentChanged;
    public event Action<ItemInstance> OnEquipped;
    public event Action<ItemInstance> OnUnequipped;

    private void Awake()
    {


        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        InitSlots();
    }

    private void InitSlots()
    {
        slots.Clear();
        foreach (EquipmentSlotType type in Enum.GetValues(typeof(EquipmentSlotType)))
        {
            slots.Add(new EquipmentSlot(type));
        }
    }

    public bool Equip(ItemInstance instance)
    {
        if (instance == null) return false;
        if (!instance.IsEquipment) return false;

        var equipData = instance.Data as EquipmentItemData;
        if (equipData == null) return false;

        var slot = slots.FirstOrDefault(s => s.slotType == equipData.slot);
        if (slot == null) return false;

        // remove khỏi inventory
        InventorySystem.Instance.RemoveExact(instance);

        // swap
        ItemInstance oldItem = slot.Equip(instance);
        if (oldItem != null)
        {
            InventorySystem.Instance.AddItem(oldItem);
            OnUnequipped?.Invoke(oldItem);
        }

        OnEquipped?.Invoke(instance);
        OnEquipmentChanged?.Invoke();
        return true;
    }

    public bool Unequip(EquipmentSlotType slotType)
    {
        var slot = slots.Find(s => s.slotType == slotType);
        if (slot == null || slot.IsEmpty)
            return false;

        var item = slot.Unequip();

        InventorySystem.Instance.AddItem(item);

        OnUnequipped?.Invoke(item);
        OnEquipmentChanged?.Invoke();
        return true;
    }
}