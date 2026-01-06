using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour, IEnemyAttack
{
    [Header("Stats")]
    [SerializeField] private EnemyStats stats;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("References")]
    [SerializeField] private EnemyAnimationController anim;
    [SerializeField] private Transform firePoint;

    [Header("Target")]
    [SerializeField] private LayerMask playerLayer;

    private float lastAttackTime;

    public bool CanAttack => Time.time >= lastAttackTime + stats.attackCooldown;
    public float Cooldown => stats.attackCooldown;

    public void StartAttack()
    {
        lastAttackTime = Time.time;
        anim.PlayAttack();
        //Shoot();
    }

    public void UpdateAttack() { }
    public void StopAttack()
    {
        anim.StopAttack();
    }

    // gọi trong animation
    public void Shoot()
    {
        SpawnProjectile();
    }

    private void SpawnProjectile()
    {
        Transform target = FindPlayer();
        if (target == null) return;

        ProjectileCore projectile = ObjectPoolManager.Instance.Spawn<ProjectileCore>(projectilePrefab);

        if (projectile == null) return;

        projectile.transform.position = firePoint.position;

        IProjectileEffect effect = CreateProjectileEffect();

        switch (stats.projectileMode)
        {
            case ProjectileMode.Position:
                SpawnToTargetPosition(projectile, target, effect);
                break;

            default:
                SpawnToDirection(projectile, target, effect);
                break;
        }
    }


    private void SpawnToDirection(ProjectileCore projectile, Transform target, IProjectileEffect effect)
    {
        if (target == null) return;

        Vector2 direction = (target.position - firePoint.position).normalized;

        projectile.InitializeDirection(
            direction,
            stats.damage,
            stats.projectileSpeed,
            effect
        );
    }
    private void SpawnToTargetPosition(ProjectileCore projectile, Transform target, IProjectileEffect effect)
    {
        if (target == null) return;

        Vector2 targetPosition = target.position; // SNAPSHOT 🔥

        projectile.InitializeTargetPosition(
            targetPosition,
            stats.damage,
            stats.projectileSpeed,
            effect
        );
    }

    private IProjectileEffect CreateProjectileEffect()
    {
        switch (stats.projectileMode)
        {
            case ProjectileMode.Position:
                return new ExplosionDamageEffect(
                    0.3f,
                    2f,
                    playerLayer
                );

            default:
                return new DirectDamageEffect();
        }
    }
    protected Transform FindPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(
            transform.position,
            stats.attackRange,
            playerLayer
        );

        return player != null ? player.transform : null;
    }
}
