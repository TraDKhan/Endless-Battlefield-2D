using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BRangedAttack;

public class BCircularBarrageSkill : BaseBossSkill
{
    public override string SkillID => "CircularBarrage";

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
    public override bool CanExecute(BossContext ctx)
    {
        if (!base.CanExecute(ctx))
            return false;

        return true;
    }

    protected override IEnumerator OnExecute(BossContext ctx)
    {
        ctx.boss.SetCastingSkill(true);

        ctx.Anim?.PlaySkill1();

        for (int burst = 0; burst < burstCount; burst++)
        {
            var ring = SpawnIdleRing(ctx);

            yield return new WaitForSeconds(chargeTime);

            foreach (var proj in ring)
                proj.Fire();

            if (burst < burstCount - 1)
                yield return new WaitForSeconds(intervalBetweenBursts);
        }

        ctx.boss.SetCastingSkill(false);
    }

    // =========================
    List<BossProjectile> SpawnIdleRing(BossContext ctx)
    {
        List<BossProjectile> result = new();

        float angleStep = 360f / projectilePerRing;

        for (int i = 0; i < projectilePerRing; i++)
        {
            float angle = angleStep * i * Mathf.Deg2Rad;
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