using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BoomerangProjectile : MonoBehaviour, IPoolable
{
    // =========================
    // POOL
    // =========================
    public PoolIdentity Identity { get; set; }

    // =========================
    // CONFIG
    // =========================
    [Header("Move")]
    [SerializeField] private float catchDistance = 0.3f;

    // =========================
    // RUNTIME
    // =========================
    private WeaponContext ctx;

    private Transform owner;
    private Vector2 forwardDir;
    private Vector3 startPos;

    private float speed;
    private float maxDistance;
    private bool returning;

    // TRACK HIT THEO ENEMY
    private HashSet<IDamageable> hitOnForward = new();
    private HashSet<IDamageable> hitOnReturn = new();

    // ================= POOL =================
    public void OnSpawn()
    {
        returning = false;
        hitOnForward.Clear();
        hitOnReturn.Clear();
    }

    public void OnDespawn() 
    {
        ctx = default;
        owner = null;
    }

    void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }

    // ================= INIT =================
    public void Init(
        WeaponContext weaponContext,
        Vector2 fireDirection,
        Transform ownerTransform
    )
    {
        ctx = weaponContext;

        owner = ownerTransform;
        forwardDir = fireDirection.normalized;

        speed = ctx.ProjectileSpeed;
        maxDistance = ctx.Range;

        startPos = transform.position;
        returning = false;

        RotateToDirection(forwardDir);
    }

    // ================= UPDATE =================
    private void Update()
    {
        UpdateMovement();
    }

    // ================= MOVE =================
    void UpdateMovement()
    {
        Vector2 moveDir;

        if (!returning)
        {
            moveDir = forwardDir;

            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            {
                returning = true;
            }
        }
        else
        {
            if (owner == null)
            {
                Despawn();
                return;
            }

            moveDir = ((Vector2)owner.position - (Vector2)transform.position).normalized;

            if (Vector2.Distance(transform.position, owner.position) <= catchDistance)
            {
                Despawn();
                return;
            }
        }

        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
        RotateToDirection(moveDir);
    }

    #region HIT

    // ================= HIT =================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsValidTarget(other)) return;
        if (!other.TryGetComponent<IDamageable>(out var target)) return;

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

            Vector2 returnDir = ((Vector2)owner.position - (Vector2)transform.position).normalized;

            ApplyDamage(target, returnDir);
        }
    }
    bool IsValidTarget(Collider2D col)
    {
        return ((1 << col.gameObject.layer) & ctx.TargetLayer) != 0;
    }

    // ================= DAMAGE + KNOCKBACK =================
    void ApplyDamage(IDamageable target, Vector2 hitDir)
    {
        bool isCrit = Random.value < ctx.CritChance;
        float damage = isCrit
            ? ctx.Damage * 1.5f
            : ctx.Damage;

        target.TakeDamage(Mathf.RoundToInt(damage));

        if (target is IKnockbackable kb)
        {
            kb.Knockback(hitDir, ctx.KnockbackForce);
        }
    }
    #endregion

    void RotateToDirection(Vector2 dir)
    {
        if (dir == Vector2.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
