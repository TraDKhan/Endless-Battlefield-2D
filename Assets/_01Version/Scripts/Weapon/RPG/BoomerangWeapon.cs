using UnityEngine;

public class BoomerangWeapon : Weapon
{
    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayer;

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

        for (int i = 0; i < count; i++)
        {
            SpawnBoomerang(direction);
        }
    }

    void SpawnBoomerang(Vector3 direction)
    {
        GameObject obj = Instantiate(
            data.projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        BoomerangProjectile boomerang = obj.GetComponent<BoomerangProjectile>();
        boomerang.Init(
            direction,
            currentStats.range,
            currentStats.projectileSpeed,
            CreateDamageContext(),
            transform
        );
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, currentStats.range);
    }
}
