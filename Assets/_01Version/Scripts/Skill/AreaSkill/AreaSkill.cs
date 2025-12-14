using UnityEngine;

public class AreaSkill : BaseSkill
{
    [SerializeField] private GameObject areaEffectPrefab;
    [SerializeField] private float tickInterval = 0.3f;

    private Transform player;
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
    // UNITY
    // =========================
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("AreaSkill: Player not found");
            enabled = false;
            return;
        }

        // AreaSkill luôn là con của Player
        transform.SetParent(player);
        transform.localPosition = Vector3.zero;
    }

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

        cooldownTimer = cooldown;

        Debug.Log($"Area Skill Lv{level} | dmg:{damage} radius:{radius}");
    }

    // =========================
    // UPDATE
    // =========================
    private void Update()
    {
        if (level <= 0) return;

        if (areaInstance == null)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                SpawnArea();
        }
        else
        {
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
        areaInstance = Instantiate(areaEffectPrefab, player);
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
        // 🔥 VỊ TRÍ LẤY TỪ PLAYER
        var hits = Physics2D.OverlapCircleAll(
            player.position,
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
        if (player == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, radius);
    }
}
