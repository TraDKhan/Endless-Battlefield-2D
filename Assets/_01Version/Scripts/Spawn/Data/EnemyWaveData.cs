using UnityEngine;

[CreateAssetMenu(menuName = "Wave/Enemy Wave")]
public class EnemyWaveData : WaveData
{
    [Header("Single Spawn")]
    public EnemySpawnData[] enemies;

    [Header("Group Spawn")]
    public bool isSpawnGroup = true;
    public EnemyGroupData[] groups;
}
