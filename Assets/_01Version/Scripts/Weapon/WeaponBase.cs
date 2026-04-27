using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected WeaponController controller;
    protected WeaponStats stats;

    protected float lastFireTime;

    [Header("Targeting")]
    [SerializeField] protected LayerMask enemyLayer;

    [Header("Rotation")]
    [SerializeField] protected bool rotateToFireDirection = true;
    [SerializeField] protected float rotationOffset = 0f;
    [SerializeField] protected bool flipSpriteByDirection = true;
    [SerializeField] protected float rotationSpeed = 0.25f;

    protected SpriteRenderer spriteRenderer;
    protected WeaponAnimationController animationController;

    protected virtual void Awake()
    {
        animationController = GetComponent<WeaponAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // ===== INIT TỪ ROOT =====
    public virtual void Initialize(WeaponController controller)
    {
        this.controller = controller;
        this.stats = controller.Stats;
    }

    public virtual void OnStatsChanged()
    {
        // hook cho subclass nếu cần
    }

    protected bool CanFire()
    {
        return Time.time >= lastFireTime + stats.Cooldown;
    }

    protected virtual void Update()
    {
        if (stats == null) return;

        //to do: tôi ưu bằng cách scan theo interval
        Transform target = FindNearestEnemy();
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            RotateToDirection(direction);

            // 2. Chỉ thực hiện logic bắn nếu đủ điều kiện
            if (CanFire())
            {
                lastFireTime = Time.time;
                if (animationController != null)
                    animationController.PlayFire();
                else
                    OnFireLogic();
            }
        }
        else
        {
            // Tùy chọn: Trở về góc mặc định nếu không có quái
            RotateToDirection(Vector2.right);
        }
    }

    public void OnFireAnimationEvent()
    {
        OnFireLogic();
    }

    protected abstract void OnFireLogic();

    // ===== Target =====
    protected Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            stats.AttackRange,
            enemyLayer
        );

        float minDistance = float.MaxValue;
        Transform nearest = null;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < minDistance)
            {
                minDistance = d;
                nearest = e.transform;
            }
        }

        return nearest;
    }

    // ===== Rotation =====
    protected void RotateToDirection(Vector2 dir)
    {
        if (!rotateToFireDirection || dir == Vector2.zero) return;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffset;

        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            720f * Time.deltaTime
        );

        if (flipSpriteByDirection && spriteRenderer != null)
        {
            spriteRenderer.flipY = dir.x < 0;
        }
    }

    protected WeaponContext CreateWeaponContext()
    {
        return new WeaponContext(
            source: controller.gameObject,
            firePoint: transform,
            stats: stats,
            targetLayer: enemyLayer,
            knockbackForce: 1f
        );
    }
    private void OnDrawGizmosSelected()
    {
        if (stats == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.AttackRange);
    }
}
