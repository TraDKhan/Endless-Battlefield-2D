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
        ProjectileCore projectile = ObjectPoolManager.Instance.Spawn<ProjectileCore>(projectilePrefab);

        if (projectile == null) return;

        projectile.transform.position = firePoint.position;

        Vector2 dir = (ctx.Target.position - firePoint.position).normalized;

        projectile.Init(dir, ctx.Target.position, ctx.Stats.damage, ctx.Stats.projectileSpeed);
    }
    #endregion
}
