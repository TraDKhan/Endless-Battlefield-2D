using UnityEngine;

public class ShotgunWeapon : Weapon
{
    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Shotgun")]
    [SerializeField] private float spreadAngle = 30f;

    private float lastFireTime;

    private void Update()
    {
        TryAutoFire();
    }

    // =========================
    // AUTO FIRE
    // =========================
    void TryAutoFire()
    {
        if (!CanFire()) return;

        Transform target = FindNearestEnemy();
        if (target == null) return;

        FireAt(target);
    }

    bool CanFire()
    {
        return Time.time >= lastFireTime + stats.Cooldown;
    }

    void FireAt(Transform target)
    {
        lastFireTime = Time.time;

        Vector3 baseDirection =
            (target.position - transform.position).normalized;

        FirePellets(baseDirection);
    }

    public override void Fire()
    {
        // Dành cho manual trigger nếu cần
    }

    // =========================
    // SHOTGUN LOGIC
    // =========================
    void FirePellets(Vector3 baseDirection)
    {
        int pelletCount = Mathf.Max(1, stats.ProjectileCount);

        float stepAngle =
            pelletCount > 1 ? spreadAngle / (pelletCount - 1) : 0f;

        float startAngle = -spreadAngle * 0.5f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = startAngle + stepAngle * i;
            Vector3 dir =
                Quaternion.Euler(0, 0, angle) * baseDirection;

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
        rb.linearVelocity = direction * stats.ProjectileSpeed;

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(CreateDamageContext());
    }

    // =========================
    // TARGETING
    // =========================
    Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            stats.Range,
            enemyLayer
        );

        float minDist = float.MaxValue;
        Transform nearest = null;

        foreach (var e in enemies)
        {
            float dist = Vector2.Distance(
                transform.position,
                e.transform.position
            );

            if (dist < minDist)
            {
                minDist = dist;
                nearest = e.transform;
            }
        }

        return nearest;
    }
}
