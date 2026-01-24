using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, ISkill
{
    [SerializeField] protected SkillUpgradeData upgradeData;

    protected Transform owner;
    protected SkillStats skillStats;

    public int Level { get; private set; }
    public SkillUpgradeData UpgradeData => upgradeData;

    // =========================
    // INIT
    // =========================
    public virtual void Init(Transform ownerTransform)
    {
        owner = ownerTransform;
        skillStats = new SkillStats();

        transform.SetParent(owner);
        transform.localPosition = Vector3.zero;
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

    protected virtual void ApplyLevelData()
    {
        var data = upgradeData.GetLevelData(Level);
        skillStats.ApplySkillData(data);
        OnStatsApplied();
    }

    protected abstract void OnStatsApplied();
}
