using System.Collections;
using UnityEngine;

public class BMeteorRainSkill : BaseBossSkill
{
    [Header("Meteor")]
    [SerializeField] GameObject meteorPrefab;
    [SerializeField] GameObject indicatorPrefab;

    [SerializeField] int meteorCount = 10;
    [SerializeField] float spawnRadius = 5f;

    [SerializeField] float spawnHeight = 8f;
    [SerializeField] float spawnDelay = 0.25f;

    protected override IEnumerator PerformSkill(BossContext ctx)
    {
        Transform player = ctx.Player;
        ctx.boss.SetCastingSkill(true);

        float distance = Vector3.Distance(ctx.boss.transform.position, player.position);
        float attackRange = ctx.Stats.attackRange;

        if (distance < attackRange)
        {
            ctx.boss.SetCastingSkill(false);
            yield break;
        }

        for (int i = 0; i < meteorCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector2 targetPos = player.position + (Vector3)offset;

            SpawnIndicator indicator = ObjectPoolManager.Instance.Spawn<SpawnIndicator>(indicatorPrefab);

            indicator.Play(targetPos, () =>
            {
                SpawnMeteor(targetPos);
            });

            CameraShake.Instance.Shake(0.2f, 0.1f);

            yield return new WaitForSeconds(spawnDelay);
        }

        ctx.boss.SetCastingSkill(false);
    }

    void SpawnMeteor(Vector3 targetPos)
    {
        Vector3 spawnPos = targetPos + Vector3.up * spawnHeight;

        Boss_Projectile_Meteor meteor = ObjectPoolManager.Instance.Spawn<Boss_Projectile_Meteor>(meteorPrefab);

        meteor.transform.position = spawnPos;
        meteor.Init(targetPos);
    }
}