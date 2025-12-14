using UnityEngine;

public class AreaSkill : BaseSkill
{
    [SerializeField] private GameObject areaEffectPrefab;
    [SerializeField] private float tickInterval = 0.3f;

    private GameObject areaInstance;

    // cached level data
    private int damage;
    private float radius;
    private float duration;
    private float cooldown;

    private float cooldownTimer;
    private float lifeTimer;
    private float tickTimer;

    // =========================
    // BASESKILL
    // =========================
    protected override void ApplyLevelData()
    {
        var data = upgradeData.GetLevelData(level);

        damage = data.damage;
        radius = data.radius;
        duration = data.duration;
        cooldown = data.cooldown;

        Debug.Log($"Area Skill Lv{level} | dmg:{damage} radius:{radius}");
    }

    // =========================
    // UPDATE
    // =========================
    private void Update()
    {
        if (level <= 0) return;

        // cooldown
        if (areaInstance == null)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                SpawnArea();
        }
        else
        {
            // lifetime
            lifeTimer -= Time.deltaTime;
            tickTimer -= Time.deltaTime;

            if (tickTimer <= 0f)
            {
                DealDamage();
                tickTimer = tickInterval;
            }

            if (lifeTimer <= 0f)
                DestroyArea();
        }
    }

    // =========================
    // CORE
    // =========================
    private void SpawnArea()
    {
        areaInstance = Instantiate(areaEffectPrefab, transform);
        areaInstance.transform.localPosition = Vector3.zero;

        var col = areaInstance.GetComponent<CircleCollider2D>();
        if (col != null)
            col.radius = radius;

        lifeTimer = duration;
        tickTimer = tickInterval;
    }

    private void DestroyArea()
    {
        Destroy(areaInstance);
        areaInstance = null;
        cooldownTimer = cooldown;
    }

    private void DealDamage()
    {
        var hits = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            LayerMask.GetMask("Enemy")
        );

        foreach (var h in hits)
            h.GetComponent<EnemyHealthController>()?.TakeDamage(damage);
    }

    // =========================
    // GIZMOS
    // =========================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
