using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade/Skill Upgrade Data")]
public class SkillUpgradeData : ScriptableObject
{
    public string skillName;
    [TextArea] public string description;
    public Sprite icon;

    public GameObject skillPrefab;

    [Header("Levels")]
    public List<SkillLevelData> levels;

    public int MaxLevel => levels.Count;

    public SkillLevelData GetLevelData(int level)
    {
        level = Mathf.Clamp(level, 1, MaxLevel);
        return levels[level - 1];
    }
}
