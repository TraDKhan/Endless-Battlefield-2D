using System.Collections;
using UnityEngine;

public class BDashStrikeSkill : BaseBossSkill
{
    public override string SkillID => "DashStrike";

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 14f;
    [SerializeField] private float stopDistance = 0.3f;

    [Header("Hit")]
    [SerializeField] private int damage = 30;
    [SerializeField] private float hitRadius = 1.2f;

    [Header("Timing")]
    [SerializeField] private float attackDelay = 0.1f;   // delay sau khi tới đích
    [SerializeField] private float retreatDelay = 0.15f;

    [Header("Safety")]
    [SerializeField] private float maxDashTime = 1.2f;

    private Vector2 safePosition;
    private bool damageApplied;

    // =========================
    protected override IEnumerator OnExecute(BossContext ctx)
    {
        if (ctx.Player == null)
            yield break;

        ctx.boss.SetCastingSkill(true);
        damageApplied = false;

        // Cache vị trí an toàn
        safePosition = ctx.boss.transform.position;

        // DASH TỚI PLAYER (KHÔNG ANIM)
        yield return DashTo(ctx, ctx.Player.position);

        // TỚI ĐÍCH → PLAY ANIM
        ctx.Movement.Stop();
        ctx.Anim.PlaySkill1();

        yield return new WaitForSeconds(attackDelay);

        // 4️⃣ HIT
        ApplyDamage(ctx);

        yield return new WaitForSeconds(retreatDelay);

        // 5️⃣ DASH BACK
        yield return DashTo(ctx, safePosition);

        ctx.boss.SetCastingSkill(false);
    }

    // =========================
    IEnumerator DashTo(BossContext ctx, Vector2 target)
    {
        float timer = 0f;

        while (Vector2.Distance(ctx.boss.transform.position, target) > stopDistance)
        {
            timer += Time.deltaTime;
            if (timer > maxDashTime)
                break;

            ctx.Movement.MoveTowards(target, dashSpeed);
            yield return null;
        }

        ctx.Movement.Stop();
    }

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
            hit.GetComponent<IDamageable>()?.TakeDamage(damage);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
#endif
}
