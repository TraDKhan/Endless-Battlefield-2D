using UnityEngine;

[System.Serializable]
public class BossSpawnData
{
    public BossController bossPrefab;
    public int count = 1;
}

[CreateAssetMenu(menuName = "Wave/Boss Wave")]
public class BossWaveData : WaveData
{
    public BossSpawnData[] bosses;
}
