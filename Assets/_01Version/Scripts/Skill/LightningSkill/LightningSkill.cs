using UnityEngine;

public class LightningSkill : BaseSkill
{
    [Header("Effect")]
    [SerializeField] private GameObject lightningEffectPrefab;

    private Transform player;

    private float cooldownTimer;

    // cached data theo level
    private int damage;
    private int strikes;
    private float radius;
    private float cooldown;

    // =========================
    // UNITY
    // =========================
    private void Awake()
    {
        // tìm player và gắn làm con
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("LightningSkill: Player not found");
            enabled = false;
            return;
        }

        transform.SetParent(player);
        transform.localPosition = Vector3.zero;
    }

    private void Update()
    {
        if (level <= 0) return; // chưa unlock

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            CastLightning();
            cooldownTimer = cooldown;
        }
    }

    // =========================
    // BASESKILL
    // =========================
    protected override void ApplyLevelData()
    {
        var data = upgradeData.GetLevelData(level);

        damage = data.damage;
        strikes = data.lightningCount;
        radius = data.radius;
        cooldown = data.cooldown;

        // reset cooldown khi lên level
        cooldownTimer = cooldown;

        Debug.Log($"Lightning Lv{level} | Strikes:{strikes} Damage:{damage}");
    }

    // =========================
    // CORE LOGIC
    // =========================
    private void CastLightning()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            player.position,
            radius,
            LayerMask.GetMask("Enemy")
        );

        if (enemies.Length == 0) return;

        for (int i = 0; i < strikes; i++)
        {
            var target = enemies[Random.Range(0, enemies.Length)];

            SpawnLightningEffect(target.transform.position);

            var hp = target.GetComponent<EnemyHealthController>();
            if (hp != null)
                hp.TakeDamage(damage);
        }
    }

    private void SpawnLightningEffect(Vector3 targetPos)
    {
        Vector3 start = targetPos + Vector3.up * 4f;

        var fx = Instantiate(lightningEffectPrefab);
        fx.GetComponent<LightningEffect>().Init(start, targetPos);
    }

    // =========================
    // GIZMOS
    // =========================
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
