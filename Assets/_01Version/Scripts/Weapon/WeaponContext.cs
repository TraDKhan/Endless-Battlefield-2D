using UnityEngine;

public struct WeaponContext
{
    // ===== Owner =====
    public GameObject Source { get; }
    public Transform FirePoint { get; }

    // ===== Combat =====
    public int Damage { get; }
    public float CritChance { get; }
    public float ProjectileSpeed { get; }
    public int ProjectileCount { get; }
    public float Range { get; }

    // ===== Extra =====
    public float KnockbackForce { get; }
    public LayerMask TargetLayer { get; }

    public WeaponContext(
        GameObject source,
        Transform firePoint,
        WeaponStats stats,
        LayerMask targetLayer,
        float knockbackForce
    )
    {
        Source = source;
        FirePoint = firePoint;

        Damage = stats.Damage;
        CritChance = stats.CritChance;
        ProjectileSpeed = stats.ProjectileSpeed;
        ProjectileCount = stats.ProjectileCount;
        Range = stats.Range;

        KnockbackForce = knockbackForce;
        TargetLayer = targetLayer;
    }
}
