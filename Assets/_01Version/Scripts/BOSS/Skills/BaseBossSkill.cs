using System.Collections;
using UnityEngine;

public abstract class BaseBossSkill : MonoBehaviour, IBossSkill
{
    [Header("Cooldown")]
    [SerializeField] protected float cooldown = 2f;

    protected float lastUseTime = -999f;

    public abstract string SkillID { get; }

    public float Cooldown => cooldown;

    public bool IsOnCooldown => Time.time < lastUseTime + cooldown;

    public virtual bool CanExecute(BossContext ctx)
    {
        return !IsOnCooldown;
    }

    public IEnumerator Execute(BossContext ctx)
    {
        lastUseTime = Time.time;
        yield return OnExecute(ctx);
    }

    protected abstract IEnumerator OnExecute(BossContext ctx);
}
