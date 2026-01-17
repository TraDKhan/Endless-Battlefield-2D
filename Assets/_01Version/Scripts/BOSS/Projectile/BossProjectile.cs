using UnityEngine;
using static BRangedAttack;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(PoolIdentity))]
public class BossProjectile : MonoBehaviour, IPoolable
{
    // =========================
    // POOL
    // =========================
    public PoolIdentity Identity { get; set; }

    // =========================
    // CONFIG
    // =========================
    [Header("Life")]
    [SerializeField] private float lifeTime = 4f;

    [Header("Homing")]
    [SerializeField] private float turnSpeed = 720f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private static readonly int HitTrigger = Animator.StringToHash("Hit");

    // =========================
    // STATE
    // =========================
    public enum ProjectileState
    {
        Idle,       // đứng yên / charge
        Moving,     // đang bay
        Hit         // trúng mục tiêu
    }

    private ProjectileState state;

    // =========================
    // RUNTIME
    // =========================
    private BossProjectileContext ctx;
    private ProjectileMoveType moveType;
    private Transform homingTarget;

    private Vector2 moveDir;
    private float lifeTimer;

    private bool isDespawning;
    private Collider2D col;

    // =========================
    // UNITY
    // =========================
    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (state != ProjectileState.Moving)
            return;

        UpdateMovement();
        UpdateLifeTime();
    }

    // =========================
    // POOL CALLBACK
    // =========================
    public void OnSpawn()
    {
        lifeTimer = 0f;
        isDespawning = false;
        state = ProjectileState.Idle;

        if (col != null)
            col.enabled = true;

        if (animator != null)
        {
            animator.Rebind();
            animator.ResetTrigger(HitTrigger);
            animator.Update(0f);
        }
    }

    public void OnDespawn()
    {
        homingTarget = null;
    }

    private void Despawn()
    {
        if (isDespawning) return;
        isDespawning = true;

        ObjectPoolManager.Instance.Despawn(this);
    }

    // =========================
    // INIT API
    // =========================

    /// <summary>
    /// Spawn projectile nhưng CHƯA bay (dùng cho skill charge / vòng tròn)
    /// </summary>
    public void InitIdle(
        BossProjectileContext context,
        Vector2 direction,
        ProjectileMoveType type,
        Transform target = null
    )
    {
        ctx = context;
        moveDir = direction.normalized;
        moveType = type;
        homingTarget = (type == ProjectileMoveType.Homing) ? target : null;

        state = ProjectileState.Idle;
        RotateToDirection(moveDir);
    }

    /// <summary>
    /// Spawn projectile và bay NGAY (dùng cho đánh thường)
    /// </summary>
    public void InitAndFire(
        BossProjectileContext context,
        Vector2 direction,
        ProjectileMoveType type,
        Transform target = null
    )
    {
        InitIdle(context, direction, type, target);
        Fire();
    }

    /// <summary>
    /// Bắt đầu bay
    /// </summary>
    public void Fire()
    {
        if (state != ProjectileState.Idle)
            return;

        state = ProjectileState.Moving;
    }

    // =========================
    // MOVEMENT
    // =========================
    private void UpdateMovement()
    {
        if (moveType == ProjectileMoveType.Homing && homingTarget != null)
        {
            UpdateHoming();
        }

        transform.position +=
            (Vector3)(moveDir * ctx.Speed * Time.deltaTime);
    }

    private void UpdateHoming()
    {
        Vector2 targetDir =
            ((Vector2)homingTarget.position - (Vector2)transform.position).normalized;

        float t = turnSpeed * Time.deltaTime / 360f;
        moveDir = Vector2.Lerp(moveDir, targetDir, t).normalized;

        RotateToDirection(moveDir);
    }

    private void UpdateLifeTime()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
            Despawn();
    }

    // =========================
    // HIT
    // =========================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (state != ProjectileState.Moving)
            return;

        if (!IsValidTarget(other))
            return;

        if (!other.TryGetComponent<IDamageable>(out var target))
            return;

        DealDamage(target);
        EnterHitState();
    }

    private void DealDamage(IDamageable target)
    {
        bool crit = Random.value < ctx.CritChance;
        int dmg = crit ? Mathf.RoundToInt(ctx.Damage * 1.5f) : ctx.Damage;
        target.TakeDamage(dmg);
    }

    private bool IsValidTarget(Collider2D other)
    {
        return ((1 << other.gameObject.layer) & ctx.TargetLayer) != 0;
    }

    private void EnterHitState()
    {
        state = ProjectileState.Hit;
        moveDir = Vector2.zero;

        if (col != null)
            col.enabled = false;

        if (animator != null)
            animator.SetTrigger(HitTrigger);
    }

    // =========================
    // ANIMATION EVENT
    // =========================
    public void AnimEvent_HitEnd()
    {
        Despawn();
    }

    // =========================
    // UTIL
    // =========================
    private void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
