using UnityEngine;

public class AreaSkillController : BaseSkill
{
    [Header("Data")]
    [SerializeField] private AreaSkillData data;
    [SerializeField] private GameObject areaPrefab;

    private GameObject areaInstance;
    private float cooldownTimer;
    private float lifeTimer;

    protected override void ApplyLevelScaling()
    {
        // Ví dụ scale
        // damage +20% mỗi level
        data.damage = Mathf.RoundToInt(data.damage * (1 + 0.2f * (level - 1)));
    }

    void Start()
    {
        Cast();
        lifeTimer = data.duration;
        cooldownTimer = data.cooldown;
    }

    void Update()
    {
        if (areaInstance != null && areaInstance.activeSelf)
        {
            lifeTimer -= Time.deltaTime;
            if (lifeTimer <= 0)
            {
                areaInstance.SetActive(false);
                cooldownTimer = data.cooldown;
            }
        }
        else
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                Cast();
                lifeTimer = data.duration;
            }
        }
    }

    void Cast()
    {
        if (areaInstance == null)
        {
            areaInstance = Instantiate(areaPrefab, transform);
        }

        areaInstance.transform.localPosition = Vector3.zero;
        areaInstance.SetActive(true);
    }
}
