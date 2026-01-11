using System.Collections;
using UnityEngine;

public class BossMeleeSkill : MonoBehaviour, IBossSkill
{
    public string SkillID => "Melee";

    private int empoweredHits;
    private float lastAttackTime = -999f;

    public void EmpowerNextHits(int count)
    {
        empoweredHits += count;
    }

    public bool CanExecute(BossContext ctx)
    {
        if (ctx.Player == null)
            return false;

        // cooldown đánh thường
        if (Time.time < lastAttackTime + ctx.Stats.attackCooldown)
            return false;

        float dist = Vector2.Distance(
            ctx.boss.transform.position,
            ctx.Player.position
        );

        return dist <= ctx.Stats.attackRange;
    }

    public IEnumerator Execute(BossContext ctx)
    {
        lastAttackTime = Time.time;

        ctx.Movement.Stop();
        ctx.Anim.PlayAttack();

        PlayDirectionalAttack(ctx);

        // ⏳ đợi animation hit frame
        yield return new WaitForSeconds(0.6f);

        // xử lý damage
        int damage = ctx.Stats.damage;

        if (empoweredHits > 0)
        {
            damage *= 2; // ví dụ cường hóa
            empoweredHits--;
        }

        DealDamage(ctx, damage);
    }
    private void DealDamage(BossContext ctx, int damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            ctx.boss.transform.position,
            ctx.Stats.attackRange,
            ctx.TargetLayer
        );

        foreach (var hit in hits)
        {
            hit.GetComponent<IDamageable>()?.TakeDamage(damage);
        }
    }

    private void PlayDirectionalAttack(BossContext ctx)
    {
        float deltaY = ctx.Player.position.y - ctx.boss.transform.position.y;

        const float verticalThreshold = 0.35f;

        if (deltaY > verticalThreshold)
        {
            ctx.Anim.PlayAttackUp();
        }
        else if (deltaY < -verticalThreshold)
        {
            ctx.Anim.PlayAttackDown();
        }
        else
        {
            ctx.Anim.PlayAttack();
        }
    }
}
