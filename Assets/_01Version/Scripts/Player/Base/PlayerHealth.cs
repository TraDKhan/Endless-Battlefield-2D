using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    public event Action<int, int> OnHealthChanged;
    public event Action OnDeath;

    private CharacterStatSystem stats;

    private bool initialized;
    private bool deathInvoked;

    #region Init

    public void Initialize(PlayerController controller)
    {
        stats = controller.StatSystem;
    }

    #endregion

    #region Sync With StatSystem

    public void RefreshFromStats()
    {
        int newMax = Mathf.RoundToInt(
            stats.GetStat(CharacterStatType.MaxHP)
        );

        if (newMax <= 0)
            newMax = 1; // failsafe

        if (!initialized)
        {
            MaxHealth = newMax;
            CurrentHealth = MaxHealth;
            initialized = true;
            deathInvoked = false;
        }
        else
        {
            float percent = CurrentHealth / (float)Mathf.Max(1, MaxHealth);
            MaxHealth = newMax;
            CurrentHealth = Mathf.RoundToInt(MaxHealth * percent);
        }

        ClampHealth();
        NotifyHealthChanged();
    }

    // Thêm vào trong PlayerHealth.cs

    public void Respawn(int healthAmount = -1)
    {
        deathInvoked = false; // Cho phép chết lần nữa

        // Nếu không truyền máu cụ thể, hồi đầy máu
        if (healthAmount <= 0)
            CurrentHealth = MaxHealth;
        else
            CurrentHealth = Mathf.Min(healthAmount, MaxHealth);

        ClampHealth();
        NotifyHealthChanged();
        if (TryGetComponent<PlayerMovement>(out var move)) move.enabled = true;
        Debug.Log("Player has been respawned.");
    }
    #endregion

    #region Damage / Heal

    public void TakeDamage(int rawDamage, bool isCrit = false)
    {
        if (rawDamage <= 0 || IsDead) return;

        int finalDamage = CalculateFinalDamage(rawDamage);

        CurrentHealth -= finalDamage;
        ClampHealth();
        NotifyHealthChanged();

        //ShowDamagePopup(finalDamage);

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
        float armor = stats.GetStat(CharacterStatType.Armor);

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
        GameManager.Instance?.Handle_GameLose();

        //tối ưu singleton truyền từ controller vào init để tránh gọi Instance nhiều lần

        GameManager.Instance?.Handle_GameLose();
        

        if (TryGetComponent<PlayerMovement>(out var move)) move.enabled = false;

        PlayerController.Instance.Anim?.PlayDeath();
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
            transform.position + Vector3.up * 0.5f, value
        );
    }

    private void ShowHealPopup(int value)
    {
        if (PopupController.Instance == null) return;

        PopupController.Instance.ShowHeal(
            transform.position + Vector3.up * 0.5f,
            value
        );
    }

    #endregion

    #region Poison

    public void ApplyPoison(int damage, float duration, float tickInterval)
    {
        StartCoroutine(PoisonCoroutine(damage, duration, tickInterval));
    }

    private IEnumerator PoisonCoroutine(int damage, float duration, float tick)
    {
        float time = 0;

        while (time < duration)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(tick);
            time += tick;
        }
    }
    #endregion

    [ContextMenu("Die")]
    public void TestDie() => Die();
    [ContextMenu ("Take 10 Damage")]
    public void TestDamage() => TakeDamage(10);
}
