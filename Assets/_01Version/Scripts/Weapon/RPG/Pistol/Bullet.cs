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

    // ===== Runtime =====
    private WeaponDamageContext damageContext;
    private int penetrationLeft;
    private float lifeTimer;

    private Vector2 moveDir;
    private float moveSpeed;

    // =========================
    // POOL CALLBACKS
    // =========================
    public void OnSpawn()
    {
        lifeTimer = 0f;
        penetrationLeft = maxPenetration;
    }

    public void OnDespawn()
    {
        // reset nếu cần
    }

    // =========================
    // INIT (gọi sau khi Spawn)
    // =========================
    public void Init(
        WeaponDamageContext context,
        Vector2 direction,
        float speed
    )
    {
        damageContext = context;
        moveDir = direction.normalized;
        moveSpeed = speed;

        RotateToDirection(moveDir);
    }

    // =========================
    // UPDATE MOVE
    // =========================
    private void Update()
    {
        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Despawn();
        }
    }

    // =========================
    // HIT LOGIC
    // =========================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var target))
            return;

        ApplyDamage(target);

        penetrationLeft--; 
        if (penetrationLeft <= 0)
            Despawn();
    }

    void ApplyDamage(IDamageable target)
    {
        bool isCrit = Random.value < damageContext.critChance;
        float finalDamage = isCrit
            ? damageContext.damage * damageContext.critMultiplier
            : damageContext.damage;

        target.TakeDamage((int)finalDamage);
    }

    // =========================
    void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Despawn()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }
}
