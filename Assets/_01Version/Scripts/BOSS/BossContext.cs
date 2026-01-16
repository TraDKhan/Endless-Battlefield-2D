using UnityEngine;

public class BossContext
{
    public BossController boss;
    public Transform Player => boss.player;
    public LayerMask TargetLayer => boss.targetLayer;
    public EnemyStats Stats => boss.stats;
    public EnemyMovement Movement => boss.movement;
    public EnemyAnimationController Anim => boss.anim;
}