using UnityEngine;

public class WeaponStats
{
    private readonly WeaponStatSystem stats;

    public WeaponStats(WeaponStatSystem stats)
    {
        this.stats = stats;
    }

    public int Damage =>
        Mathf.RoundToInt(stats.GetStat(WeaponStatType.Damage));

    public float Cooldown =>
        stats.GetStat(WeaponStatType.Cooldown);

    public float CritChance =>
        stats.GetStat(WeaponStatType.CritChance);

    public int ProjectileCount =>
        Mathf.RoundToInt(stats.GetStat(WeaponStatType.ProjectileCount));

    public float AttackRange =>
        stats.GetStat(WeaponStatType.AttackRange);

    public float ProjectileSpeed =>
        stats.GetStat(WeaponStatType.ProjectileSpeed);
}
