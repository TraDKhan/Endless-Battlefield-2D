using System.Collections;
using UnityEngine;

public class BSummonSkill : BaseBossSkill
{
    public override string SkillID => "Summon";

    [Header("Summon")]
    [SerializeField] private GameObject summonObjectPrefab;
    [SerializeField] private int baseCount = 3;
    [SerializeField] private float radius = 2.5f;

    [Header("Phase Requirement")]
    [SerializeField] int minPhase = 2;

    [Header("Animation")]
    [SerializeField] float summonAnimDuration = 0.6f;

    // ===== STATE =====
    private int lastSummonedPhase = -1;
    private int summonTimes = 0;

    public override bool CanExecute(BossContext ctx)
    {
        if (!base.CanExecute(ctx)) return false;

        int phase = ctx.boss.CurrentPhase;

        if (phase < minPhase) return false;

        // đã summon trong phase này rồi → không cho dùng lại
        if (lastSummonedPhase == phase) return false;

        return true;
    }

    protected override IEnumerator OnExecute(BossContext ctx)
    {
        // đánh dấu đã summon ở phase hiện tại
        lastSummonedPhase = ctx.boss.CurrentPhase;
        summonTimes++;

        ctx.Movement?.Stop();
        ctx.Anim?.SetMoving(false);

        // PLAY ANIMATION
        ctx.Anim?.PlaySkill2();

        yield return new WaitForSeconds(summonAnimDuration);

        int summonCount = CalculateSummonCount();

        for (int i = 0; i < summonCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * radius;
            Vector2 spawnPos = (Vector2)ctx.boss.transform.position + offset;

            Object.Instantiate(
                summonObjectPrefab,
                spawnPos,
                Quaternion.identity
            );

            yield return new WaitForSeconds(0.15f);
        }
    }

    private int CalculateSummonCount()
    {
        return baseCount + summonTimes * 2;
    }
}