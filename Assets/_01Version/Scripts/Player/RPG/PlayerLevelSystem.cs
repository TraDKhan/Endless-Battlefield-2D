using System;
using UnityEngine;

public class PlayerLevelSystem
{
    public int CurrentLevel { get; private set; } = 1;
    public int CurrentEXP { get; private set; }

    public int ExpToNextLevel => 50 + (CurrentLevel - 1) * 50;

    // ===== EVENT =====
    public event Action<int, int, int> OnExpChanged; // curExp, nextExp, level
    public event Action<int> OnLevelUp;
    
    public void AddEXP(int value)
    {
        if (value <= 0) return;

        CurrentEXP += value;

        while (CurrentEXP >= ExpToNextLevel)
        {
            CurrentEXP -= ExpToNextLevel;
            CurrentLevel++;
            OnLevelUp?.Invoke(CurrentLevel);
        }

        OnExpChanged?.Invoke(CurrentEXP, ExpToNextLevel, CurrentLevel);
    }
}
