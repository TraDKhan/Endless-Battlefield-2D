using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;
    private bool isDead;

    public bool IsDead => isDead;

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged; 

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

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        OnDeath?.Invoke();
    }
}
