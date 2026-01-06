using UnityEngine;

public class Projectile : MonoBehaviour, IPoolable
{
    public PoolIdentity Identity { get; set; }

    [Header("Life")]
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int maxPenetration = 1;

    private int penetrationLeft;
    private float lifeTimer;

    private Vector2 moveDir;
    private float damage;
    private float moveSpeed;

    public void OnSpawn()
    {
        lifeTimer = 0f;
        penetrationLeft = maxPenetration;
    }

    public void OnDespawn() { }
    public void Init(Vector2 direction, int dmg, float speed)
    {
        moveDir = direction.normalized;
        damage = dmg;
        moveSpeed = speed;

        RotateToDirection(moveDir);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDir * moveSpeed * Time.deltaTime);

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Despawn();
        }
    }

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
        target.TakeDamage((int)damage);
    }

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
