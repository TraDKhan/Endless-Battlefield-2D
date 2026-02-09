using System.Collections.Generic;
using UnityEngine;

public class OrbitalSkill : BaseSkill
{
    [SerializeField] private GameObject orbPrefab;

    private readonly List<OrbProjectile> orbs = new();

    private void Start()
    {
        if (owner == null)
        {
            owner = GameObject.FindWithTag("Player")?.transform;
            Init(owner);

            OnUnlock();
        }
    }

    protected override void OnStatsApplied()
    {
        int targetCount = skillStats.GetInt(SkillStatType.ProjectileCount);

        float radius = skillStats.GetStat(SkillStatType.AttackRange);
        float speed = skillStats.GetStat(SkillStatType.RotateSpeed);
        float damage = skillStats.GetStat(SkillStatType.Damage);

        SyncOrbCount(targetCount, radius, speed, damage);
        RecalculateAngles();
    }

    private void SyncOrbCount(int count, float radius, float speed, float damage)
    {
        // Spawn thêm
        while (orbs.Count < count)
        {
            var orbObj = Instantiate(orbPrefab);
            orbObj.transform.SetParent(transform);

            var orb = orbObj.GetComponent<OrbProjectile>();
            orbs.Add(orb);

            orb.Init(owner, radius, speed, 0f, damage);
        }

        // Xóa bớt (nếu có debuff)
        while (orbs.Count > count)
        {
            Destroy(orbs[^1].gameObject);
            orbs.RemoveAt(orbs.Count - 1);
        }

        // Update stat
        foreach (var orb in orbs)
            orb.Init(owner, radius, speed, 0f, damage);
    }

    private void RecalculateAngles()
    {
        int count = orbs.Count;
        if (count == 0) return;

        float step = 360f / count;

        for (int i = 0; i < count; i++)
            orbs[i].Init(
                owner,                          
                skillStats.
                GetStat(SkillStatType.AttackRange),                          
                skillStats.GetStat(SkillStatType.RotateSpeed),                          
                step * i,                          
                skillStats.GetStat(SkillStatType.Damage)
            );
    }
}
