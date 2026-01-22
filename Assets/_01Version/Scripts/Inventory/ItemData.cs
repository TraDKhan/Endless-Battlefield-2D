using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Info")]
    public string itemID;
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;
    public ItemType itemType;

    [Header("Equipment")]
    public EquipmentSlot equipmentSlot;

    [Header("Stats")]
    public List<StatEntry> stats;

    [Header("Stack")]
    public bool stackable;
    public int maxStack = 1;
}
