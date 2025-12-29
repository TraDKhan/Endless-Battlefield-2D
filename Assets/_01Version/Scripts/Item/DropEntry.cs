using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public ItemData item;
    [Range(0, 100)]
    public float dropChance;   // % rớt
    public int amount = 1;
}