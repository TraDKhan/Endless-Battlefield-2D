using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Life")]
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int maxPenetration = 1;

    private WeaponDamageContext damageContext;
    private int penetrationLeft;
    private float spawnTime;

    private Vector2 moveDir;
    // ============================
    // INIT
    // ============================
    public void Init(WeaponDamageContext context, Vector2 direction)
    {
        damageContext = context;
        penetrationLeft = maxPenetration;
        spawnTime = Time.time;

        moveDir = direction.normalized;

        RotateToDirection(moveDir);
    }

    void RotateToDirection(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // ============================
    // HIT LOGIC
    // ============================
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IDamageable>(out var target))
            return;

        ApplyDamage(target);

        penetrationLeft--;

        if (penetrationLeft <= 0)
            DestroyBullet();
    }

    void ApplyDamage(IDamageable target)
    {
        bool isCrit = Random.value < damageContext.critChance;
        float finalDamage = isCrit
            ? damageContext.damage * damageContext.critMultiplier
            : damageContext.damage;

        target.TakeDamage((int)finalDamage);
    }
    private void Update()
    {
        if (Time.time >= spawnTime + lifeTime)
            Destroy(gameObject);
    }
    void DestroyBullet()
    {
        Destroy(gameObject);
        // sau này đổi sang pool
    }
}
