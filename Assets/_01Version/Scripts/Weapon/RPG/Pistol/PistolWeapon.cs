using UnityEngine;

public class PistolWeapon : Weapon
{
    protected override void OnFireLogic()
    {
        Transform target = FindNearestEnemy();
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        RotateToDirection(direction);
        //AimWeapon(direction);
        SpawnProjectile(direction, target);
    }

    void SpawnProjectile(Vector2 direction, Transform target)
    {
        Bullet bullet = ObjectPoolManager.Instance.Spawn<Bullet>(controller.Data.weaponPrefab);

        if (bullet == null) return;

        bullet.transform.position = transform.position;

        bullet.Init(
            CreateWeaponContext(),
            direction,
            ProjectileMoveType.Homing,
            target
        );
    }
}
