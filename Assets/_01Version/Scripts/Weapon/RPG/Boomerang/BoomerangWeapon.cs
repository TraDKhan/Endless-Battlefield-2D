using UnityEngine;

public class BoomerangWeapon : Weapon
{
    protected override void OnFireLogic()
    {
        Transform target = FindNearestEnemy();
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        RotateToDirection(direction);

        int count = Mathf.Max(1, stats.ProjectileCount);

        for (int i = 0; i < count; i++)
        {
            SpawnBoomerang(direction);
        }
    }

    // ===== SPAWN
    void SpawnBoomerang(Vector3 direction)
    {
        BoomerangProjectile boomerang =
            ObjectPoolManager.Instance
                .Spawn<BoomerangProjectile>(data.projectilePrefab);

        if (boomerang == null) return;

        boomerang.transform.position = transform.position;
        boomerang.transform.rotation = Quaternion.identity;

        boomerang.Init(
            direction,
            stats.Range,
            stats.ProjectileSpeed,
            CreateDamageContext(),
            transform
        );
    }
}
