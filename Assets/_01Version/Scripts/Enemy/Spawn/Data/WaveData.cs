using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Enemy Spawn/Wave Data", order = 0)]
public class WaveData : ScriptableObject
{
    public string waveName;

    [Header("Wave Type")]
    public bool isBossWave;

    [Header("Single Spawn")]
    public EnemySpawnData[] enemies;

    [Header("Group Spawn")]
    public bool isSpawnGroup = true;
    public EnemyGroupData[] groups;
}
