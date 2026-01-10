using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

    public bool IsDead => CurrentHealth <= 0;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private bool initialized;
    private bool deathInvoked;

    #region Init
    public void SetMaxHealth(int newMaxHealth)
    {
        if (newMaxHealth <= 0) return;

        int oldMax = MaxHealth;
        MaxHealth = newMaxHealth;

        if (!initialized)
        {
            CurrentHealth = MaxHealth;
            initialized = true;
        }
        else
        {
            // Giữ % HP
            float percent = oldMax > 0
                ? (float)CurrentHealth / oldMax
                : 1f;

            CurrentHealth = Mathf.RoundToInt(MaxHealth * percent);
        }

        ClampHealth();
        NotifyHealthChanged();
    }
    #endregion

    #region Damage / Heal
    public void TakeDamage(int damage)
    {
        if (damage <= 0 || IsDead) return;

        int finalDamage = damage; // sau này xử lý armor

        CurrentHealth -= damage;
        ClampHealth();
        NotifyHealthChanged();

        PopupController.Instance.ShowDamage(damage, transform.position + Vector3.up * 0.5f);

        if (IsDead)
            Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead) return;

        CurrentHealth += amount;
        ClampHealth();
        NotifyHealthChanged();

        PopupController.Instance.ShowHeal(amount, transform.position + Vector3.up * 0.5f);
    }
    #endregion

    #region Death
    private void Die()
    {
        if (deathInvoked) return;

        deathInvoked = true;
        OnDeath?.Invoke();
    }
    #endregion

    #region Utils
    private void ClampHealth()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
    }
    #endregion

#if UNITY_EDITOR
    [ContextMenu("Test Text popup")]
    private void TestDamage()
    {
        TakeDamage(120);
        Heal(100);
    }

#endif
}
