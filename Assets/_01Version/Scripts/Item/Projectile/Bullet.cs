using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour, IPoolable
{
    // ===== Pool =====
    public PoolIdentity Identity { get; set; }

    // ===== Config =====
    [Header("Life")]
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int maxPenetration = 1;

    [Header("Homing")]
    [SerializeField] private float turnSpeed = 720f; // độ/giây

    // ===== Runtime =====
    private WeaponContext ctx;
    private BulletMoveType moveType;
    private Transform homingTarget;

    private Vector2 moveDir;
    private int penetrationLeft;
    private float lifeTimer;
    private float moveSpeed;

    #region POOL CALLBACKS

    public void OnSpawn()
    {
        lifeTimer = 0f;
        penetrationLeft = maxPenetration;
    }

    public void OnDespawn()
    {
        ctx = default;
        homingTarget = null;
    }
    void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
    #endregion

    public void Init(
        WeaponContext weaponContext,
        Vector2 direction,
        BulletMoveType type,
        Transform target = null
    )
    {
        this.ctx = weaponContext;
        this.moveDir = direction.normalized;
        this.moveSpeed = ctx.ProjectileSpeed;
        this.moveType = type;

        homingTarget = (type == BulletMoveType.Homing) ? target : null;

        RotateToDirection(moveDir);
    }

    #region UPDATE MOVE

    private void Update()
    {
        UpdateMovement();
        UpdateLifeTime();
    }
    void UpdateMovement()
    {
        if (moveType == BulletMoveType.Homing && homingTarget != null)
        {
            UpdateHomingDirection();
        }

        transform.position +=
            (Vector3)(moveDir * moveSpeed * Time.deltaTime);
    }
    void UpdateHomingDirection()
    {
        Vector2 targetDir =
            ((Vector2)homingTarget.position - (Vector2)transform.position).normalized;

        moveDir = Vector2.Lerp(
            moveDir,
            targetDir,
            turnSpeed * Mathf.Deg2Rad * Time.deltaTime
        ).normalized;

        RotateToDirection(moveDir);
    }
    void UpdateLifeTime()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Despawn();
        }
    }
    #endregion

    #region HIT LOGIC

    // ===== TRIGGER ===== \\
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!IsValidTarget(other)) return;

        if (!other.TryGetComponent<IDamageable>(out var target))
            return;

        ApplyDamage(target);

        penetrationLeft--;
        if (penetrationLeft <= 0)
            Despawn();
    }

    // ===== CHECK TARGET ===== \\
    bool IsValidTarget(Collider2D col)
    {
        return ((1 << col.gameObject.layer) & ctx.TargetLayer) != 0;
    }

    // ===== DAMAGE ===== \\
    void ApplyDamage(IDamageable target)
    {
        bool isCrit = Random.value < ctx.CritChance;
        float finalDamage = isCrit
            ? ctx.Damage * 1.5f
            : ctx.Damage;

        target.TakeDamage((int)finalDamage);
    }
    #endregion
    // ===== UTIL ===== \\

    void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
