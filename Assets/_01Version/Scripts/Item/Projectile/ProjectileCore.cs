using UnityEngine;

public class ProjectileCore : MonoBehaviour, IPoolable
{
    public PoolIdentity Identity { get; set; }

    [Header("Lifetime")]
    [SerializeField] private float maxLifetime = 5f;

    private float lifetimeTimer;

    private Vector2 moveDirection;
    private Vector2 targetPosition;
    private float speed;

    private int damage;
    private bool reachedTarget;

    private ProjectileMode moveMode;
    private IProjectileEffect projectileEffect;

    public void OnSpawn()
    {
        lifetimeTimer = 0f;
        reachedTarget = false;
    }

    public void OnDespawn() { }

    // ============================
    // INIT
    // ============================
    public void InitializeDirection(
        Vector2 direction,
        int baseDamage,
        float moveSpeed,
        IProjectileEffect effect
    )
    {
        moveMode = ProjectileMode.Direction;
        moveDirection = direction.normalized;
        damage = baseDamage;
        speed = moveSpeed;
        projectileEffect = effect;

        RotateTowards(moveDirection);
    }

    public void InitializeTargetPosition(
        Vector2 targetPos,
        int baseDamage,
        float moveSpeed,
        IProjectileEffect effect
    )
    {
        moveMode = ProjectileMode.Position;
        targetPosition = targetPos;
        damage = baseDamage;
        speed = moveSpeed;
        projectileEffect = effect;

        RotateTowards(targetPosition - (Vector2)transform.position);
    }

    private void Update()
    {
        Move();
        HandleLifetime();
    }

    private void Move()
    {
        if (reachedTarget) return;

        if (moveMode == ProjectileMode.Direction)
        {
            transform.position +=
                (Vector3)(moveDirection * speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );

            if (Vector2.Distance(transform.position, targetPosition) <= 0.05f)
            {
                reachedTarget = true;
                projectileEffect?.Apply(this, null);
            }
        }
    }

    private void HandleLifetime()
    {
        lifetimeTimer += Time.deltaTime;
        if (lifetimeTimer >= maxLifetime)
            Despawn();
    }

    // ============================
    // PUBLIC API
    // ============================
    public int Damage => damage;

    public void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }

    private void RotateTowards(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
