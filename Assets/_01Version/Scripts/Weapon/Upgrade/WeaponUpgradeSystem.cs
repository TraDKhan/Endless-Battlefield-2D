using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour
{
    public static WeaponUpgradeSystem Instance { get; private set; }

    public event Action OnWeaponStatsChanged;

    private Dictionary<WeaponStatType, StatUpgrade> statUpgrades
        = new Dictionary<WeaponStatType, StatUpgrade>();

    private void Awake()
    {
        Instance = this;
    }

    // =============================
    // APPLY UPGRADE
    // =============================
    public void ApplyUpgrade(WeaponStatType stat, float valuePerLevel)
    {
        if (!statUpgrades.TryGetValue(stat, out var upgrade))
        {
            upgrade = new StatUpgrade
            {
                level = 0,
                valuePerLevel = valuePerLevel
            };
        }

        upgrade.level++;
        statUpgrades[stat] = upgrade;

        OnWeaponStatsChanged?.Invoke();
    }

    // =============================
    // QUERY
    // =============================
    public float GetWeaponStatBonus(WeaponStatType stat)
    {
        return statUpgrades.TryGetValue(stat, out var upgrade)
            ? upgrade.Value
            : 0;
    }

    public int GetStatLevel(WeaponStatType stat)
    {
        return statUpgrades.TryGetValue(stat, out var upgrade)
            ? upgrade.level
            : 0;
    }
}

[System.Serializable]
public struct StatUpgrade
{
    public int level;
    public float valuePerLevel;

    public float Value => level * valuePerLevel;
}