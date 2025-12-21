using System;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    private int maxHealth = 1000;
    private int currentHealth;
    private bool isEnabled;

    public event Action OnDeath;

    public void Init(int maxHP)
    {
        maxHealth = maxHP;
        currentHealth = maxHP;
    }

    public void ResetHP()
    {
        currentHealth = maxHealth;
        isEnabled = true;
    }

    public void Enable() => isEnabled = true;
    public void Disable() => isEnabled = false;

    public void TakeDamage(int damage)
    {
        if (!isEnabled) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }
    void Die()
    {
        currentHealth = 0;
        OnDeath?.Invoke();
    }
    public void OnSpawn()
    {
        currentHealth = maxHealth;
    }
    public void OnDespawn()
    {
        OnDeath = null;
    }
}
