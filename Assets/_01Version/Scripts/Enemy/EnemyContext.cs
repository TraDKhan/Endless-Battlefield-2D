using UnityEngine;

public class EnemyContext
{
    public EnemyController Controller { get; }
    public EnemyBase Base { get; }
    public EnemyStats Stats { get; }
    public EnemyMovement Movement { get; }
    public EnemyAnimationController Anim { get; }
    public Transform Target { get; }
    public LayerMask TargetLayer { get; }

    public EnemyContext(EnemyController controller)
    {
        Controller = controller;
        Base = controller.enemyBase;
        Stats = controller.stats;
        Movement = controller.movement;
        Anim = controller.anim;
        Target = controller.player;
        TargetLayer = controller.targetLayer;
    }
}
