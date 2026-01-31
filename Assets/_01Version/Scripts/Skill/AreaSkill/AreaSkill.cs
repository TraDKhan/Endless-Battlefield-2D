using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AreaSkill : BaseSkill
{
    [Header("Effect")]
    [SerializeField] private AreaEffect areaEffectPrefab;
    [SerializeField] private float tickInterval = 0.3f;

    private AreaEffect activeEffect;
    private float cooldownTimer;
    private float lifeTimer;

    // ===================
    // DEBUG
    // ===================
    private void Start()
    {
        if (owner == null)
        {
            owner = GameObject.FindWithTag("Player")?.transform;

            OnUnlock(); // ép mở skill
        }
    }

    // =========================
    // APPLY LEVEL
    // =========================
    protected override void OnStatsApplied()
    {
        cooldownTimer = 0f; // cast ngay khi unlock
    }

    private void Update()
    {
        if (Level <= 0 || owner == null) return;

        if (activeEffect == null)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
                SpawnEffect();
        }
        else
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0f)
                DespawnEffect();
        }
    }
    // =========================
    // CORE
    // =========================
    private void SpawnEffect()
    {
        int damage = skillStats.GetInt(SkillStatType.Damage);
        float radius = skillStats.GetStat(SkillStatType.AttackRange);
        float duration = skillStats.GetStat(SkillStatType.Duration);
        float cooldown = skillStats.GetStat(SkillStatType.Cooldown);

        activeEffect = Instantiate(areaEffectPrefab, owner);
        activeEffect.transform.localPosition = Vector3.zero;

        activeEffect.Init(damage, tickInterval);

        float diameter = radius * 2f;
        activeEffect.SetScale(diameter);

        lifeTimer = duration;
        cooldownTimer = cooldown;
    }

    private void DespawnEffect()
    {
        if (activeEffect != null)
            Destroy(activeEffect.gameObject);

        activeEffect = null;
    }
}
