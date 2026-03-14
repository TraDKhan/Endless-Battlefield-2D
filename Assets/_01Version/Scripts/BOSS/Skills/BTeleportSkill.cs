using System.Collections;
using UnityEngine;

public class BTeleportSkill : BaseBossSkill
{
    [Header("Teleport Conditions")]
    [SerializeField] private float minTeleportDistance = 6f;

    [Header("Skill Damage")]
    [SerializeField] private int skillDamage = 30;
    [SerializeField] private float damageRadius = 2.5f;

    [Header("Animation")]
    [SerializeField] float skillAnimMaxTime = 1.2f;

    [Header("Ranged Behaviour")]
    [SerializeField] bool isRangedBoss = false;
    [SerializeField] float repositionDelay = 0.4f;

    BossContext cachedCtx;

    private bool damageApplied;
    private bool skillFinished;

    protected override bool CheckCondition(BossContext ctx)
    {
        if (ctx.Player == null)
            return false;

        float dist = Vector2.Distance(
            ctx.boss.transform.position,
            ctx.Player.position
        );

        if (dist <= ctx.Stats.attackRange + ctx.Stats.personalSpace)
            return false;

        if (dist < minTeleportDistance)
            return false;

        return true;
    }

    protected override IEnumerator PerformSkill(BossContext ctx)
    {
        cachedCtx = ctx;
        damageApplied = false;
        skillFinished = false;

        ctx.boss.SetCastingSkill(true);

        // TELEPORT
        Vector2 dir = (ctx.Player.position - ctx.boss.transform.position).normalized;

        ctx.boss.transform.position = (Vector2)ctx.Player.position - dir * damageRadius;

        yield return new WaitForSeconds(0.15f);

        // PLAY ANIMATION
        ctx.Anim?.Play_BTeleportSkill();

        // WAIT ANIMATION
        float timer = 0f;

        while (!skillFinished && timer < skillAnimMaxTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // =========================
        // RANGED BOSS BEHAVIOUR
        // =========================
        if (isRangedBoss)
        {
            yield return new WaitForSeconds(repositionDelay);

            ctx.boss.SetCastingSkill(false);

            Vector2 target = GetRangedPosition(ctx);

            yield return MoveTo(ctx, target);
        }
        else
        {
            // MELEE BOSS BEHAVIOUR
            var melee = ctx.boss.GetComponent<BMeeleAttack>();

            if (melee != null)
            {
                melee.EmpowerNextHits(2);
            }
        }

        yield return new WaitForSeconds(2f);
        ctx.boss.SetCastingSkill(false);
    }

    // =========================
    Vector2 GetRangedPosition(BossContext ctx)
    {
        float desiredDist = ctx.Stats.attackRange * 0.5f;

        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector2 offset = new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        ) * desiredDist;

        return (Vector2)ctx.Player.position + offset;
    }

    // =========================
    IEnumerator MoveTo(BossContext ctx, Vector2 target)
    {
        float timer = 0f;
        float maxTime = 1.2f;

        while (Vector2.Distance(ctx.boss.transform.position, target) > 0.2f)
        {
            timer += Time.deltaTime;

            if (timer > maxTime)
                break;

            ctx.Movement?.MoveTowards(target, ctx.Stats.moveSpeed);

            yield return null;
        }

        ctx.Movement?.Stop();
    }

    public override void OnAnimationEvent(BossAnimEvent animEvent)
    {
        switch (animEvent)
        {
            case BossAnimEvent.Hit:
                ApplyTeleportDamage();
                break;

            case BossAnimEvent.End:
                skillFinished = true;
                break;
        }
    }

    void ApplyTeleportDamage()
    {
        if (damageApplied || cachedCtx == null)
            return;

        damageApplied = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            cachedCtx.boss.transform.position,
            damageRadius,
            cachedCtx.TargetLayer
        );

        foreach (var hit in hits)
        {
            hit.GetComponent<IDamageable>()
                ?.TakeDamage(skillDamage);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
#endif
}

