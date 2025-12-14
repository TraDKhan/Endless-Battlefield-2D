using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, ISkill
{
    protected CharacterStats ownerStats;

    [SerializeField] protected SkillUpgradeData upgradeData;
    public SkillUpgradeData UpgradeData => upgradeData;

    protected int level = 0;
    public int Level => level;

    public virtual void Init(CharacterStats stats)
    {
        ownerStats = stats;
    }

    public virtual void OnUnlock()
    {
        level = 1;
        ApplyLevelData();
    }

    public virtual void OnLevelUp()
    {
        if (level >= upgradeData.MaxLevel) return;

        level++;
        ApplyLevelData();
    }

    protected abstract void ApplyLevelData();
}