using UnityEngine;

public class BoomerangWeapon : Weapon
{
    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayer;

    private float lastFireTime;

    private void Update()
    {
        TryAutoFire();
    }

    // ================= AUTO FIRE =================
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

        Vector3 direction =
            (target.position - transform.position).normalized;

        int count = Mathf.Max(1, stats.ProjectileCount);

        for (int i = 0; i < count; i++)
            SpawnBoomerang(direction);
    }

    public override void Fire()
    {
        // manual trigger nếu cần
    }

    // ================= SPAWN =================
    void SpawnBoomerang(Vector3 direction)
    {
        GameObject obj = Instantiate(
            data.projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        BoomerangProjectile boomerang =
            obj.GetComponent<BoomerangProjectile>();

        boomerang.Init(
            direction,
            stats.Range,
            stats.ProjectileSpeed,
            CreateDamageContext(),
            transform
        );
    }

    // ================= TARGET =================
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
