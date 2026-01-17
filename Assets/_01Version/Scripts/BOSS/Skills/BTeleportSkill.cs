using System.Collections;
using UnityEngine;

public class BTeleportSkill : BaseBossSkill
    {
    public override string SkillID => "Teleport";

    [Header("Teleport Conditions")]
    [SerializeField] private float minTeleportDistance = 6f;

    [Header("Skill Damage")]
    [SerializeField] private int skillDamage = 30;
    [SerializeField] private float damageRadius = 2.5f;

    [Header("Animation")]
    [SerializeField] float skillAnimMaxTime = 1.2f;

    BossContext cachedCtx;
    private bool damageApplied;
    private bool skillFinished;

    public override bool CanExecute(BossContext ctx)
    {
        if (!base.CanExecute(ctx))
            return false;

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

    protected override IEnumerator OnExecute(BossContext ctx)
    {
        cachedCtx = ctx;
        damageApplied = false;
        skillFinished = false;

        // STOP
        ctx.Movement?.Stop();
        ctx.Anim?.SetMoving(false);

        // TELEPORT
        Vector2 dir = (ctx.Player.position - ctx.boss.transform.position).normalized;
        //ctx.boss.transform.position = (Vector2)ctx.Player.position - dir * ctx.Stats.attackRange;
        ctx.boss.transform.position = (Vector2)ctx.Player.position - dir * damageRadius;
        yield return new WaitForSeconds(0.15f);

        // PLAY ANIM
        ctx.Anim?.Play_BTeleportSkill();

        // WAIT EVENT OR TIMEOUT
        float timer = 0f;
        while (!skillFinished && timer < skillAnimMaxTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // EMPOWER MELEE BASIC
        var melee = ctx.boss.GetComponent<BMeeleAttack>();
        if (melee != null)
        {
            melee.EmpowerNextHits(2);
        }
    }

    // =========================
    // ANIMATION EVENT – HIT
    // =========================
    public void AnimEvent_ApplyTeleportSkillDamage()
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

    // =========================
    // ANIMATION EVENT – END
    // =========================
    public void AnimEvent_FinishSkill()
    {
        skillFinished = true;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
#endif
}

