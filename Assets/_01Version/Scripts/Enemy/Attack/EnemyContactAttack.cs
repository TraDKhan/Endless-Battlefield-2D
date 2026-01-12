using UnityEngine;

public class EnemyContactAttack : MonoBehaviour, IEnemyAttack
{
    private EnemyContext ctx;

    private bool isAttacking;
    private bool targetInRange;
    private IDamageable currentTarget;

    private float lastDamageTime;

    public void Init(EnemyContext context)
    {
        ctx = context;
    }

    // Có thể bắt đầu 1 đợt attack mới?
    public bool CanAttack => Time.time >= lastDamageTime + ctx.Stats.attackCooldown;

    public float Cooldown => ctx.Stats.attackCooldown;

    #region IEnemyAttack

    public void StartAttack()
    {
        isAttacking = true;
        ctx.Anim?.PlayAttack();
    }

    public void UpdateAttack()
    {
        if (!isAttacking) return;
        if (!targetInRange) return;
        if (currentTarget == null) return;

        if (Time.time < lastDamageTime + ctx.Stats.attackCooldown)
            return;

        lastDamageTime = Time.time;
        currentTarget.TakeDamage(ctx.Stats.damage);
    }

    public void StopAttack()
    {
        isAttacking = false;
    }

    #endregion

    #region Trigger Detection ONLY
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & ctx.TargetLayer) == 0)
            return;

        currentTarget = other.GetComponent<IDamageable>();
        targetInRange = currentTarget != null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & ctx.TargetLayer) == 0)
            return;

        if (other.GetComponent<IDamageable>() == currentTarget)
        {
            targetInRange = false;
            currentTarget = null;
        }
    }

    #endregion
}
