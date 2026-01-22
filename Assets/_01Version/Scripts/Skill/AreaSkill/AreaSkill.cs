using UnityEngine;

public class AreaSkill : BaseSkill
{
    [Header("Effect")]
    [SerializeField] private AreaEffect areaEffectPrefab;
    [SerializeField] private float tickInterval = 0.3f;

    private AreaEffect activeEffect;

    // ===== Level data =====
    private int damage;
    private float radius;
    private float duration;
    private float cooldown;

    private float cooldownTimer;
    private float lifeTimer;

    // =========================
    // INIT
    // =========================
    public override void Init(Transform ownerTransform, CharacterStats characterStats)
    {
        base.Init(ownerTransform, characterStats);
        transform.SetParent(owner);
        transform.localPosition = Vector3.zero;
    }

    protected override void ApplyLevelData()
    {
        var data = upgradeData.levels[Level - 1];

        damage = data.damage;
        radius = data.radius;
        duration = data.duration;
        cooldown = data.cooldown;

        cooldownTimer = 0f; // cast ngay khi unlock / level up
    }

    // =========================
    // UPDATE
    // =========================
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
        activeEffect = Instantiate(areaEffectPrefab, owner);
        activeEffect.transform.localPosition = Vector3.zero;

        activeEffect.Init(damage, tickInterval);

        // 🔑 Scale = diameter (Prefab scale = 1 tương ứng radius = 0.5)
        float diameter = radius * 2f;
        activeEffect.SetScale(diameter);

        lifeTimer = duration;
    }

    private void DespawnEffect()
    {
        if (activeEffect != null)
            Destroy(activeEffect.gameObject);

        activeEffect = null;
        cooldownTimer = cooldown;
    }
    private void Start()
    {
        if (owner == null)
        {
            owner = GameObject.FindWithTag("Player")?.transform;
            stats = PlayerController.Instance.Stats;

            OnUnlock(); // ép mở skill
        }
    }
}
