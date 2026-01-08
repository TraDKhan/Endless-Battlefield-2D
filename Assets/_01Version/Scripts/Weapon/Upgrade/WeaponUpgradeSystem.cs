using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour
{
    public static WeaponUpgradeSystem Instance { get; private set; }

    public event Action OnWeaponStatsChanged;

    private Dictionary<WeaponStatType, StatUpgradeProgress> statUpgrades
        = new Dictionary<WeaponStatType, StatUpgradeProgress>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // =============================
    // APPLY UPGRADE
    // =============================
    public void ApplyUpgrade(WeaponStatType stat, float value)
    {
        if (!statUpgrades.ContainsKey(stat))
            statUpgrades[stat] = new StatUpgradeProgress(value);

        statUpgrades[stat].LevelUp();

        OnWeaponStatsChanged?.Invoke();
    }

    // =============================
    // QUERY
    // =============================
    public float GetWeaponStatBonus(WeaponStatType stat)
    {
        return statUpgrades.TryGetValue(stat, out var p)
            ? p.GetValue()
            : 0;
    }

    public int GetStatLevel(WeaponStatType stat)
    {
        return statUpgrades.TryGetValue(stat, out var p)
            ? p.Level
            : 0;
    }
}
