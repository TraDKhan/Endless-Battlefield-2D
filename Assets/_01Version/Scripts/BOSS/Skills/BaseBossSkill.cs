using System.Collections;
using UnityEngine;

public abstract class BaseBossSkill : MonoBehaviour, IBossSkill
{
    [SerializeField] float cooldown = 5f;

    float cooldownTimer;

    public float Cooldown => cooldown;
    public bool IsOnCooldown => cooldownTimer > 0;

    protected virtual void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
    }

    public bool CanUse(BossContext ctx)
    {
        if (IsOnCooldown)
            return false;

        return CheckCondition(ctx);
    }

    protected virtual bool CheckCondition(BossContext ctx)
    {
        return true;
    }

    public IEnumerator Execute(BossContext ctx)
    {
        cooldownTimer = cooldown;
        yield return PerformSkill(ctx);
    }

    protected abstract IEnumerator PerformSkill(BossContext ctx);
}
