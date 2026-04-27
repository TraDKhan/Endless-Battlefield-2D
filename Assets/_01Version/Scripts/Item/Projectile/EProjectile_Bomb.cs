using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EProjectile_Bomb : ProjectileCore
{
    [Header("Explosion")]
    [SerializeField] private Animator animator;
    [SerializeField] private float explosionDelay = 0.5f;

    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private LayerMask playerLayer;

    protected override void OnHit(Collider2D target)
    {
        StartCoroutine(Explode(target));
    }

    private IEnumerator Explode(Collider2D target)
    {
        isProcessingHit = true;
        speed = 0;
        reachedTarget = true;


        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, playerLayer);

        foreach (var hit in hits)
        {
            PlayerHealth player = hit.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

        yield return new WaitForSeconds(explosionDelay);

        Despawn();
    }

    protected override void OnLifetimeExpired()
    {
        if (!isProcessingHit)
        {
            StartCoroutine(Explode(null));
        }
    }
}