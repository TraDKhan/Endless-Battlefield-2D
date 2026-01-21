using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[CreateAssetMenu(menuName = "Upgrade/Weapon Stat")]
public class WeaponStatUpgradeData : UpgradeData
{
    public WeaponStatType statType;
    public float valuePerLevel;
    public int maxLevel = 5;
    public override int GetCurrentLevel()
    {
        return WeaponUpgradeSystem.Instance.GetStatLevel(statType);
    }

    public override string GetTitle()
    {
        return upgradeName;
    }

    public override string GetValueText()
    {
        return $"{statType.ToString()}: +{valuePerLevel}";
    }

    public override string GetDescription()
    {
        int nextLevel = Mathf.Min(GetCurrentLevel() + 1, maxLevel);
        return $"+{valuePerLevel} {statType}\n{description}";
    }
    public override bool CanApply()
    {
        return WeaponUpgradeSystem.Instance.GetStatLevel(statType) < maxLevel;
    }

    public override void Apply()
    {
        WeaponUpgradeSystem.Instance.ApplyUpgrade(statType, valuePerLevel);
    }

}
