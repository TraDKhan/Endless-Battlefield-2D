using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    private EnemyContext ctx;
    private float lastAttackTime;

    public void Init(EnemyContext context)
    {
        ctx = context;
    }

    #region IEnemyAttack
    public bool CanAttack => Time.time >= lastAttackTime + ctx.Stats.attackCooldown;
    public float Cooldown => ctx.Stats.attackCooldown;

    public void StartAttack()
    {
        lastAttackTime = Time.time;
        ctx.Anim.PlayAttack();
    }

    public void UpdateAttack() { }

    public void StopAttack() { }

    #endregion

    #region DAMAGE (Animation Event)

    // GỌI TỪ ANIMATION EVENT
    public void DealDamage()
    {
        if (ctx.Target == null) return;

        Vector2 origin = ctx.Controller.transform.position;

        float facing = Mathf.Sign(ctx.Controller.transform.localScale.x);
        Vector2 forward = Vector2.right * facing;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            origin,
            ctx.Stats.attackRange,
            ctx.TargetLayer
        );

        foreach (var hit in hits)
        {
            Vector2 dir = ((Vector2)hit.transform.position - origin).normalized;
            if (Vector2.Dot(forward, dir) <= 0f) continue;

            hit.GetComponent<IDamageable>()?.TakeDamage(ctx.Stats.damage);
        }
    }
    #endregion
}
