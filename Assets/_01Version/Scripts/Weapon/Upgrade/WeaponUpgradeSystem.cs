using UnityEngine;

public class WeaponUpgradeSystem : MonoBehaviour
{
    public Weapon currentWeapon;

    public void ApplyUpgrade(WeaponUpgrade upgrade)
    {
        currentWeapon.ApplyUpgrade(upgrade.bonusStats);
    }
}
