using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage;
    public float lifetime = 3f;
    public LayerMask targetLayer;

    private float spawnTime;
    private Transform target;

    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if (target != null)
        {
            // Di chuyển hướng về target
            Vector2 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime, Space.World);

            // Optional: xoay projectile về hướng di chuyển
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        else
        {
            // Nếu không có target, di chuyển thẳng theo local x
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }

        // Hủy projectile sau lifetime
        if (Time.time >= spawnTime + lifetime)
            Destroy(gameObject);
    }

    // Set target từ PlayerRangedAttack
    public void SetTarget(Transform t)
    {
        target = t;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ va chạm với targetLayer
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            if (other.TryGetComponent<IDamageable>(out var targetComponent))
            {
                targetComponent.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
