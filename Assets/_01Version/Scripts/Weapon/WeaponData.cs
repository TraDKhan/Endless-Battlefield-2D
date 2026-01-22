using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Info")]
    public string weaponName;

    [Header("Base Stats")]
    public List<StatEntry> baseStats;

    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Header("Socket")]
    public WeaponSlotType slotType;

    // =========================
    // BACKWARD SUPPORT
    // =========================
    public float GetBaseStat(StatType type)
    {
        foreach (var stat in baseStats)
        {
            if (stat.statType == type)
                return stat.value;
        }
        return 0f;
    }
}
