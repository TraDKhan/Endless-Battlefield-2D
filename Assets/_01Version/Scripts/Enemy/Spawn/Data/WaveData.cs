using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "Enemy Spawn/Wave Data", order = 0)]
public class WaveData : ScriptableObject
{
    public string waveName;

    [Header("Single Spawn")]
    public EnemySpawnData[] enemies;

    [Header("Group Spawn")]
    public EnemyGroupData[] groups;

    [Tooltip("Wave này có được spawn không")]
    public bool isSpawnGroup = true;
}
