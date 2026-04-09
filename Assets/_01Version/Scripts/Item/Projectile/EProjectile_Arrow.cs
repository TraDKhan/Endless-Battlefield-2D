using System.Collections;
using UnityEngine;

public class EProjectile_Arrow : ProjectileCore
{
    protected override void OnHit(Collider2D target)
    {
        PlayerHealthController player = target.GetComponent<PlayerHealthController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        Despawn();
    }
}