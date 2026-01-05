using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    [Header("References")]
    [SerializeField] private EnemyStats stats;
    [SerializeField] private EnemyAnimationController anim;

    [Header("Attack Settings")]
    [SerializeField] private float attackRadius = 1.2f;
    [SerializeField] private LayerMask targetLayer;

    [Header("Debug")]
    [SerializeField] private bool drawGizmos = true;

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
            attackRadius,
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

    #region Gizmos

    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Vector3 origin = transform.position;
        float facing = Application.isPlaying
            ? Mathf.Sign(transform.localScale.x)
            : 1f;

        Vector3 forward = Vector3.right * facing;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, attackRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(origin, origin + forward * attackRadius);
    }

    #endregion
}
