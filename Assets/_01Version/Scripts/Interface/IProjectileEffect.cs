using UnityEngine;

public interface IProjectileEffect
{
    void Apply(ProjectileCore projectile, Collider2D hitTarget);
}
