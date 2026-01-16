using System.Collections;
using UnityEngine;

public class BossSkillExecutor : MonoBehaviour
{
    BossContext context;

    bool isExecuting;
    float globalAttackCooldown;

    public bool IsBusy => isExecuting || globalAttackCooldown > 0f;
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
        if (globalAttackCooldown > 0f)
            globalAttackCooldown -= Time.deltaTime;
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
        globalAttackCooldown = skill.Cooldown;
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
        globalAttackCooldown = context.Stats.attackCooldown;
    }
}
