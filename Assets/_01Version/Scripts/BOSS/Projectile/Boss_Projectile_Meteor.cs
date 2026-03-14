using UnityEngine;

[RequireComponent(typeof(PoolIdentity))]
public class Boss_Projectile_Meteor : MonoBehaviour, IPoolable
{
    public PoolIdentity Identity { get; set; }

    [SerializeField] float fallSpeed = 12f;
    [SerializeField] float cooldown = 0.5f;
    [SerializeField] int damage = 10;
    [SerializeField] float damageRadius = 1.2f;
    [SerializeField] float damageInterval = 0.3f;

    float cooldownTimer;
    float damageTimer;

    Vector3 targetPos;
    bool impacted;

    public void Init(Vector3 target)
    {
        targetPos = target;
    }

    void Update()
    {
        if (!impacted)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                fallSpeed * Time.deltaTime
            );

            if ((transform.position - targetPos).sqrMagnitude < 0.0025f)
            {
                Impact();
            }
        }
        else
        {
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                new Vector3(2f, 2f, 1f),
                Time.deltaTime * 10f
            );

            DamageArea();

            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                ObjectPoolManager.Instance.Despawn(this);
            }
        }
    }

    void Impact()
    {
        impacted = true;
        damageTimer = 0f;
    }
    void DamageArea()
    {
        damageTimer -= Time.deltaTime;

        if (damageTimer > 0) return;

        damageTimer = damageInterval;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            damageRadius
        );

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log(damage);
                hit.GetComponent<IDamageable>()?.TakeDamage(damage);
            }
        }
    }
    public void OnSpawn()
    {
        impacted = false;
        cooldownTimer = cooldown;
        transform.localScale = Vector3.one;
        transform.rotation = Quaternion.identity;
    }

    public void OnDespawn()
    {
        impacted = true;
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + Vector3.down *0.1f, damageRadius);
    }
}