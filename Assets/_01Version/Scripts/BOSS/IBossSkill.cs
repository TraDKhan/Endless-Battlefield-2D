using System.Collections;
using UnityEngine;

public interface IBossSkill
{
    string SkillID { get; }

    bool CanExecute(BossContext ctx);
    IEnumerator Execute(BossContext ctx);
}
public class BossContext
{
    public BossController boss;
    public Transform Player => boss.player;
    public  LayerMask TargetLayer => boss.targetLayer;
    public EnemyStats Stats => boss.stats;
    public EnemyMovement Movement => boss.movement;
    public EnemyAnimationController Anim => boss.anim;
}

