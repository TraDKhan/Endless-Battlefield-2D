using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BoomerangProjectile : MonoBehaviour, IPoolable
{
    public PoolIdentity Identity { get; set; }

    [Header("Move")]
    private Vector3 startPos;
    private Vector3 forwardDir;
    private float maxDistance;
    private float speed;
    private Transform owner;

    [Header("Damage")]
    private WeaponDamageContext damageContext;

    private bool returning;

    // ⭐ TRACK HIT THEO ENEMY
    private HashSet<IDamageable> hitOnForward = new();
    private HashSet<IDamageable> hitOnReturn = new();

    // ================= POOL =================
    public void OnSpawn()
    {
        returning = false;
        hitOnForward.Clear();
        hitOnReturn.Clear();
    }

    public void OnDespawn() { }

    // ================= INIT =================
    public void Init(
        Vector3 dir,
        float range,
        float moveSpeed,
        WeaponDamageContext ctx,
        Transform ownerTransform
    )
    {
        forwardDir = dir.normalized;
        maxDistance = range;
        speed = moveSpeed;
        damageContext = ctx;
        owner = ownerTransform;

        startPos = transform.position;
        returning = false;
    }

    // ================= UPDATE =================
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
            moveDir = forwardDir;

            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
                returning = true;
        }
        else
        {
            if (owner == null)
            {
                Despawn();
                return;
            }

            moveDir = (owner.position - transform.position).normalized;

            if (Vector3.Distance(transform.position, owner.position) < 0.3f)
            {
                Despawn();
                return;
            }
        }

        transform.position += moveDir * speed * Time.deltaTime;
    }

    // ================= HIT =================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var target))
            return;

        // ===== FORWARD =====
        if (!returning)
        {
            if (hitOnForward.Contains(target)) return;

            hitOnForward.Add(target);
            ApplyDamage(target, forwardDir);
        }
        // ===== RETURN =====
        else
        {
            if (hitOnReturn.Contains(target)) return;

            hitOnReturn.Add(target);

            Vector2 returnDir = (owner.position - transform.position).normalized;

            ApplyDamage(target, returnDir);
        }
    }

    // ================= DAMAGE + KNOCKBACK =================
    void ApplyDamage(IDamageable target, Vector2 hitDir)
    {
        bool isCrit = Random.value < damageContext.critChance;
        float finalDamage = isCrit
            ? damageContext.damage * damageContext.critMultiplier
            : damageContext.damage;

        target.TakeDamage((int)finalDamage);

        //// ⭐ KNOCKBACK
        if (target is IKnockbackable kb)
        {
            kb.Knockback(hitDir, damageContext.knockbackForce);
        }
    }

    // ================= DESPAWN =================
    void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
}
