using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipmentSystem : MonoBehaviour
{
    public static EquipmentSystem Instance;

    [SerializeField] private List<EquipmentSlot> slots = new();

    public IReadOnlyList<EquipmentSlot> Slots => slots;

    public event Action OnEquipmentChanged;

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
        if (instance == null)
        {
            Debug.LogError("Equip failed: instance null");
            return false;
        }

        if (!instance.IsEquipment)
        {
            Debug.LogError("Equip failed: not equipment");
            return false;
        }
        var data = instance.Data as EquipmentItemData;
        if (data == null)
        {
            Debug.LogError("Equip failed: Data is not EquipmentItemData");
            return false;
        }

        Debug.Log("Equip success path: " + data.itemName + " -> " + data.slot);

        if (instance == null) return false;
        if (!instance.IsEquipment) return false;

        var equipData = instance.Data as EquipmentItemData;
        if (equipData == null) return false;

        var slot = slots.FirstOrDefault(s => s.slotType == equipData.slot);

        if (slot == null) return false;

        // remove khỏi inventory
        InventorySystem.Instance.RemoveExact(instance);

        // swap nếu slot đã có đồ
        ItemInstance oldItem = slot.Equip(instance);

        Debug.Log($"After Equip, slot {slot.slotType} has item: " +
            (slot.Item == null ? "NULL" : slot.Item.Data.itemName));


        if (oldItem != null)
        {
            InventorySystem.Instance.AddItem(oldItem);
        }

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

        OnEquipmentChanged?.Invoke();
        return true;
    }
}