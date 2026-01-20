using UnityEngine;

public class EnemyContactAttack : MonoBehaviour, IEnemyAttack
{
    private EnemyContext ctx;

    private IDamageable currentTarget;
    private bool isAttacking;

    private float lastAttackTime;

    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    public void Init(EnemyContext context)
    {
        ctx = context;
    }

    public bool CanAttack =>
        currentTarget != null &&
        Time.time >= lastAttackTime + ctx.Stats.attackCooldown;

    public float Cooldown => ctx.Stats.attackCooldown;

    #region IEnemyAttack

    public void StartAttack()
    {
        if (currentTarget == null) return;

        isAttacking = true;
        ctx.Anim?.PlayAttack();
    }

    public void UpdateAttack()
    {
        if (!isAttacking) return;
        if (currentTarget == null) return;
        if (!CanAttack) return;
        if (!IsTargetInAttackRange()) return;

        lastAttackTime = Time.time;
        currentTarget.TakeDamage(ctx.Stats.damage);
    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    #endregion

    #region Target Detection (Trigger only)

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsTargetLayer(other.gameObject.layer))
            return;

        if (currentTarget != null) return;

        currentTarget = other.GetComponent<IDamageable>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsTargetLayer(other.gameObject.layer))
            return;

        if (other.GetComponent<IDamageable>() == currentTarget)
        {
            currentTarget = null;
            StopAttack();
        }
    }

    #endregion

    #region Helpers

    bool IsTargetLayer(int layer)
    {
        return ((1 << layer) & ctx.TargetLayer) != 0;
    }

    bool IsTargetInAttackRange()
    {
        var targetMono = currentTarget as MonoBehaviour;
        if (targetMono == null) return false;

        float dist = Vector2.Distance(
            transform.position,
            targetMono.transform.position
        );

        return dist <= ctx.Stats.attackRange;
    }

    #endregion
}
