using System.Collections;
using UnityEngine;

public class TeleportExecutionSkill : MonoBehaviour, IBossSkill
{
    public string SkillID => "TeleportExecution";

    [Header("Teleport Conditions")]
    [SerializeField] private float minTeleportDistance = 6f;

    [Header("Skill Damage")]
    [SerializeField] private int skillDamage = 30;
    [SerializeField] private float damageRadius = 2.5f;

    [Header("Cooldown")]
    [SerializeField] private float skillCooldown = 6f;

    private LayerMask targetLayer;
    private Transform bossTransform;
    private float lastCastTime = -999f;
    private bool damageApplied;

    public bool CanExecute(BossContext ctx)
    {
        if (ctx.Player == null)
            return false;

        if (Time.time < lastCastTime + skillCooldown)
            return false;

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

    public IEnumerator Execute(BossContext ctx)
    {
        lastCastTime = Time.time;
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

        // Chờ animation Skill1
        yield return new WaitForSeconds(0.6f);

        // ===== SAU KHI SKILL1 XONG → CƯỜNG HÓA MELEE =====
        var meleeSkill = ctx.boss.GetComponent<BossMeleeSkill>();
        if (meleeSkill != null)
        {
            meleeSkill.EmpowerNextHits(2);
        }
    }
    public void OnAnimationEvent(BossContext ctx)
    {
        if (ctx.Player == null)
            return;
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
