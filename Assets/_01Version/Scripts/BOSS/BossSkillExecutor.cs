using System.Collections;
using UnityEngine;

public class BossSkillExecutor : MonoBehaviour
{
    BossController boss;
    BossContext context;
    public BossContext Context => context;

    bool isExecuting;
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
        isExecuting = true;
        yield return skill.Execute(context);
        isExecuting = false;
    }
}
