using UnityEngine;

public class PistolWeapon : Weapon
{
    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayer;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        TryAutoShoot();
    }

    void TryAutoShoot()
    {
        if (!CanShoot()) return;

        Transform target = FindNearestEnemy();

        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        Shoot(direction);
    }

    public override void Shoot(Vector3 direction)
    {
        base.Shoot(direction);
        SpawnProjectile(direction);
    }

    void SpawnProjectile(Vector3 direction)
    {
        GameObject bulletObj = Instantiate(
            data.projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        Rigidbody2D rb = bulletObj.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * currentStats.projectileSpeed;

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(CreateDamageContext());
    }


    Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            currentStats.range,
            enemyLayer
        );

        float minDistance = Mathf.Infinity;
        Transform nearest = null;

        foreach (Collider2D enemy in enemies)
        {
            float dist = Vector2.Distance(
                transform.position,
                enemy.transform.position
            );

            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    //// Debug vẽ range
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, currentStats.range);
    //}
}
