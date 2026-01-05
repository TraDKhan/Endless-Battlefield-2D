public interface IEnemyAttack
{
    bool CanAttack { get; }
    float Cooldown { get; }

    void StartAttack();
    void UpdateAttack();
    void StopAttack();
}
