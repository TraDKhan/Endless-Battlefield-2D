using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Unlock Weapon")]
public class UnLockWeaponUpgrade : UpgradeData
{
    public GameObject weaponPrefab;

    // ======================
    // LOGIC
    // ======================

    public override bool CanApply(UpgradeSystem system)
    {
        return !system.HasWeapon(this);
    }

    public override void Apply(UpgradeSystem system)
    {
        system.UnlockWeapon(this);
    }

    // ======================
    // UI
    // ======================

    public override int GetCurrentLevel(UpgradeSystem system)
    {
        return system.HasWeapon(this) ? 1 : 0;
    }

    public override string GetValueText(UpgradeSystem system)
    {
        return system.HasWeapon(this)
        ? "Unlocked"
        : "Unlock weapon";
    }

    public override string GetDescription()
    {
        return description;
    }

    public override string GetLevelText(UpgradeSystem system)
    {
        return "Unlock";
    }
}
