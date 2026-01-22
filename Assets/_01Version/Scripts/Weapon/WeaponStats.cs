using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponStats
{
    // =========================
    // INTERNAL
    // =========================
    private Dictionary<StatType, float> baseStats = new();
    private Dictionary<StatType, float> finalStats = new();

    private WeaponData data;
    private WeaponUpgradeSystem upgrade;

    // =========================
    // BACKWARD FIELDS (GIỮ LOGIC CŨ)
    // =========================
    public int Damage => Mathf.RoundToInt(GetStat(StatType.Damage));
    public float Cooldown => GetStat(StatType.Cooldown);
    public float CritChance => GetStat(StatType.CritChance);
    public int ProjectileCount => Mathf.RoundToInt(GetStat(StatType.ProjectileCount));
    public float AttackRange => GetStat(StatType.AttackRange);
    public float ProjectileSpeed => GetStat(StatType.ProjectileSpeed);

    // =========================
    // CONSTRUCTOR
    // =========================
    public WeaponStats(WeaponData data)
    {
        this.data = data;
        InitBaseStats();
    }

    private void InitBaseStats()
    {
        baseStats[StatType.Damage] = data.GetBaseStat(StatType.Damage);
        baseStats[StatType.Cooldown] = data.GetBaseStat(StatType.Cooldown);
        baseStats[StatType.CritChance] = data.GetBaseStat(StatType.CritChance);
        baseStats[StatType.ProjectileCount] = data.GetBaseStat(StatType.ProjectileCount);
        baseStats[StatType.AttackRange] = data.GetBaseStat(StatType.AttackRange);
        baseStats[StatType.ProjectileSpeed] = data.GetBaseStat(StatType.ProjectileSpeed);
    }

    public void BindUpgradeSystem(WeaponUpgradeSystem upgrade)
    {
        this.upgrade = upgrade;
        Recalculate();
    }

    // =========================
    // RECALCULATE
    // =========================
    public void Recalculate()
    {
        finalStats = new Dictionary<StatType, float>(baseStats);

        ApplyModifiers(upgrade?.GetModifiers());

        ClampStats();
    }

    private void ApplyModifiers(IEnumerable<StatModifier> modifiers)
    {
        if (modifiers == null) return;

        foreach (var mod in modifiers)
        {
            if (!finalStats.ContainsKey(mod.statType))
                finalStats[mod.statType] = 0;

            if (mod.modType == StatModType.Flat)
                finalStats[mod.statType] += mod.value;
            else
                finalStats[mod.statType] *= (1 + mod.value);
        }
        //foreach (var kv in finalStats)
        //    Debug.Log($"FinalStat: {kv.Key} = {kv.Value}");
    }

    private void ClampStats()
    {
        // cooldown không bao giờ <= 0
        if (finalStats.ContainsKey(StatType.Cooldown))
            finalStats[StatType.Cooldown] = Mathf.Max(0.05f, finalStats[StatType.Cooldown]);
    }

    // =========================
    // NEW API
    // =========================
    public float GetStat(StatType type)
    {
        return finalStats.TryGetValue(type, out var v) ? v : 0f;
    }
}
