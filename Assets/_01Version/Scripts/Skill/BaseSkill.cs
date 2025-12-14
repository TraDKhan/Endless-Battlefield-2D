using UnityEngine;

public abstract class BaseSkill : MonoBehaviour, ISkill
{
    protected CharacterStats ownerStats;

    [Header("Skill Level")]
    [SerializeField] protected int level = 1;
    [SerializeField] protected int maxLevel = 5;

    public virtual void Init(CharacterStats stats)
    {
        ownerStats = stats;
    }

    public virtual void OnUnlock()
    {
        level = 1;
        gameObject.SetActive(true);
    }

    public virtual void OnLevelUp()
    {
        if (level < maxLevel)
            level++;

        ApplyLevelScaling();
    }

    protected abstract void ApplyLevelScaling();
}
