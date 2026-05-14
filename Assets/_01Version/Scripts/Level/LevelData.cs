using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public int levelIndex;

    [Header("Rewards")]
    public List<RewardData> rewards;

    public Sprite levelIcon;
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
    public Sprite icon;
    public Sprite backgroundIcon;
    public bool isSpecial;
    public int amount;
}