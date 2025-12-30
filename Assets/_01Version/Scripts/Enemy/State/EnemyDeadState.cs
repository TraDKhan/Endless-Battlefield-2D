public class EnemyDeadState : IEnemyState
{
    EnemyController enemy;

    public EnemyDeadState(EnemyController enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        //enemy.movement.Stop();
        enemy.gameObject.SetActive(false);
    }

    public void Update() { }
    public void FixedUpdate() { }
    public void Exit() { }
}
