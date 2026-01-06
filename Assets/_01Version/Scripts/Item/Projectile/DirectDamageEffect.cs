using UnityEngine;

public class DirectDamageEffect : IProjectileEffect
{
    public void Apply(ProjectileCore projectile, Collider2D hitTarget)
    {
        if (!hitTarget.TryGetComponent<IDamageable>(out var damageable))
            return;

        damageable.TakeDamage(projectile.Damage);
    }
}
