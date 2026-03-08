using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Player Upgrade")]
public class PlayerUpgradeData : UpgradeData
{
    public CharacterStatType statType;
    public float valuePerLevel;

    public override bool CanApply(UpgradeSystem system)
    {
        return true; // sau này giới hạn level ở đây
    }

    public override void Apply(UpgradeSystem system)
    {
        system.PlayerSystem.Apply(this);
    }

    public override int GetCurrentLevel(UpgradeSystem system)
    {
        return system.PlayerSystem.GetLevel(statType);
    }

    public override string GetValueText(UpgradeSystem system)
    {
        int nextLevel = GetCurrentLevel(system) + 1;
        float value = nextLevel * valuePerLevel;
        return $"+{value} {statType}";
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
