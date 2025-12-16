using UnityEngine;

public class LightningSkill : BaseSkill
{
    [Header("Effect")]
    [SerializeField] private GameObject lightningEffectPrefab;

    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayer;

    // ===== Runtime =====
    private float cooldownTimer;

    // ===== Cached per level =====
    private int damage;
    private int strikes;
    private float radius;
    private float cooldown;

    // =========================
    // UPDATE
    // =========================
    private void Update()
    {
        if (Level <= 0 || owner == null) return;

        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            CastLightning();
            cooldownTimer = cooldown;
        }
    }

    // =========================
    // LEVEL DATA
    // =========================
    protected override void ApplyLevelData()
    {
        var data = UpgradeData.GetLevelData(Level);

        damage = data.damage;
        strikes = data.lightningCount;
        radius = data.radius;
        cooldown = data.cooldown;

        cooldownTimer = cooldown;
    }

    // =========================
    // CORE LOGIC
    // =========================
    private void CastLightning()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            owner.position,
            radius,
            enemyLayer
        );

        if (enemies.Length == 0) return;

        for (int i = 0; i < strikes; i++)
        {
            Collider2D target = enemies[Random.Range(0, enemies.Length)];

            SpawnLightningFX(target.transform.position);

            var hp = target.GetComponent<EnemyHealthController>();
            if (hp == null) continue;

            int finalDamage = damage;

            // Có thể mở rộng crit / buff tại đây
            hp.TakeDamage(finalDamage);
        }
    }

    private void SpawnLightningFX(Vector3 targetPos)
    {
        Vector3 start = targetPos + Vector3.up * 4f;

        var fx = Instantiate(lightningEffectPrefab);
        fx.GetComponent<LightningEffect>()
          ?.Init(start, targetPos);
    }

    // =========================
    // DEBUG
    // =========================
    private void OnDrawGizmosSelected()
    {
        if (owner == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(owner.position, radius);
    }
}
