using UnityEngine;

public class BoomerangWeapon : Weapon
{
    //[Header("Targeting")]
    //[SerializeField] private LayerMask enemyLayer;

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
        GameObject obj = Instantiate(
            data.projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        BoomerangProjectile boomerang = obj.GetComponent<BoomerangProjectile>();

        boomerang.Init(
            direction,
            stats.Range,
            stats.ProjectileSpeed,
            CreateDamageContext(),
            transform
        );
    }
}
