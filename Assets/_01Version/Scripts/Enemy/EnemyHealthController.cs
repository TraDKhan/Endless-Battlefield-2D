using System;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;
    private bool isDead;

    public bool IsDead => isDead;

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged; // current, max

    // =========================
    // UNITY
    // =========================
    private void Awake()
    {
        ResetHealth();
    }

    // =========================
    // PUBLIC API
    // =========================
    public void Init(int maxHP)
    {
        maxHealth = maxHP;
        ResetHealth();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    // =========================
    // DAMAGE
    // =========================
    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
            return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth == 0)
            Die();
    }

    // =========================
    // INTERNAL
    // =========================
    private void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();
    }
}
