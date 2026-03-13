using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BRangedAttack;

public class BCircularBarrageSkill : BaseBossSkill
{
    [Header("Burst Settings")]
    [SerializeField] int burstCount = 3;
    [SerializeField] int projectilePerRing = 16;
    [SerializeField] float ringRadius = 1.2f;

    [Header("Timing")]
    [SerializeField] float chargeTime = 0.4f;
    [SerializeField] float intervalBetweenBursts = 0.6f;

    [Header("Projectile")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float projectileSpeed = 6f;

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

        // Skill này nên dùng khi player không quá gần
        if (dist < ctx.Stats.attackRange + 1.5f)
            return false;

        return true;
    }

    // =========================
    // EXECUTE
    // =========================

    protected override IEnumerator PerformSkill(BossContext ctx)
    {
        ctx.boss.SetCastingSkill(true);

        ctx.Movement?.Stop();
        ctx.Anim?.PlaySkill1();

        for (int burst = 0; burst < burstCount; burst++)
        {
            List<BossProjectile> ring = SpawnIdleRing(ctx);

            // charge trước khi bắn
            yield return new WaitForSeconds(chargeTime);

            foreach (var proj in ring)
            {
                if (proj != null)
                    proj.Fire();
            }

            if (burst < burstCount - 1)
                yield return new WaitForSeconds(intervalBetweenBursts);
        }

        ctx.boss.SetCastingSkill(false);
    }

    // =========================
    // SPAWN PROJECTILE RING
    // =========================

    List<BossProjectile> SpawnIdleRing(BossContext ctx)
    {
        List<BossProjectile> result = new();

        float angleOffsetDeg = Random.Range(15f, 30f);
        float angleStep = 360f / projectilePerRing;

        for (int i = 0; i < projectilePerRing; i++)
        {
            float angle = (angleStep * i + angleOffsetDeg) * Mathf.Deg2Rad;

            Vector2 dir = new Vector2(
                Mathf.Cos(angle),
                Mathf.Sin(angle)
            );

            Vector2 spawnPos =
                (Vector2)ctx.boss.transform.position +
                dir * ringRadius;

            BossProjectile proj =
                ObjectPoolManager.Instance
                    .Spawn<BossProjectile>(projectilePrefab);

            if (proj == null)
                continue;

            proj.transform.position = spawnPos;

            proj.InitIdle(
                CreateProjectileContext(ctx),
                dir,
                ProjectileMoveType.Straight
            );

            result.Add(proj);
        }

        return result;
    }

    // =========================
    // PROJECTILE CONTEXT
    // =========================

    BossProjectileContext CreateProjectileContext(BossContext ctx)
    {
        return new BossProjectileContext
        {
            Damage = ctx.Stats.damage,
            Speed = projectileSpeed,
            CritChance = 0f,
            TargetLayer = ctx.TargetLayer
        };
    }
}