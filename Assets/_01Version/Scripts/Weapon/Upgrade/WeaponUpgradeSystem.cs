using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour
{
    public event Action OnWeaponStatsChanged;

    // =========================
    // INTERNAL DATA
    // =========================
    private Dictionary<StatType, StatUpgrade> statUpgrades = new();

    public void ApplyUpgrade(WeaponUpgradeData data)
    {
        if (!statUpgrades.TryGetValue(data.statType, out var up))
        {
            up = new StatUpgrade
            {
                level = 0,
                valuePerLevel = data.valuePerLevel,
                modType = data.modType
            };
        }

        up.level++;
        statUpgrades[data.statType] = up;

        OnWeaponStatsChanged?.Invoke();
    }

    public IEnumerable<StatModifier> GetModifiers()
    {
        foreach (var kv in statUpgrades)
        {
            yield return new StatModifier
            {
                statType = kv.Key,
                modType = kv.Value.modType,
                value = kv.Value.Value
            };
        }
    }

    // =========================
    // UI SUPPORT
    // =========================
    public int GetStatLevel(StatType stat)
    {
        return statUpgrades.TryGetValue(stat, out var upgrade)
            ? upgrade.level
            : 0;
    }

    public float GetStatValue(StatType stat)
    {
        return statUpgrades.TryGetValue(stat, out var upgrade)
            ? upgrade.Value
            : 0f;
    }
}