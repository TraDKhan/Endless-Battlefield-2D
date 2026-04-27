using System;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour, IDamageable
{
    private int maxHealth;
    private int currentHealth;
    private bool isDead;
    private EnemyContext enemyContext;
    private BossContext bossContext;

    public bool IsDead => isDead;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public event Action OnDeath;
    public event Action<int, int> OnHealthChanged; 

    public void Init(int maxHP, EnemyContext ectx = null, BossContext bctx = null)
    {
        maxHealth = maxHP;
        ResetHealth();

        Debug.Log(ectx);
        Debug.Log(bctx);
        enemyContext = ectx;
        bossContext = bctx;
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

        AudioManager.Instance?.PlayEnemyHit();

        if (enemyContext != null)
            enemyContext.Anim?.PlayHit();

        if (bossContext != null)
            bossContext.Anim?.PlayHit();

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
        OnDeath?.Invoke();

        if (enemyContext != null)
            enemyContext.Anim?.SetDead(true);

        if (bossContext != null)
            bossContext.Anim?.SetDead(true);

        GameManager.Instance?.AddEnemyKill();
    }

    [ContextMenu("Damage")]
    private void TestDamge()
    {
        TakeDamage(300);
    }
}
