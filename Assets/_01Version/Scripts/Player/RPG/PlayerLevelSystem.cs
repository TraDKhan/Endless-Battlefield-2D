using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerLevelSystem : MonoBehaviour
{
    public int CurrentLevel { get; private set; } = 1;
    public int CurrentEXP { get; private set; }

    public int ExpToNextLevel => 50 + (CurrentLevel - 1) * 25;

    // ===== EVENT =====
    public event Action<int, int, int> OnExpChanged; // curExp, nextExp, level
    public event Action<int> OnLevelUp;              // newLevel
    [ContextMenu("ADD LEVEL")]
    public void AddEXP1()
    {
        CurrentEXP += 1000;

        while (true)
        {
            int expNeeded = ExpToNextLevel;
            if (CurrentEXP < expNeeded)
                break;

            CurrentEXP -= expNeeded;
            CurrentLevel++;

            OnLevelUp?.Invoke(CurrentLevel);
        }

        OnExpChanged?.Invoke(CurrentEXP, ExpToNextLevel, CurrentLevel);
    }
    public void AddEXP(int value)
    {
        if (value <= 0) return;

        CurrentEXP += value;

        while (true)
        {
            int expNeeded = ExpToNextLevel;
            if (CurrentEXP < expNeeded)
                break;

            CurrentEXP -= expNeeded;
            CurrentLevel++;

            OnLevelUp?.Invoke(CurrentLevel);
        }

        OnExpChanged?.Invoke(CurrentEXP, ExpToNextLevel, CurrentLevel);
    }

    [ContextMenu("LEVEL UP")]
    private void LevelUp()
    {
        CurrentLevel++;

        OnLevelUp?.Invoke(CurrentLevel);
        OnExpChanged?.Invoke(CurrentEXP, ExpToNextLevel, CurrentLevel);
    }

    // ===== API cho CharacterStats =====

    private int bonusHealth;
    private int bonusArmor;
    private float bonusMoveSpeed;
    public int GetBonusHealth() => bonusHealth;
    public int GetBonusArmor() => bonusArmor;
    public float GetBonusMoveSpeed() => bonusMoveSpeed;
}
