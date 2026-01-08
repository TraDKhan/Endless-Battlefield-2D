using UnityEngine;

public class AreaSkill : BaseSkill
{
    [Header("Effect")]
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
    // INIT
    // =========================
    public override void Init(Transform ownerTransform, CharacterStats characterStats)
    {
        base.Init(ownerTransform, characterStats);

        transform.SetParent(owner);
        transform.localPosition = Vector3.zero;
    }

    // =========================
    // BASESKILL
    // =========================
    protected override void ApplyLevelData()
    {
        var data = upgradeData.levels[Level - 1];

        damage = data.damage;
        radius = data.radius;
        duration = data.duration;
        cooldown = data.cooldown;

        cooldownTimer = cooldown;
    }

    // =========================
    // UPDATE
    // =========================
    private void Update()
    {
        if (Level <= 0) return;

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
        Debug.Log("Spawm skill");
        areaInstance = Instantiate(areaEffectPrefab, owner);
        areaInstance.transform.localPosition = Vector3.zero;

        if (areaInstance.TryGetComponent(out CircleCollider2D col))
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
            owner.position,
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
        if (owner == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(owner.position, radius);
    }
    private void Start()
    {
        if (owner == null)
        {
            owner = GameObject.FindWithTag("Player")?.transform;
            stats = CharacterStatsController.Instance.Stats;

            OnUnlock(); // ép mở skill
        }
    }
}
