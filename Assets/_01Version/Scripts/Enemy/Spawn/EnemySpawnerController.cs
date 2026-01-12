using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Prefabs (Registered In Pool)")]
    [SerializeField] private SpawnIndicator spawnIndicatorPrefab;

    [Header("Spawn Radius")]
    [SerializeField] private float minSpawnRadius = 6f;
    [SerializeField] private float maxSpawnRadius = 10f;

    [Header("Wave Settings")]
    [SerializeField] private WaveData[] allWaves;
    [SerializeField] private float spawnInterval = 0.6f;

    [Header("Events")]
    [SerializeField] private WaveEventChannel waveEventChannel;

    private int enemiesAlive = 0;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("Player not found");
        }

        StartCoroutine(WaveLoop());
    }

    #region Wave Loop
    IEnumerator WaveLoop()
    {
        for (int i = 0; i < allWaves.Length; i++)
        {
            WaveData wave = allWaves[i];

            // 🔔 Báo UI preview
            waveEventChannel?.OnWavePreview?.Invoke(wave, i + 1);

            // ⏳ ĐỢI UI báo bắt đầu
            bool canStart = false;

            System.Action startHandler = null;
            startHandler = () =>
            {
                waveEventChannel.OnWaveStart -= startHandler;
                canStart = true;
            };

            waveEventChannel.OnWaveStart += startHandler;

            yield return new WaitUntil(() => canStart);

            Debug.Log($"[Spawner] Start Wave: {wave.waveName}");

            yield return StartCoroutine(SpawnWave(wave));
            yield return new WaitUntil(() => enemiesAlive <= 0);

            waveEventChannel?.OnWaveCleared?.Invoke();
        }

        Debug.Log("[Spawner] All waves completed!");
    }
    #endregion

    #region Wave Spawn
    IEnumerator SpawnWave(WaveData wave)
    {
        // 🔹 Spawn đơn
        if (wave.enemies != null && wave.enemies.Length > 0)
        {
            yield return StartCoroutine(SpawnSingleEnemies(wave.enemies));
        }

        // 🔹 Spawn theo đàn
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
                spawnList.Add(data.enemyPrefab); // lấy prefab riêng
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
                Vector2 pos = center + offset;

                StartCoroutine(SpawnWithIndicator(enemy.enemyPrefab, pos));
            }
        }
    }
    #endregion

    #region Spawn With Indicator (POOL)
    IEnumerator SpawnWithIndicator(GameObject enemyPrefab, Vector2 position)
    {
        SpawnIndicator indicator =
            ObjectPoolManager.Instance.Spawn<SpawnIndicator>(spawnIndicatorPrefab.gameObject);

        if (indicator == null)
        {
            Debug.LogError("SpawnIndicator pool missing!");
            yield break;
        }

        bool done = false;

        indicator.Play(position, () =>
        {
            SpawnEnemy(enemyPrefab, position);
            done = true;
        });

        // đợi indicator xong (nó sẽ tự despawn)
        yield return new WaitUntil(() => done);
    }
    #endregion

    #region Core Spawn (POOL)
    void SpawnEnemy(GameObject prefab, Vector2 position)
    {
        EnemyBase enemy =
            ObjectPoolManager.Instance.Spawn<EnemyBase>(prefab);

        if (enemy == null) return;

        enemy.transform.position = position;
        enemy.transform.rotation = Quaternion.identity;
        enemy.transform.localScale = Vector3.one;

        enemiesAlive++;

        EnemyHealthController health = enemy.GetComponent<EnemyHealthController>();
        if (health != null)
        {
            System.Action deathHandler = null;
            deathHandler = () =>
            {
                health.OnDeath -= deathHandler;
                OnEnemyDead(enemy);
            };
            health.OnDeath += deathHandler;
        }
    }

    void OnEnemyDead(EnemyBase enemy)
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
        ObjectPoolManager.Instance.Despawn(enemy);
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