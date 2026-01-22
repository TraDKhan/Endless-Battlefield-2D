using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/Weapon Data")]
public class WeaponData : ScriptableObject
{
    // =========================
    // INFO
    // =========================
    [Header("Info")]
    public string weaponName;

    [Header("Base Stats")]
    public List<StatEntry> baseStats = new();

    [Header("Projectile")]
    public GameObject projectilePrefab;

    [Header("Socket")]
    public WeaponSlotType slotType;

    // =========================
    // DEFAULT STAT CONFIG
    // =========================
    private static readonly Dictionary<StatType, float> DefaultStats = new()
    {
        { StatType.Damage, 1f },
        { StatType.Cooldown, 0.5f },
        { StatType.AttackRange, 3f },
        { StatType.ProjectileSpeed, 10f },
        { StatType.ProjectileCount, 1f },
        { StatType.CritChance, 0f }
    };

    // =========================
    // EDITOR AUTO FILL (CÁCH 2)
    // =========================
    private void OnValidate()
    {
        if (baseStats == null)
            baseStats = new List<StatEntry>();

        foreach (var kv in DefaultStats)
        {
            EnsureStat(kv.Key, kv.Value);
        }
    }

    private void EnsureStat(StatType type, float defaultValue)
    {
        foreach (var stat in baseStats)
        {
            if (stat.statType == type)
                return;
        }

        baseStats.Add(new StatEntry
        {
            statType = type,
            value = defaultValue
        });
    }

    // =========================
    // RUNTIME ACCESS
    // =========================
    public float GetBaseStat(StatType type)
    {
        foreach (var stat in baseStats)
        {
            if (stat.statType == type)
                return stat.value;
        }

        // Fallback (rất hiếm khi xảy ra)
        return DefaultStats.TryGetValue(type, out var v) ? v : 0f;
    }

    // =========================
    // VALIDATION (CÁCH 3)
    // =========================
    public bool IsValid(out string error)
    {
        //if (projectilePrefab == null)
        //{
        //    error = "Projectile prefab is missing";
        //    return false;
        //}

        if (GetBaseStat(StatType.AttackRange) <= 0f)
        {
            error = "AttackRange must be > 0";
            return false;
        }

        if (GetBaseStat(StatType.Cooldown) <= 0f)
        {
            error = "Cooldown must be > 0";
            return false;
        }

        if (GetBaseStat(StatType.ProjectileSpeed) <= 0f)
        {
            error = "ProjectileSpeed must be > 0";
            return false;
        }

        error = null;
        return true;
    }
}
