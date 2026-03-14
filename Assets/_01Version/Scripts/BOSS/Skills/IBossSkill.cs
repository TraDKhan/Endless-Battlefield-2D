using System.Collections;

public interface IBossSkill
{
    float Cooldown { get; }
    bool IsOnCooldown { get; }

    bool CanUse(BossContext ctx);
    IEnumerator Execute(BossContext ctx);
    void OnAnimationEvent(BossAnimEvent animEvent);
}