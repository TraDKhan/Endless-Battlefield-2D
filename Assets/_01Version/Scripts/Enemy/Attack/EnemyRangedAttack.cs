using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour, IEnemyAttack
{
    private EnemyContext ctx;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("Fire Point")]
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;

    public void Init(EnemyContext context)
    {
        ctx = context;
    }
    #region IENEMY ATTACK

    public bool CanAttack => Time.time >= lastAttackTime + ctx.Stats.attackCooldown;
    public float Cooldown => ctx.Stats.attackCooldown;

    public void StartAttack()
    {
        lastAttackTime = Time.time;
        ctx.Anim?.PlayAttack();
    }

    public void UpdateAttack() { }
    public void StopAttack() { }

    #endregion

    #region Animation Event

    // ===== GỌI TỪ ANIMATION ===== \\
    public void Shoot()
    {
        if (ctx.Target == null) return;
        SpawnProjectile(ctx.Target);
    }

    #endregion

    #region SPAWN LOGIC

    private void SpawnProjectile(Transform target)
    {
        ProjectileCore projectile =
            ObjectPoolManager.Instance.Spawn<ProjectileCore>(projectilePrefab);

        if (projectile == null) return;

        projectile.transform.position = firePoint.position;

        IProjectileEffect effect = CreateProjectileEffect();

        switch (ctx.Stats.projectileMode)
        {
            case ProjectileMode.Position:
                SpawnToTargetPosition(projectile, target, effect);
                break;

            default:
                SpawnToDirection(projectile, target, effect);
                break;
        }
    }

    private void SpawnToDirection(
        ProjectileCore projectile,
        Transform target,
        IProjectileEffect effect
    )
    {
        Vector2 dir =
            ((Vector2)target.position - (Vector2)firePoint.position).normalized;

        projectile.InitializeDirection(
            dir,
            ctx.Stats.damage,
            ctx.Stats.projectileSpeed,
            effect
        );
    }
    private void SpawnToTargetPosition(
        ProjectileCore projectile,
        Transform target,
        IProjectileEffect effect
    )
    {
        Vector2 targetPos = target.position; // snapshot
        projectile.InitializeTargetPosition(
            targetPos,
            ctx.Stats.damage,
            ctx.Stats.projectileSpeed,
            effect
        );
    }

    private IProjectileEffect CreateProjectileEffect()
    {
        switch (ctx.Stats.projectileMode)
        {
            case ProjectileMode.Position:
                return new ExplosionDamageEffect(
                    3f,
                    2f,
                    ctx.TargetLayer
                );

            default:
                return new DirectDamageEffect();
        }
    }
    #endregion
}
