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

    public WeaponStats(WeaponData data)
    {
        this.data = data;
    }
    public void BindUpgradeSystem(WeaponUpgradeSystem upgrade)
    {
        this.upgrade = upgrade;
        Recalculate();
    }
    public void Recalculate()
    {
        if (data == null) return;
        if (upgrade == null) return;

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
