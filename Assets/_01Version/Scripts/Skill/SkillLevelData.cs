using UnityEngine;

[System.Serializable]
public class SkillLevelData
{
    [Header("Combat")]
    public int damage;
    public float cooldown;
    public float duration;
    public float radius;

    [Header("Extra")]
    public int projectileCount;

    [Header("Lightning")]
    public int lightningCount;   // SỐ TIA SÉT
    public float range;
}
