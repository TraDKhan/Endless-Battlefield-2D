using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Skill_FireArrow : BaseSkill
{
    [Header("Fire Arrow")]
    [SerializeField] private GameObject firerArrow;

    [Header("Enemy Targer")]
    [SerializeField] private LayerMask enemyLayer;

    private readonly List<Projectile_FireArrow> arrows = new();

    private float cooldownTimer;
    private int damage;
    private int quantity;
    private float cooldown;
    [SerializeField] private float moveSpeed = 2f;
    //Todo: tỉ lệ chí mạng làm sau khi đồng bộ với vũ khí
    private float cirtChane;
    private void Start()
    {
        if (owner == null)
        {
            owner = GameObject.FindWithTag("Player")?.transform;
            Init(owner);

            OnUnlock();
        }
    }

    private void Update()
    {
        if (owner == null) return;

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer <= 0f)
        {
            Spawn_FireArrow();
            cooldownTimer = cooldown;
        }
    }

    protected override void OnStatsApplied()
    {
        damage = skillStats.GetInt(SkillStatType.Damage);
        quantity = skillStats.GetInt(SkillStatType.ProjectileCount);
        cooldown = skillStats.GetStat(SkillStatType.Cooldown);

        cooldownTimer = cooldown;  
    }

    private void Spawn_FireArrow()
    {
        arrows.Clear();

        float step = 360f / quantity;

        for (int i = 0; i < quantity; i++)
        {
            float angle = step * i;

            GameObject obj = Instantiate(firerArrow, owner.position, Quaternion.identity);

            var arrow = obj.GetComponent<Projectile_FireArrow>();

            arrow.Int(owner, damage, moveSpeed);
            arrow.SetAngle(angle);

            arrows.Add(arrow);
        }

        StartCoroutine(ReleaseArrows());
    }
    private IEnumerator ReleaseArrows()
    {
        yield return new WaitForSeconds(0.8f);

        foreach (var arrow in arrows)
        {
            Transform target = FindRandomEnemy();

            Vector2 dir;

            if (target != null)
                dir = (target.position - arrow.transform.position).normalized;
            else
                dir = Random.insideUnitCircle.normalized;

            arrow.Launch(dir);
        }
    }
    private Transform FindRandomEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.position, 8f, enemyLayer);

        if (hits.Length == 0)
            return null;

        int index = Random.Range(0, hits.Length);

        return hits[index].transform;
    }
}
