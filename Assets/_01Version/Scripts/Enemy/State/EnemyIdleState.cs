public class EnemyIdleState : IEnemyState
{
    EnemyController enemy;

    public EnemyIdleState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter() 
    {
        enemy.anim.SetMoving(false);
    }

    public void Update()
    {
                  
    }

    public void FixedUpdate() { }

    public void Exit() { }
}