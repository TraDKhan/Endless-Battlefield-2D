using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour
{
    public static WeaponUpgradeSystem Instance { get; private set; }

    public event Action OnWeaponStatsChanged;

    // =========================
    // INTERNAL DATA
    // =========================
    private Dictionary<StatType, StatUpgrade> statUpgrades = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // =========================
    // APPLY UPGRADE
    // =========================
    public void ApplyUpgrade(StatType stat, float valuePerLevel, StatModType modType)
    {
        if (!statUpgrades.TryGetValue(stat, out var upgrade))
        {
            upgrade = new StatUpgrade
            {
                level = 0,
                valuePerLevel = valuePerLevel,
                modType = modType
            };
        }

        upgrade.level++;
        statUpgrades[stat] = upgrade;

        OnWeaponStatsChanged?.Invoke();
    }

    // =========================
    // BUILD MODIFIERS
    // =========================
    public List<StatModifier> GetAllModifiers()
    {
        List<StatModifier> result = new();

        foreach (var kvp in statUpgrades)
        {
            var upgrade = kvp.Value;

            result.Add(new StatModifier
            {
                statType = kvp.Key,
                modType = upgrade.modType,
                value = upgrade.Value
            });
        }

        return result;
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