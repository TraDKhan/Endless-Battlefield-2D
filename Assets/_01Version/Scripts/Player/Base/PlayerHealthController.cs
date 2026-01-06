using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    static public PlayerHealthController Instance;
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public bool isDead;

    public event Action<int, int> OnHealthChanged;    
    public event Action OnDeath;

    public bool IsDead => isDead;
    private bool initialized = false;

    private void Awake()
    {
        Instance = this;
    }
    public void SetMaxHealth(int newMaxHealth)
    {
        CurrentHealth += newMaxHealth - MaxHealth;
        MaxHealth = newMaxHealth;

        // Lần đầu thiết lập — full HP
        if (!initialized)
        {
            CurrentHealth = MaxHealth;
            initialized = true;
        }
        else
        {
            // Nếu max hp giảm mà current > max → hạ xuống
            if (CurrentHealth > MaxHealth)
                CurrentHealth = MaxHealth;
        }
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
    public void TakeDamage(int damage)
    {
        if (damage <= 0) return;

        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth == 0)
            Die();
    }
    [ContextMenu("Attack")]
    public void TakeDamage1()
    {
        CurrentHealth -= 120;
        if (CurrentHealth < 0) CurrentHealth = 0;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);

        if (CurrentHealth == 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0) return;

        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
            CurrentHealth = MaxHealth;

        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
