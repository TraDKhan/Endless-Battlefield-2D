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

    bool spawnTriggered;
    bool skillFinished;
    BossContext cachedCtx;
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

    protected override IEnumerator PerformSkill(BossContext ctx)
    {
        cachedCtx = ctx;

        spawnTriggered = false;
        skillFinished = false;

        lastSummonedPhase = ctx.boss.CurrentPhase;
        summonTimes++;

        ctx.boss.SetCastingSkill(true);

        // PLAY ANIMATION
        ctx.Anim?.PlaySkill2();

        float timer = 0f;

        while (!skillFinished && timer < summonAnimDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    public override void OnAnimationEvent(BossAnimEvent animEvent)
    {
        switch (animEvent)
        {
            case BossAnimEvent.Spawn:
                SpawnSummons();
                break;

            case BossAnimEvent.End:
                skillFinished = true;
                cachedCtx.boss.SetCastingSkill(false);
                break;
        }
    }

    void SpawnSummons()
    {
        if (spawnTriggered || cachedCtx == null)
            return;

        spawnTriggered = true;

        int summonCount = CalculateSummonCount();

        StartCoroutine(SpawnRoutine(summonCount));
    }

    IEnumerator SpawnRoutine(int summonCount)
    {
        for (int i = 0; i < summonCount; i++)
        {
            Vector2 offset =
                Random.insideUnitCircle.normalized * radius;

            Vector2 spawnPos = (Vector2)cachedCtx.boss.transform.position + offset;

            //todo: chuyển thành object pool sau
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