public interface IPlayerAttackController
{
    void Attack(IDamageable target);
    bool CanAttack();
}
