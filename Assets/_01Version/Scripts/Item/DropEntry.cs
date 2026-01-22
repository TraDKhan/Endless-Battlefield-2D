using UnityEngine;

[System.Serializable]
public class DropEntry
{
    public ItemData_tamthoi item;
    [Range(0, 100)]
    public float dropChance;   // % rớt
    public int amount = 1;
}