using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Header("Base Stats")]
    public List<WStatEntry> baseStats = new();

    [Header("weapon")]
    public GameObject weaponPrefab;

    [Header("Socket")]
    public WeaponSlotType slotType;

    public float GetBaseStat(WeaponStatType type)
    {
        foreach (var entry in baseStats)
        {
            if (entry.statType == type)
                return entry.value;
        }
        return 0f;
    }
}
