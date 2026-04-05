using System;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    private int maxHealth;
    private int currentHealth;
    private bool isDead;

    public bool IsDead => isDead;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

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

    public void TakeDamage(int damage, bool isCirt = false)
    {
        if (isDead || damage <= 0) return;

        //phát audio
        AudioManager.Instance?.PlayEnemyHit();

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if(isCirt)
            PopupController.Instance.ShowCirtDamage(transform.position + Vector3.up * 0.5f, damage);
        else
            PopupController.Instance.ShowDamage(transform.position + Vector3.up * 0.5f, damage);

        if (currentHealth == 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"{gameObject.name} DIE");
        OnDeath?.Invoke();

        GameManager.Instance?.AddEnemyKill();
    }

    [ContextMenu("Damage")]
    private void TestDamge()
    {
        TakeDamage(300);
    }
}
