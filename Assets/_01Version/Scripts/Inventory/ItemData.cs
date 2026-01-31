using UnityEngine;

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

    [Header("Stack")]
    public bool stackable = false;
    public int maxStack = 1;
}