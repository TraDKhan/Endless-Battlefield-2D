using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private EnemyStats stats;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private float lastAttackTime;

    public bool CanAttack =>
        Time.time >= lastAttackTime + stats.attackCooldown;

    public float Cooldown => stats.attackCooldown;

    public void StartAttack()
    {
        lastAttackTime = Time.time;
        Shoot();
    }

    private void Shoot()
    {
        var projectile = Instantiate(
            projectilePrefab,
            firePoint.position,
            Quaternion.identity
        );

        //projectile.GetComponent<Projectile>().Init(stats.damage, transform.right);
    }

    public void UpdateAttack() { }
    public void StopAttack() { }
}
