using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour, IStatSource<WeaponStatType>
{
    private readonly Dictionary<WeaponStatType, StatUpgrade> upgrades = new();

    // =========================
    // APPLY UPGRADE
    // =========================
    public void ApplyUpgrade(WeaponUpgradeData data)
    {
        if (!upgrades.TryGetValue(data.statType, out var up))
        {
            up = new StatUpgrade
            {
                level = 0,
                valuePerLevel = data.valuePerLevel
            };
        }

        up.level++;
        upgrades[data.statType] = up;
    }

    // =========================
    // IStatSource
    // =========================
    public IEnumerable<StatModifier<WeaponStatType>> GetModifiers()
    {
        foreach (var kv in upgrades)
        {
            yield return new StatModifier<WeaponStatType>
            {
                statType = kv.Key,
                value = kv.Value.Value
            };
        }
    }

    // =========================
    // QUERY (UI / DEBUG)
    // =========================
    public int GetLevel(WeaponStatType stat)
        => upgrades.TryGetValue(stat, out var up) ? up.level : 0;

    public float GetValue(WeaponStatType stat)
        => upgrades.TryGetValue(stat, out var up) ? up.Value : 0f;
}
