using System;
using UnityEngine;

public class PlayerManaController : MonoBehaviour
{
    [Header("Regen Config")]
    [SerializeField] private int regenAmount = 1;
    [SerializeField] private float regenInterval = 1f;

    public int CurrentMP { get; private set; }
    public int MaxMP => stats != null ? stats.MaxMP : 0;

    public event Action<int, int> OnMPChanged;

    private CharacterStats stats;
    private float regenTimer;

    private bool initialized;

    #region Init

    public void Initialize(PlayerController controller)
    {
        stats = controller.CharacterStats;
    }

    #endregion

    #region Sync With StatSystem

    /// <summary>
    /// Gọi khi StatSystem.Recalculate()
    /// </summary>
    public void RefreshFromStats(bool keepPercent = true)
    {
        int newMax = MaxMP;
        if (newMax <= 0) return;

        if (!initialized)
        {
            CurrentMP = newMax;
            initialized = true;
        }
        else
        {
            float percent = keepPercent
                ? CurrentMP / (float)Mathf.Max(1, newMax)
                : 1f;

            CurrentMP = Mathf.RoundToInt(newMax * percent);
        }

        Clamp();
        regenTimer = 0f;
        Notify();
    }

    #endregion

    #region Update

    private void Update()
    {
        RegenTick();
    }

    private void RegenTick()
    {
        if (!initialized) return;
        if (CurrentMP >= MaxMP) return;
        if (regenAmount <= 0 || regenInterval <= 0f) return;

        regenTimer += Time.deltaTime;
        if (regenTimer < regenInterval) return;

        regenTimer = 0f;
        CurrentMP += regenAmount;
        Clamp();
        Notify();
    }

    #endregion

    #region Consume

    public bool CanConsume(int amount)
    {
        return initialized && CurrentMP >= amount;
    }

    public bool Consume(int amount)
    {
        if (!CanConsume(amount)) return false;

        CurrentMP -= amount;
        Clamp();
        Notify();
        return true;
    }

    #endregion

    #region Utils

    private void Clamp()
    {
        CurrentMP = Mathf.Clamp(CurrentMP, 0, MaxMP);
    }

    private void Notify()
    {
        OnMPChanged?.Invoke(CurrentMP, MaxMP);
    }

    #endregion

    #region Runtime Control (Optional)

    public void SetRegen(int amount, float interval)
    {
        regenAmount = Mathf.Max(0, amount);
        regenInterval = Mathf.Max(0f, interval);
    }

    #endregion
}
