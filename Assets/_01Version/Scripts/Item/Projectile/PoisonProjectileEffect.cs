using UnityEngine;

public class PoisonProjectileEffect : IProjectileEffect
{
    private readonly float poisonDuration;
    private readonly int poisonDamagePerSecond;

    public PoisonProjectileEffect(float duration, int dps)
    {
        poisonDuration = duration;
        poisonDamagePerSecond = dps;
    }

    public void Apply(ProjectileCore projectile, Collider2D hitTarget)
    {
        if (hitTarget.TryGetComponent<IDamageable>(out var damageable))
            damageable.TakeDamage(projectile.Damage);

        if (hitTarget.TryGetComponent<IStatusEffectReceiver>(out var status))
            status.ApplyPoison(poisonDuration, poisonDamagePerSecond);
    }
}
