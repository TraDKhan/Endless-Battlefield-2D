using UnityEngine;

public class AreaSkillController : BaseSkill
{
    [Header("Data")]
    [SerializeField] private AreaSkillData data;
    [SerializeField] private GameObject areaPrefab;
    [SerializeField] private AreaSkill areaSkill;

    private GameObject areaInstance;
    private float cooldownTimer;
    private float lifeTimer;

    protected override void ApplyLevelData()
    {
        var data = upgradeData.GetLevelData(level);

        areaSkill.SetData(
            data.damage,
            data.radius,
            data.duration,
            data.cooldown
        );
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
