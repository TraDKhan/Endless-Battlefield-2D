using UnityEngine;
using static BossRangedSkill;

[RequireComponent(typeof(Collider2D))]
[RequireComponent (typeof(PoolIdentity))]
public class BossProjectile : MonoBehaviour, IPoolable
{
    // ===== Pool =====
    public PoolIdentity Identity { get; set; }

    // ===== Config =====
    [Header("Life")]
    [SerializeField] float lifeTime = 4f;
    [SerializeField] int maxPenetration = 1;

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
    int penetrationLeft;
    private bool isDespawning;

    // =========================
    // POOL
    // =========================
    public void OnSpawn()
    {
        lifeTimer = 0f;
        penetrationLeft = maxPenetration;
        isDespawning = false;

        if (animator != null)
        {
            animator.Rebind();
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
        if (isDespawning) return;

        if (!IsValidTarget(other)) return;

        if (!other.TryGetComponent<IDamageable>(out var target)) return;

        DealDamage(target);
        penetrationLeft--;

        if (penetrationLeft > 0)
        {
            PlayHitEffect();
        }
        else
        {
            PlayHitEffect();
            Despawn();
        }
    }

    void DealDamage(IDamageable target)
    {
        bool isCrit = Random.value < ctx.CritChance;
        int dmg = isCrit ? Mathf.RoundToInt(ctx.Damage * 1.5f) : ctx.Damage;

        target.TakeDamage(dmg);
    }

    // =========================
    // HIT FX
    // =========================
    private void PlayHitEffect()
    {
        if (animator != null)
        {
            animator.SetTrigger(HitTrigger);
        }
    }

    // =========================
    // UTIL
    // =========================
    bool IsValidTarget(Collider2D col)
    {
        return ((1 << col.gameObject.layer) & ctx.TargetLayer) != 0;
    }

    private void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // Animation Event (optional)
    public void AnimationHitEnd()
    {
        if (penetrationLeft <= 0)
            Despawn();
    }
}
