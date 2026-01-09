using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Weapon Unlock Data")]
public class WeaponUpgradeData : UpgradeData
{
    public GameObject weaponPrefab;

    public override bool CanApply()
    {
        return !UpgradeSystem.Instance.HasWeapon(this);
    }

    public override void Apply()
    {
        UpgradeSystem.Instance.UnlockWeapon(this);
    }

    public override int GetCurrentLevel()
    {
        return UpgradeSystem.Instance.HasWeapon(this) ? 1 : 0;
    }

    public override string GetTitle()
    {
        return upgradeName;
    }

    public override string GetDescription()
    {
        return description;
    }

    public override string GetLevelText()
    {
        return "Unlock";
    }
}
