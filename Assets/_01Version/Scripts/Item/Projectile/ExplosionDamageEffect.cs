using System.Collections;
using UnityEngine;

public class ExplosionDamageEffect : IProjectileEffect
{
    private readonly float delay;
    private readonly float radius;
    private readonly LayerMask targetLayer;

    public ExplosionDamageEffect(float delay, float radius, LayerMask layer)
    {
        this.delay = delay;
        this.radius = radius;
        this.targetLayer = layer;
    }

    public void Apply(ProjectileCore projectile, Collider2D _)
    {
        projectile.StartCoroutine(ExplodeAfterDelay(projectile));
    }

    private IEnumerator ExplodeAfterDelay(ProjectileCore projectile)
    {
        yield return new WaitForSeconds(delay);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            projectile.transform.position,
            radius,
            targetLayer
        );

        foreach (var hit in hits)
        {
            if (hit.TryGetComponent<IDamageable>(out var damageable))
                damageable.TakeDamage(projectile.Damage);
        }
        Debug.Log("De Spawn");
        projectile.Despawn();
    }
}
