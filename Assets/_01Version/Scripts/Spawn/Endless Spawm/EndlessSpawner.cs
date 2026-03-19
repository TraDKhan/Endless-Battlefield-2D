using System.Collections;
using UnityEngine;

public class EndlessSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private EnemyDirector enemyDirector;

    [Header("Spawn Settings")]
    [SerializeField] private int maxEnemiesAlive = 150;

    [Header("Difficulty Curve")]
    [SerializeField] private AnimationCurve difficultyCurve;

    [Header("Budget")]
    [SerializeField] private float baseBudgetPerSecond = 1f;

    [Header("Burst")]
    [SerializeField] private float burstInterval = 30f;
    [SerializeField] private int burstCount = 10;

    private float time;
    private float spawnBudget;
    private int enemiesAlive;

    private float nextBurstTime = 20f;
    private float nextBossTime = 60f;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        StartCoroutine(MainLoop());
    }

    IEnumerator MainLoop()
    {
        while (true)
        {
            time += Time.deltaTime;

            float difficulty = difficultyCurve.Evaluate(time / 600f); // normalize 0→10 phút

            HandleBudget(difficulty);
            HandleSpawning(difficulty);
            HandleBurst(difficulty);
            HandleBoss();

            yield return null;
        }
    }

    #region Budget System

    void HandleBudget(float difficulty)
    {
        float intensity = (float)enemiesAlive / maxEnemiesAlive;

        // nếu đang quá đông → giảm spawn
        float modifier = Mathf.Lerp(1f, 0.2f, intensity);

        spawnBudget += baseBudgetPerSecond * difficulty * modifier * Time.deltaTime;
    }

    #endregion

    #region Spawn

    void HandleSpawning(float difficulty)
    {
        if (enemiesAlive >= maxEnemiesAlive)
            return;

        while (spawnBudget >= 1f)
        {
            spawnBudget -= 1f;

            SpawnEnemy(difficulty);
        }
    }

    void SpawnEnemy(float difficulty)
    {
        GameObject prefab = enemyDirector.GetEnemy(time, difficulty);

        Vector2 pos = GetSpawnPosition();

        EnemyController enemy = ObjectPoolManager.Instance.Spawn<EnemyController>(prefab);

        enemy.transform.SetPositionAndRotation(pos, Quaternion.identity);
        enemy.transform.localScale = Vector3.one;

        enemiesAlive++;
        enemy.OnEnemyDead += HandleEnemyDead;
    }

    #endregion

    #region Burst (bao vây)

    void HandleBurst(float difficulty)
    {
        if (time < nextBurstTime) return;

        int count = Mathf.RoundToInt(burstCount * difficulty);

        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(difficulty);
        }

        nextBurstTime += burstInterval;
    }

    #endregion

    #region Boss

    void HandleBoss()
    {
        if (time < nextBossTime) return;

        BossController boss = enemyDirector.GetBoss(time);

        Vector2 pos = GetSpawnPosition();

        BossController instance = Instantiate(boss, pos, Quaternion.identity);

        enemiesAlive++;
        instance.OnBossDead += HandleBossDead;

        nextBossTime += 60f;
    }

    #endregion

    #region Utils

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

    Vector2 GetSpawnPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float dist = Random.Range(8f, 12f);

        return (Vector2)player.position +
               new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;
    }

    #endregion
}