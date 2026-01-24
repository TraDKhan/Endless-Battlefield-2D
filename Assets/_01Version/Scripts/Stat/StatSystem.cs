using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatSystem
{
    private readonly Dictionary<StatContext, Dictionary<StatType, float>> baseStats = new();
    private readonly Dictionary<StatContext, Dictionary<StatType, float>> finalStats = new();

    private readonly List<IStatSource> sources = new();

    // =========================
    // BASE STATS
    // =========================
    public void SetBaseStat(StatContext context, StatType type, float value)
    {
        if (!baseStats.ContainsKey(context))
            baseStats[context] = new();

        baseStats[context][type] = value;
    }

    // =========================
    // SOURCES
    // =========================
    public void AddSource(IStatSource source)
    {
        if (source == null || sources.Contains(source)) return;
        sources.Add(source);
        Recalculate();
    }

    public void RemoveSource(IStatSource source)
    {
        if (sources.Remove(source))
            Recalculate();
    }

    // =========================
    // CALCULATION
    // =========================
    public void Recalculate()
    {
        finalStats.Clear();

        // Copy base
        foreach (var ctx in baseStats)
            finalStats[ctx.Key] = new Dictionary<StatType, float>(ctx.Value);

        // Group modifier by context
        var modsByContext = sources
            .SelectMany(s => s.GetModifiers())
            .GroupBy(m => m.context);

        foreach (var group in modsByContext)
        {
            if (!finalStats.ContainsKey(group.Key))
                finalStats[group.Key] = new();

            ApplyModifiers(finalStats[group.Key], group.ToList());
        }

        Clamp();
    }

    private void ApplyModifiers(
        Dictionary<StatType, float> stats,
        List<StatModifier> mods
    )
    {
        // Flat
        foreach (var m in mods.Where(m => m.modType == StatModType.Flat))
            stats[m.statType] = stats.GetValueOrDefault(m.statType) + m.value;

        // Percent
        foreach (var m in mods.Where(m => m.modType == StatModType.Percent))
            stats[m.statType] *= (1 + m.value);
    }

    private void Clamp()
    {
        if (finalStats.TryGetValue(StatContext.Weapon, out var weapon))
        {
            if (weapon.ContainsKey(StatType.Cooldown))
                weapon[StatType.Cooldown] = Mathf.Max(0.05f, weapon[StatType.Cooldown]);
        }
    }

    // =========================
    // QUERY
    // =========================
    public float GetStat(StatContext context, StatType type)
    {
        return finalStats.TryGetValue(context, out var stats) &&
               stats.TryGetValue(type, out var v)
            ? v
            : 0f;
    }
}
