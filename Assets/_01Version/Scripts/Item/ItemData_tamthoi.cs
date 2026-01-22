using UnityEngine;

[CreateAssetMenu(menuName = "Item/New Item")]
public class ItemData_tamthoi : ScriptableObject
{
    public string itemName;
    public ItemRarity rarity;
    public GameObject prefab;
}
