using UnityEngine;

public class WeaponStats
{
    private readonly StatSystem statSystem;

    public WeaponStats(StatSystem system)
    {
        statSystem = system;
    }

    // ===== BACKWARD API =====
    public int Damage => Mathf.RoundToInt(
        statSystem.GetStat(StatContext.Weapon, StatType.Damage));

    public float Cooldown =>
        statSystem.GetStat(StatContext.Weapon, StatType.Cooldown);

    public float CritChance =>
        statSystem.GetStat(StatContext.Weapon, StatType.CritChance);

    public int ProjectileCount => Mathf.RoundToInt(
        statSystem.GetStat(StatContext.Weapon, StatType.ProjectileCount));

    public float AttackRange =>
        statSystem.GetStat(StatContext.Weapon, StatType.AttackRange);

    public float ProjectileSpeed =>
        statSystem.GetStat(StatContext.Weapon, StatType.ProjectileSpeed);
}
