using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Player Upgrade")]
public class PlayerUpgradeData : UpgradeData
{
    public StatType statType;
    public float valuePerLevel;
    public StatModType modType = StatModType.Flat;

    public override bool CanApply(UpgradeSystem system)
    {
        return true; // sau này giới hạn level ở đây
    }

    public override void Apply(UpgradeSystem system)
    {
        system.ApplyPlayerStatUpgrade(this);
    }

    public override int GetCurrentLevel(UpgradeSystem system)
    {
        return system.GetPlayerStatLevel(statType);
    }

    public override string GetValueText(UpgradeSystem system)
    {
        int nextLevel = GetCurrentLevel(system) + 1;
        float value = nextLevel * valuePerLevel;

        string sign = modType == StatModType.Percent ? "%" : "";
        return $"+{value}{sign} {statType}";
    }

    public override string GetDescription()
    {
        return description;
    }
    public override string GetLevelText(UpgradeSystem system)
    {
        int currentLevel = GetCurrentLevel(system) + 1;
        int nextLevel = currentLevel + 1;

        return $"Level {currentLevel} -> Level {nextLevel}";
    }
}
