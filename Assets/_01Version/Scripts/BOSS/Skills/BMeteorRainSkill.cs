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

        for (int i = 0; i < meteorCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector2 targetPos = player.position + (Vector3)offset;

            SpawnIndicator indicator = ObjectPoolManager.Instance.Spawn<SpawnIndicator>(indicatorPrefab);

            indicator.Play(targetPos, () =>
            {
                SpawnMeteor(targetPos);
            });

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    void SpawnMeteor(Vector3 playerPos)
    {
        // chọn điểm rơi trên mặt đất
        Vector2 offset = Random.insideUnitCircle * spawnRadius;

        Vector3 targetPos = new Vector3(
            playerPos.x + offset.x,
            playerPos.y + offset.y,
            0
        );

        // spawn trên trời ngay phía trên điểm rơi
        Vector3 spawnPos = targetPos + Vector3.up * spawnHeight;

        GameObject meteor = Instantiate(meteorPrefab, spawnPos, Quaternion.identity);

        meteor.GetComponent<Boss_Projectile_Meteor>().Init(targetPos);
    }
}