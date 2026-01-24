using System.Collections.Generic;

[System.Serializable]
public class SkillData
{
    public List<StatEntry> baseStats;
    public float GetStat(StatType type)
    {
        var entry = baseStats.Find(s => s.statType == type);
        return entry != null ? entry.value : 0f;
    }
}
