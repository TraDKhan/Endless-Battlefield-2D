using System.Collections.Generic;
using UnityEngine;

public class SkillStats
{
    private readonly Dictionary<StatType, float> stats = new();

    public void ApplySkillData(SkillData data)
    {
        stats.Clear();
        if (data == null) return;

        foreach (var s in data.baseStats)
            stats[s.statType] = s.value;
    }

    public float GetStat(StatType type)
        => stats.TryGetValue(type, out var v) ? v : 0f;

    public int GetInt(StatType type)
        => Mathf.RoundToInt(GetStat(type));
}
