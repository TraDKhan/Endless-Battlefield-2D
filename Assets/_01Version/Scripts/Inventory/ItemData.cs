using UnityEngine;

[CreateAssetMenu(menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemID;
    public string itemName;
    public Sprite icon;
    public ItemType itemType;
    public string description;
    public bool stackable;
    public int maxStack;
}
