using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour, IStatSource<WeaponStatType>
{
    private readonly Dictionary<WeaponStatType, StatUpgrade> upgrades = new();

    public void Apply(WeaponUpgradeData data)
    {
        if(!upgrades.TryGetValue(data.statType, out var up))
        {
            up = new StatUpgrade
            {
                level = 0,
                valuePerLevel = data.valuePerLevel
            };
        }

        up.level++;
        upgrades[data.statType] = up;

        WeaponController.Instance.AddStatSource(this);
        WeaponController.Instance.StatSystem.Recalculate();
        WeaponController.Instance.WeaponBase.OnStatsChanged();
    }

    /* Todo: áp dụng chỉ số cho toàn bộ vũ khí
    private void NotifyWeapons()
    {
        foreach (var weapon in FindObjectsByType<WeaponController>(FindObjectsSortMode.None))
        {
            weapon.StatSystem.Recalculate();
            weapon.OnStatsChanged();
        }
    }
    */

    // =========================
    // IStatSource
    // =========================
    public IEnumerable<StatModifier<WeaponStatType>> GetModifiers()
    {
        foreach (var kv in upgrades)
        {
            Debug.Log($"Weapon Upgrade {kv.Key} = {kv.Value.Value}");
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
