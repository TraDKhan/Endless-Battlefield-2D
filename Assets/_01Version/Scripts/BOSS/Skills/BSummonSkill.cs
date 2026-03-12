using System.Collections;
using UnityEngine;

public class BSummonSkill : BaseBossSkill
{
    [Header("Summon")]
    [SerializeField] GameObject summonObjectPrefab;
    [SerializeField] int baseCount = 3;
    [SerializeField] float radius = 2.5f;

    [Header("Phase Requirement")]
    [SerializeField] int minPhase = 2;

    [Header("Animation")]
    [SerializeField] float summonAnimDuration = 0.6f;

    // ===== STATE =====
    int lastSummonedPhase = -1;
    int summonTimes = 0;

    // =========================
    // CONDITION
    // =========================
    protected override bool CheckCondition(BossContext ctx)
    {
        if (ctx.boss == null)
            return false;

        int phase = ctx.boss.CurrentPhase;

        if (phase < minPhase)
            return false;

        // đã summon trong phase này rồi → không cho dùng lại
        if (lastSummonedPhase == phase)
            return false;

        return true;
    }

    // =========================
    // EXECUTE
    // =========================
    protected override IEnumerator PerformSkill(BossContext ctx)
    {
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
            Vector2 spawnPos =
                (Vector2)ctx.boss.transform.position + offset;

            Instantiate(
                summonObjectPrefab,
                spawnPos,
                Quaternion.identity
            );

            yield return new WaitForSeconds(0.15f);
        }
    }

    // =========================
    int CalculateSummonCount()
    {
        return baseCount + summonTimes * 2;
    }
}