using System;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth => stats != null ? stats.MaxHP : 0;
    public bool IsDead => CurrentHealth <= 0;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private PlayerController owner;
    private CharacterStats stats;

    private bool initialized;
    private bool deathInvoked;

    #region Init

    public void Initialize(PlayerController controller)
    {
        owner = controller;
        stats = controller.CharacterStats;
    }

    #endregion

    #region Sync With StatSystem

    /// <summary>
    /// Gọi khi StatSystem Recalculate (equip / upgrade)
    /// </summary>
    public void RefreshFromStats()
    {
        int newMax = MaxHealth;
        if (newMax <= 0) return;

        if (!initialized)
        {
            CurrentHealth = newMax;
            initialized = true;
            deathInvoked = false;
        }
        else
        {
            // giữ % HP
            float percent = CurrentHealth / (float)Mathf.Max(1, MaxHealth);
            CurrentHealth = Mathf.RoundToInt(newMax * percent);
        }

        ClampHealth();
        NotifyHealthChanged();
    }

    #endregion

    #region Damage / Heal

    public void TakeDamage(int rawDamage)
    {
        if (rawDamage <= 0 || IsDead) return;

        int finalDamage = CalculateFinalDamage(rawDamage);

        CurrentHealth -= finalDamage;
        ClampHealth();
        NotifyHealthChanged();

        ShowDamagePopup(finalDamage);

        if (IsDead)
            Die();
    }

    public void Heal(int amount)
    {
        if (amount <= 0 || IsDead) return;

        CurrentHealth += amount;
        ClampHealth();
        NotifyHealthChanged();

        ShowHealPopup(amount);
    }

    #endregion

    #region Damage Formula (Armor-ready)

    private int CalculateFinalDamage(int rawDamage)
    {
        float armor = owner.StatSystem.GetStat(
            StatContext.Character,
            StatType.Armor
        );

        // Công thức mẫu (dễ tweak)
        float reduction = armor / (armor + 100f);
        float final = rawDamage * (1f - reduction);

        return Mathf.Max(1, Mathf.RoundToInt(final));
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

    #region Popup

    private void ShowDamagePopup(int value)
    {
        if (PopupController.Instance == null) return;

        PopupController.Instance.ShowDamage(
            value,
            transform.position + Vector3.up * 0.5f
        );
    }

    private void ShowHealPopup(int value)
    {
        if (PopupController.Instance == null) return;

        PopupController.Instance.ShowHeal(
            value,
            transform.position + Vector3.up * 0.5f
        );
    }

    #endregion
}
