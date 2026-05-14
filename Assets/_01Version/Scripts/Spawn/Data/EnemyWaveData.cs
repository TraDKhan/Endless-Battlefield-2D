using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Wave/Enemy Wave")]
public class EnemyWaveData : WaveData
{
    [Header("Enemy List")]
    public EnemySpawnData[] enemies;

    public float spawnWaitTime = 5f;
    [Header("Cluster Settings")]
    [Range(0, 1)]
    public float clusterChance = 0.2f;

    public int minClusterSize = 2;

    public int maxClusterSize = 5;

    public float clusterRadius = 0.5f;
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int count;
}
