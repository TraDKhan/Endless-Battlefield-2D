using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Indicator")]
    [SerializeField] private SpawnIndicator spawnIndicatorPrefab;

    [Header("Spawn Radius")]
    [SerializeField] private float minSpawnRadius = 6f;
    [SerializeField] private float maxSpawnRadius = 10f;

    [Header("Wave Settings")]
    [SerializeField] private WaveData[] allWaves;
    [SerializeField] private float spawnInterval = 0.6f;

    [Header("Events")]
    [SerializeField] private WaveEventChannel waveEventChannel;

    private int enemiesAlive;

    #region UNITY

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        StartCoroutine(WaveLoop());
    }

    #endregion

    #region WAVE LOOP

    IEnumerator WaveLoop()
    {
        for (int i = 0; i < allWaves.Length; i++)
        {
            WaveData wave = allWaves[i];

            waveEventChannel?.OnWavePreview?.Invoke(wave, i + 1);
            yield return WaitForWaveStart();

            Debug.Log($"[Spawner] Start Wave: {wave.waveName}");

            if (wave is EnemyWaveData enemyWave)
                yield return SpawnEnemyWave(enemyWave);

            else if (wave is BossWaveData bossWave)
                yield return SpawnBossWave(bossWave);

            yield return new WaitUntil(() => enemiesAlive <= 0);
            waveEventChannel?.OnWaveCleared?.Invoke();
        }

        Debug.Log("[Spawner] All waves completed!");
    }

    IEnumerator WaitForWaveStart()
    {
        bool canStart = false;

        System.Action handler = null;
        handler = () =>
        {
            waveEventChannel.OnWaveStart -= handler;
            canStart = true;
        };

        waveEventChannel.OnWaveStart += handler;
        yield return new WaitUntil(() => canStart);
    }

    #endregion

    #region ENEMY WAVE

    IEnumerator SpawnEnemyWave(EnemyWaveData wave)
    {
        if (wave.enemies != null && wave.enemies.Length > 0)
            yield return SpawnSingleEnemies(wave.enemies);

        if (wave.isSpawnGroup && wave.groups != null)
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

    IEnumerator SpawnSingleEnemies(EnemySpawnData[] enemies)
    {
        List<GameObject> spawnList = new();

        foreach (var data in enemies)
        {
            for (int i = 0; i < data.count; i++)
                spawnList.Add(data.enemyPrefab);
        }

        Shuffle(spawnList);

        foreach (GameObject prefab in spawnList)
        {
            Vector2 pos = GetSpawnPositionAroundPlayer();
            yield return SpawnWithIndicator(prefab, pos, SpawnEnemy);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnGroup(EnemyGroupData group)
    {
        Vector2 center = GetSpawnPositionAroundPlayer();

        foreach (var enemy in group.enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                Vector2 pos = center + Random.insideUnitCircle * group.groupRadius;
                StartCoroutine(
                    SpawnWithIndicator(enemy.enemyPrefab, pos, SpawnEnemy)
                );
            }
        }
    }

    #endregion

    #region BOSS WAVE (NO POOL)

    IEnumerator SpawnBossWave(BossWaveData wave)
    {
        foreach (BossSpawnData bossData in wave.bosses)
        {
            for (int i = 0; i < bossData.count; i++)
            {
                Vector2 pos = GetSpawnPositionAroundPlayer();

                yield return SpawnWithIndicator(
                    bossData.bossPrefab.gameObject,
                    pos,
                    SpawnBoss
                );

                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    void SpawnBoss(GameObject prefab, Vector2 position)
    {
        BossController boss =
            Instantiate(prefab, position, Quaternion.identity)
            .GetComponent<BossController>();

        enemiesAlive++;
    }

    #endregion

    #region CORE SPAWN

    void SpawnEnemy(GameObject prefab, Vector2 position)
    {
        EnemyController enemy =
            ObjectPoolManager.Instance.Spawn<EnemyController>(prefab);

        if (enemy == null)
        {
            Debug.LogError($"[Spawner] FAILED TO SPAWN ENEMY: {prefab.name}");
            return;
        }

        if (enemy == null) return;

        enemy.transform.SetPositionAndRotation(position, Quaternion.identity);
        enemy.transform.localScale = Vector3.one;

        enemiesAlive++;
        enemy.OnEnemyDead += HandleEnemyDead;
    }

    IEnumerator SpawnWithIndicator(
        GameObject prefab,
        Vector2 position,
        System.Action<GameObject, Vector2> spawnAction
    )
    {
        SpawnIndicator indicator =
            ObjectPoolManager.Instance.Spawn<SpawnIndicator>(
                spawnIndicatorPrefab.gameObject
            );

        bool done = false;

        indicator.Play(position, () =>
        {
            spawnAction?.Invoke(prefab, position);
            done = true;
        });

        yield return new WaitUntil(() => done);
    }

    #endregion

    #region DEAD CALLBACKS

    void HandleEnemyDead(EnemyController enemy)
    {
        enemy.OnEnemyDead -= HandleEnemyDead;
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }

    void HandleBossDead(BossController boss)
    {
        boss.OnBossDead -= HandleBossDead;
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
    }

    #endregion

    #region UTILS

    Vector2 GetSpawnPositionAroundPlayer()
    {
        float angle = Random.Range(0f, 360f);
        float dist = Random.Range(minSpawnRadius, maxSpawnRadius);

        return (Vector2)player.position +
               new Vector2(
                   Mathf.Cos(angle * Mathf.Deg2Rad),
                   Mathf.Sin(angle * Mathf.Deg2Rad)
               ) * dist;
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
