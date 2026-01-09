using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Player Stat")]
public class PlayerStatUpgradeData : UpgradeData
{
    public PlayerStatType statType;
    public float value;
    public int maxLevel;
    public override int GetCurrentLevel()
    {
        return UpgradeSystem.Instance.GetPlayerStatLevel(statType);
    }

    public override string GetTitle()
    {
        return upgradeName;
    }

    public override string GetDescription()
    {
        if (GetCurrentLevel() == 0)
            return $"+{value} Health\n{description}";

        return $"Nâng cấp {upgradeName} lên Lv {GetCurrentLevel() + 1}";
    }
    public override bool CanApply()
    {
        return UpgradeSystem.Instance.GetPlayerStatLevel(statType) < maxLevel;
    }

    public override void Apply()
    {
        UpgradeSystem.Instance.IncreasePlayerStatLevel(statType, value);
    }
}
