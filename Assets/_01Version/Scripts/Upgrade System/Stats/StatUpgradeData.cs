using System.Collections.Generic;
using UnityEngine;
public enum StatType
{
    MaxHealth,
    Damage,
    MoveSpeed,
    AttackSpeed,
}

[CreateAssetMenu(menuName = "Upgrade/Stat Upgrade Data")]
public class StatUpgradeData : ScriptableObject
{
    public StatType statType;

    [TextArea] public string description;
    public Sprite icon;

    public List<StatUpgradeLevelData> levels;

    public int MaxLevel => levels.Count;

    public float GetValue(int level)
    {
        level = Mathf.Clamp(level, 1, MaxLevel);
        return levels[level - 1].value;
    }
}
