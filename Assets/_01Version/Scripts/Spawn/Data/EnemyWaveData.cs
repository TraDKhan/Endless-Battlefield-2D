using UnityEngine;

[CreateAssetMenu(menuName = "Wave/Enemy Wave")]
public class EnemyWaveData : WaveData
{
    [Header("Enemy List")]
    public EnemySpawnData[] enemies;

    [Header("Cluster Settings")]
    [Range(0, 1)]
    [Tooltip("Tỉ lệ xuất hiện cụm quái (0 = luôn lẻ, 1 = luôn cụm)")]
    public float clusterChance = 0.2f;

    [Tooltip("Số lượng quái tối thiểu trong 1 cụm")]
    public int minClusterSize = 2;

    [Tooltip("Số lượng quái tối đa trong 1 cụm")]
    public int maxClusterSize = 5;

    [Tooltip("Bán kính tản ra của các quái trong cụm")]
    public float clusterRadius = 0.5f;
}

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int count;
}
