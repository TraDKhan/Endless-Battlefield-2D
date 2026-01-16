using UnityEngine;
using System.Collections;

public abstract class BaseBossSkill : MonoBehaviour//, IBossSkill
{
    [Header("Skill Type")]
    [SerializeField] protected BossSkillType skillType;

    [Header("Cooldown")]
    [SerializeField] protected float cooldown = 2f;

    protected float lastUseTime = -999f;

    public abstract string SkillID { get; }
    public BossSkillType SkillType => skillType;
    public bool IsOnCooldown => Time.time < lastUseTime + cooldown;

    public virtual bool CanExecute(BossContext ctx)
    {
        if (skillType == BossSkillType.Special)
            return !IsOnCooldown;

        return true;
    }

    public IEnumerator Execute(BossContext ctx)
    {
        if (skillType == BossSkillType.Special)
            lastUseTime = Time.time;

        yield return OnExecute(ctx);
    }

    protected abstract IEnumerator OnExecute(BossContext ctx);
}
