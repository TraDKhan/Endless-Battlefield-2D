using UnityEngine;

public enum UpgradeType
{
    Stat,
    Skill
}

public enum StatType
{
    MaxHealth,
    Damage,
    MoveSpeed,
    AttackSpeed,
    Defense
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Game/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string description;
    public Sprite icon;

    public UpgradeType upgradeType;

    // Nếu là Stat
    public StatType statType;
    public float statValue;

    // Nếu là Skill
    public GameObject skillPrefab;
}
