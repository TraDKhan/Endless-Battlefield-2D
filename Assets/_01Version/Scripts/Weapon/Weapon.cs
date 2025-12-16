using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData data;

    protected WeaponStats stats;
    protected WeaponUpgradeSystem upgradeSystem;

    protected virtual void Awake()
    {
        upgradeSystem = WeaponUpgradeSystem.Instance;
        stats = new WeaponStats(data, upgradeSystem);

        upgradeSystem.OnWeaponStatsChanged += OnWeaponStatsChanged;
    }

    protected virtual void OnDestroy()
    {
        if (upgradeSystem != null)
            upgradeSystem.OnWeaponStatsChanged -= OnWeaponStatsChanged;
    }

    protected virtual void OnWeaponStatsChanged()
    {
        stats.Recalculate();
    }

    // ===== Damage Context chuẩn hoá =====
    protected WeaponDamageContext CreateDamageContext()
    {
        return new WeaponDamageContext
        {
            damage = stats.Damage,
            critChance = stats.CritChance,
            critMultiplier = 2f,
            source = gameObject
        };
    }

    public abstract void Fire();
}

public struct WeaponDamageContext
{
    public float damage;
    public float critChance;
    public float critMultiplier;
    public GameObject source;
}
