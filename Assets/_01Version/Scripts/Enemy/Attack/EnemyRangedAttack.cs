using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private EnemyAnimationController anim;
    [SerializeField] private Transform firePoint;
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

    private void Shoot()
    {
        Transform target = FindPlayer();
        if (target == null) return;

        Vector2 direction = (target.position - firePoint.position).normalized;
        SpawnProjectile(direction);
    }

    void SpawnProjectile(Vector2 direction)
    {
        Projectile bullet = ObjectPoolManager.Instance.Spawn<Projectile>(projectilePrefab);
        if (bullet == null) return;

        bullet.transform.position = firePoint.position;
        bullet.Init(
            direction,
            stats.damage,
            3
        );
    }

    protected Transform FindPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(
            transform.position,
            stats.preferredRange,
            playerLayer
        );

        return player != null ? player.transform : null;
    }

    public void UpdateAttack() { }
    public void StopAttack() 
    { 
        anim.StopAttack();
    }
    private void OnDrawGizmos()
    {
        if (stats == null) return;
        //Detect Range 
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stats.detectRange);
        //Attack Range 
        Gizmos.color = Color.red; Gizmos.DrawWireSphere(transform.position, stats.attackRange);
        //Nếu là melee enemy
        if (stats.enemyType == EnemyAttackType.Ranged)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, stats.preferredRange);
        }
    }
}
