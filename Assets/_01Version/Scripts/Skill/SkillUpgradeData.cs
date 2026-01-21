using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Skill Upgrade Data")]
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

    public override string GetTitle()
    {
        return upgradeName;
    }

    public override int GetCurrentLevel()
    {
        return UpgradeSystem.Instance.GetSkillLevel(this);
    }

    public override string GetValueText()
    {
        int currentLevel = GetCurrentLevel();

        if (currentLevel >= MaxLevel)
            return "Đã đạt cấp tối đa";

        SkillData current = currentLevel > 0 ? GetLevelData(currentLevel) : null;
        SkillData next = GetLevelData(currentLevel + 1);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

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
        if (GetCurrentLevel() == 0)
            return description;

        return $"Nâng cấp {upgradeName} lên Lv {GetCurrentLevel() + 1}";
    }

    public override bool CanApply()
    {
        int current = UpgradeSystem.Instance.GetSkillLevel(this);
        return current < MaxLevel;
    }

    public override void Apply()
    {
        UpgradeSystem.Instance.ApplySkillUpgrade(this);
    }
}
