using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, ISkill
{
    [SerializeField] protected SkillUpgradeData upgradeData;

    public SkillUpgradeData UpgradeData => upgradeData;
    public int Level { get; protected set; }

    protected Transform owner;
    protected CharacterStats stats;

    // =========================
    // INIT
    // =========================
    public virtual void Init(Transform ownerTransform, CharacterStats characterStats)
    {
        owner = ownerTransform;
        stats = characterStats;
    }

    // =========================
    // LIFECYCLE
    // =========================
    public virtual void OnUnlock()
    {
        Level = 1;
        ApplyLevelData();
    }

    public virtual void OnLevelUp()
    {
        Level++;
        ApplyLevelData();
    }

    // =========================
    // CORE
    // =========================
    protected abstract void ApplyLevelData();
}
