using System.Collections;
using UnityEngine;

public class BMeeleAttack : BaseBasicAttack
{
    [Header("Melee Settings")]
    [SerializeField] float attackAnimDuration = 0.7f;
    [SerializeField] float hitRadiusMultiplier = 1f;

    private int empoweredHits;
    private BossContext cachedCtx;

    // =========================
    // EMPOWER
    // =========================
    public void EmpowerNextHits(int count)
    {
        empoweredHits += count;
    }

    // =========================
    // BASIC ATTACK
    // =========================
    public override IEnumerator Attack(BossContext ctx)
    {
        cachedCtx = ctx;

        ctx.Movement?.Stop();
        PlayDirectionalAttack(ctx);

        // ⏱ đợi animation kết thúc
        yield return new WaitForSeconds(attackAnimDuration);
    }

    // =========================
    // ANIMATION EVENT
    // =========================
    public void AnimEvent_Hit()
    {
        if (cachedCtx == null || cachedCtx.Player == null)
            return;

        int damage = cachedCtx.Stats.damage;

        if (empoweredHits > 0)
        {
            damage *= 2;
            empoweredHits--;
        }

        DealDamage(cachedCtx, damage);
    }

    // =========================
    // DAMAGE
    // =========================
    void DealDamage(BossContext ctx, int damage)
    {
        float radius = ctx.Stats.attackRange * hitRadiusMultiplier;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            ctx.boss.transform.position,
            radius,
            ctx.TargetLayer
        );

        foreach (var hit in hits)
        {
            hit.GetComponent<IDamageable>()?.TakeDamage(damage);
        }
    }

    // =========================
    // ANIMATION
    // =========================
    void PlayDirectionalAttack(BossContext ctx)
    {
        float deltaY =
            ctx.Player.position.y - ctx.boss.transform.position.y;

        const float verticalThreshold = 0.35f;

        if (deltaY > verticalThreshold)
            ctx.Anim.PlayAttackUp();
        else if (deltaY < -verticalThreshold)
            ctx.Anim.PlayAttackDown();
        else
            ctx.Anim.PlayAttack();
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            1f
        );
    }
#endif
}
