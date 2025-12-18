using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public WeaponData data;

    protected WeaponStats stats;
    protected WeaponUpgradeSystem upgradeSystem;
    protected WeaponAnimationController animationController;
    protected ArmPivotController arm;

    protected float lastFireTime;

    [Header("Targeting")]
    [SerializeField] protected LayerMask enemyLayer;

    [Header("Rotation")]
    [SerializeField] protected bool rotateToFireDirection = true;
    [SerializeField] protected float rotationOffset = 0f; // ví dụ sprite hướng phải = 0
    [SerializeField] protected bool flipSpriteByDirection = true;

    protected SpriteRenderer spriteRenderer;
    protected virtual void Awake()
    {
        upgradeSystem = WeaponUpgradeSystem.Instance;
        animationController = GetComponent<WeaponAnimationController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        arm = GetComponent<ArmPivotController>();

        stats = new WeaponStats(data, upgradeSystem);
        upgradeSystem.OnWeaponStatsChanged += OnWeaponStatsChanged;
    }

    protected virtual void OnDestroy()
    {
        if (upgradeSystem != null)
            upgradeSystem.OnWeaponStatsChanged -= OnWeaponStatsChanged;
    }

    protected virtual void OnWeaponStatsChanged()
    {
        stats.Recalculate();
    }

    protected bool CanFire()
    {
        return Time.time >= lastFireTime + stats.Cooldown;
    }

    // ===== Auto fire (survivor) 
    protected virtual void Update()
    {
        if (!CanFire()) return;

        // Kiểm tra có mục tiêu trong tầm
        Transform target = FindNearestEnemy();
        if (target == null) return;

        lastFireTime = Time.time;

        if (animationController != null)
        {
            animationController.PlayFire();
        }
        else
        {
            OnFireLogic();
        }
    }

    // ===== Animation Event 
    public void OnFireAnimationEvent()
    {
        OnFireLogic();
    }
    protected abstract void OnFireLogic();

    // ===== Target
    protected Transform FindNearestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            transform.position,
            stats.Range,
            LayerMask.GetMask("Enemy")
        );

        float minDistance = float.MaxValue;
        Transform nearest = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    // ===== Rotation
    protected void RotateToDirection(Vector2 direction)
    {
        if (!rotateToFireDirection) return;
        if (direction == Vector2.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += rotationOffset;

        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (flipSpriteByDirection && spriteRenderer != null)
        {
            spriteRenderer.flipY = direction.x < 0;
        }
    }
    protected void AimWeapon(Vector2 direction)
    {
        arm?.AimAt(direction);
    }
    // ===== Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats != null ? stats.Range : 1f);
    }

    protected WeaponDamageContext CreateDamageContext()
    {
        return new WeaponDamageContext
        {
            damage = stats.Damage,
            critChance = stats.CritChance,
            critMultiplier = 2f,
            source = gameObject
        };
    }
}

public struct WeaponDamageContext
{
    public float damage;
    public float critChance;
    public float critMultiplier;
    public GameObject source;
}
