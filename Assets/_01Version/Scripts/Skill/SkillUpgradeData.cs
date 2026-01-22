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

    // ======================
    // LOGIC
    // ======================

    public override bool CanApply(UpgradeSystem system)
    {
        return GetCurrentLevel(system) < MaxLevel;
    }

    public override void Apply(UpgradeSystem system)
    {
        system.ApplySkillUpgrade(this);
    }

    public override string GetTitle()
    {
        return upgradeName;
    }

    public override int GetCurrentLevel(UpgradeSystem system)
    {
        return system.GetSkillLevel(this);
    }

    public override string GetValueText(UpgradeSystem system)
    {
        int currentLevel = GetCurrentLevel(system);

        if (currentLevel >= MaxLevel)
            return "Level Max";

        SkillData current = currentLevel > 0 ? GetLevelData(currentLevel) : null;
        SkillData next = GetLevelData(currentLevel + 1);

        StringBuilder sb = new System.Text.StringBuilder();

        void AddChange<T>(string label, T? cur, T nextVal) where T : struct
        {
            if (cur == null || !cur.Equals(nextVal))
                sb.AppendLine($"{label}: {(cur == null ? "" : cur.ToString() + " → ")}{nextVal}");
        }

        AddChange("Damage", current?.damage, next.damage);
        AddChange("Cooldown", current?.cooldown, next.cooldown);
        AddChange("Duration", current?.duration, next.duration);
        AddChange("Radius", current?.radius, next.radius);
        AddChange("Lightning", current?.lightningCount, next.lightningCount);

        return sb.ToString();
    }

    public override string GetDescription()
    {
        return description;
    }
}
