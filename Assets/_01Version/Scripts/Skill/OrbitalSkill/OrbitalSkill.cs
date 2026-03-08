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

        SyncOrbCount(targetCount);
        UpdateOrbStats(radius, speed, damage);
        RecalculateAngles();
    }

    private void SyncOrbCount(int count)
    {
        // Spawn thêm orb
        while (orbs.Count < count)
        {
            GameObject orbObj = Instantiate(orbPrefab, transform);
            OrbProjectile orb = orbObj.GetComponent<OrbProjectile>();

            orbs.Add(orb);
        }

        // Xóa bớt orb
        while (orbs.Count > count)
        {
            Destroy(orbs[^1].gameObject);
            orbs.RemoveAt(orbs.Count - 1);
        }
    }

    private void UpdateOrbStats(float radius, float speed, float damage)
    {
        foreach (var orb in orbs)
        {
            orb.SetStats(owner, radius, speed, damage);
        }
    }

    private void RecalculateAngles()
    {
        int count = orbs.Count;
        if (count == 0) return;

        float step = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = step * i;
            orbs[i].SetAngle(angle);
        }
    }
}