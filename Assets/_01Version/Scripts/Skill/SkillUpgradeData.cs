using System.Collections.Generic;
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
    public override int GetCurrentLevel()
    {
        return UpgradeSystem.Instance.GetSkillLevel(this);
    }

    public override string GetTitle()
    {
        return upgradeName;
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
