using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    [Header("References")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private EnemyAnimationController anim;

    [Header("Attack Settings")]
    [SerializeField] private LayerMask targetLayer;

    private float lastAttackTime;

    #region IEnemyAttack

    public bool CanAttack => Time.time >= lastAttackTime + stats.attackCooldown;

    public float Cooldown => stats.attackCooldown;

    public void StartAttack()
    {
        lastAttackTime = Time.time;
        anim.PlayAttack();
    }

    public void UpdateAttack() { }

    public void StopAttack()
    {
        anim.StopAttack();
    }

    #endregion

    #region DAMAGE (Animation Event)

    // GỌI TỪ ANIMATION EVENT
    public void DealDamage()
    {
        Vector2 origin = transform.position;

        float facing = Mathf.Sign(transform.localScale.x);
        Vector2 forward = Vector2.right * facing;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            origin,
            stats.attackRange,
            targetLayer
        );

        foreach (var hit in hits)
        {
            Vector2 dirToTarget = ((Vector2)hit.transform.position - origin).normalized;

            // Chỉ đánh phía trước
            float dot = Vector2.Dot(forward, dirToTarget);
            if (dot <= 0f) continue;

            hit.GetComponent<IDamageable>()
                ?.TakeDamage(stats.damage);
        }
    }
    #endregion
}
