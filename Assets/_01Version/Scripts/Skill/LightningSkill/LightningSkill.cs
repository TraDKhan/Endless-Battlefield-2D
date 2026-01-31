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
    // STATS APPLY
    // =========================
    protected override void OnStatsApplied()
    {
        damage = skillStats.GetInt(SkillStatType.Damage);
        strikes = skillStats.GetInt(SkillStatType.LightningCount);
        radius = skillStats.GetStat(SkillStatType.AttackRange);
        cooldown = skillStats.GetStat(SkillStatType.Cooldown);

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
            var target = enemies[Random.Range(0, enemies.Length)];

            SpawnLightningFX(target.transform.position);

            var hp = target.GetComponent<EnemyHealthController>();
            if (hp == null) continue;

            hp.TakeDamage(damage);
        }
    }

    private void SpawnLightningFX(Vector3 pos)
    {
        var fx = Instantiate(lightningEffectPrefab, pos, Quaternion.identity);

        var effect = fx.GetComponent<LightningEffect>();
        if (effect != null)
        {
            effect.Init(damage, enemyLayer);
        }
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
