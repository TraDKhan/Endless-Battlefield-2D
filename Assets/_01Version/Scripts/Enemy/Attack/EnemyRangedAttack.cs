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
    private void Shoot()
    {
        Transform target = FindPlayer();
        if (target == null) return;

        Vector2 direction = (target.position - firePoint.position).normalized;
        SpawnProjectile(direction);
    }

    void SpawnProjectile(Vector2 direction)
    {
        ProjectileCore projectile = ObjectPoolManager.Instance.Spawn<ProjectileCore>(projectilePrefab);

        if (projectile == null) return;

        projectile.transform.position = firePoint.position;

        IProjectileEffect effect = CreateProjectileEffect();
        projectile.Initialize(
            direction,
            stats.damage,
            stats.projectileSpeed,
            effect
        );
    }
    private IProjectileEffect CreateProjectileEffect()
    {
        switch (stats.projectileType)
        {
            case ProjectileType.Bomb:
                return new ExplosionDamageEffect(0.3f, 2.5f, playerLayer);

            case ProjectileType.Poison:
                return new PoisonProjectileEffect(4f, 2);

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
    private void SpawnBomb()
    {
        Transform target = FindPlayer();
        if (target == null) return;

        Vector2 targetPosition = target.position; // SNAPSHOT 🔥

        ProjectileCore bomb = ObjectPoolManager.Instance.Spawn<ProjectileCore>(projectilePrefab);

        bomb.transform.position = firePoint.position;

        IProjectileEffect effect =
            new ExplosionDamageEffect(
                delay: 1.2f,
                radius: 2.5f,
                layer: playerLayer
            );

        bomb.InitializeTargetPosition(
            targetPosition,
            stats.damage,
            stats.projectileSpeed,
            effect
        );
    }
}
