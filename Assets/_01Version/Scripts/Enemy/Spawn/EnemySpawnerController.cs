using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private SpawnIndicator spawnIndicatorPrefab;

    [Header("Spawn Radius")]
    [SerializeField] private float minSpawnRadius = 6f;
    [SerializeField] private float maxSpawnRadius = 10f;

    [Header("Wave Settings")]
    [SerializeField] private WaveData[] allWaves;
    [SerializeField] private float spawnInterval = 0.6f;
    [SerializeField] private float timeBetweenWaves = 3f;

    private int enemiesAlive = 0;

    void Start()
    {
        StartCoroutine(WaveLoop());
    }

    #region Wave Loop
    IEnumerator WaveLoop()
    {
        foreach (WaveData wave in allWaves)
        {
            Debug.Log($"[Spawner] Start Wave: {wave.waveName}");

            yield return StartCoroutine(SpawnWave(wave));

            yield return new WaitUntil(() => enemiesAlive <= 0);

            Debug.Log($"[Spawner] Wave Cleared: {wave.waveName}");
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("[Spawner] All waves completed!");
    }
    #endregion

    #region Wave Spawn
    IEnumerator SpawnWave(WaveData wave)
    {
        // 1️⃣ Spawn đơn (logic cũ)
        if (wave.enemies != null && wave.enemies.Length > 0)
        {
            yield return StartCoroutine(SpawnSingleEnemies(wave.enemies));
        }

        // 2️⃣ Spawn theo đàn (nếu có)
        if (wave.isSpawnGroup && wave.groups != null && wave.groups.Length > 0)
        {
            foreach (EnemyGroupData group in wave.groups)
            {
                for (int i = 0; i < group.groupCount; i++)
                {
                    SpawnGroup(group);
                    yield return new WaitForSeconds(spawnInterval);
                }
            }
        }
    }
    #endregion

    #region Single Spawn (Random)
    IEnumerator SpawnSingleEnemies(EnemySpawnData[] enemies)
    {
        List<GameObject> spawnList = new();

        foreach (EnemySpawnData data in enemies)
        {
            for (int i = 0; i < data.count; i++)
                spawnList.Add(data.enemyPrefab);
        }

        Shuffle(spawnList);

        foreach (GameObject prefab in spawnList)
        {
            Vector2 pos = GetSpawnPositionAroundPlayer();
            yield return StartCoroutine(SpawnWithIndicator(prefab, pos));
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    #endregion

    #region Group Spawn
    void SpawnGroup(EnemyGroupData group)
    {
        Vector2 center = GetSpawnPositionAroundPlayer();

        foreach (EnemySpawnData enemy in group.enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                Vector2 offset = Random.insideUnitCircle * group.groupRadius;
                Vector2 spawnPos = center + offset;

                StartCoroutine(SpawnWithIndicator(enemy.enemyPrefab, spawnPos));
            }
        }
    }
    #endregion

    #region Spawn With Indicator
    IEnumerator SpawnWithIndicator(GameObject prefab, Vector2 position)
    {
        SpawnIndicator indicator = Instantiate(spawnIndicatorPrefab);
        yield return indicator.Play(position, () =>
        {
            SpawnEnemy(prefab, position);
        });

        Destroy(indicator.gameObject);
    }
    #endregion

    #region Core Spawn
    void SpawnEnemy(GameObject prefab, Vector2 position)
    {
        GameObject enemy = Instantiate(prefab, position, Quaternion.identity);
        enemiesAlive++;

        EnemyHealthController health = enemy.GetComponent<EnemyHealthController>();
        if (health != null)
        {
            health.OnDeath += OnEnemyDead;
        }
    }

    void OnEnemyDead()
    {
        enemiesAlive--;
    }
    #endregion

    #region Utils
    Vector2 GetSpawnPositionAroundPlayer()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(minSpawnRadius, maxSpawnRadius);

        return (Vector2)player.position +
               new Vector2(
                   Mathf.Cos(angle * Mathf.Deg2Rad),
                   Mathf.Sin(angle * Mathf.Deg2Rad)
               ) * distance;
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    #endregion
}
