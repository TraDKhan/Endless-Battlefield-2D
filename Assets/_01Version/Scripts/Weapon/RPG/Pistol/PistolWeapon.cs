using UnityEngine;

public class PistolWeapon : Weapon
{
    //[Header("Targeting")]
    //[SerializeField] private LayerMask enemyLayer;
    protected override void OnFireLogic()
    {
        Transform target = FindNearestEnemy();
        if (target == null) return;

        Vector2 direction = (target.position - transform.position).normalized;

        //RotateToDirection(direction);
        AimWeapon(direction);
        SpawnProjectile(direction);
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
}
