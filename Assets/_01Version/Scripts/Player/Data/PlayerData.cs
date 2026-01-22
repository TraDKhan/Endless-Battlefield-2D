using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Info")]
    public string playerName;

    [Header("Base Stats")]
    public List<StatEntry> baseStats;

    // =========================
    // BACKWARD COMPATIBILITY
    // =========================
    public float GetBaseStat(StatType type)
    {
        foreach (var entry in baseStats)
        {
            if (entry.statType == type)
                return entry.value;
        }

        return 0f;
    }
}
