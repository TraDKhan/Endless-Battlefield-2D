using System.Collections;
using UnityEngine;

public class EProjectile_Arrow : ProjectileCore
{
    protected override void OnHit(Collider2D target)
    {
        PlayerHealth player = target.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Despawn();
    }
}