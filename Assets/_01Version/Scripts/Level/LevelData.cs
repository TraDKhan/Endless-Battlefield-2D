using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelIndex;

    [Header("Gameplay")]
    public int difficulty;
    public int targetScore;

    [Header("Rewards")]
    public List<RewardData> rewards;

    public Sprite levelIcon;
}

[CreateAssetMenu(fileName = "LevelDatabase", menuName = "Game/Level Database")]
public class LevelDatabase : ScriptableObject
{
    public LevelData[] levels;

    public LevelData GetLevel(int index)
    {
        return levels[index - 1];
    }

    public int MaxLevel => levels.Length;
}

public enum RewardType
{
    Coin,
    Gem,
    Energy
}

[System.Serializable]
public class RewardData
{
    public RewardType type;
    public int amount;
}