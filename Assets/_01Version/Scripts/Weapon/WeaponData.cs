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
    public GameObject projectilePrefab;

    [Header("Socket")]
    public WeaponSlotType slotType;
    public WeaponData()
    {
        baseStats = new List<WStatEntry>
        {
            new WStatEntry { statType = WeaponStatType.Damage, value = 10f },
            new WStatEntry { statType = WeaponStatType.Cooldown, value = 1f },
            new WStatEntry { statType = WeaponStatType.AttackRange, value = 5f },
            new WStatEntry { statType = WeaponStatType.CritChance, value = 0.1f }
        };
    }
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
