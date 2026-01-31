using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Skill Data")]
public class SkillUpgradeData : UpgradeData
{
    public GameObject skillPrefab;

    [Header("Levels")]
    public SkillData[] levels;

    public int MaxLevel => levels.Length;

    public SkillData GetLevelData(int level)
    {
        int index = Mathf.Clamp(level - 1, 0, levels.Length - 1);
        return levels[index];
    }

    public override bool CanApply(UpgradeSystem system)
        => GetCurrentLevel(system) < MaxLevel;

    public override void Apply(UpgradeSystem system)
        => system.ApplySkillUpgrade(this);

    public override int GetCurrentLevel(UpgradeSystem system)
        => system.GetSkillLevel(this);

    public override string GetValueText(UpgradeSystem system)
    {
        int level = GetCurrentLevel(system);

        if (level >= MaxLevel)
            return "Level Max";

        SkillData cur = level > 0 ? GetLevelData(level) : null;
        SkillData next = GetLevelData(level + 1);

        StringBuilder sb = new();

        foreach (var stat in next.baseStats)
        {
            float curVal = cur?.GetBaseStat(stat.statType) ?? 0;
            if (!Mathf.Approximately(curVal, stat.value))
                sb.AppendLine($"{stat.statType}: {curVal} → {stat.value}");
        }

        return sb.ToString();
    }
}
