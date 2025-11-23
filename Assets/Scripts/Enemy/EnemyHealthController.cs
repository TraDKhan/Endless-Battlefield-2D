using UnityEngine;
using System;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int maxHealth = 20;
    public int currentHealth;

    public bool IsDead { get; private set; }

    // Sự kiện để AI, Animation, Drop Item có thể đăng ký
    public event Action OnDeath;
    public event Action<int> OnDamaged;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;   // Tránh xử lý lại khi đã chết

        currentHealth -= damage;
        OnDamaged?.Invoke(damage);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        IsDead = true;

        OnDeath?.Invoke();

        // Nếu enemy không có logic AI chết, có thể gỡ bỏ object:
        Destroy(gameObject, 1.2f); // đợi animation chết chạy xong
    }
}
