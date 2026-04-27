using System.Collections;
using UnityEngine;

public class EProjectile_Poison : ProjectileCore
{
    [SerializeField] private float poisonDuration = 3f;
    [SerializeField] private float tickInterval = 1f;

    protected override void OnHit(Collider2D target)
    {
        PlayerHealth player = target.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.ApplyPoison(damage, poisonDuration, tickInterval);
        }

        Despawn();
    }
}