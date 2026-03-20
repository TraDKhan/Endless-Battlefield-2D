using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Dữ liệu gốc của nhân vật
/// </summary>
[CreateAssetMenu(fileName = "New Player", menuName = "Player/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Info")]
    public int id;
    public Sprite icon;
    public string playerName;

    [Header("Prefab")]
    public GameObject prefab;
    public GameObject weaponPrefab;

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
