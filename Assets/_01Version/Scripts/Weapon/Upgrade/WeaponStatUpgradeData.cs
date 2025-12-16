using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Weapon Stat")]
public class WeaponStatUpgradeData : UpgradeData
{
    public WeaponStatType statType;
    public float value;
    public int maxLevel;
    public override int GetCurrentLevel()
    {
        return WeaponUpgradeSystem.Instance.GetStatLevel(statType);
    }

    public override string GetTitle()
    {
        return statType.ToString();
    }

    public override string GetDescription()
    {
        int nextLevel = Mathf.Min(GetCurrentLevel() + 1, maxLevel);
        return $"+{value} {statType}";
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
