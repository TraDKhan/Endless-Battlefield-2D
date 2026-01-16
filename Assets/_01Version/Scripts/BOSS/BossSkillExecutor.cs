using System.Collections;
using UnityEngine;

public class BossSkillExecutor : MonoBehaviour
{
    BossController boss;
    BossContext context;

    IBossSkill currentSkill;
    bool isExecuting;

    public BossContext Context => context;
    public bool IsBusy => isExecuting;

    void Awake()
    {
        boss = GetComponent<BossController>();
        context = new BossContext
        {
            boss = boss
        };
    }

    public void ExecuteSkill(IBossSkill skill)
    {
        if (isExecuting || skill == null) return;
        StartCoroutine(RunSkill(skill));
    }

    IEnumerator RunSkill(IBossSkill skill)
    {
        currentSkill = skill;
        isExecuting = true;

        yield return skill.Execute(context);
        isExecuting = false;
        currentSkill = null;
    }

    public void OnAnimationEvent(string eventId)
    {
        if (currentSkill is IAnimEventSkill evtSkill)
        {
            evtSkill.OnAnimationEvent(context, eventId);
        }
    }
}
