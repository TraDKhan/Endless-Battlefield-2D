using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemID;
    public string itemName;

    [Header("Visual")]
    public Sprite icon;

    [Header("Type")]
    public ItemType itemType;

    [TextArea]
    public string description;

    [Header("Stats")]
    public List<StatEntry> stats;

    [Header("Stack")]
    public bool stackable = false;
    public int maxStack = 1;
}

