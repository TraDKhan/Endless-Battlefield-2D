using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Dữ liệu gốc của nhân vật
/// </summary>
[CreateAssetMenu(fileName = "New Player", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Info")]
    public string playerName;

    [Header("Base Stats")]
    public List<CStatEntry> baseStats;

    public float GetBaseStat(CharacterStatType type)
    {
        foreach (var entry in baseStats)
        {
            if (entry.statType == type)
                return entry.value;
        }
        return 0f;
    }
}
