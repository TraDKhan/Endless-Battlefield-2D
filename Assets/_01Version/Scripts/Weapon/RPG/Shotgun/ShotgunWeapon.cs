using UnityEngine;

public class ShotgunWeapon : Weapon
{
    //[Header("Targeting")]
    //[SerializeField] private LayerMask enemyLayer;

    [Header("Shotgun")]
    [SerializeField] private float spreadAngle = 30f;
    protected override void OnFireLogic()
    {
        Transform target = FindNearestEnemy();
        if (target == null) return;

        Vector2 baseDirection = (target.position - transform.position).normalized;

        RotateToDirection(baseDirection);

        FirePellets(baseDirection);
    }

    // ===== Shotgun logic 
    void FirePellets(Vector3 baseDirection)
    {
        int pelletCount = Mathf.Max(1, stats.ProjectileCount);

        float stepAngle = pelletCount > 1 ? spreadAngle / (pelletCount - 1) : 0f;

        float startAngle = -spreadAngle * 0.5f;

        for (int i = 0; i < pelletCount; i++)
        {
            float angle = startAngle + stepAngle * i;
            Vector3 dir = Quaternion.Euler(0, 0, angle) * baseDirection;

            SpawnProjectile(dir);
        }
    }

    // ===== Spawmn 
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
}
