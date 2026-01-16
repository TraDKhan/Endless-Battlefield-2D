using System.Collections;
using UnityEngine;

public class TeleportExecutionSkill : BaseBossSkill, IAnimEventSkill
{
    public override string SkillID => "TeleportExecution";

    [Header("Teleport Conditions")]
    [SerializeField] private float minTeleportDistance = 6f;

    [Header("Skill Damage")]
    [SerializeField] private int skillDamage = 30;
    [SerializeField] private float damageRadius = 2.5f;

    private LayerMask targetLayer;
    private Transform bossTransform;
    private bool damageApplied;
    private bool finished;

    public override bool CanExecute(BossContext ctx)
    {
        if(!base.CanExecute(ctx)) return false;

        if (ctx.Player == null) return false;

        float dist = Vector2.Distance(
            ctx.boss.transform.position,
            ctx.Player.position
        );

        // Trong tầm đánh thường → không teleport
        if (dist <= ctx.Stats.attackRange + ctx.Stats.personalSpace)
            return false;

        // Quá gần
        if (dist < minTeleportDistance)
            return false;

        targetLayer = ctx.TargetLayer;
        bossTransform = ctx.boss.transform;

        return true;
    }

    protected override IEnumerator OnExecute(BossContext ctx)
    {
        damageApplied = false;

        // Dừng di chuyển
        ctx.Movement.Stop();
        ctx.Anim.SetMoving(false);

        // Teleport
        float angle = Random.Range(0f, 360f);
        Vector2 offset = new Vector2(
            Mathf.Cos(angle),
            Mathf.Sin(angle)
        ).normalized;

        ctx.boss.transform.position = (Vector2)ctx.Player.position + offset * ctx.Stats.attackRange;

        yield return new WaitForSeconds(0.15f);

        // ===== PLAY SKILL1 ANIMATION =====
        ctx.Anim.PlaySkill1();

        // Chờ animation kết thúc bằng event
        yield return new WaitUntil(() => finished);
        finished = false;

        // ===== SAU KHI SKILL1 XONG → CƯỜNG HÓA MELEE =====
        var meleeSkill = ctx.boss.GetComponent<BossMeleeSkill>();
        if (meleeSkill != null)
        {
            meleeSkill.EmpowerNextHits(2);
        }
    }
    public void OnAnimationEvent(BossContext ctx, string eventId)
    {
        if (eventId == "ApplyDamage")
            ApplySkillDamage();

        if (eventId == "EndSkill")
            finished = true;
    }

    // ===== GỌI TỪ ANIMATION EVENT =====
    public void ApplySkillDamage()
    {
        if (damageApplied) return;
        damageApplied = true;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            bossTransform.position,
            damageRadius,
            targetLayer
        );

        foreach (var hit in hits)
        {
            var health = hit.GetComponent<IDamageable>();
            if (health != null)
            {
                health.TakeDamage(skillDamage);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
#endif
}
