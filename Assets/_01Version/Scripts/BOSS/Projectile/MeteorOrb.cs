using UnityEngine;

[RequireComponent(typeof(PoolIdentity))]
public class MeteorOrb : MonoBehaviour, IPoolable
{
    public PoolIdentity Identity { get; set; }

    private Vector3 target;
    private float speed;

    private SpriteRenderer sr;
    private Animator animator;

    [Header("Explosion")]
    [SerializeField] float explodeDistance = 0.1f;
    [SerializeField] float damageRadius = 1.2f;
    [SerializeField] int damage = 10;
    [SerializeField] LayerMask damageLayer;

    bool exploded = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Init(Vector3 targetPos, float moveSpeed)
    {
        target = targetPos;
        speed = moveSpeed;
        exploded = false;

        Vector3 dir = (target - transform.position).normalized;

        sr.flipX = dir.x < 0;
    }

    void Update()
    {
        if (exploded) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target) <= explodeDistance)
        {
            Explode();
        }
    }

    void Explode()
    {
        exploded = true;
        animator.SetTrigger("On");
    }

    //gọi từ animation event
    void DealDamageToExplode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            damageRadius,
            damageLayer
        );

        foreach (var hit in hits)
        {
            var health = hit.GetComponent<IDamageable>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }

    // gọi từ animation event
    public void OnExplosionEnd()
    {
        ObjectPoolManager.Instance.Despawn(this);
    }

    // ================= POOL =================

    public void OnSpawn()
    {
        exploded = false;

        animator.ResetTrigger("On");
        //animator.Play("Flying", 0, 0f);
    }

    public void OnDespawn()
    {
        exploded = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}