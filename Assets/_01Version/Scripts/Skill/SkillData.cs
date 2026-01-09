using UnityEngine;

[System.Serializable]
public class SkillData
{
    [Header("Base")]
    public int damage;
    public float cooldown;
    public float duration;
    public float radius;

    [Header("Lightning")]
    public int lightningCount;   // SỐ TIA SÉT
}
