using UnityEngine;

public class WeaponStats
{
    public int Damage;
    public float Cooldown;
    public float CritChance;
    public int ProjectileCount;
    public float Range;
    public float ProjectileSpeed;

    private WeaponData data;
    private WeaponUpgradeSystem upgrade;

    public WeaponStats(WeaponData data, WeaponUpgradeSystem upgrade)
    {
        this.data = data;
        this.upgrade = upgrade;

        Recalculate();
    }

    public void Recalculate()
    {
        Damage = data.baseDamage
            + Mathf.RoundToInt(upgrade.GetWeaponStatBonus(WeaponStatType.Damage));

        Cooldown = Mathf.Max(0.05f,
            data.baseCooldown
            - upgrade.GetWeaponStatBonus(WeaponStatType.Cooldown));

        CritChance = data.baseCritChance
            + upgrade.GetWeaponStatBonus(WeaponStatType.CritChance);

        ProjectileCount = data.baseProjectileCount
            + Mathf.RoundToInt(upgrade.GetWeaponStatBonus(WeaponStatType.ProjectileCount));

        Range = data.baseRange
            + upgrade.GetWeaponStatBonus(WeaponStatType.Range);

        ProjectileSpeed = data.baseProjectileSpeed
            + upgrade.GetWeaponStatBonus(WeaponStatType.ProjectileSpeed);
    }
}
