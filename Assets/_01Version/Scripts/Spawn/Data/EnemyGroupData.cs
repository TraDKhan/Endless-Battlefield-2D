using UnityEngine;

[System.Serializable]
public class EnemyGroupData
{
    [Tooltip("Danh sách quái trong đàn")]
    public EnemySpawnData[] enemies;

    [Tooltip("Số đàn được spawn trong wave")]
    public int groupCount = 1;

    [Tooltip("Khoảng cách giữa các con trong đàn")]
    public float groupRadius = 1.5f;
}
