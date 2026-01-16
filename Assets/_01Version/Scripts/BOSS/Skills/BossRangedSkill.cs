using System.Collections;
using UnityEngine;

public class BossRangedSkill : MonoBehaviour, IBossSkill, IAnimEventSkill
{
    public string SkillID => "Ranged";

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private int empoweredShots;

    public void EmpowerNextShots(int count)
    {
        empoweredShots += count;
    }

    // =========================
    // CAN EXECUTE
    // =========================
    public bool CanExecute(BossContext ctx)
    {
        if (ctx.Player == null) return false;

        float dist = Vector2.Distance(
            ctx.boss.transform.position,
            ctx.Player.position
        );

        return dist <= ctx.Stats.attackRange && dist > ctx.Stats.personalSpace;
    }

    // =========================
    // EXECUTE
    // =========================
    public IEnumerator Execute(BossContext ctx)
    {
        ctx.Movement?.Stop();

        ctx.Anim?.PlayAttack();

        yield return new WaitForSeconds(0.35f);
        //yield break;
    }

    // =========================
    // ANIMATION EVENT
    // =========================
    public void OnAnimationEvent(BossContext ctx, string eventId)
    {
        if (ctx.Player == null) return;

        if (eventId == "Shoot")
            FireProjectile(ctx);
    }

    // =========================
    // FIRE
    // =========================
    private void FireProjectile(BossContext ctx)
    {
        Debug.Log("FIRE");

        int damage = ctx.Stats.damage;

        if (empoweredShots > 0)
        {
            damage *= 2;
            empoweredShots--;
        }

        BossProjectile projectile = ObjectPoolManager.Instance
            .Spawn<BossProjectile>(projectilePrefab);

        projectile.transform.position = firePoint.position;

        projectile.Init(
            CreateProjectileContext(ctx, damage),
            (ctx.Player.position - firePoint.position).normalized,
            ProjectileMoveType.Homing,
            ctx.Player
        );
    }

    // =========================
    // WEAPON CONTEXT
    // =========================
    public struct BossProjectileContext
    {
        public int Damage;
        public float Speed;
        public float CritChance;
        public LayerMask TargetLayer;
    }

    // =========================
    // PROJECTILE CONTEXT
    // =========================
    private BossProjectileContext CreateProjectileContext(
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
