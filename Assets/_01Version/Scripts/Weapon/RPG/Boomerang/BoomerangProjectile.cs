using System.Collections.Generic;
using UnityEngine;
using static Weapon;

public class BoomerangProjectile : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 direction;
    private float maxDistance;
    private float speed;
    private Transform owner;

    private DamageContext damageContext;

    private bool returning;

    // tránh hit trùng enemy quá nhanh
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    public void Init(
        Vector3 dir,
        float range,
        float moveSpeed,
        DamageContext ctx,
        Transform ownerTransform
    )
    {
        direction = dir;
        maxDistance = range;
        speed = moveSpeed;
        damageContext = ctx;
        owner = ownerTransform;

        startPos = transform.position;
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 targetDir;

        if (!returning)
        {
            targetDir = direction;

            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            {
                returning = true;
            }
        }
        else
        {
            if (owner == null)
            {
                Destroy(gameObject);
                return;
            }

            targetDir = (owner.position - transform.position).normalized;

            if (Vector3.Distance(transform.position, owner.position) < 0.3f)
            {
                Destroy(gameObject);
            }
        }

        transform.position += targetDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy")) return;

        if (hitEnemies.Contains(other)) return;

        hitEnemies.Add(other);

        float finalDamage = damageContext.damage;

        if (Random.value < damageContext.critChance)
        {
            finalDamage *= damageContext.critMultiplier;
        }

        other.GetComponent<EnemyHealthController>()?.TakeDamage((int)finalDamage);
    }
}
