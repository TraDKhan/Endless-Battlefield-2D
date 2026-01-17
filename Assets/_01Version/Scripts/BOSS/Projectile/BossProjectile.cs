using UnityEngine;
using static BRangedAttack;

[RequireComponent(typeof(Collider2D))]
[RequireComponent (typeof(PoolIdentity))]
public class BossProjectile : MonoBehaviour, IPoolable
{
    // ===== Pool =====
    public PoolIdentity Identity { get; set; }

    // ===== Config =====
    [Header("Life")]
    [SerializeField] float lifeTime = 4f;

    [Header("Homing")]
    [SerializeField] float turnSpeed = 720f;

    [Header("Animation")]
    [SerializeField] Animator animator;

    static readonly int HitTrigger = Animator.StringToHash("Hit");

    // ===== Runtime =====
    BossProjectileContext ctx;
    ProjectileMoveType moveType;
    Transform homingTarget;

    Vector2 moveDir;

    float lifeTimer;
    private bool isHit;
    private bool isDespawning;

    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    // =========================
    // POOL
    // =========================
    public void OnSpawn()
    {
        lifeTimer = 0f;
        isHit = false;
        isDespawning = false;

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

    void Despawn()
    {
        if (isDespawning) return;
        isDespawning = true;

        ObjectPoolManager.Instance.Despawn(this);
    }

    // =========================
    // INIT
    // =========================
    public void Init(
        BossProjectileContext context,
        Vector2 direction,
        ProjectileMoveType type,
        Transform target = null
    )
    {
        ctx = context;
        moveDir = direction.normalized;
        moveType = type;

        homingTarget = (type == ProjectileMoveType.Homing)? target : null;

        RotateToDirection(moveDir);
    }

    // =========================
    // UPDATE
    // =========================
    private void Update()
    {
        if (isHit) return;

        UpdateMovement();
        UpdateLifeTime();
    }

    private void UpdateMovement()
    {
        if (isDespawning) return;

        if (moveType == ProjectileMoveType.Homing && homingTarget != null)
        {
            UpdateHoming();
        }

        transform.position += (Vector3)(moveDir * ctx.Speed * Time.deltaTime);
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
        if (isHit || isDespawning) return;

        if (!IsValidTarget(other)) return;

        if (!other.TryGetComponent<IDamageable>(out var target)) return;

        DealDamage(target);
        PlayHitEffect();
        EnterHitState();
    }

    void DealDamage(IDamageable target)
    {
        bool isCrit = Random.value < ctx.CritChance;
        int dmg = isCrit ? Mathf.RoundToInt(ctx.Damage * 1.5f) : ctx.Damage;

        target.TakeDamage(dmg);
    }

    bool IsValidTarget(Collider2D col)
    {
        return ((1 << col.gameObject.layer) & ctx.TargetLayer) != 0;
    }

    // =========================
    // HIT STATE
    // =========================
    void EnterHitState()
    {
        isHit = true;

        // Ngừng di chuyển
        moveDir = Vector2.zero;

        // Tắt collider tránh hit nhiều lần
        if (col != null)
            col.enabled = false;
    }

    void PlayHitEffect()
    {
        if (animator != null)
        {
            Debug.Log("Projectile Hit Player");
            animator.SetTrigger(HitTrigger);
        }
    }

    // =========================
    // ANIMATION EVENT
    // =========================
    public void AnimaEvent_HitEnd()
    {
        Despawn();
    }

    // =========================
    // UTIL
    // =========================
    void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
