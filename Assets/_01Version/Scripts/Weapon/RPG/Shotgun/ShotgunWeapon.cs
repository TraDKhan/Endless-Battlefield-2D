using UnityEngine;

public class ShotgunWeapon : Weapon
{
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
    void SpawnProjectile(Vector2 direction)
    {
        Bullet bullet = ObjectPoolManager.Instance.Spawn<Bullet>(data.projectilePrefab);

        if (bullet == null) return;

        bullet.transform.position = transform.position;

        bullet.Init(
            CreateWeaponContext(),
            direction,
            ProjectileMoveType.Straight
        );
    }
}
