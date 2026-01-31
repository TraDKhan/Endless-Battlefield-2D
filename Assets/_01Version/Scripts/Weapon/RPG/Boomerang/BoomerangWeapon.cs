using UnityEngine;

public class BoomerangWeapon : Weapon
{
    protected override void OnFireLogic()
    {
        Transform target = FindNearestEnemy();
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        RotateToDirection(direction);

        WeaponContext ctx = CreateWeaponContext();

        int count = Mathf.Max(1, ctx.ProjectileCount);

        for (int i = 0; i < count; i++)
        {
            SpawnBoomerang(ctx, direction);
        }
    }

    // ===== SPAWN
    void SpawnBoomerang(WeaponContext ctx, Vector3 direction)
    {
        BoomerangProjectile boomerang =
            ObjectPoolManager.Instance
                .Spawn<BoomerangProjectile>(controller.Data.weaponPrefab);

        if (boomerang == null) return;

        boomerang.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        boomerang.Init(
             ctx,
             direction,
             transform   // owner
         );
    }
}
