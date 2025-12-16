using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BoomerangProjectile : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 direction;
    private float maxDistance;
    private float speed;
    private Transform owner;

    private WeaponDamageContext damageContext;

    private bool returning;
    private HashSet<Collider2D> hitTargets = new HashSet<Collider2D>();

    // ================= INIT =================
    public void Init(
        Vector3 dir,
        float range,
        float moveSpeed,
        WeaponDamageContext ctx,
        Transform ownerTransform
    )
    {
        direction = dir;
        maxDistance = range;
        speed = moveSpeed;
        damageContext = ctx;
        owner = ownerTransform;

        startPos = transform.position;
        returning = false;
        hitTargets.Clear();
    }

    private void Update()
    {
        Move();
    }

    // ================= MOVE =================
    void Move()
    {
        Vector3 moveDir;

        if (!returning)
        {
            moveDir = direction;

            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
                returning = true;
        }
        else
        {
            if (owner == null)
            {
                Destroy(gameObject);
                return;
            }

            moveDir = (owner.position - transform.position).normalized;

            if (Vector3.Distance(transform.position, owner.position) < 0.3f)
            {
                Destroy(gameObject);
                return;
            }
        }

        transform.position += moveDir * speed * Time.deltaTime;
    }

    // ================= HIT =================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitTargets.Contains(other)) return;

        if (!other.TryGetComponent<IDamageable>(out var target))
            return;

        hitTargets.Add(other);

        bool isCrit = Random.value < damageContext.critChance;
        float finalDamage = isCrit
            ? damageContext.damage * damageContext.critMultiplier
            : damageContext.damage;

        target.TakeDamage((int)finalDamage);
    }
}
