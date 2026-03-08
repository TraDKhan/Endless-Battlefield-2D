public interface IDamageable
{
    void TakeDamage(int damage, bool isCirt = false);
    bool IsDead { get; }
}
