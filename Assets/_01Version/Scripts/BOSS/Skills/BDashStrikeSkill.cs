using System.Collections;
using UnityEngine;

public class BDashStrikeSkill : BaseBossSkill
{
    [Header("Dash")]
    [SerializeField] float dashSpeed = 14f;
    [SerializeField] float stopDistance = 0.3f;

    [Header("Hit")]
    [SerializeField] int damage = 30;
    [SerializeField] float hitRadius = 1.2f;

    [Header("Timing")]
    [SerializeField] float attackDelay = 0.1f;
    [SerializeField] float retreatDelay = 0.15f;

    [Header("Safety")]
    [SerializeField] float maxDashTime = 1.2f;

    Vector2 safePosition;
    bool damageApplied;

    // =========================
    // CONDITION
    // =========================

    protected override bool CheckCondition(BossContext ctx)
    {
        if (ctx.Player == null)
            return false;

        float dist = Vector2.Distance(
            ctx.boss.transform.position,
            ctx.Player.position
        );

        // Chỉ dash khi player hơi xa
        if (dist < ctx.Stats.attackRange + 0.5f)
            return false;

        return true;
    }

    // =========================
    // EXECUTE
    // =========================

    protected override IEnumerator PerformSkill(BossContext ctx)
    {
        if (ctx.Player == null)
            yield break;

        ctx.boss.SetCastingSkill(true);
        damageApplied = false;

        // Lưu vị trí ban đầu
        safePosition = ctx.boss.transform.position / 2;

        // DASH TỚI PLAYER
        yield return DashTo(ctx, ctx.Player.position);

        // TỚI ĐÍCH → PLAY ANIM
        ctx.Movement?.Stop();
        ctx.Anim?.PlaySkill1();

        yield return new WaitForSeconds(attackDelay);

        // APPLY DAMAGE
        ApplyDamage(ctx);

        yield return new WaitForSeconds(retreatDelay);

        // DASH BACK
        yield return DashTo(ctx, safePosition);

        ctx.boss.SetCastingSkill(false);
    }

    // =========================
    // DASH LOGIC
    // =========================

    IEnumerator DashTo(BossContext ctx, Vector2 target)
    {
        float timer = 0f;

        while (Vector2.Distance(ctx.boss.transform.position, target) > stopDistance)
        {
            timer += Time.deltaTime;

            if (timer > maxDashTime)
                break;

            ctx.Movement?.MoveTowards(target, dashSpeed);

            yield return null;
        }

        ctx.Movement?.Stop();
        //ctx.boss.transform.position = target;
    }

    // =========================
    // DAMAGE
    // =========================

    void ApplyDamage(BossContext ctx)
    {
        if (damageApplied)
            return;

        damageApplied = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            ctx.boss.transform.position,
            hitRadius,
            ctx.TargetLayer
        );

        foreach (var hit in hits)
        {
            hit.GetComponent<IDamageable>()
                ?.TakeDamage(damage);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
#endif
}
