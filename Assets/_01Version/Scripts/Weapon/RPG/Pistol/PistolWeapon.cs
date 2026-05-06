using System.Collections;
using UnityEngine;

public class PistolWeapon : WeaponBase
{
    protected override void OnFireLogic()
    {
        Transform target = FindNearestEnemy();
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        RotateToDirection(direction);
        //AimWeapon(direction);
        StartCoroutine(SpawnProjectile(direction, target));
    }

    IEnumerator SpawnProjectile(Vector2 direction, Transform target)
    {
        int pelletCount = stats.ProjectileCount;
        for (int i = 0; i < pelletCount; i++)
        {
            Bullet bullet = ObjectPoolManager.Instance.Spawn<Bullet>(controller.Data.projectilePrefab);
            AudioManager.Instance?.PlayShoot();

            if (bullet == null)
            {
                Debug.Log("Bullet NULL");
                yield break;
            }

            bullet.transform.position = transform.position;

            bullet.Init(
                CreateWeaponContext(),
                direction,
                ProjectileMoveType.Homing,
                target
            );

            yield return new WaitForSeconds(0.2f);
        }
    }

}
