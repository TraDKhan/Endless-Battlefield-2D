using UnityEngine;

public enum UpgradeType
{
    Stat,
    Skill
}

[CreateAssetMenu(menuName = "Upgrade/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    public UpgradeType upgradeType;

    // Stat
    public StatUpgradeData statUpgradeData;

    // Skill
    public SkillUpgradeData skillUpgradeData;
}
