using UnityEngine;

public abstract class ProjectileCore : MonoBehaviour, IPoolable
{
    public PoolIdentity Identity { get; set; }

    [Header("Lifetime")]
    [SerializeField] protected float maxLifetime = 5f;
    [SerializeField] protected ProjectileMode moveMode = ProjectileMode.Direction;

    private float lifetimeTimer;
    protected Vector2 direction;
    protected Vector2 targetPosition;

    protected float speed;
    protected int damage;
    protected bool reachedTarget;
    protected bool isProcessingHit;

    public void OnSpawn()
    {
        lifetimeTimer = 0f;
        reachedTarget = false;
        isProcessingHit = false;

        direction = Vector2.zero;
    }

    public void OnDespawn() { }

    // ============================
    // INIT
    // ============================
    public virtual void Init(
        Vector2 dir,
        Vector2 targetPos,
        int dmg,
        float moveSpeed
    )
    {
        direction = dir.normalized;
        damage = dmg;
        speed = moveSpeed;
        targetPosition = targetPos;

        if (moveMode == ProjectileMode.Direction)
            RotateTowards(direction);

        //if (moveMode == ProjectileMode.Position)
        //    RotateTowards(targetPosition - (Vector2)transform.position);

    }

    private void Update()
    {
        Move();
        HandleLifetime();
    }

    protected virtual void Move()
    {
        if (reachedTarget) return;

        if (moveMode == ProjectileMode.Direction)
        {
            transform.position += (Vector3)(direction * speed * Time.deltaTime);
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
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isProcessingHit) return;

        if (other.CompareTag("Player"))
        {
            isProcessingHit = true;
            OnHit(other);
        }
    }

    protected abstract void OnHit(Collider2D target);

    private void HandleLifetime()
    {
        lifetimeTimer += Time.deltaTime;

        if (lifetimeTimer >= maxLifetime)
            OnLifetimeExpired();
    }

    private void RotateTowards(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    protected virtual void OnLifetimeExpired()
    {
        Despawn();
    }

    protected void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
}
