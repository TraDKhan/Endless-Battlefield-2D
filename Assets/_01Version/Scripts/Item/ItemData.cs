using UnityEngine;

[CreateAssetMenu(menuName = "Item/New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemRarity rarity;
    public GameObject prefab;
}
