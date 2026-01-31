using System.Collections.Generic;

[System.Serializable]
public class SkillData
{
    public string skillName;
    public List<SKStatEntry> baseStats;

    public float GetBaseStat(SkillStatType type)
    {
        foreach (var entry in baseStats)
        {
            if (entry.statType == type)
                return entry.value;
        }
        return 0f;
    }
}
