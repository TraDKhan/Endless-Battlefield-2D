using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMeteorShootSkill : BaseBossSkill
{
    [Header("Square Size")]
    [SerializeField] Vector2 size = new Vector2(5, 5);

    [Header("Spawn")]
    [SerializeField] int pointCount = 10;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] List<GameObject> meteorPrefabs;

    [Header("Warning")]
    [SerializeField] GameObject warningPrefab;
    [SerializeField] float warningDuration = 0.8f;

    List<Vector3> startPoints = new();
    List<Vector3> endPoints = new();

    protected override IEnumerator PerformSkill(BossContext ctx)
    {
        Transform player = ctx.Player;
        Transform boss = transform;

        if (boss.position.y + 2f <= player.position.y)
            yield break;

        ctx.boss.SetCastingSkill(true);

        GeneratePoints(player, boss);

        // Spawn warning tại target zone
        for (int i = 0; i < endPoints.Count; i++)
        {
            var warning = ObjectPoolManager.Instance.Spawn<VFX_Warning_Circle>(warningPrefab);
            warning.Play(endPoints[i]);

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(warningDuration);
        
        // Spawn meteor
        for (int i = 0; i < startPoints.Count; i++)
        {
            GameObject perfab = meteorPrefabs[Random.Range(0, meteorPrefabs.Count)];

            MeteorOrb meteor = ObjectPoolManager.Instance.Spawn<MeteorOrb>(perfab);

            meteor.transform.position = startPoints[i];
            meteor.Init(endPoints[i], moveSpeed);
            CameraShake.Instance.Shake(0.2f, 0.1f);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(2f);
        ctx.boss.SetCastingSkill(false);
    }

    Vector3 startCenter;
    Vector3 endCenter;

    void GeneratePoints(Transform player, Transform boss)
    {
        startPoints.Clear();
        endPoints.Clear();

        startCenter = boss.position + Vector3.up * 3f;   // vùng spawn ở boss
        endCenter = player.position;   // vùng target ở player

        for (int i = 0; i < pointCount; i++)
        {
            float x = Random.Range(-size.x / 2, size.x / 2);
            float y = Random.Range(-size.y / 2, size.y / 2);

            Vector3 offset = new Vector3(x, y, 0);

            startPoints.Add(startCenter + offset);
            endPoints.Add(endCenter + offset);
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(startCenter, size);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(endCenter, size);

        Gizmos.color = Color.red;

        for (int i = 0; i < startPoints.Count; i++)
        {
            Gizmos.DrawSphere(startPoints[i], 0.15f);
            Gizmos.DrawSphere(endPoints[i], 0.15f);
            Gizmos.DrawLine(startPoints[i], endPoints[i]);
        }
    }
}