using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrain : MonoBehaviour
{
    BossContext context;

    IBossSkill[] skills;
    IBasicAttack basicAttack;

    IBossSkill currentSkill;
    bool busy;
    float thinkTimer;

    const float THINK_INTERVAL = 0.4f;

    void Awake()
    {
        context = new BossContext
        {
            boss = GetComponent<BossController>()
        };

        basicAttack = GetComponentInChildren<IBasicAttack>();

        skills = GetComponentsInChildren<IBossSkill>();
    }

    void Update()
    {
        if (busy)
            return;

        thinkTimer -= Time.deltaTime;

        if (thinkTimer > 0)
            return;

        thinkTimer = THINK_INTERVAL;

        Decide();
    }

    void Decide()
    {
        List<IBossSkill> available = new();

        foreach (var s in skills)
        {
            if (s.CanUse(context))
                available.Add(s);
        }

        if (available.Count > 0)
        {
            var skill = available[Random.Range(0, available.Count)];
            StartCoroutine(RunSkill(skill));
            return;
        }

        if (basicAttack != null && basicAttack.CanAttack(context))
        {
            StartCoroutine(RunBasic());
        }
    }

    IEnumerator RunSkill(IBossSkill skill)
    {
        busy = true;
        currentSkill = skill;

        yield return skill.Execute(context);

        currentSkill = null;
        busy = false;
    }

    IEnumerator RunBasic()
    {
        busy = true;
        yield return basicAttack.Attack(context);
        busy = false;
    }

    public void Anim_RouteEvent(BossAnimEvent animEvent)
    {
        currentSkill?.OnAnimationEvent(animEvent);
    }
}