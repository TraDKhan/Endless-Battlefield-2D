using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour, IEnemyAttack
{
    private EnemyContext ctx;
    private float lastAttackTime;

    public void Init(EnemyContext context) => ctx = context;

    public bool CanAttack => Time.time >= lastAttackTime + ctx.Stats.attackCooldown;
    public float Cooldown => ctx.Stats.attackCooldown;


    public void StartAttack()
    {
        lastAttackTime = Time.time;
        ctx.Anim?.PlayAttack();
    }

    public void UpdateAttack() { }
    public void StopAttack() { }

    #region DAMAGE LOGIC

    // GỌI TỪ ANIMATION EVENT
    public void DealDamage()
    {

        Collider2D hit = Physics2D.OverlapCircle(transform.position, ctx.Stats.attackRange, ctx.TargetLayer);
        if (hit != null)
        {
            // Kiểm tra hướng (Dot Product) nếu cần quái chỉ đánh phía trước
            Vector2 dir = (hit.transform.position - transform.position).normalized;
            float facing = Mathf.Sign(transform.localScale.x);
            if (Vector2.Dot(Vector2.right * facing, dir) > 0)
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(ctx.Stats.damage);
            }
        }
    }
    #endregion
}
