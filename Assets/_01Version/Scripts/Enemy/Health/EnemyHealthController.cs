using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    private int maxHealth;
    private int currentHealth;
    private bool isDead;

    public bool IsDead => isDead;

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged; 

    public void Init(int maxHP)
    {
        Debug.Log(maxHP);
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

        Debug.LogWarning("Nhận " + damage + "ST" + "Còn " + currentHealth);
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        PopupController.Instance.ShowDamage(damage, transform.position + Vector3.up * 0.5f);

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
