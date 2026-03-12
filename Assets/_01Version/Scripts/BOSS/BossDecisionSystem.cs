using System.Collections.Generic;
using UnityEngine;

public class BossDecisionSystem : MonoBehaviour
{
    [Header("Registered Skills")]
    [SerializeField] private List<MonoBehaviour> skillBehaviours;

    private readonly List<IBossSkill> skills = new();
    private readonly List<IBossSkill> validSkills = new();

    void Awake()
    {
        skills.Clear();

        foreach (var m in skillBehaviours)
        {
            if (m is IBossSkill skill)
            {
                skills.Add(skill);
            }
            else
            {
                Debug.LogWarning(
                    $"{m.name} does NOT implement IBossSkill",
                    m
                );
            }
        }
    }

    public IBossSkill ChooseSkill(BossContext ctx)
    {
        validSkills.Clear();

        foreach (var skill in skills)
        {
            if (skill.CanUse(ctx))
            {
                validSkills.Add(skill);
            }
        }

        if (validSkills.Count == 0)
            return null;

        return validSkills[Random.Range(0, validSkills.Count)];
    }
}
