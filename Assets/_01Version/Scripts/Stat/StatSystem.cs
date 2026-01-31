using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatSystem<TStat> where TStat : System.Enum
{
    private readonly Dictionary<TStat, float> baseStats = new();
    protected readonly Dictionary<TStat, float> finalStats = new();
    private readonly List<IStatSource<TStat>> sources = new();

    // ================= BASE =================
    public void SetBaseStat(TStat type, float value)
    {
        baseStats[type] = value;
        Recalculate();
    }

    // ================= SOURCE =================
    public void AddSource(IStatSource<TStat> source)
    {
        if (source == null || sources.Contains(source)) return;
        sources.Add(source);
        Recalculate();
    }

    public void RemoveSource(IStatSource<TStat> source)
    {
        if (sources.Remove(source))
            Recalculate();
    }

    // ================= CALC =================
    public void Recalculate()
    {
        finalStats.Clear();

        // 1. Copy base stats
        foreach (var kv in baseStats)
        {
            finalStats[kv.Key] = kv.Value;
        }

        // 2. Apply all modifiers as flat
        foreach (var source in sources)
        {
            foreach (var mod in source.GetModifiers())
            {
                if (finalStats.ContainsKey(mod.statType))
                    finalStats[mod.statType] += mod.value;
                else
                    finalStats[mod.statType] = mod.value;
            }
        }

        // 3. Clamp
        Clamp();
    }

    protected virtual void Clamp() { }

    // ================= QUERY =================
    public float GetStat(TStat type)
        => finalStats.TryGetValue(type, out var v) ? v : 0f;
}