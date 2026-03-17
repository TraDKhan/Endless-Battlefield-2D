using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Skill_FireArrow : BaseSkill
{
    [Header("Fire Arrow")]
    [SerializeField] private GameObject fireArrowPrefab;

    [Header("Enemy Targer")]
    [SerializeField] private LayerMask enemyLayer;

    private float cooldownTimer;
    private int damage;
    private int quantity;
    private float cooldown;
    [SerializeField] private float moveSpeed = 2f;

    //Todo: tỉ lệ chí mạng làm sau khi đồng bộ với vũ khí
    private float cirtChance;
    private readonly List<Projectile_FireArrow> arrowBuffer = new();
    private bool isWaitingForTarget = false;

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
        if (isWaitingForTarget) return;

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
        arrowBuffer.Clear();

        if (quantity <= 0) return;

        isWaitingForTarget = true;

        float step = 360f / quantity;

        for (int i = 0; i < quantity; i++)
        {
            float angle = step * i;

            var arrow = ObjectPoolManager.Instance.Spawn<Projectile_FireArrow>(fireArrowPrefab);

            arrow.transform.position = owner.position; // ❗ thiếu cái này
            arrow.Int(owner, damage, moveSpeed);
            arrow.SetAngle(angle);

            arrowBuffer.Add(arrow);
        }

        //tránh
        //Coroutine cũ bị mất dữ liệu
        //Arrow launch sai / thiếu / bug random
        var arrowsCopy = new List<Projectile_FireArrow>(arrowBuffer);
        StartCoroutine(ReleaseArrows(arrowsCopy));
    }

    private IEnumerator ReleaseArrows(List<Projectile_FireArrow> arrows)
    {
        yield return new WaitForSeconds(0.8f);
        List<Transform> enemies = null;

        while (true)
        {
            enemies = GetEnemies();

            if (enemies.Count > 0)
                break;

            yield return null;
        }

        foreach (var arrow in arrows)
        {
            if (arrow == null) continue;

            Transform target = null;

            if (enemies.Count > 0)
            {
                int index = Random.Range(0, enemies.Count);
                target = enemies[index];
            }

            Vector2 dir;

            if (target == null || !target.gameObject.activeInHierarchy)
            {
                dir = Random.insideUnitCircle.normalized;
            }
            else
            {
                dir = (target.position - arrow.transform.position).normalized;
            }

            yield return new WaitForSeconds(0.05f);
            arrow.Launch(dir);
        }
        isWaitingForTarget = false;
    }

    private List<Transform> GetEnemies()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(owner.position, 8f, enemyLayer);

        List<Transform> result = new List<Transform>(hits.Length);

        foreach (var hit in hits)
        {
            if (hit != null && hit.gameObject.activeInHierarchy)
            {
                result.Add(hit.transform);
            }
        }

        return result;
    }
}
