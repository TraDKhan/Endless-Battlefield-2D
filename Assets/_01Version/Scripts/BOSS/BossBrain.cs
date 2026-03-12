using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBrain : MonoBehaviour
{
    [SerializeField] List<MonoBehaviour> skillBehaviours;

    IBossSkill[] skills;
    IBasicAttack basicAttack;

    BossContext context;

    bool busy;
    float thinkTimer;

    const float THINK_INTERVAL = 0.4f;

    void Awake()
    {
        context = new BossContext
        {
            boss = GetComponent<BossController>()
        };

        basicAttack = GetComponent<IBasicAttack>();

        List<IBossSkill> list = new();

        foreach (var m in skillBehaviours)
        {
            if (m is IBossSkill s)
                list.Add(s);
        }

        skills = list.ToArray();
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
        yield return skill.Execute(context);
        busy = false;
    }

    IEnumerator RunBasic()
    {
        busy = true;
        yield return basicAttack.Attack(context);
        busy = false;
    }
}