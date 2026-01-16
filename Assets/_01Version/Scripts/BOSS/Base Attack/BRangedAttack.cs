using System.Collections;
using UnityEngine;

public class BRangedAttack : BaseBasicAttack
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float attackAnimDuration = 0.8f;

    BossContext cachedCtx;

    public override IEnumerator Attack(BossContext ctx)
    {
        cachedCtx = ctx;

        ctx.Movement?.Stop();
        ctx.Anim?.PlayAttack();

        yield return new WaitForSeconds(attackAnimDuration);
    }

    // =========================
    // ANIMATION EVENT
    // =========================
    public void AnimEvent_Fire()
    {
        Debug.Log(cachedCtx + cachedCtx.Player.ToString());
        if (cachedCtx == null || cachedCtx.Player == null)
            return;

        FireProjectile(cachedCtx);
    }

    // =========================
    // FIRE
    // =========================
    void FireProjectile(BossContext ctx)
    {
        Debug.Log("FIRE");

        BossProjectile projectile =
            ObjectPoolManager.Instance
                .Spawn<BossProjectile>(projectilePrefab);

        projectile.transform.position = firePoint.position;

        projectile.Init(
            CreateProjectileContext(ctx, ctx.Stats.damage),
            (ctx.Player.position - firePoint.position).normalized,
            ProjectileMoveType.Homing,
            ctx.Player
        );
    }

    // =========================
    public struct BossProjectileContext
    {
        public int Damage;
        public float Speed;
        public float CritChance;
        public LayerMask TargetLayer;
    }
    BossProjectileContext CreateProjectileContext(
        BossContext ctx,
        int damage
    )
    {
        return new BossProjectileContext
        {
            Damage = damage,
            Speed = ctx.Stats.projectileSpeed,
            CritChance = 0f,
            TargetLayer = ctx.TargetLayer
        };
    }
}
