using System.Collections;
using UnityEngine;

public class SummonHellfireSkill : MonoBehaviour//, IBossSkill
{
    public string SkillID => "SummonHellfire";

    [Header("Summon")]
    [SerializeField] private GameObject hellfirePrefab;
    [SerializeField] private int baseCount = 3;
    [SerializeField] private float radius = 2.5f;

    [Header("Cooldown")]
    [SerializeField] private float cooldown = 15f;

    [Header("Phase Requirement")]
    [SerializeField] private int minPhase = 2;

    private float lastCastTime = -999f;

    public bool CanExecute(BossContext ctx)
    {
        if (ctx.boss.CurrentPhase < minPhase)
            return false;

        if (Time.time < lastCastTime + cooldown)
            return false;

        return true;
    }

    public IEnumerator Execute(BossContext ctx)
    {
        lastCastTime = Time.time;

        int summonCount = CalculateSummonCount(ctx.boss.CurrentPhase);

        ctx.Movement.Stop();
        ctx.Anim.SetMoving(false);

        // ===== PLAY SKILL2 ANIMATION =====
        ctx.Anim.PlaySkill2();
        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < summonCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle.normalized * radius;
            Vector2 spawnPos = (Vector2)ctx.boss.transform.position + offset;           

            Instantiate(hellfirePrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(0.15f);
        }
    }

    private int CalculateSummonCount(int phase)
    {
        return baseCount + phase * 2;
    }
}
