using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Weapon Stat")]
public class WeaponStatUpgradeData : UpgradeData
{
    public WeaponStatType statType;
    public float value;
    public int maxLevel = 5;
    public override int GetCurrentLevel()
    {
        return WeaponUpgradeSystem.Instance.GetStatLevel(statType);
    }

    public override string GetTitle()
    {
        return upgradeName;
    }

    public override string GetDescription()
    {
        int nextLevel = Mathf.Min(GetCurrentLevel() + 1, maxLevel);
        return $"+{value} {statType}\n{description}";
    }
    public override bool CanApply()
    {
        return WeaponUpgradeSystem.Instance.GetStatLevel(statType) < maxLevel;
    }

    public override void Apply()
    {
        WeaponUpgradeSystem.Instance.ApplyUpgrade(statType, value);
    }

}
