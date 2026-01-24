using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterStats
{
    private readonly Dictionary<StatType, float> baseStats = new();
    private readonly Dictionary<StatType, float> finalStats = new();

    private readonly List<IStatSource> sources = new();

    public event Action OnStatsChanged;

    public CharacterStats(PlayerData data, params IStatSource[] statSources)
    {
        InitBaseStats(data);

        foreach (var s in statSources)
            AddSource(s);

        RecalculateStats();
    }

    private void InitBaseStats(PlayerData data)
    {
        foreach (var entry in data.baseStats)
            baseStats[entry.statType] = entry.value;
    }
    public void AddSource(IStatSource source)
    {
        if (source == null || sources.Contains(source))
            return;

        sources.Add(source);
        RecalculateStats();
    }

    public void RemoveSource(IStatSource source)
    {
        if (source == null)
            return;

        if (sources.Remove(source))
            RecalculateStats();
    }

    // ======================
    // CALCULATION
    // ======================
    public void RecalculateStats()
    {
        finalStats.Clear();

        // Copy base
        foreach (var kv in baseStats)
            finalStats[kv.Key] = kv.Value;

        List<StatModifier> allMods = new();
        foreach (var s in sources)
            allMods.AddRange(s.GetModifiers());

        ApplyModifiers(allMods);

        foreach (var kv in finalStats)
            Debug.Log($"FinalStat: {kv.Key} = {kv.Value}");

        OnStatsChanged?.Invoke();
    }

    private void ApplyModifiers(List<StatModifier> mods)
    {
        // Flat trước
        foreach (var m in mods.Where(m => m.modType == StatModType.Flat))
            finalStats[m.statType] = GetStat(m.statType) + m.value;

        // Percent sau
        foreach (var m in mods.Where(m => m.modType == StatModType.Percent))
            finalStats[m.statType] *= (1 + m.value);
    }

    public float GetStat(StatType type) => finalStats.TryGetValue(type, out var v) ? v : 0f;
    public int GetMaxHealth() => Mathf.RoundToInt(GetStat(StatType.MaxHP));
    public int GetMaxEnergy() => Mathf.RoundToInt(GetStat(StatType.MaxMP));
}