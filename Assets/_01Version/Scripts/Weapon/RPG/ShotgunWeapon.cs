using UnityEngine;

public class ShotgunWeapon : Weapon
{
    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Shotgun")]
    [SerializeField] private float spreadAngle = 30f;

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

        int count = Mathf.Max(1, currentStats.projectileCount);

        float step = count > 1 ? spreadAngle / (count - 1) : 0;
        float startAngle = -spreadAngle / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = startAngle + step * i;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * direction;

            SpawnProjectile(dir);
        }
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

        float minDist = float.MaxValue;
        Transform nearest = null;

        foreach (Collider2D e in enemies)
        {
            float dist = Vector2.Distance(transform.position, e.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, currentStats.range);
    //}
}
