using System.Collections;
using UnityEngine;

public class BossSkillExecutor : MonoBehaviour
{
    BossContext context;

    bool isExecuting;
    float skillCooldown;
    float basicCooldown;

    public bool IsBusy => isExecuting;
    public bool IsSkillOnCooldown => skillCooldown > 0f;
    public bool IsBasicOnCooldown => basicCooldown > 0f;

    public BossContext Context => context;

    void Awake()
    {
        context = new BossContext
        {
            boss = GetComponent<BossController>()
        };
    }

    void Update()
    {
        if (skillCooldown > 0) skillCooldown -= Time.deltaTime;
        if (basicCooldown > 0) basicCooldown -= Time.deltaTime;
    }

    // ================= SKILL =================
    public void ExecuteSkill(IBossSkill skill)
    {
        if (IsBusy || skill == null)
            return;

        StartCoroutine(RunSkill(skill));
    }

    IEnumerator RunSkill(IBossSkill skill)
    {
        isExecuting = true;
        yield return skill.Execute(context);
        isExecuting = false;
        skillCooldown = skill.Cooldown;
    }

    // ================= BASIC =================
    public void ExecuteBasicAttack(IBasicAttack attack)
    {
        if (IsBusy || attack == null)
            return;

        StartCoroutine(RunBasicAttack(attack));
    }

    IEnumerator RunBasicAttack(IBasicAttack attack)
    {
        isExecuting = true;
        yield return attack.Attack(context);
        isExecuting = false;
        basicCooldown = context.Stats.attackCooldown;
    }
}
